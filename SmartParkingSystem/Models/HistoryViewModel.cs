using System.ComponentModel.DataAnnotations;

namespace SmartParkingSystem.Models
{
    public class HistoryViewModel
    {
        // Danh sách kết quả tìm được
        public List<ParkingSession>? Sessions { get; set; }

        // Tổng tiền của danh sách đó
        public decimal TotalRevenue { get; set; }

        // Các ô bộ lọc
        public string? SelectedVehicleType { get; set; } // Loại xe đang chọn
        public DateTime? FromDate { get; set; }         // Từ ngày
        public DateTime? ToDate { get; set; }           // Đến ngày
    }
}