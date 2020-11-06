using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Goss.ClimbingTheLeaderBoard.Tests
{
    public class FileConverterTests
    {
        private readonly Fixture _fixture;
        private readonly AutoMocker _mocker;
        private string[] _fileContents;

        public FileConverterTests()
        {
            _fixture = new Fixture();
            _mocker = new AutoMocker();
            _fileContents = new[] {"7", "100 100 50 40 40 20 10", "4", "5 25 50 120"};
            _mocker.GetMock<IFileSystem>().Setup(x => x.ReadFile(It.IsAny<string>())).ReturnsAsync(() => _fileContents);
        }

        private FileConverter ClassUnderTest => _mocker.CreateInstance<FileConverter>();

        [Fact]
        public async Task Convert_WhereFileExists_ReadsFile()
        {
            // arrange
            var filePath = _fixture.Create<string>();
            // act
            await ClassUnderTest.Convert(filePath);
            // assert
            _mocker.GetMock<IFileSystem>().Verify(x => x.ReadFile(filePath));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        public void Convert_WhereFileDoesNotHaveFourLines_ThrowsException(int numberOfLines)
        {
            // arrange
            _fileContents = new[] {"7", "100 100 50 40 40 20 10", "4", "5 25 50 120", "1", "123"}.Take(numberOfLines).ToArray();
            // act
            Func<Task> act = async () => await ClassUnderTest.Convert(_fixture.Create<string>());
            // assert
            act.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Convert_WhereFileHasNonNumericCharacters_ThrowsException(int lineWithNonNumericCharacter)
        {
            // arrange
            _fileContents = new[] {"7", "100 100 50 40 40 20 10", "4", "5 25 50 120"};
            _fileContents[lineWithNonNumericCharacter - 1] += " X";
            // act
            Func<Task> act = async () => await ClassUnderTest.Convert(_fixture.Create<string>());
            // assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task Convert_WithCorrectFormattedInput_MapsNumberOfPlayers()
        {
            // arrange
            // act
            var result = await ClassUnderTest.Convert(_fixture.Create<string>());
            // assert
            result.Should().NotBe(null);
            result.NumberOfPlayers.Should().Be(Convert.ToInt32(_fileContents.First()));
        }

        [Fact]
        public async Task Convert_WithCorrectFormattedInput_MapsLeaderBoardScores()
        {
            // arrange
            // act
            var result = await ClassUnderTest.Convert(_fixture.Create<string>());
            // assert
            result.Should().NotBe(null);
            result.LeaderBoardScores.Should().BeEquivalentTo(_fileContents.Skip(1).Take(1).First().Split(" ")
                .Select(x => Convert.ToInt32(x)));
        }

        [Fact]
        public void Convert_WithIncorrectLeaderBoardValues_ThrowsException()
        {
            // arrange
            _fileContents[1] += " 1";
            // act
            Func<Task> act = async () => await ClassUnderTest.Convert(_fixture.Create<string>());
            // assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task Convert_WithCorrectFormattedInput_MapsPlayersGames()
        {
            // arrange
            // act
            var result = await ClassUnderTest.Convert(_fixture.Create<string>());
            // assert
            result.Should().NotBe(null);
            result.PlayersGames.Should().Be(Convert.ToInt32(_fileContents.Skip(2).Take(1).First()));
        }

        [Fact]
        public async Task Convert_WithCorrectFormattedInput_MapsPlayersScores()
        {
            // arrange
            // act
            var result = await ClassUnderTest.Convert(_fixture.Create<string>());
            // assert
            result.Should().NotBe(null);
            result.PlayersScores.Should().BeEquivalentTo(_fileContents.Skip(3).Take(1).First().Split(" ")
                .Select(x => Convert.ToInt32(x)));
        }

        [Fact]
        public void Convert_WithIncorrectPlayersScores_ThrowsException()
        {
            // arrange
            _fileContents[3] += " 1";
            // act
            Func<Task> act = async () => await ClassUnderTest.Convert(_fixture.Create<string>());
            // assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void Convert_WithLeaderBoardNotInDescendingOrder_ThrowsException()
        {
            // arrange
            _fileContents[1] = "1 100 50 40 40 20 10";
            // act
            Func<Task> act = async () => await ClassUnderTest.Convert(_fixture.Create<string>());
            // assert
            act.Should().Throw<Exception>();
        }


        [Fact]
        public void Convert_WithPlayerScoresNotInAscendingOrder_ThrowsException()
        {
            // arrange
            _fileContents[3] = "1005 25 50 120";
            // act
            Func<Task> act = async () => await ClassUnderTest.Convert(_fixture.Create<string>());
            // assert
            act.Should().Throw<Exception>();
        }
    }
}