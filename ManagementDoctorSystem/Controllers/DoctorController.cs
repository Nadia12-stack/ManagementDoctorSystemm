using AspNetCoreGeneratedDocument;
using ManagementDoctorSystem.Models;
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
        public IActionResult CompleteAppointment(int doctorId)
        {
            var doctor = _context.doctors.FirstOrDefault(d => d.Id == doctorId);
            if (doctor == null)
            {
                TempData["Error"] = "Doctor not found.";
                return RedirectToAction("BookAppointment");
            }

            var model = new PatientVM(
                doctorId: doctor.Id,
                patientName: "",
                appointmentDate: DateTime.Today,
                appointmentTime: new TimeSpan(8, 0, 0),
                DoctorName: doctor.Name
            );

            return View(model);
        }



        [HttpPost]
        public IActionResult CompleteAppointment(PatientVM patientVM)
        {
            var Patientt = _context.Patients.AsQueryable();
            var dayOfWeek = patientVM.appointmentDate.DayOfWeek;
            if (dayOfWeek == DayOfWeek.Friday || dayOfWeek == DayOfWeek.Saturday)
            {
                TempData["Error"] = "Appointments are only available from Sunday to Thursday.";
                return RedirectToAction("CompleteAppointment", new { doctorId = patientVM.doctorId });

            }


            int hour = patientVM.appointmentTime.Hours;
            int minute = patientVM.appointmentTime.Minutes;


            if (hour < 8 || hour >= 17)
            {
                TempData["Error"] = "Appointments are available between 8:00 AM and 5:00 PM only.";
                return RedirectToAction("CompleteAppointment", new { doctorId = patientVM.doctorId });
            }


            if (minute != 0 && minute != 30)
            {
                TempData["Error"] = "Appointments must be scheduled in 30-minute intervals (e.g., 8:00 or 8:30).";
                return RedirectToAction("CompleteAppointment", new { doctorId = patientVM.doctorId });
            }


            var isTaken = _context.Patients.Any(p =>
                p.AppointmentDate.Date == patientVM.appointmentDate.Date &&
                p.AppointmentTime == patientVM.appointmentTime);

            if (isTaken)
            {
                TempData["Error"] = "This time slot is already booked. Please choose another.";
                return RedirectToAction("CompleteAppointment", new { doctorId = patientVM.doctorId });

            }


            var existingPatient = _context.Patients.FirstOrDefault(p =>
                p.Name == patientVM.patientName &&
                p.AppointmentDate == patientVM.appointmentDate &&
                p.AppointmentTime == patientVM.appointmentTime);


            if (existingPatient != null)
            {
                existingPatient.AppointmentDate = patientVM.appointmentDate;
                existingPatient.AppointmentTime = patientVM.appointmentTime;

                _context.SaveChanges();

                TempData["Success"] = "Appointment successfully updated!";
                return RedirectToAction("CompleteAppointment", new { doctorId = patientVM.doctorId });
            }
            else
            {
                var patients = _context.Patients;
                ViewBag.Patients = patients.ToList();
                var doctors = _context.doctors;
                ViewBag.doctors = doctors.ToList();

                var newPatient = new Patient
                {
                    Name = patientVM.patientName,
                    AppointmentDate = patientVM.appointmentDate,
                    AppointmentTime = patientVM.appointmentTime,
                    DoctorId = patientVM.doctorId
                };
                _context.Patients.Add(newPatient);
              

            }

            _context.SaveChanges();
            TempData["Success"] = "Appointment successfully booked!";
            return RedirectToAction("CompleteAppointment", new { doctorId = patientVM.doctorId });
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
                    AppointmentTime = p.AppointmentTime,
                    DoctorId = p.DoctorId
                })
                .ToList();

            return View(reservations);
        }





    }

}