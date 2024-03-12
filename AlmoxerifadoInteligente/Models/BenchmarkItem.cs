namespace AlmoxerifadoInteligente.Models
{
    public class BenchmarkingItem
    {
        public int Id { get; set; }
        public string NomeLoja1 { get; set; }
        public string NomeLoja2 { get; set; }
        public decimal PrecoLoja1 { get; set; }
        public decimal PrecoLoja2 { get; set; }
        public decimal Economia { get; set; }
        public string Email { get; set; }
        public int IdProduto { get; set; }

        public virtual Produto? IdProdutoNavigation { get; set; }

    }
}
