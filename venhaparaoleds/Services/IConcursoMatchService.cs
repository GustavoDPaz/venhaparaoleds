using venhaparaoleds.Models;

public interface IConcursoMatchService
{
    List<Candidato> BuscarCandidatosCompativeis(string codigo);
}