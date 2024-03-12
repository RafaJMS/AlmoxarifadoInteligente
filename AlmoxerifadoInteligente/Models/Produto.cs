﻿using System;
using System.Collections.Generic;

namespace AlmoxerifadoInteligente.Models
{
    public partial class Produto
    {
        public int IdProduto { get; set; }
        public string Descricao { get; set; } = null!;
        public decimal Preco { get; set; }
        public int EstoqueAtual { get; set; }
        public int EstoqueMinimo { get; set; }

    }
}
