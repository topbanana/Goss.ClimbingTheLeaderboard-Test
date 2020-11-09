using System.Text;

namespace Goss.ClimbingTheLeaderBoard.Models
{
    public class ResponseModel
    {
        public ResponseModel(int[] positions)
        {
            Positions = positions;
        }

        public int[] Positions { get; }

        public override string ToString()
        {
            var items = new StringBuilder();
            foreach (var position in Positions)
            {
                items.AppendLine(position.ToString());
            }

            return items.ToString();
        }
    }
}