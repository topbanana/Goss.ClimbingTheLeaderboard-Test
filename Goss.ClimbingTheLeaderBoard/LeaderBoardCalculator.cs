using System.Linq;
using Goss.ClimbingTheLeaderBoard.Models;

namespace Goss.ClimbingTheLeaderBoard
{
    /// <inheritdoc />
    public class LeaderBoardCalculator : ILeaderBoardCalculator
    {
        /// <inheritdoc />
        public ResponseModel Calculate(RequestModel request)
        {
            var playerPositions = new int[request.PlayersGames];
            var leaderBoard = request.LeaderBoardScores.Distinct().OrderByDescending(x => x).ToArray();
            for (var gameNumber = request.PlayersGames; gameNumber > 0; gameNumber--)
            {
                var position = 0;
                while (++position <= leaderBoard.Length)
                {
                    if (request.PlayersScores[gameNumber - 1] >= leaderBoard[position - 1])
                    {
                        playerPositions[gameNumber - 1] = position;
                        break;
                    }
                }

                if (playerPositions[gameNumber - 1] == 0)
                {
                    playerPositions[gameNumber - 1] = leaderBoard.Length + 1;
                }
            }

            return new ResponseModel(playerPositions);
        }
    }
}