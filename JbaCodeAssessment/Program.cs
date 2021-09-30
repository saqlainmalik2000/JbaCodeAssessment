using JbaCodeAssessment.Data;
using JbaCodeAssessment.Services.Implementations;
using JbaCodeAssessment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JbaCodeAssessment
{
    class Program
    {
        private const string PromptString = "Enter file path to import, or [Exit] to exit program:";

        static void Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            Console.WriteLine("JBA Code Assessment - Saqlain Malik");
            Console.WriteLine(PromptString);
            var response = Console.ReadLine();


            while (response.ToUpper() != "EXIT")
            {
                _ = Task.Run(() => ImportFile(host, response));
                Console.WriteLine(PromptString);
                response = Console.ReadLine();
            }
        }

        private static async Task ImportFile(IHost host, string filePath)
        {
            //using var fileManager = new FileManager();
            var fileManager = ActivatorUtilities.CreateInstance<FileManager>(host.Services);

            try
            {
                Console.WriteLine($"Importing file: {filePath}");
                var result = await fileManager.ProcessAsync(filePath);
                Console.WriteLine(result.HeaderText);
                Console.WriteLine($"File import complete");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"INFORMATION: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR ENCOUNTERED: {ex.Message}");
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddTransient<IFileManager, FileManager>();
                services.AddDbContext<JbaContext>(options => options.UseInMemoryDatabase("JbaDb"));
            });
            
    }
}
