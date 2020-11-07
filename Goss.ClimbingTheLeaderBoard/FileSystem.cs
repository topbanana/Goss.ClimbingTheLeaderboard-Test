using System.IO;
using System.Threading.Tasks;

namespace Goss.ClimbingTheLeaderBoard
{
    /// <inheritdoc />
    public class FileSystem : IFileSystem
    {
        /// <inheritdoc />
        public Task<string[]> ReadFile(string filePath)
        {
            return File.ReadAllLinesAsync(filePath);
        }
    }
}