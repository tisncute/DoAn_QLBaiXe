using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartParkingSystem.Data;
using SmartParkingSystem.Helpers;
using SmartParkingSystem.Models;

namespace SmartParkingSystem.Controllers
{
    [Authorize]
    public class ParkingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParkingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Dashboard: Hiện danh sách xe
        public async Task<IActionResult> Index(string searchString)
        {
            // Mặc định: Lấy danh sách xe chưa ra
            var query = _context.ParkingSessions.Where(x => !x.IsCompleted);

            // Nếu có từ khóa tìm kiếm thì lọc theo biển số
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.LicensePlate.Contains(searchString));
            }

            // Sắp xếp xe mới vào lên đầu
            var result = await query.OrderByDescending(x => x.CheckInTime).ToListAsync();

            return View(result);
        }

        // 2. Chức năng Xe Vào (GET)
        [HttpGet]
        public IActionResult CheckIn()
        {
            var activeSessions = _context.ParkingSessions.Where(x => !x.IsCompleted).ToList();
            ViewBag.ActiveSessions = activeSessions;
            ViewBag.CurrentCount = activeSessions.Count;
            ViewBag.MaxCapacity = 6;
            return View();
        }

        // 3. Chức năng Xe Vào (POST - Xử lý)
        [HttpPost]
        public async Task<IActionResult> CheckIn(ParkingSession model)
        {
            if (ModelState.IsValid)
            {
                // 1. Kiểm tra xem người dùng ĐÃ CLICK CHỌN Ô CHƯA?
                if (string.IsNullOrEmpty(model.SpotName))
                {
                    ModelState.AddModelError("SpotName", "Vui lòng click chọn 1 vị trí đỗ xe trên sơ đồ (A1-A6)!");
                    return ReloadCheckInView(model); // Load lại trang và báo lỗi
                }

                // 2. Kiểm tra xem biển số này đang gửi trong bãi chưa?
                bool isAlreadyIn = _context.ParkingSessions.Any(x => x.LicensePlate == model.LicensePlate && !x.IsCompleted);
                if (isAlreadyIn)
                {
                    ModelState.AddModelError("LicensePlate", "Xe này đang ở trong bãi, chưa Check-out!");
                    return ReloadCheckInView(model);
                }

                // 3. Kiểm tra xem ô đỗ đó đã bị người khác giành mất chưa? (Chống trùng lặp)
                bool isSpotTaken = _context.ParkingSessions.Any(x => x.SpotName == model.SpotName && !x.IsCompleted);
                if (isSpotTaken)
                {
                    ModelState.AddModelError("SpotName", $"Vị trí {model.SpotName} vừa có xe đỗ. Vui lòng chọn ô khác!");
                    return ReloadCheckInView(model);
                }

                // 4. Lưu vào Database
                model.CheckInTime = DateTime.Now;
                model.IsCompleted = false;

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return ReloadCheckInView(model);
        }

        // Hàm phụ để nạp lại dữ liệu Sơ đồ bãi xe nếu bị lỗi
        private IActionResult ReloadCheckInView(ParkingSession model)
        {
            var activeSessions = _context.ParkingSessions.Where(x => !x.IsCompleted).ToList();
            ViewBag.ActiveSessions = activeSessions;
            ViewBag.CurrentCount = activeSessions.Count;
            ViewBag.MaxCapacity = 6;
            return View(model);
        }

        // 4. Chức năng Xe Ra (Tìm kiếm & Tính tiền)
        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null) return NotFound();

            var session = await _context.ParkingSessions.FindAsync(id);
            if (session == null) return NotFound();

            // Tính toán tiền giả định ngay lúc bấm nút
            session.CheckOutTime = DateTime.Now;
            session.ParkingFee = FeeCalculator.Calculate(session.VehicleType, session.CheckInTime, session.CheckOutTime.Value);

            return View(session);
        }

        // 5. Xác nhận Thanh toán
        [HttpPost]
        public async Task<IActionResult> ConfirmCheckOut(int id, string plateOut)
        {
            var session = await _context.ParkingSessions.FindAsync(id);
            if (session != null)
            {
                // So sánh biển số lúc vào (Database) và biển số lúc ra (Form gửi lên)
                if (session.LicensePlate.Trim() != plateOut.Trim())
                {
                    // Nếu KHÔNG KHỚP -> Báo lỗi ngay lập tức
                    TempData["Error"] = "PHÁT HIỆN GIAN LẬN! Biển số lúc ra (" + plateOut + ") không khớp với lúc vào!";

                    // Tính lại tiền để hiển thị lại View
                    session.CheckOutTime = DateTime.Now;
                    session.ParkingFee = FeeCalculator.Calculate(session.VehicleType, session.CheckInTime, session.CheckOutTime.Value);

                    return View("CheckOut", session);
                }

                // --- NẾU KHỚP THÌ CHO QUA ---
                session.IsCompleted = true;
                session.CheckOutTime = DateTime.Now;
                session.ParkingFee = FeeCalculator.Calculate(session.VehicleType, session.CheckInTime, session.CheckOutTime.Value);

                _context.Update(session);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Xe {session.LicensePlate} hợp lệ. Đã thu tiền và mở cổng.";
            }
            return RedirectToAction(nameof(Index));
        }

        // Lịch sử và doanh thu theo ngày
        public async Task<IActionResult> History(string vehicleType, DateTime? fromDate, DateTime? toDate)
        {
            // 1. Bắt đầu với danh sách xe ĐÃ RA KHỎI BẾN
            var query = _context.ParkingSessions.Where(x => x.IsCompleted);

            // 2. Lọc theo loại xe (Nếu người dùng có chọn)
            if (!string.IsNullOrEmpty(vehicleType) && vehicleType != "All")
            {
                query = query.Where(x => x.VehicleType == vehicleType);
            }

            // 3. Lọc theo ngày (Từ ngày... Đến ngày...)
            if (fromDate.HasValue)
            {
                // Lấy từ đầu ngày đó (00:00:00)
                query = query.Where(x => x.CheckOutTime >= fromDate.Value.Date);
            }
            if (toDate.HasValue)
            {
                // Lấy đến cuối ngày đó (23:59:59)
                query = query.Where(x => x.CheckOutTime <= toDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            // 4. Lấy dữ liệu và sắp xếp
            var resultList = await query.OrderByDescending(x => x.CheckOutTime).ToListAsync();

            // 5. Đóng gói vào ViewModel để gửi sang View
            var model = new HistoryViewModel
            {
                Sessions = resultList,
                TotalRevenue = resultList.Sum(x => x.ParkingFee),
                SelectedVehicleType = vehicleType,
                FromDate = fromDate,
                ToDate = toDate
            };

            return View(model);
        }
        
        // XUẤT EXCEL DOANH THU
        public async Task<IActionResult> ExportExcel()
        {
            var data = await _context.ParkingSessions
                                     .Where(x => x.IsCompleted)
                                     .OrderByDescending(x => x.CheckOutTime)
                                     .ToListAsync();

            var builder = new System.Text.StringBuilder();
            builder.AppendLine("Bien So,Loai Xe,Gio Vao,Gio Ra,Tien Thu");

            foreach (var item in data)
            {
                string vao = item.CheckInTime.ToString("yyyy-MM-dd HH:mm:ss");
                string ra = item.CheckOutTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";

                builder.AppendLine($"{item.LicensePlate},{item.VehicleType},{vao},{ra},{item.ParkingFee}");
            }

            return File(System.Text.Encoding.UTF8.GetPreamble().Concat(System.Text.Encoding.UTF8.GetBytes(builder.ToString())).ToArray(), "text/csv", "BaoCaoDoanhThu.csv");
        }

        // KHU VỰC DÀNH CHO KHÁCH HÀNG ĐẶT CHỖ TRƯỚC
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Booking()
        {
            // Lấy các ô đã bị chiếm (cả xe đang gửi lẫn xe đã đặt)
            var activeSessions = _context.ParkingSessions.Where(x => !x.IsCompleted).ToList();
            ViewBag.ActiveSessions = activeSessions;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Booking(ParkingSession model)
        {
            if (string.IsNullOrEmpty(model.SpotName))
            {
                ModelState.AddModelError("SpotName", "Vui lòng click chọn 1 vị trí đỗ xe trên sơ đồ!");
            }
            if (string.IsNullOrEmpty(model.CustomerName) || string.IsNullOrEmpty(model.PhoneNumber))
            {
                ModelState.AddModelError("CustomerName", "Vui lòng nhập đầy đủ Tên và Số điện thoại!");
            }

            // Kiểm tra xem ô đó có bị ai nhanh tay đặt mất chưa
            bool isSpotTaken = _context.ParkingSessions.Any(x => x.SpotName == model.SpotName && !x.IsCompleted);
            if (isSpotTaken)
            {
                ModelState.AddModelError("SpotName", "Rất tiếc, ô này vừa có người đặt. Vui lòng chọn ô khác!");
            }

            if (ModelState.IsValid)
            {
                model.CheckInTime = DateTime.Now; // Giờ khách bấm đặt
                model.IsCompleted = false;
                model.IsBooked = true; // Đánh dấu đây là Đơn Đặt Chỗ, xe chưa tới

                _context.Add(model);
                await _context.SaveChangesAsync();

                // Trả về trang thông báo thành công
                TempData["SuccessMessage"] = $"Đặt chỗ thành công! Vị trí của bạn là {model.SpotName}.";
                return RedirectToAction(nameof(Booking));
            }

            ViewBag.ActiveSessions = _context.ParkingSessions.Where(x => !x.IsCompleted).ToList();
            return View(model);
        }

        // XÁC NHẬN KHÁCH ĐÃ ĐẾN VÀ NHẬN XE VÀO BÃI
        [HttpPost]
        public async Task<IActionResult> ConfirmArrival(int id)
        {
            // Tìm giao dịch đặt chỗ
            var session = await _context.ParkingSessions.FindAsync(id);
            if (session != null && session.IsBooked)
            {
                // Hủy trạng thái Đặt chỗ -> Chuyển thành Đang đỗ thật sự
                session.IsBooked = false;

                // Bắt đầu tính giờ gửi xe từ đúng thời điểm khách đưa xe vào bãi
                session.CheckInTime = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // Tải lại trang chủ
        }

        // HỦY LỊCH ĐẶT CHỖ
        [HttpPost]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var session = await _context.ParkingSessions.FindAsync(id);
            if (session != null && session.IsBooked)
            {
                _context.ParkingSessions.Remove(session);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã hủy lịch đặt chỗ của khách {session.CustomerName} (Biển số: {session.LicensePlate}) thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        // XÓA LỊCH SỬ
        [HttpPost]
        public async Task<IActionResult> DeleteHistory(int id)
        {
            var session = await _context.ParkingSessions.FindAsync(id);
            if (session != null)
            {
                string bienSo = session.LicensePlate ?? "Chưa rõ";

                _context.ParkingSessions.Remove(session);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã xóa lịch sử của xe {bienSo} thành công!";
            }
            return RedirectToAction(nameof(History));
        }

        // XÓA TOÀN BỘ LỊCH SỬ
        [HttpPost]
        public async Task<IActionResult> DeleteAllHistory()
        {
            var allHistory = await _context.ParkingSessions.Where(s => s.IsCompleted).ToListAsync();

            if (allHistory.Any())
            {
                _context.ParkingSessions.RemoveRange(allHistory);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã xóa sạch {allHistory.Count} bản ghi lịch sử. Doanh thu đã được reset về 0!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không có dữ liệu lịch sử nào để xóa.";
            }

            return RedirectToAction(nameof(History));
        }
    }
}