using System.Threading.Tasks;
using Goss.ClimbingTheLeaderBoard.Models;

namespace Goss.ClimbingTheLeaderBoard
{
    /// <summary>
    /// Calculates the players positions in the leader-board
    /// </summary>
    public interface ILeaderBoardCalculator
    {
        /// <summary>
        /// Calculates the player's position on the leader-board
        /// </summary>
        /// <param name="request">The model with all the required information to calculate the player's position</param>
        /// <returns>The players positions on the leader board</returns>
        Task<ResponseModel> Calculate(RequestModel request);
    }
}