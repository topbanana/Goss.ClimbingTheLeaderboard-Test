using System.Linq;
using System.Threading.Tasks;
using Goss.ClimbingTheLeaderBoard.Models;

namespace Goss.ClimbingTheLeaderBoard
{
    public class LeaderBoardTaskCalculator : ILeaderBoardCalculator
    {
        /// <inheritdoc />
        public async Task<ResponseModel> Calculate(RequestModel request)
        {
            var leaderBoard = request.LeaderBoardScores.Distinct().OrderByDescending(x => x).ToArray();
            var tasks = request.PlayersScores.Select(x => CalculateGamePosition(x, leaderBoard));
            var playerPositions = await Task.WhenAll(tasks);

            return new ResponseModel(playerPositions);
        }

        private Task<int> CalculateGamePosition(int playerScore, int[] leaderBoard)
        {
            return Task.Run(() =>
            {
                var position = 0;
                var playerPosition = leaderBoard.Length + 1;
                while (++position <= leaderBoard.Length)
                {
                    if (playerScore < leaderBoard[position - 1])
                    {
                        continue;
                    }

                    playerPosition = position;
                    break;
                }

                return playerPosition;
            });
        }
    }
}