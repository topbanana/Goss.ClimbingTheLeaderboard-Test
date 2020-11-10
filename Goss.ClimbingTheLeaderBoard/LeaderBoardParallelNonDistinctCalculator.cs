using System.Threading.Tasks;
using Goss.ClimbingTheLeaderBoard.Models;

namespace Goss.ClimbingTheLeaderBoard
{
    public class LeaderBoardParallelNonDistinctCalculator : ILeaderBoardCalculator
    {
        /// <inheritdoc />
        public Task<ResponseModel> Calculate(RequestModel request)
        {
            var playerPositions = new int[request.PlayersGames];
            Parallel.For(0, request.PlayersGames, index =>
            {
                var position = 0;
                var leaderBoardScoresIndex = 0;
                var lastLeaderBoardScore = 0;
                while (leaderBoardScoresIndex < request.LeaderBoardScores.Length)
                {
                    if (lastLeaderBoardScore != request.LeaderBoardScores[leaderBoardScoresIndex])
                    {
                        position++;
                        lastLeaderBoardScore = request.LeaderBoardScores[leaderBoardScoresIndex];
                    }

                    leaderBoardScoresIndex++;
                    if (request.PlayersScores[index] < lastLeaderBoardScore)
                    {
                        continue;
                    }

                    playerPositions[index] = position;
                    break;
                }

                if (playerPositions[index] == 0)
                {
                    playerPositions[index] = position + 1;
                }
            });

            return Task.FromResult(new ResponseModel(playerPositions));
        }
    }
}