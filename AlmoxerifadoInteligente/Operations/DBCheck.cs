using AlmoxerifadoInteligente.API.Scraps;
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
        public static async void VerificarNovoProduto(string num,string emailAdress)

        {
            string codUsu = LogRegister.CodRobo;
            string email = emailAdress;
            string phoneNumber = num;
            string url = "https://localhost:7286/api/Produtos";

            try
            {

                using (HttpClient client = new HttpClient())
                {

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();

                        List<Produto> novosProdutos = ObterNovosProdutos(responseData);

                        foreach (Produto produto in novosProdutos)
                        {
                           
                            if (!produtosVerificados.Exists(p => p.IdProduto == produto.IdProduto))
                            {

                                Console.WriteLine($"Novo produto encontrado: ID {produto.IdProduto}, Nome: {produto.Descricao}\n");

                                produtosVerificados.Add(produto);

                                if (!ProdutoJaRegistrado(produto.IdProduto,codUsu))
                                {
                                    LogRegister logRegister = new();
                                    LogRegister.RegistrarLog( DateTime.Now, "ConsultaAPI - Verificar Produto", "Sucesso", produto.IdProduto);

                                    MercadoLivreScraper mercadoLivreScraper = new();

                                    mercadoLivreScraper.ObterData(produto.Descricao, produto.IdProduto);

                                    string mercadoLivrePreco = mercadoLivreScraper.ObterPreco(produto.Descricao, produto.IdProduto);

                                    string mercadoLivreNome = mercadoLivreScraper.ObterNome(produto.Descricao);

                                    string mercadoLivreLink = mercadoLivreScraper.ObterLink(produto.Descricao);

                                    MagazineScraper magazineLuizaScraper = new();

                                    string magazineLuizaPreco = magazineLuizaScraper.ObterPreco(produto.Descricao, produto.IdProduto);

                                    string magazineLuizaNome = magazineLuizaScraper.ObterNome(produto.Descricao);

                                    string magazineLuizaLink = magazineLuizaScraper.ObterLink(produto.Descricao);

                                    string responseBench = Benchmarking.CompareValue(magazineLuizaPreco, mercadoLivrePreco, mercadoLivreLink, magazineLuizaLink);

                                    if (responseBench != null) LogRegister.RegistrarLog(DateTime.Now, "Benchmarking", "Sucesso", produto.IdProduto);

                                    else LogRegister.RegistrarLog(DateTime.Now, "Benchmarking", "Erro", produto.IdProduto);

                                    bool responseEmail = SendEmail.EnviarEmail(email, produto.Descricao, magazineLuizaNome, magazineLuizaPreco, mercadoLivreNome, mercadoLivrePreco, responseBench);

                                    if (responseEmail == true) LogRegister.RegistrarLog(DateTime.Now, "SendEmail", "Sucesso", produto.IdProduto);

                                    else LogRegister.RegistrarLog(DateTime.Now, "SendEmail", "Erro", produto.IdProduto);

                                    if (phoneNumber != null)
                                    {
                                        SendMessage.EnviarMsg(produto.IdProduto, phoneNumber, produto.Descricao, magazineLuizaNome, magazineLuizaPreco, mercadoLivreNome, mercadoLivrePreco, responseBench);
                                    }

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
