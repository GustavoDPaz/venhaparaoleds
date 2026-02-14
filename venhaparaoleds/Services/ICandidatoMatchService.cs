
using venhaparaoleds.Models;

public interface ICandidatoMatchService
{
    List<Concurso> BuscarConcursosCompativeis(string cpf);
}