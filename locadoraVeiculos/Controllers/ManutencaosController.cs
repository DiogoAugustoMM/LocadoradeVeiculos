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
    public class ManutencoesController : ControllerBase
    {
        private readonly LocadoraContext _context;

        public ManutencoesController(LocadoraContext context)
        {
            _context = context;
        }


        // GET: api/Manutencaos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetManutencaoComVeiculo(int id)
        {
            var query = await (from manutencao in _context.Manutencoes
                               join veiculo in _context.Veiculos on manutencao.VeiculoId equals veiculo.ID into veiculosJoin
                               from veiculo in veiculosJoin.DefaultIfEmpty()
                               where manutencao.ManutencaoId == id // Filtra pela manutenção com o ID especificado
                               select new
                               {
                                   ManutencaoId = manutencao.ManutencaoId,
                                   DataManutencao = manutencao.Data,
                                   CustoManutencao = manutencao.Custo,
                                   Descricao = manutencao.DescricaoServico,
                                   VeiculoModelo = veiculo != null ? veiculo.Modelo : "Desconhecido",
                                   VeiculoPlaca = veiculo != null ? veiculo.Placa : "Desconhecido"
                               }).FirstOrDefaultAsync(); // Usa FirstOrDefault para pegar um único registro

            if (query == null)
            {
                return NotFound(); // Retorna 404 se não encontrar a manutenção
            }

            return Ok(query); // Retorna a manutenção encontrada
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetManutencoesComVeiculos()
        {
            var query = await (from manutencao in _context.Manutencoes
                               join veiculo in _context.Veiculos on manutencao.VeiculoId equals veiculo.ID into veiculosJoin
                               from veiculo in veiculosJoin.DefaultIfEmpty()
                               select new
                               {
                                   ManutencaoId = manutencao.ManutencaoId,
                                   DataManutencao = manutencao.Data,
                                   CustoManutencao = manutencao.Custo,
                                   Descricao = manutencao.DescricaoServico,
                                   VeiculoModelo = veiculo != null ? veiculo.Modelo : "Desconhecido",
                                   VeiculoPlaca = veiculo != null ? veiculo.Placa : "Desconhecido"
                               }).ToListAsync();

            return Ok(query);
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
            if (_context.Veiculos.Find(manutencao.VeiculoId) == null)
            {
                return NotFound($"Veículo com ID {manutencao.VeiculoId} não encontrado.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Manutencoes.Add(manutencao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetManutencaoComVeiculo), new { id = manutencao.ManutencaoId }, manutencao);
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
