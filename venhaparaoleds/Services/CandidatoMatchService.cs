using Venhaparaoleds.Data;
using venhaparaoleds.Models;

public class CandidatoMatchService : ICandidatoMatchService
{
    private readonly AppDbContext _context;

    public CandidatoMatchService(AppDbContext context)
    {
        _context = context;
    }

    public List<Concurso> BuscarConcursosCompativeis(string cpf)
    {
        var candidato = _context.Candidatos.FirstOrDefault(c => c.CPF == cpf);
        
        if (candidato == null) return new List<Concurso>();
        
        return _context.Concursos.ToList()
            .Where(concurso =>
                concurso.Vagas.Any(vaga => 
                    candidato.Profissoes.Any(prof => 
                        prof.Trim().Equals(vaga.Trim(), StringComparison.OrdinalIgnoreCase)
                    )
                )
            ).ToList();
    }
}