using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Venhaparaoleds.Data;
using venhaparaoleds.Models;

namespace venhaparaoleds.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas aos Candidatos,
/// incluindo cadastro, listagem, remoção e busca de concursos compatíveis.
/// </summary>
public class CandidatosController : Controller 
{
    private readonly ICandidatoMatchService _candidatoMatchService;
    private readonly AppDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="CandidatosController"/>.
    /// </summary>
    /// <param name="candidatoMatchService">Serviço de lógica para match entre candidatos e concursos.</param>
    /// <param name="context">Contexto do banco de dados SQLite.</param>
    public CandidatosController(ICandidatoMatchService candidatoMatchService, AppDbContext context)
    {
        _candidatoMatchService = candidatoMatchService;
        _context = context;
    }
    
    /// <summary>
    /// Busca concursos compatíveis com as profissões de um candidato específico através do CPF.
    /// </summary>
    /// <param name="cpf">O CPF do candidato a ser pesquisado.</param>
    /// <returns>Uma View contendo a lista de concursos que deram match com o perfil do candidato.</returns>
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

    /// <summary>
    /// Recupera a lista completa de candidatos registrados no sistema.
    /// </summary>
    /// <returns>Retorna um JSON com todos os candidatos ou uma mensagem de erro caso o banco esteja vazio.</returns>
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

    /// <summary>
    /// Realiza o cadastro de um novo candidato no banco de dados.
    /// </summary>
    /// <param name="novoCandidato">Objeto contendo os dados do candidato a ser inserido.</param>
    /// <returns>Uma mensagem de confirmação de criação com sucesso.</returns>
    [HttpPost]
    [Route("Candidatos/adicionar")]
    public IActionResult Criar(Candidato novoCandidato)
    {
        _context.Candidatos.Add(novoCandidato);
        _context.SaveChanges();

        return Ok("Usuário Criado com Sucesso!");
    }

    /// <summary>
    /// Remove um candidato do sistema com base no seu identificador único (ID).
    /// </summary>
    /// <param name="id">ID do candidato a ser removido.</param>
    /// <returns>Mensagem de sucesso ou erro 404 caso o candidato não seja localizado.</returns>
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