using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Goss.ClimbingTheLeaderBoard.Models;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Goss.ClimbingTheLeaderBoard.Tests
{
    public class ProgramTests
    {
        private readonly Fixture _fixture;
        private readonly AutoMocker _mocker;

        public ProgramTests()
        {
            _fixture = new Fixture();
            _mocker = new AutoMocker();
            _fixture.Register(() => new RequestModel(1, new[] {1}, 1, new[] {1}));
        }

        private Program ClassUnderTest => _mocker.CreateInstance<Program>();

        [Fact]
        public async Task Execute_WithFilePath_ReadsInputModelFromSuppliedFileName()
        {
            // arrange
            var fileName = _fixture.Create<string>();
            // act
            await ClassUnderTest.Execute(fileName);
            // assert
            _mocker.GetMock<IFileConverter>().Verify(x => x.Convert(fileName));
        }

        [Fact]
        public async Task Execute_WithFilePath_InvokesCalculatorWithModel()
        {
            // arrange
            var expected = _fixture.Create<RequestModel>();
            _mocker.GetMock<IFileConverter>()
                .Setup(x => x.Convert(It.IsAny<string>()))
                .ReturnsAsync(expected);
            // act
            await ClassUnderTest.Execute(_fixture.Create<string>());
            // assert
            _mocker.GetMock<ILeaderBoardCalculator>().Verify(x => x.Calculate(expected));
        }

        [Fact]
        public async Task Execute_WithFilePath_ReturnsResponseFromCalculator()
        {
            // arrange
            var expected = _fixture.Create<ResponseModel>();
            _mocker.GetMock<ILeaderBoardCalculator>()
                .Setup(x => x.Calculate(It.IsAny<RequestModel>()))
                .ReturnsAsync(expected);
            // act
            var result = await ClassUnderTest.Execute(_fixture.Create<string>());
            // assert
            result.Should().BeSameAs(expected);
        }
    }
}