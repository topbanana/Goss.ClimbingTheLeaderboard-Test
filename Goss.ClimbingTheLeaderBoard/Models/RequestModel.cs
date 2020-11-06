using System;

namespace Goss.ClimbingTheLeaderBoard.Models
{
    public class RequestModel
    {
        public RequestModel(int numberOfPlayers, int[] leaderBoardScores, int playersGames, int[] playersScores)
        {
            NumberOfPlayers = numberOfPlayers;
            if (leaderBoardScores.Length != numberOfPlayers)
            {
                throw new ArgumentException(
                    $"Incorrect number of leader-board-scores, should be {numberOfPlayers} but was {leaderBoardScores.Length}.",
                    nameof(leaderBoardScores));
            }

            if (!IsSortedDescending(leaderBoardScores))
            {
                throw new ArgumentException("Leader-board scores are not in descending order.",
                    nameof(leaderBoardScores));
            }

            LeaderBoardScores = leaderBoardScores;

            PlayersGames = playersGames;
            if (playersScores.Length != playersGames)
            {
                throw new ArgumentException(
                    $"Incorrect number of players-scores, should be {playersGames} but was {playersScores.Length}.",
                    nameof(playersScores));
            }

            if (!IsSorted(playersScores))
            {
                throw new ArgumentException("Players-scores are not in ascending order.", nameof(playersScores));
            }

            PlayersScores = playersScores;
        }

        public int NumberOfPlayers { get; }
        public int[] LeaderBoardScores { get; }
        public int PlayersGames { get; }
        public int[] PlayersScores { get; }

        private static bool IsSorted(int[] arr)
        {
            for (var i = 1; i < arr.Length; i++)
            {
                if (arr[i - 1] > arr[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsSortedDescending(int[] arr)
        {
            for (var i = arr.Length - 2; i >= 0; i--)
            {
                if (arr[i] < arr[i + 1])
                {
                    return false;
                }
            }

            return true;
        }
    }
}