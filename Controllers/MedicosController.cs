using Consultorio.Api.Data;
using Consultorio.Api.DTOs;
using Consultorio.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consultorio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicosController(AppDbContext c)
        {
            _context = c;
        }

        [HttpPost]
        public async Task<ActionResult<Medico>> CriarMedico(Medico medico)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //var consultorioEx = await _context.Consultorios.FindAsync(medico.ConsultorioId);
            //if(consultorioEx == null) return BadRequest("Consultório não encontrado.");

            var consultorioEx = await _context.Consultorios.AnyAsync(c => c.Id == medico.ConsultorioId);
            if (!consultorioEx) return BadRequest("Consultório não encontrado.");

            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarTodosMedicos), new { id = medico.Id }, medico);
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodosMedicos()
        {
            var medicos = await _context.Medicos.Select(m => new { m.Id, m.Nome, m.Crm, m.ConsultorioId, nomeConsultorio = m.Consultorio.Nome }).ToListAsync();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicoResponseDto>> ListarMedicoPorId(int id)
        {
            var medico = await _context.Medicos
                .Include(m => m.Consultorio)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medico == null) return NotFound("Médico não encontrado.");

            var medicoDto = new MedicoResponseDto
            {
                Id = medico.Id,
                Nome = medico.Nome,
                Crm = medico.Crm,
                ConsultorioId = medico.ConsultorioId,
                ConsultorioNome = medico.Consultorio != null ? medico.Consultorio.Nome : "Consultorio não Existe"

            };

            return Ok(medicoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarMedico(int id, Medico medico)
        {
            if (id != medico.Id)
            {
                return BadRequest("ID do medico não corresponde ao ID da URL.");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var medicoExistente = await _context.Medicos.FindAsync(id);

            if (medicoExistente == null)
            {
                return NotFound();
            }

            medicoExistente.Nome = medico.Nome;
            medicoExistente.Crm = medico.Crm;
            medicoExistente.ConsultorioId = medico.ConsultorioId;

            _context.Update(medicoExistente);
            await _context.SaveChangesAsync();
            return Ok(medicoExistente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarMedico(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
