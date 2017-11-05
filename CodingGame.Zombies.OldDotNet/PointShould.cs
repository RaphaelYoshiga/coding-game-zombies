using NUnit.Framework;
using Shouldly;

namespace CodingGame.Zombies.OldDotNet
{
    [TestFixture]
    public class PointShould
    {

        [Test]
        public void CalculateDistance()
        {
            var point = new Point(0,0);
            var anotherPoint = new Point(0, 1);

            point.CalculateDistance(anotherPoint).ShouldBe(1);
        }
    }
}
