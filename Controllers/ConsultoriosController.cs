using Consultorio.Api.Data;
using Consultorio.Api.Models;
using Consultorio.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consultorio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultoriosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ViaCepService _viaCep;

        public ConsultoriosController(AppDbContext context, ViaCepService viaCepService)
        {
            _context = context;
            _viaCep = viaCepService;

        }

        [HttpGet]
        public async Task<IActionResult> ListarTodosConsultorios()
        {
            var consultorios = await _context.Consultorios.ToListAsync();
            return Ok(consultorios);
        }

        [HttpPost]
        public async Task<IActionResult> CriarConsultorio(Models.Consultorio consultorio)
        {
            if (string.IsNullOrEmpty(consultorio.Cep) || consultorio.Cep.Length != 8)
            {
                return BadRequest("O campo CEP é obrigatório.");
            }

            else
            {
                var endereco = await _viaCep.ObterEnderecoPorCepAsync(consultorio.Cep);
                consultorio.Logradouro = endereco.logradouro;
                consultorio.Bairro = endereco.bairro;
                consultorio.Localidade = endereco.localidade;
                consultorio.Uf = endereco.uf;
            }
            _context.Consultorios.Add(consultorio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarTodosConsultorios), new { id = consultorio.Id }, consultorio);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ListarConsultorioPorId(int id)
        {
            var consultorio = await _context.Consultorios.FindAsync(id);
            if (consultorio == null)
            {
                return NotFound();
            }
            return Ok(consultorio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarConsultorio(int id, Models.Consultorio consultorio)
        {
            if (id != consultorio.Id)
            {
                return BadRequest("ID do consultorio não corresponde ao ID da URL.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var consultorioExistente = await _context.Consultorios.FindAsync(id);

            if (consultorioExistente == null)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(consultorio.Cep) || consultorio.Cep.Length != 8)
            {
                return BadRequest("O campo CEP é obrigatório.");
            }
            else
            {
                var endereco = await _viaCep.ObterEnderecoPorCepAsync(consultorio.Cep);
                consultorioExistente.Logradouro = endereco.logradouro;
                consultorioExistente.Bairro = endereco.bairro;
                consultorioExistente.Localidade = endereco.localidade;
                consultorioExistente.Uf = endereco.uf;
            }
            consultorioExistente.Nome = consultorio.Nome;
            consultorioExistente.Numero = consultorio.Numero;

            _context.Update(consultorioExistente);
            await _context.SaveChangesAsync();
            return Ok(consultorioExistente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarConsultorio(int id)
        {
            var consultorio = await _context.Consultorios.FindAsync(id);
            if (consultorio == null)
            {
                return NotFound();
            }
            _context.Consultorios.Remove(consultorio);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
