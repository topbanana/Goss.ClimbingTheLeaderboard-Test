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
            var leaderBoardScores = new LeaderBoardScore[request.NumberOfPlayers + request.PlayersGames];
            var index = 0;
            foreach (var score in request.LeaderBoardScores)
            {
                leaderBoardScores[index++] = new LeaderBoardScore(score, Player.Existing);
            }

            foreach (var score in request.PlayersScores)
            {
                leaderBoardScores[index++] = new LeaderBoardScore(score, Player.New);
            }

            var groupedScores = leaderBoardScores.GroupBy(x => x.Score).OrderByDescending(x => x.Key);
            var playerPositions = new int[request.PlayersGames];
            index = 0;
            var position = 1;
            foreach (var groupedScore in groupedScores)
            {
                if (groupedScore.Any(x => x.Player == Player.New))
                {
                    playerPositions[index++] = position;
                }

                position++;
            }

            return new ResponseModel(playerPositions.ToArray());
        }
    }
}