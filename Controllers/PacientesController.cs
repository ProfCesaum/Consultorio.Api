using Consultorio.Api.Data;
using Consultorio.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consultorio.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PacientesController : ControllerBase
{
    private readonly AppDbContext _context;

    public PacientesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodosPacientes()
    {
        var pacientes = await _context.Pacientes.ToListAsync();
        return Ok(pacientes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ListarPacientePorId(int id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);
        if (paciente == null)
        {
            return NotFound();
        }
        return Ok(paciente);
    }

    [HttpPost]
    public async Task<IActionResult> CriarPaciente(Paciente paciente)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (await _context.Pacientes.AnyAsync(p => p.Cpf == paciente.Cpf || p.Email == paciente.Email))
        {
            return BadRequest("CPF ou Email já cadastrado.");
        }

        _context.Pacientes.Add(paciente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ListarPacientePorId), new { id = paciente.Id }, paciente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditarPaciente(int id, Paciente paciente)
    {
        if (id != paciente.Id)
        {
            return BadRequest("ID do paciente não corresponde ao ID da URL.");
        }
        if (!ModelState.IsValid) return BadRequest(ModelState);


        var pacienteExistente = await _context.Pacientes.FindAsync(id);

        if (pacienteExistente == null)
        {
            return NotFound();
        }

        if (await _context.Pacientes.AnyAsync(p => (p.Cpf == paciente.Cpf || p.Email == paciente.Email) && p.Id != id))
            return BadRequest("CPF ou Email já cadastrado para outro paciente.");


        pacienteExistente.Nome = paciente.Nome;
        pacienteExistente.Email = paciente.Email;
        pacienteExistente.Cpf = paciente.Cpf;

        _context.Update(pacienteExistente);
        await _context.SaveChangesAsync();
        return Ok(pacienteExistente);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarPaciente(int id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);
        if (paciente == null)
        {
            return NotFound();
        }
        _context.Pacientes.Remove(paciente);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}