using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Venhaparaoleds.Data;
using venhaparaoleds.Models;

namespace venhaparaoleds.Controllers;

public class ConcursosController : Controller
{
    private readonly IConcursoMatchService _concursoMatchService;
    private readonly AppDbContext _context;
    
    public ConcursosController(AppDbContext context, IConcursoMatchService concursoMatchService)
    {
        _concursoMatchService = concursoMatchService;
        _context = context;
    }


    [HttpGet]
    [Route("Concursos/BuscarPorCodigo")]
    public IActionResult BuscarPorCodigo(string codigo)
    {
        if (string.IsNullOrEmpty(codigo))
            return View("~/Views/Home/Concursos.cshtml", new List<Candidato>());

   
        var matches = _concursoMatchService.BuscarCandidatosCompativeis(codigo);

        return View("~/Views/Home/Concursos.cshtml", matches);
    }
    
    [HttpGet]
    [Route("Concursos/listar")]
    public IActionResult Listar()
    {
        var concursos = _context.Concursos.ToList();

        return Ok(concursos);
    }
    
    [HttpPost]
    [Route("Concursos/Criar")]
    public IActionResult Criar(Concurso novoConcurso)
    {
        _context.Concursos.Add(novoConcurso);
        _context.SaveChanges();
        
        return Ok("Concurso cadastrado com sucesso!");
    }
    
    [HttpDelete]
    [Route("Concursos/retirar/{id}")]
    public IActionResult Retirar(int id)
    {
        var concursoAlvo = _context.Concursos.Find(id);

        if (concursoAlvo == null)
        {
            return NotFound("Concurso não encontrado.");
        }

        _context.Concursos.Remove(concursoAlvo);
        _context.SaveChanges();
        
        return Ok("Concurso Retirado com Sucesso!");
    }
}