using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Goss.ClimbingTheLeaderBoard.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Goss.ClimbingTheLeaderBoard
{
    public class Program
    {
        private readonly IFileConverter _fileConverter;
        private readonly ILeaderBoardCalculator _leaderBoardCalculator;

        public Program(IFileConverter fileConverter, ILeaderBoardCalculator leaderBoardCalculator)
        {
            _fileConverter = fileConverter ?? throw new ArgumentNullException(nameof(fileConverter));
            _leaderBoardCalculator =
                leaderBoardCalculator ?? throw new ArgumentNullException(nameof(leaderBoardCalculator));
        }

        public async Task<ResponseModel> Execute(string filePath)
        {
            var inputModel = await _fileConverter.Convert(filePath);
            return _leaderBoardCalculator.Calculate(inputModel);
        }

        private static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("Missing file name as argument.");
            }

            var provider = CreateServiceProvider();
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                args[0]);
            var results = await provider.GetService<Program>().Execute(filePath);
            await Console.Out.WriteLineAsync(results.ToString());
        }

        /// <summary>
        /// Register the dependencies for the application
        /// </summary>
        /// <returns>An <see cref="IServiceProvider"/> with the application's dependencies</returns>
        private static IServiceProvider CreateServiceProvider()
        {
            return new ServiceCollection()
                .AddScoped<Program>()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddScoped<IFileConverter, FileConverter>()
                .AddScoped<ILeaderBoardCalculator, LeaderBoardCalculator>()
                .BuildServiceProvider();
        }
    }
}