using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using locadoraVeiculos.Models;

namespace locadoraVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManutencaosController : ControllerBase
    {
        private readonly LocadoraContext _context;

        public ManutencaosController(LocadoraContext context)
        {
            _context = context;
        }

        // GET: api/Manutencaos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manutencao>>> GetManutencoes()
        {
            return await _context.Manutencoes.ToListAsync();
        }

        // GET: api/Manutencaos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Manutencao>> GetManutencao(int id)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);

            if (manutencao == null)
            {
                return NotFound();
            }

            return manutencao;
        }

        // PUT: api/Manutencaos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutManutencao(int id, Manutencao manutencao)
        {
            if (id != manutencao.ManutencaoId)
            {
                return BadRequest();
            }

            _context.Entry(manutencao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManutencaoExists(id))
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

        // POST: api/Manutencaos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Manutencao>> PostManutencao(Manutencao manutencao)
        {
            _context.Manutencoes.Add(manutencao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetManutencao", new { id = manutencao.ManutencaoId }, manutencao);
        }

        // DELETE: api/Manutencaos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManutencao(int id)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null)
            {
                return NotFound();
            }

            _context.Manutencoes.Remove(manutencao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ManutencaoExists(int id)
        {
            return _context.Manutencoes.Any(e => e.ManutencaoId == id);
        }
    }
}
