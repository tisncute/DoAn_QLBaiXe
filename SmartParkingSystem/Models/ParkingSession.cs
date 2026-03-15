using System.ComponentModel.DataAnnotations;

namespace SmartParkingSystem.Models
{
    public class ParkingSession
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập biển số")]
        [Display(Name = "Biển số xe")]
        public string? LicensePlate { get; set; } // Ví dụ: 59A-123.45

        [Display(Name = "Loại xe")]
        public string? VehicleType { get; set; } // Sẽ lưu chữ "XeMay" hoặc "OTo"

        [Display(Name = "Giờ vào")]
        public DateTime CheckInTime { get; set; } // Lưu ngày giờ lúc bấm nút Vào

        public DateTime? BookingTime { get; set; }

        [Display(Name = "Giờ ra")]
        public DateTime? CheckOutTime { get; set; }

        [Display(Name = "Thành tiền (VNĐ)")]
        public decimal ParkingFee { get; set; }

        public bool IsCompleted { get; set; } = false; // Mặc định là False (Xe đang trong bến)

        public string? SpotName { get; set; }

        public string? CustomerName { get; set; }

        public string? PhoneNumber { get; set; }

        public bool IsBooked { get; set; }
    }
}