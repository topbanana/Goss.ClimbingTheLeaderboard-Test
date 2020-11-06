using System.Threading.Tasks;
using Goss.ClimbingTheLeaderBoard.Models;

namespace Goss.ClimbingTheLeaderBoard
{
    /// <summary>
    /// Converts the input into a domain model to operate upon
    /// </summary>
    public interface IFileConverter
    {
        /// <summary>
        /// Converts the given filename into the request model
        /// </summary>
        /// <param name="filePath">The full filepath of the file to convert</param>
        /// <returns>The <see cref="RequestModel"/> representing the contents of the given file</returns>
        Task<RequestModel> Convert(string filePath);
    }
}