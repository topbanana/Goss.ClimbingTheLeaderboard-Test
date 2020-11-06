using System;
using System.Linq;
using System.Threading.Tasks;
using Goss.ClimbingTheLeaderBoard.Models;

namespace Goss.ClimbingTheLeaderBoard
{
    /// <inheritdoc />
    public class FileConverter : IFileConverter
    {
        private readonly IFileSystem _fileSystem;

        public FileConverter(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<RequestModel> Convert(string filePath)
        {
            var lines = await _fileSystem.ReadFile(filePath);
            if (lines.Length != 4)
            {
                throw new InvalidOperationException(
                    $"File {filePath} does not have 4 lines, but has {lines.Length} lines.");
            }

            var numberOfPlayers = System.Convert.ToInt32(lines.First());
            var leaderBoardScores =
                lines.Skip(1).Take(1).First().Split(" ").Select(x =>
                {
                    if (!int.TryParse(x, out var result))
                    {
                        throw new InvalidOperationException($"Non-numeric value in leader-board record '{x}'.");
                    }
                    return result;
                });
            var playersGames = System.Convert.ToInt32(lines.Skip(2).Take(1).First());
            var playersScores =
                lines.Skip(3).Take(1).First().Split(" ").Select(x =>
                {
                    if (!int.TryParse(x, out var result))
                    {
                        throw new InvalidOperationException($"Non-numeric value in players-scores record '{x}'.");
                    }
                    return result;
                });

            return new RequestModel(
                numberOfPlayers, 
                leaderBoardScores.ToArray(), 
                playersGames,
                playersScores.ToArray());
        }
    }
}