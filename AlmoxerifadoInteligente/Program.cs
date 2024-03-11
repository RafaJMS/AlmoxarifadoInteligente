using AlmoxerifadoInteligente.API;
using AlmoxerifadoInteligente.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using RaspagemMagMer.Operations;

class Program
{
    public static void Main(string[] args)
    {
        
        string email = SendEmail.OpcaoEmail();
        while (email == null)
        {
            Console.WriteLine("Email Inválido, tente novamente!");
            email = SendEmail.OpcaoEmail();

        }
        string phoneNumber = SendMessage.OpcaoMsg();

        DBCheck.VerificarNovoProduto(phoneNumber, email);

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<ApiStarter>(); 
            });
}

