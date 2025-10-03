namespace ManagementDoctorSystem.Models
{
    public class Pationt
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }  
        public TimeOnly AppointmentTime { get; set; } 

    }
}
