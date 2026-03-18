using Consultorio.Api.Data;
using Consultorio.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consultorio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConsultasController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consulta>>> ListarTodosConsultas()
        {
            var consultas = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();
            return Ok(consultas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ListarConsultaPorId(int id)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (consulta == null)
            {
                return NotFound();
            }
            return Ok(consulta);
        }

        [HttpPost]
        public async Task<IActionResult> CriarConsulta(Consulta consulta)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var paciente = await _context.Pacientes.FindAsync(consulta.PacienteId);
            if (paciente == null) return BadRequest("Paciente não encontrado.");

            var medico = await _context.Medicos.AnyAsync(c => c.Id == consulta.MedicoId);
            if (!medico) return BadRequest("Medico não encontrado.");



            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarConsultaPorId), new { id = consulta.Id }, consulta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarConsulta(int id, Consulta consulta)
        {
            if (id != consulta.Id)
            {
                return BadRequest("ID da consulta não corresponde ao ID da URL.");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var consultaExistente = await _context.Consultas.FindAsync(id);
            if (consultaExistente == null) return NotFound();

            var paciente = await _context.Pacientes.AnyAsync(c => c.Id == consulta.MedicoId);
            if (!paciente) return BadRequest("Paciente não encontrado.");

            var medico = await _context.Medicos.AnyAsync(c => c.Id == consulta.MedicoId);
            if (!medico) return BadRequest("Medico não encontrado.");

            consultaExistente.PacienteId = consulta.PacienteId;
            consultaExistente.MedicoId = consulta.MedicoId;
            consultaExistente.DataHora = consulta.DataHora;
            consultaExistente.Obs = consulta.Obs;


            _context.Update(consultaExistente);
            await _context.SaveChangesAsync();
            return Ok(consultaExistente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarConsulta(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }
            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
