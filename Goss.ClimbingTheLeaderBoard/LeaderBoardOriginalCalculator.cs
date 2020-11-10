using System.Linq;
using System.Threading.Tasks;
using Goss.ClimbingTheLeaderBoard.Models;

namespace Goss.ClimbingTheLeaderBoard
{
    /// <inheritdoc />
    public class LeaderBoardOriginalCalculator : ILeaderBoardCalculator
    {
        /// <inheritdoc />
        public Task<ResponseModel> Calculate(RequestModel request)
        {
            var playerPositions = new int[request.PlayersGames];
            var leaderBoard = request.LeaderBoardScores.Distinct().OrderByDescending(x => x).ToArray();
            for (var index = 0; index < request.PlayersGames; index++)
            {
                var position = 0;
                playerPositions[index] = leaderBoard.Length + 1;
                while (++position <= leaderBoard.Length)
                {
                    if (request.PlayersScores[index] < leaderBoard[position - 1])
                    {
                        continue;
                    }

                    playerPositions[index] = position;
                    break;
                }
            }

            return Task.FromResult(new ResponseModel(playerPositions));
        }
    }
}