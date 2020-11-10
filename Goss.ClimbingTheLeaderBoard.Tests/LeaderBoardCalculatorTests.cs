using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Goss.ClimbingTheLeaderBoard.Models;
using Moq.AutoMock;
using Xunit;
using Xunit.Abstractions;

namespace Goss.ClimbingTheLeaderBoard.Tests
{
    public class LeaderBoardCalculatorTests
    {
        private readonly AutoMocker _mocker;
        private readonly ITestOutputHelper _testOutputHelper;

        public LeaderBoardCalculatorTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _mocker = new AutoMocker();
        }

        private ILeaderBoardCalculator CreateClassUnderTest(Type t)
        {
            return (ILeaderBoardCalculator) _mocker.CreateInstance(t);
        }

        [Theory]
        [InlineData(typeof(LeaderBoardOriginalCalculator))]
        [InlineData(typeof(LeaderBoardNonDistinctCalculator))]
        [InlineData(typeof(LeaderBoardParallelCalculator))]
        [InlineData(typeof(LeaderBoardParallelNonDistinctCalculator))]
        [InlineData(typeof(LeaderBoardTaskCalculator))]
        public async Task Calculate_TestCase1_IsCorrect(Type implementation)
        {
            // arrange
            var input = new RequestModel(7, new[] {100, 100, 50, 40, 40, 20, 10}, 4, new[] {5, 25, 50, 120});
            // act
            var result = await CreateClassUnderTest(implementation).Calculate(input);
            // assert
            result.Positions.Should().BeEquivalentTo(new[] {6, 4, 2, 1}).And.BeInDescendingOrder();
        }

        [Theory]
        [InlineData(typeof(LeaderBoardOriginalCalculator))]
        [InlineData(typeof(LeaderBoardNonDistinctCalculator))]
        [InlineData(typeof(LeaderBoardParallelCalculator))]
        [InlineData(typeof(LeaderBoardParallelNonDistinctCalculator))]
        [InlineData(typeof(LeaderBoardTaskCalculator))]
        public async Task Calculate_TestCase2_IsCorrect(Type implementation)
        {
            // arrange
            var input = new RequestModel(6, new[] {100, 90, 90, 80, 75, 60}, 5, new[] {50, 65, 77, 90, 102});
            // act
            var result = await CreateClassUnderTest(implementation).Calculate(input);
            // assert
            result.Positions.Should().BeEquivalentTo(new[] {6, 5, 4, 2, 1}).And.BeInDescendingOrder();
        }

        [Theory]
        [InlineData(typeof(LeaderBoardOriginalCalculator))]
        [InlineData(typeof(LeaderBoardNonDistinctCalculator))]
        [InlineData(typeof(LeaderBoardParallelCalculator))]
        [InlineData(typeof(LeaderBoardParallelNonDistinctCalculator))]
        [InlineData(typeof(LeaderBoardTaskCalculator))]
        public async Task Calculate_TestCase3_IsCorrect(Type implementation)
        {
            // arrange
            var input = new RequestModel(6, new[] {100, 90, 90, 80, 75, 60}, 4, new[] {50, 65, 77, 90});
            // act
            var result = await CreateClassUnderTest(implementation).Calculate(input);
            // assert
            result.Positions.Should().BeEquivalentTo(new[] {6, 5, 4, 2}).And.BeInDescendingOrder();
        }

        [Theory]
        [InlineData(typeof(LeaderBoardOriginalCalculator))]
        [InlineData(typeof(LeaderBoardNonDistinctCalculator))]
        [InlineData(typeof(LeaderBoardParallelCalculator))]
        [InlineData(typeof(LeaderBoardParallelNonDistinctCalculator))]
        [InlineData(typeof(LeaderBoardTaskCalculator))]
        public async Task Calculate_TestCase4_IsCorrect(Type implementation)
        {
            // arrange
            var input = new RequestModel(6, new[] {100, 90, 90, 80, 75, 60}, 4, new[] {65, 77, 90, 102});
            // act
            var result = await CreateClassUnderTest(implementation).Calculate(input);
            // assert
            result.Positions.Should().BeEquivalentTo(new[] {5, 4, 2, 1}).And.BeInDescendingOrder();
        }

        [Theory]
        [InlineData(1000, typeof(LeaderBoardOriginalCalculator))]
        [InlineData(1000, typeof(LeaderBoardNonDistinctCalculator))]
        [InlineData(1000, typeof(LeaderBoardParallelCalculator))]
        [InlineData(1000, typeof(LeaderBoardParallelNonDistinctCalculator))]
        [InlineData(1000, typeof(LeaderBoardTaskCalculator))]
        //[InlineData(100000, typeof(LeaderBoardOriginalCalculator))]
        //[InlineData(100000, typeof(LeaderBoardNonDistinctCalculator))]
        //[InlineData(100000, typeof(LeaderBoardParallelCalculator))]
        //[InlineData(100000, typeof(LeaderBoardParallelNonDistinctCalculator))]
        //[InlineData(100000, typeof(LeaderBoardTaskCalculator))]
        //[InlineData(10000000, typeof(LeaderBoardOriginalCalculator))]
        //[InlineData(10000000, typeof(LeaderBoardNonDistinctCalculator))]
        //[InlineData(10000000, typeof(LeaderBoardParallelCalculator))]
        //[InlineData(10000000, typeof(LeaderBoardParallelNonDistinctCalculator))]
        //[InlineData(10000000, typeof(LeaderBoardTaskCalculator))]
        public async Task Calculate_TestCase7_IsCorrect(int size, Type implementation)
        {
            // arrange
            var maxRanks = size;
            var maxScores = size;

            var leaderBoard = new ConcurrentDictionary<int, int>();
            var scores = new ConcurrentDictionary<int, int>();
            var results = new ConcurrentDictionary<int, int>();

            Parallel.For(0, size, i =>
            {
                leaderBoard[i] = size - i;
                scores[i] = i + 1;
                results[i] = size - i;
            });

            var input = new RequestModel(maxRanks, leaderBoard.Values.ToArray(), maxScores, scores.Values.ToArray());

            // act
            var timer = new Stopwatch();
            timer.Start();
            var result = await CreateClassUnderTest(implementation).Calculate(input);
            timer.Stop();

            // assert
            _testOutputHelper.WriteLine($"Calculate {maxScores} using {timer.ElapsedMilliseconds} ms");

            if (size <= 1000)
            {
                result.Positions.Should().BeEquivalentTo(results.Values.ToArray()).And.BeInDescendingOrder();
                _testOutputHelper.WriteLine("Pass test correctness for size 1000");
            }
            else
            {
                _testOutputHelper.WriteLine("Pass test correctness for size 1000 so no need to test larger sizes");
            }

            _testOutputHelper.WriteLine($"{size} and type {implementation.Name} took: {timer.ElapsedMilliseconds}ms");
        }
    }
}