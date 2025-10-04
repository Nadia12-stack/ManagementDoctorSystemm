using AspNetCoreGeneratedDocument;

using ManagementManagementDoctorSystem.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementDoctorSystem.Controllers
{
    public class DoctorController : Controller
    {

        private ApplicationDbContext _context = new();
        [HttpGet]
        public IActionResult BookAppointment(FilterDoctorVM filterDoctorVM)
        {
            var doctors = _context.doctors.AsQueryable();

            if (filterDoctorVM.Name is not null)
            {
                doctors = doctors.Where(e => e.Name.Contains(filterDoctorVM.Name));
                ViewBag.Name = filterDoctorVM.Name;
            }

            if (filterDoctorVM.specialization is not null)
            {
                doctors = doctors.Where(e => e.Specialization.Contains(filterDoctorVM.specialization));
                ViewBag.specialization = filterDoctorVM.specialization;
            }


            ViewBag.doctors = doctors.AsEnumerable();

            return View(doctors.AsNoTracking().AsEnumerable());
        }

        [HttpGet]
        public IActionResult CompleteAppointment()
        {
            var patients = _context.Patients.ToList();
            return View(patients);
        }


        [HttpPost]
        public IActionResult CompleteAppointment(PatientVM patientVM)
        {
            var Patient = _context.Patients.AsQueryable();
            var dayOfWeek = patientVM.appointmentDate.DayOfWeek;
            if (dayOfWeek == DayOfWeek.Friday || dayOfWeek == DayOfWeek.Saturday)
            {
                TempData["Error"] = "Appointments are only available from Sunday to Thursday.";
                return RedirectToAction("CompleteAppointment");
                
            }

            
            int hour = patientVM.appointmentTime.Hours;
            int minute = patientVM. appointmentTime.Minutes;

            if (hour < 8 || hour >= 17)
            {
                TempData["Error"] = "Appointments are available between 8:00 AM and 5:00 PM only.";
                return RedirectToAction("CompleteAppointment");
            }

            
            if (minute != 0 && minute != 30)
            {
                TempData["Error"] = "Appointments must be scheduled in 30-minute intervals (e.g., 8:00 or 8:30).";
                return RedirectToAction("CompleteAppointment");
            }

            
            var isTaken = _context.Patients.Any(p =>
                p.AppointmentDate.Date == patientVM.appointmentDate.Date &&
                p.AppointmentTime == patientVM.appointmentTime);

            if (isTaken)
            {
                TempData["Error"] = "This time slot is already booked. Please choose another.";
                return RedirectToAction("CompleteAppointment");

            }

            
            var existingPatient = _context.Patients.FirstOrDefault(p => p.Name == patientVM.patientName);

            if (existingPatient != null)
            {
                existingPatient.AppointmentDate = patientVM.appointmentDate;
                existingPatient.AppointmentTime = patientVM.appointmentTime;

                _context.SaveChanges();

                TempData["Success"] = "Appointment successfully updated!";
                return RedirectToAction("ReservationsManagement");
            }
            else
            {
                TempData["Error"] = "Patient not found in the system.";
                return RedirectToAction("CompleteAppointment");
            }

          

        }

        [HttpGet]
        public IActionResult ReservationsManagement(ReservationVM reservationVM)
        {
            var reservations = _context.Patients
                .Include(p => p.Doctor) 
                .Select(p => new ReservationVM
                {
                    DoctorName = p.Doctor.Name,
                    PatientName = p.Name,
                    AppointmentDate = p.AppointmentDate,
                    AppointmentTime = p.AppointmentTime
                })
                .ToList(); 

            return View(reservations); 
        }





    }

}
