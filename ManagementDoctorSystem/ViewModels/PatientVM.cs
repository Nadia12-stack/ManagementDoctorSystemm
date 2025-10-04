namespace ManagementDoctorSystem.ViewModels
{
    public record PatientVM(
        string patientName, DateTime appointmentDate, TimeSpan appointmentTime
        );
    
}
