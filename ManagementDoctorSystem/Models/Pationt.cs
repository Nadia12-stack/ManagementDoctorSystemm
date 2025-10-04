namespace ManagementDoctorSystem.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }  
        public TimeSpan AppointmentTime { get; set; }


        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;


    }
}
