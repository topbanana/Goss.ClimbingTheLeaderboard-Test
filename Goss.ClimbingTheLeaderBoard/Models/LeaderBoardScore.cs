namespace Goss.ClimbingTheLeaderBoard.Models
{
    public class LeaderBoardScore
    {
        public LeaderBoardScore(int score, Player player)
        {
            Score = score;
            Player = player;
        }

        public int Score { get; }
        public Player Player { get; }
    }
}