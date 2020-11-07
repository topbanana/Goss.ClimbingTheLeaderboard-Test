using System;
using AutoFixture;
using FluentAssertions;
using Goss.ClimbingTheLeaderBoard.Models;
using Moq.AutoMock;
using Xunit;

namespace Goss.ClimbingTheLeaderBoard.Tests
{
    public class LeaderBoardCalculatorTests
    {
        private readonly Fixture _fixture;
        private readonly AutoMocker _mocker;

        public LeaderBoardCalculatorTests()
        {
            _fixture = new Fixture();
            _mocker = new AutoMocker();
            _fixture.Register(() => new RequestModel(1, new[] {1}, 1, new[] {1}));
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
            result.Positions.Should().BeEquivalentTo(new[] {8, 5, 3, 1});
        }

        [Fact]
        public void Calculate_TestCase1_ToStringIsCorrect()
        {
            // arrange
            var input = new RequestModel(7, new[] {100, 100, 50, 40, 40, 20, 10}, 4, new[] {5, 25, 50, 120});
            // act
            var result = ClassUnderTest.Calculate(input);
            // assert
            result.ToString().Should()
                .Be($"8{Environment.NewLine}5{Environment.NewLine}3{Environment.NewLine}1{Environment.NewLine}");
        }

        [Fact]
        public void Calculate_TestCase2_ToStringIsCorrect()
        {
            // arrange
            var input = new RequestModel(6, new[] {100, 90, 90, 80, 75, 60}, 5, new[] {50, 65, 77, 90, 102});
            // act
            var result = ClassUnderTest.Calculate(input);
            // assert
            result.ToString().Should()
                .Be(
                    $"9{Environment.NewLine}7{Environment.NewLine}5{Environment.NewLine}3{Environment.NewLine}1{Environment.NewLine}");
        }

        [Fact]
        public void Calculate_TestCase2_IsCorrect()
        {
            // arrange
            var input = new RequestModel(6, new[] {100, 90, 90, 80, 75, 60}, 5, new[] {50, 65, 77, 90, 102});
            // act
            var result = ClassUnderTest.Calculate(input);
            // assert
            result.Positions.Should().BeEquivalentTo(new[] {9, 7, 5, 3, 1});
        }
    }
}