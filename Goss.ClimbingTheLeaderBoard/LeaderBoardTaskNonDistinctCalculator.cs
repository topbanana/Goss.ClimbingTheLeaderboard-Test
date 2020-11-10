using System.Linq;
using System.Threading.Tasks;
using Goss.ClimbingTheLeaderBoard.Models;

namespace Goss.ClimbingTheLeaderBoard
{
    /// <inheritdoc />
    public class LeaderBoardTaskNonDistinctCalculator : ILeaderBoardCalculator
    {
        /// <inheritdoc />
        public async Task<ResponseModel> Calculate(RequestModel request)
        {
            var tasks = request.PlayersScores.Select(x => CalculateGamePosition(x, request.LeaderBoardScores));
            var playerPositions = await Task.WhenAll(tasks);

            return new ResponseModel(playerPositions);
        }

        private Task<int> CalculateGamePosition(int playerScore, int[] leaderBoardScores)
        {
            return Task.Run(() =>
            {
                var playerPosition = 0;

                var position = 0;
                var leaderBoardScoresIndex = 0;
                var lastLeaderBoardScore = 0;
                while (leaderBoardScoresIndex < leaderBoardScores.Length)
                {
                    if (lastLeaderBoardScore != leaderBoardScores[leaderBoardScoresIndex])
                    {
                        position++;
                        lastLeaderBoardScore = leaderBoardScores[leaderBoardScoresIndex];
                    }

                    leaderBoardScoresIndex++;
                    if (playerScore < lastLeaderBoardScore)
                    {
                        continue;
                    }

                    playerPosition = position;
                    break;
                }

                if (playerPosition == 0)
                {
                    playerPosition = position + 1;
                }

                return playerPosition;
            });
        }
    }
}