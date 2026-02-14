using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Venhaparaoleds.Data;
using venhaparaoleds.Models;

namespace venhaparaoleds.Controllers;

/// <summary>
/// Controller responsável pela gestão de Concursos Públicos, 
/// incluindo a criação, exclusão e a lógica de busca de candidatos qualificados.
/// </summary>
public class ConcursosController : Controller
{
    private readonly IConcursoMatchService _concursoMatchService;
    private readonly AppDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ConcursosController"/> com as dependências injetadas.
    /// </summary>
    /// <param name="context">Instância do banco de dados (AppDbContext).</param>
    /// <param name="concursoMatchService">Serviço especializado na lógica de cruzamento de dados de concursos.</param>
    public ConcursosController(AppDbContext context, IConcursoMatchService concursoMatchService)
    {
        _concursoMatchService = concursoMatchService;
        _context = context;
    }

    /// <summary>
    /// Pesquisa candidatos cujas profissões são compatíveis com as vagas de um concurso específico através do seu código.
    /// </summary>
    /// <param name="codigo">O código identificador único do concurso (ex: 1234).</param>
    /// <returns>Uma View com a lista de <see cref="Candidato"/> que atendem aos requisitos do edital.</returns>
    [HttpGet]
    [Route("Concursos/BuscarPorCodigo")]
    public IActionResult BuscarPorCodigo(string codigo)
    {
        if (string.IsNullOrEmpty(codigo))
            return View("~/Views/Home/Concursos.cshtml", new List<Candidato>());

        // O serviço encapsula a lógica complexa para manter o controller dentro dos padrões de Clean Code
        var matches = _concursoMatchService.BuscarCandidatosCompativeis(codigo);

        return View("~/Views/Home/Concursos.cshtml", matches);
    }

    /// <summary>
    /// Recupera a coleção completa de concursos registrados no banco de dados.
    /// </summary>
    /// <returns>Um objeto JSON contendo a lista de concursos.</returns>
    [HttpGet]
    [Route("Concursos/listar")]
    public IActionResult Listar()
    {
        var concursos = _context.Concursos.ToList();

        return Ok(concursos);
    }

    /// <summary>
    /// Registra um novo concurso no sistema.
    /// </summary>
    /// <param name="novoConcurso">Objeto contendo os dados do concurso (Órgão, Edital, Código e Vagas).</param>
    /// <returns>Mensagem de confirmação de sucesso no cadastro.</returns>
    [HttpPost]
    [Route("Concursos/Criar")]
    public IActionResult Criar(Concurso novoConcurso)
    {
        _context.Concursos.Add(novoConcurso);
        _context.SaveChanges();
        
        return Ok("Concurso cadastrado com sucesso!");
    }

    /// <summary>
    /// Remove um concurso do banco de dados através do seu identificador numérico (ID).
    /// </summary>
    /// <param name="id">O ID do concurso a ser removido.</param>
    /// <returns>Resposta de sucesso ou erro 404 caso o concurso não seja localizado no banco.</returns>
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