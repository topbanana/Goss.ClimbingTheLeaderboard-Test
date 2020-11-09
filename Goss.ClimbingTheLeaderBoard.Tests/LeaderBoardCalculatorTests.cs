using FluentAssertions;
using Goss.ClimbingTheLeaderBoard.Models;
using Moq.AutoMock;
using Xunit;

namespace Goss.ClimbingTheLeaderBoard.Tests
{
    public class LeaderBoardCalculatorTests
    {
        private readonly AutoMocker _mocker;

        public LeaderBoardCalculatorTests()
        {
            _mocker = new AutoMocker();
        }

        private LeaderBoardCalculator ClassUnderTest => _mocker.CreateInstance<LeaderBoardCalculator>();

        [Fact]
        public void Calculate_TestCase1_IsCorrect()
        {
            // arrange
            var input = new RequestModel(7, new[] {100, 100, 50, 40, 40, 20, 10}, 4, new[] {5, 25, 50, 120});
            // act
            var result = ClassUnderTest.Calculate(input);
            // assert
            result.Positions.Should().BeEquivalentTo(new[] {6, 4, 2, 1}).And.BeInDescendingOrder();
        }

        [Fact]
        public void Calculate_TestCase2_IsCorrect()
        {
            // arrange
            var input = new RequestModel(6, new[] {100, 90, 90, 80, 75, 60}, 5, new[] {50, 65, 77, 90, 102});
            // act
            var result = ClassUnderTest.Calculate(input);
            // assert
            result.Positions.Should().BeEquivalentTo(new[] {6, 5, 4, 2, 1}).And.BeInDescendingOrder();
        }

        [Fact]
        public void Calculate_TestCase3_IsCorrect()
        {
            // arrange
            var input = new RequestModel(6, new[] {100, 90, 90, 80, 75, 60}, 4, new[] {50, 65, 77, 90});
            // act
            var result = ClassUnderTest.Calculate(input);
            // assert
            result.Positions.Should().BeEquivalentTo(new[] {6, 5, 4, 2}).And.BeInDescendingOrder();
        }

        [Fact]
        public void Calculate_TestCase4_IsCorrect()
        {
            // arrange
            var input = new RequestModel(6, new[] {100, 90, 90, 80, 75, 60}, 4, new[] {65, 77, 90, 102});
            // act
            var result = ClassUnderTest.Calculate(input);
            // assert
            result.Positions.Should().BeEquivalentTo(new[] {5, 4, 2, 1}).And.BeInDescendingOrder();
        }
    }
}