namespace ManagementDoctorSystem.ViewModels
{
    public record PatientVM(
     int doctorId,   string patientName, DateTime appointmentDate, TimeSpan appointmentTime, string DoctorName
        );
    
}
