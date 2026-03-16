# HỆ THỐNG QUẢN LÝ BÃI GIỮ XE THÔNG MINH

Đây là kho lưu trữ mã nguồn cho **Đồ án cơ sở 01**, ngành Công nghệ Thông tin, Trường Đại học Nam Cần Thơ.

## 1. Thông tin sinh viên thực hiện
* **Họ và tên:** Lê Dương Huỳnh Tín
* **MSSV:** 222908
* **Lớp:** DH23TIN09
* **Giảng viên hướng dẫn:** ThS. Trần Văn Thiện

## 2. Hướng dẫn chạy thử nghiệm (Live Demo)
Trang web đã được triển khai trực tuyến trên máy chủ SmarterASP.NET. Do chính sách bảo mật chống truy cập ảo của nhà mạng, vui lòng sử dụng thông tin xác thực dưới đây để vượt qua màn hình bảo vệ ban đầu:

* **Link truy cập:** [http://tisncute-001-site1.ktempurl.com](http://tisncute-001-site1.ktempurl.com)
* **User Name:** `11299981`
* **Password:** `60-dayfreetrial`

*(Lưu ý: Đây chỉ là tài khoản để mở khóa đường link. Sau khi vào được giao diện chính của bãi đỗ xe, thầy có thể sử dụng các chức năng hệ thống bình thường).*

## 3. Các chức năng chính
* **Khách hàng:** Xem sơ đồ bãi đỗ (ô trống/đã có người) và đặt chỗ gửi xe trực tuyến.
* **Quản trị viên (Admin):** * Ghi nhận xe vào bến (Check-in) và cho xe ra (Check-out).
  * Cảnh báo tự động nếu sai lệch biển số khi xe ra.
  * Tự động tính phí gửi xe dựa trên thời gian thực.
  * Tạo mã QR thanh toán động hiển thị ngay trên màn hình.

## 4. Công nghệ sử dụng
* **Backend:** C# ASP.NET Core MVC
* **Database:** SQL Server (Entity Framework Core - Code First)
* **Frontend:** HTML, CSS, JavaScript (Giao diện Dark Mode cho Admin)
* **Tích hợp:** VietQR API (Tạo mã thanh toán động)

## 5. Tài liệu đính kèm
* Báo cáo đồ án (Đã bao gồm đầy đủ phần phụ lục chi tiết của dự án).
* Script dữ liệu (`.sql`).
