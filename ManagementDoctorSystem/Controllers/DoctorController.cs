using AspNetCoreGeneratedDocument;

using ManagementManagementDoctorSystem.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementDoctorSystem.Controllers
{
    public class DoctorController : Controller
    {
        //private readonly ILogger<DoctorController> _logger;
        private ApplicationDbContext _context = new();
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
                doctors = doctors.Where(e => e.Name.Contains(filterDoctorVM.specialization));
                ViewBag.specialization = filterDoctorVM.specialization;
            }

            var doctor = _context.doctors;
            ViewBag.doctors = doctors.AsEnumerable();

            return View(doctors.AsNoTracking().AsEnumerable());
        }


        public IActionResult CompleteAppointment()
        {
            var patients = _context.pationts.AsQueryable();
            ViewBag.Id = patients.ToList();

            return View(patients.ToList());
        }
        public IActionResult ReservationsManagement()
        {
            var doctors = _context.doctors.AsQueryable();
            var patients = _context.pationts.AsQueryable();
            return View();
        }
    }
}
