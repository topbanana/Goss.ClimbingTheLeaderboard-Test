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
                var foundPosition = false;
                var position = 1;
                while (position <= leaderBoard.Length && !foundPosition)
                {
                    if (request.PlayersScores[gameNumber - 1] >= leaderBoard[position - 1])
                    {
                        playerPositions[gameNumber - 1] = position;
                        foundPosition = true;
                    }
                    else
                    {
                        position++;
                    }
                }

                if (!foundPosition)
                {
                    playerPositions[gameNumber - 1] = leaderBoard.Length + 1;
                }
            }

            return new ResponseModel(playerPositions);
        }
    }
}