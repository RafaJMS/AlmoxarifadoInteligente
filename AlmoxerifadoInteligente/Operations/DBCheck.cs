﻿using AlmoxerifadoInteligente.API.Scraps;
using AlmoxerifadoInteligente.Models;
using Newtonsoft.Json;
using RaspagemMagMer.Scraps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemMagMer.Operations
{
    public class DBCheck
    {
        
        static List<Produto> produtosVerificados = new();
        public static async void VerificarNovoProduto()

        {
            string codUsu = LogRegister.CodRobo;
            string url = "https://localhost:7286/api/Produtos";

            try
            {

                using (HttpClient client = new HttpClient())
                {

                    HttpResponseMessage response = await client.GetAsync(url);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseData);
                        List<Produto> novosProdutos = ObterNovosProdutos(responseData);

                        foreach (Produto produto in novosProdutos)
                        {
                           
                            if (!produtosVerificados.Exists(p => p.IdProduto == produto.IdProduto))
                            {

                                Console.WriteLine($"Novo produto encontrado: ID {produto.IdProduto}, Nome: {produto.Descricao}\n");

                                produtosVerificados.Add(produto);

                                if (!ProdutoJaRegistrado(produto.IdProduto,codUsu))
                                {
                                    Benchmarking.CompareValue(produto.Descricao, produto.IdProduto);
                                    //LogRegister logRegister = new();
                                   
                                    //LogRegister.RegistrarLog( DateTime.Now, "ConsultaAPI - Verificar Produto", "Sucesso", produto.IdProduto);
                                   
                                    //LogRegister.RegistrarLog(DateTime.Now, "Benchmarking", "Sucesso", produto.IdProduto);

                                    //LogRegister.RegistrarLog(DateTime.Now, "Benchmarking", "Erro", produto.IdProduto);

                                    //bool responseEmail = SendEmail.EnviarEmail(email, produto.Descricao, magazineLuizaNome, magazineLuizaPreco, mercadoLivreNome, mercadoLivrePreco, responseBench);

                                    //if (responseEmail == true) LogRegister.RegistrarLog(DateTime.Now, "SendEmail", "Sucesso", produto.IdProduto);

                                    //else LogRegister.RegistrarLog(DateTime.Now, "SendEmail", "Erro", produto.IdProduto);

                                    //if (phoneNumber != null)
                                    //{
                                    //    SendMessage.EnviarMsg(produto.IdProduto, phoneNumber, produto.Descricao, magazineLuizaNome, magazineLuizaPreco, mercadoLivreNome, mercadoLivrePreco, responseBench);
                                    //}

                                }
                            }
                        }
                    }
                    else
                    {
                        // Imprimir mensagem de erro caso a requisição falhe
                        Console.WriteLine($"Erro: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro ao fazer a requisição: {ex.Message}");
            }
        }

        static List<Produto> ObterNovosProdutos(string responseData)
        {

            List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(responseData);
            return produtos;
        }

        static bool ProdutoJaRegistrado(int idProduto,string codRobo)
        {
            using (var context = new AlmoxarifadoDBContext())
            {
                return context.Logs.Any(log => log.IdProduto == idProduto && log.CodigoRobo == codRobo);
            }
        }
       
    }
}
