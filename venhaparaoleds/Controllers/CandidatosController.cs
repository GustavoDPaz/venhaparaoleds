using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Venhaparaoleds.Data;
using venhaparaoleds.Models;

namespace venhaparaoleds.Controllers;

public class CandidatosController : Controller 
{
    private readonly ICandidatoMatchService _candidatoMatchService;
    private readonly AppDbContext _context;
    
    public CandidatosController(ICandidatoMatchService candidatoMatchService, AppDbContext context)
    {
        _candidatoMatchService = candidatoMatchService;
        _context = context;
    }
    
    [HttpGet]
    [Route("Candidatos/BuscarPorCpf")]
    public IActionResult BuscarPorCpf(string cpf)
    {
        if (string.IsNullOrEmpty(cpf)) 
            return View("~/Views/Home/Candidatos.cshtml", new List<Concurso>());

        // Chama o serviço para resolver o problema complexo de cruzamento de dados
        var matches = _candidatoMatchService.BuscarConcursosCompativeis(cpf);
        
        return View("~/Views/Home/Candidatos.cshtml", matches);
    }
    
    [HttpGet]
    [Route("Candidatos/listar")]
    public IActionResult Listar()
    {
        var candidato = _context.Candidatos.ToList();
            
        if (!candidato.Any())
        {
            return NotFound(new { mensagem = "Nenhum candidato cadastrado no banco de dados." });
        }

        return Ok(candidato);
    }

    [HttpPost]
    [Route("Candidatos/adicionar")]
    public IActionResult Criar(Candidato novoCandidato)
    {
        _context.Candidatos.Add(novoCandidato);
        _context.SaveChanges();

        return Ok("Usuário Criado com Sucesso!");
    }
    
    [HttpDelete]
    [Route("Candidatos/retirar/{id}")]
    public IActionResult Retirar(int id)
    {
        var candidatoAlvo = _context.Candidatos.Find(id);
            
        if (candidatoAlvo == null)
        {
            return NotFound("Candidato não encontrado.");
        }
            
        _context.Candidatos.Remove(candidatoAlvo);
        _context.SaveChanges();
        
        return Ok("Usuário Retirado com Sucesso!");
    }
}