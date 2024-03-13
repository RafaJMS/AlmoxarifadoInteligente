﻿using System;
using System.Collections.Generic;

namespace AlmoxerifadoInteligente.Models
{
    public partial class BenchmarkingItem
    {
        public int Id { get; set; }
        public string NomeLoja1 { get; set; } = null!;
        public string NomeLoja2 { get; set; } = null!;
        public decimal PrecoLoja1 { get; set; }
        public decimal PrecoLoja2 { get; set; }
        public decimal Economia { get; set; }
        public string Email { get; set; } = null!;
        public int IdProduto { get; set; }
        public int? IdProdutoNavigationIdProduto { get; set; }

        public virtual Produto? IdProdutoNavigationIdProdutoNavigation { get; set; }
    }
}
