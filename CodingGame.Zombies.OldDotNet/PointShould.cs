using NUnit.Framework;
using Shouldly;

namespace CodingGame.Zombies.OldDotNet
{
    [TestFixture]
    public class PointShould
    {
        [TestCaseSource("SimpleDistanceMeasures")]
        public void CalculateDistance(Position position, Position anotherPosition, int expectedDistance)
        {
            var distance = position.CalculateDistance(anotherPosition);
            distance.ShouldBe(expectedDistance);
        }

        [TestCaseSource("HarderDistances")]
        public void CalculateHarderDistances(Position position, Position anotherPosition, int expectedDistance)
        {
            var distance = position.CalculateDistance(anotherPosition);
            distance.ShouldBe(expectedDistance);
        }

        static object[] SimpleDistanceMeasures =
        {
            new object[] {new Position(0,0), new Position(0, 1), 1},
            new object[] {new Position(0,0), new Position(0, 2), 2},
            new object[] {new Position(0,0), new Position(1, 2), 3},
        };

        static object[] HarderDistances =
        {
            new object[] {new Position(0,0), new Position(400, 400), 800},
            new object[] {new Position(16000,9000), new Position(0, 0), 25000},
        };
    }
}
