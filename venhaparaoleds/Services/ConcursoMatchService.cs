using Venhaparaoleds.Data;
using venhaparaoleds.Models;

public class ConcursoMatchService : IConcursoMatchService
{
    private readonly AppDbContext _context;

    public ConcursoMatchService(AppDbContext context)
    {
        _context = context;
    }

    public List<Candidato> BuscarCandidatosCompativeis(string codigo)
    {
        var concurso = _context.Concursos.FirstOrDefault(c => c.Codigo == codigo);

        if (concurso == null) return new List<Candidato>();
        
        return _context.Candidatos.ToList()
            .Where(candidato =>
                candidato.Profissoes.Any(prof =>
                    concurso.Vagas.Any(vaga =>
                        vaga.Trim().Equals(prof.Trim(), StringComparison.OrdinalIgnoreCase)
                    )
                )
            ).ToList();
    }
}