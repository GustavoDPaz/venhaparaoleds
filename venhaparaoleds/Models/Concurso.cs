namespace venhaparaoleds.Models
{
    public class Concurso
    {
        public int Id { get; set; }
        public required string Orgao { get; set; }
        public required string Edital { get; set; }
        public required string Codigo { get; set; }
        public List<string> Vagas { get; set; } = [];

    }
}
