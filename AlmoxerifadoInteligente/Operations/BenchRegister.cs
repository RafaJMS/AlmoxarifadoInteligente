using AlmoxerifadoInteligente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemMagMer.Operations
{
    public class BenchRegister
    {

        public static void RegistrarBench(string nomeMer, string nomeMag ,decimal precoMer, decimal precoMag, decimal economia, int idProd)
        {
            using var context = new AlmoxarifadoDBContext();
            var benchmarkitem = new BenchmarkingItem
            {
                NomeLoja1 = nomeMer,
                NomeLoja2 = nomeMag,
                PrecoLoja1 = precoMer,
                PrecoLoja2 = precoMag,
                Email = "aleatorio",
                Economia = economia,
                IdProduto = idProd
            };
            context.BenchmarkingItem.Add(benchmarkitem);
            context.SaveChanges();
        }
    }
}