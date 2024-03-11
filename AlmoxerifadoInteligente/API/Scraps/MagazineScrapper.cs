using HtmlAgilityPack;
using RaspagemMagMer.Operations;

namespace AlmoxerifadoInteligente.API.Scraps
{
    public class MagazineScraper
    {
        public string ObterPreco(string descricaoProduto, int idProduto)
        {
            string url = $"https://www.magazineluiza.com.br/busca/{descricaoProduto}";

            try
            {

                HtmlWeb web = new HtmlWeb();


                HtmlDocument document = web.Load(url);


                HtmlNode firstProductPriceNode = document.DocumentNode.SelectSingleNode("//p[@class='sc-kpDqfm eCPtRw sc-bOhtcR dOwMgM']");


                if (firstProductPriceNode != null)
                {

                    string firstProductPrice = firstProductPriceNode.InnerText.Trim();

                    LogRegister.RegistrarLog(DateTime.Now, "WebScraping - Magazine Luiza", "Sucesso", idProduto);

                    return firstProductPrice;
                }
                else
                {
                    Console.WriteLine("Preço não encontrado.");

                    LogRegister.RegistrarLog(DateTime.Now, "WebScraping - Magazine Luiza", "Preço não encontrado", idProduto);

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

                LogRegister.RegistrarLog(DateTime.Now, "WebScraping - Magazine Luiza", $"Erro: {ex.Message}", idProduto);

                return null;
            }
        }

        public string ObterNome(string descricaoProduto)
        {
            string url = $"https://www.magazineluiza.com.br/busca/{descricaoProduto}";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(url);
            HtmlNode firstProductPriceName = document.DocumentNode.SelectSingleNode("//h2[@class='sc-fvwjDU fbccdO']");

            if (firstProductPriceName != null)
            {
                string firstProductName = firstProductPriceName.InnerText.Trim();
                return firstProductName;
            }
            else
            {
                return null;
            }
        }

        public string ObterLink(string descricaoProduto)
        {
            string url = $"https://lista.mercadolivre.com.br/{descricaoProduto.Replace(' ', '+')}";
            return url;
        }
    }
}