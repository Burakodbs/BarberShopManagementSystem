namespace BarberShopManagementSystem.ViewModels {
    public class EmployeeAppointmentStatsViewModel {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int TotalAppointments { get; set; }
        public decimal TotalEarnings { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public decimal AverageAppointmentPrice { get; set; }
        public decimal CompletionRate { get; set; }
    }
}
