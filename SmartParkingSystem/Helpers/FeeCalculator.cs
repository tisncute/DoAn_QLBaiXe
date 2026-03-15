namespace SmartParkingSystem.Helpers
{
    public static class FeeCalculator
    {
        public static decimal Calculate(string type, DateTime inTime, DateTime outTime)
        {
            var duration = outTime - inTime;
            double totalHours = duration.TotalHours;

            if (type == "XeMay")
            {
                // Xe máy: Dưới 4 tiếng 5k, Trên 4 tiếng 10k
                return totalHours <= 4 ? 5000 : 10000;
            }
            else
            {
                // Ô tô: 2 tiếng đầu 30k. Mỗi tiếng sau thêm 10k.
                if (totalHours <= 2) return 30000;

                var extraHours = Math.Ceiling(totalHours - 2);
                return 30000 + (decimal)(extraHours * 10000);
            }
        }
    }
}