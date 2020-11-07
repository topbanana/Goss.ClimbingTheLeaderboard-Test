using System.Threading.Tasks;

namespace Goss.ClimbingTheLeaderBoard
{
    /// <summary>
    /// Abstracts out the file system
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Retrieves the contents of a file
        /// </summary>
        /// <param name="filePath">The full path of the file</param>
        /// <returns>Each line of the file as a string</returns>
        Task<string[]> ReadFile(string filePath);
    }
}