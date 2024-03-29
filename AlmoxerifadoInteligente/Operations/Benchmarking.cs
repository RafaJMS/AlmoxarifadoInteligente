﻿using AlmoxerifadoInteligente.API.Scraps;
using AlmoxerifadoInteligente.Models;
using RaspagemMagMer.Scraps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemMagMer.Operations
{
    public class Benchmarking
    {
       
        public static List<object> CompareValue(string descricaoProduto, int idProduto)
        {
            List<object> data = new List<object>();
            char[] charRemove = { 'R', '$', ' ' };

            try
            {

                MercadoLivreScraper mercadoLivre = new();
                mercadoLivre.ObterData(descricaoProduto, idProduto);

                MagazineScraper magazineLuiza = new();
                magazineLuiza.ObterData(descricaoProduto,idProduto);


                decimal mercadoPreco = Convert.ToDecimal(mercadoLivre.Preco.Trim(charRemove));
                decimal magazinePreco = Convert.ToDecimal(magazineLuiza.Preco.Trim(charRemove));

                if (magazinePreco > mercadoPreco)

                {
                    decimal result = EconomiaOperation(magazinePreco, mercadoPreco);
                   
                    data.Add(Convert.ToString(result));
                    data.Add(mercadoLivre.Nome);
                    data.Add(mercadoLivre.Link);
                    data.Add(mercadoPreco);

                    BenchRegister.RegistrarBench(mercadoLivre.Nome, magazineLuiza.Nome, mercadoLivre.Link, magazineLuiza.Link, mercadoPreco, magazinePreco, result, idProduto);

                    LogRegister.RegistrarLog(DateTime.Now, "Benchmarking", "Sucesso", idProduto);

                    return data;


                }
                else if (magazinePreco < mercadoPreco)
                {
                    decimal result = EconomiaOperation(mercadoPreco, magazinePreco);
                    data.Add(Convert.ToString(result));
                    data.Add(magazineLuiza.Nome);
                    data.Add(magazineLuiza.Link);
                    data.Add(magazinePreco);

                    BenchRegister.RegistrarBench(mercadoLivre.Nome, magazineLuiza.Nome, mercadoLivre.Link, magazineLuiza.Link, mercadoPreco, magazinePreco, result, idProduto);

                    LogRegister.RegistrarLog(DateTime.Now, "Benchmarking", "Sucesso", idProduto);

                    return data;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogRegister.RegistrarLog(DateTime.Now, "Benchmarking", "Erro", idProduto);
                List<object> erro = new List<object>();
                erro.Add(ex.Message);
                Console.WriteLine(erro[0]);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return erro;
                

            }
        }
            
        public static decimal EconomiaOperation(decimal precoMaior, decimal precoMenor)
        {
            decimal result = precoMaior - precoMenor;
            return result;
        }
    }
}
