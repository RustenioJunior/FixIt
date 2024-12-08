using Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace csharp_crud_api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly AppointmentContext _context;
    private readonly StatusContext _statusContext;
    private readonly MachineContext _machineContext;
    public AppointmentController(AppointmentContext context, StatusContext statusContext, MachineContext machineContext) 
    {
        _context = context;
        _statusContext = statusContext;
        _machineContext = machineContext;
    }

     // GET: api/Appointments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
    {
        return await _context.Appointments.ToListAsync();
    }

    // GET: api/Appointments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Appointment>> GetUAppointment(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment == null)
        {
            return NotFound();
        }

        return appointment;
    }

    // POST: api/Appointments
    [HttpPost]
    public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var statusExists = _statusContext.Statuss.Any(st => st.Id == appointment.Status_id);
        if (!statusExists)
        {
            return BadRequest("The status type does not exist.");
        }
        var machineExists = _machineContext.Machines.Any(m => m.Id == appointment.Machine_id);
        if (!machineExists)
        {
            return BadRequest("The machine  does not exist.");
        }
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUAppointment), new { id = appointment.Id }, appointment);
    }

     // PUT: api/appointments/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
    {
        
        if (id != appointment.Id)
        {
            return BadRequest();
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var statusExists = _statusContext.Statuss.Any(st => st.Id == appointment.Status_id);
        if (!statusExists)
        {
            return BadRequest("The status type does not exist.");
        }
        var machineExists = _machineContext.Machines.Any(m => m.Id == appointment.Machine_id);
        if (!machineExists)
        {
            return BadRequest("The machine  does not exist.");
        }

        _context.Entry(appointment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AppointmentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/appointment/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AppointmentExists(int id)
    {
        return _context.Appointments.Any(e => e.Id == id);
    }

    // dummy method to test the connection
    [HttpGet("hello")]
    public string Test()
    {
        return "Hello World!";
    }

}