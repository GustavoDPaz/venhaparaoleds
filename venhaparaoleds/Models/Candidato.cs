namespace venhaparaoleds.Models
{
    public class Candidato
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string DataNascimento { get; set; }
        public required string CPF { get; set; }
        public List<string> Profissoes { get; set; } = [];
    }
}
