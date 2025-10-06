using ManagementDoctorSystem.Models;

namespace ManagementDoctorSystem.ViewModels
{
    public class ReservationVM
    {
        public string DoctorName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string PatientName { get; set; }= string.Empty;
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }

        public List<Doctor>? Doctors { get; set; }
       
       
    }
}
