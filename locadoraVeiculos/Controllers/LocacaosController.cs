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
    public class LocacoesController : ControllerBase
    {
        private readonly LocadoraContext _context;

        public LocacoesController(LocadoraContext context)
        {
            _context = context;
        }

        // GET: api/Locacaos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Locacao>>> GetLocacoes()
        {
            return await _context.Locacoes.ToListAsync();
        }

        // GET: api/Locacaos/5
        [HttpGet("LocacoesComDetalhesPorID/{id}")]
        public async Task<ActionResult<dynamic>> GetLocacaoComDetalhes(int id)
        {
            var query = await (from locacao in _context.Locacoes
                               join cliente in _context.Clientes on locacao.ClienteId equals cliente.ID
                               join veiculo in _context.Veiculos on locacao.VeiculoId equals veiculo.ID
                               where locacao.LocacaoId == id // Filtra pela locação com o ID especificado
                               select new
                               {
                                   LocacaoId = locacao.LocacaoId,
                                   DataLocacao = locacao.Data,
                                   ValorTotal = locacao.ValorTotal,
                                   ClienteNome = cliente.Nome,
                                   ClienteEmail = cliente.Email,
                                   VeiculoModelo = veiculo.Modelo,
                                   VeiculoPlaca = veiculo.Placa
                               }).FirstOrDefaultAsync(); // Usa FirstOrDefault para pegar um único registro

            if (query == null)
            {
                return NotFound(); // Retorna 404 se não encontrar a locação
            }

            return Ok(query); // Retorna a locação encontrada
        }
        [HttpGet("LocacoesComDetalhes")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetLocacoesComDetalhes()
        {
            var query = await (from locacao in _context.Locacoes
                               join cliente in _context.Clientes on locacao.ClienteId equals cliente.ID
                               join veiculo in _context.Veiculos on locacao.VeiculoId equals veiculo.ID
                               select new
                               {
                                   LocacaoId = locacao.LocacaoId,
                                   DataLocacao = locacao.Data,
                                   ValorTotal = locacao.ValorTotal,
                                   ClienteNome = cliente.Nome,
                                   ClienteEmail = cliente.Email,
                                   VeiculoModelo = veiculo.Modelo,
                                   VeiculoPlaca = veiculo.Placa
                               }).ToListAsync();

            return Ok(query);
        }

        // PUT: api/Locacaos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocacao(int id, Locacao locacao)
        {
            if (id != locacao.LocacaoId)
            {
                return BadRequest();
            }

            _context.Entry(locacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocacaoExists(id))
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

        // POST: api/Locacaos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Locacao>> PostLocacao(Locacao locacao)
        {
            // Verificar se o veículo existe
            var veiculo = await _context.Veiculos.FindAsync(locacao.VeiculoId);
            if (veiculo == null)
            {
                return NotFound($"Veículo com ID {locacao.VeiculoId} não encontrado.");
            }

            // Verificar se o cliente existe
            var cliente = await _context.Clientes.FindAsync(locacao.ClienteId);
            if (cliente == null)
            {
                return NotFound($"Cliente com ID {locacao.ClienteId} não encontrado.");
            }

            // Verificar se o funcionário existe
            var funcionario = await _context.Funcionarios.FindAsync(locacao.FuncionarioId);
            if (funcionario == null)
            {
                return NotFound($"Funcionário com ID {locacao.FuncionarioId} não encontrado.");
            }

            // Adicionar a locação ao contexto e salvar
            _context.Locacoes.Add(locacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocacaoComDetalhes), new { id = locacao.LocacaoId }, locacao);

        }



        // DELETE: api/Locacaos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocacao(int id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
            {
                return NotFound();
            }

            _context.Locacoes.Remove(locacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocacaoExists(int id)
        {
            return _context.Locacoes.Any(e => e.LocacaoId == id);
        }
        
    }
}
