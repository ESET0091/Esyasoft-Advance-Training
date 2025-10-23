//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using MediBuddy.Data.Context;
//using MediBuddy.Data.Entities;

//namespace MediBuddy.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PatientsController : ControllerBase
//    {
//        private readonly AplicationDBContext _context;

//        public PatientsController(AplicationDBContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Patients
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
//        {
//            return await _context.Patients
//                .Include(p => p.User)
//                .ToListAsync();
//        }

//        // GET: api/Patients/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Patient>> GetPatient(int id)
//        {
//            var patient = await _context.Patients
//                .Include(p => p.User)
//                .FirstOrDefaultAsync(p => p.PatientId == id);

//            if (patient == null)
//            {
//                return NotFound();
//            }

//            return patient;
//        }

//        // POST: api/Patients
//        [HttpPost]
//        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
//        {
//            _context.Patients.Add(patient);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetPatient", new { id = patient.PatientId }, patient);
//        }
//    }
//}
