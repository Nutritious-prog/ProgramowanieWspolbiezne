using Data;
using System.Drawing;

namespace DataTests
{
    internal class BallTests
    {
        Random random = new Random();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BallMovementTest()
        {
            int diameter = random.Next(40) + 20;
            PointF vector = new PointF(0, 0);
            Ball testBall = new Ball(
                    0,
                    0,
                    random.Next(120, 144), diameter,
                     0,
                     0,
                    random.NextDouble() + 0.1,
                    vector);

            Assert.IsTrue(testBall.Diameter == diameter);
            Assert.IsTrue(testBall._vector == vector);

            testBall._vector = new PointF(5, 5);
            testBall.Move();
            Assert.IsTrue(testBall.XCoordinate == 5);
            Assert.IsTrue(testBall.YCoordinate == 5);

        }

        [Test]
        public void BallDestinationTest()
        {
            int diameter = 20;
            PointF vector = new PointF(0, 0);
            Ball testBall = new Ball(
                    0,
                    0,
                    random.Next(120, 144), diameter,
                     0,
                     0,
                    random.NextDouble() + 0.1,
                    vector);


            testBall.DestinationPlaneX = 640; // sprawdzenie zbyt dużego destinationPlaneX
            Assert.That(640 - diameter, Is.EqualTo(testBall.DestinationPlaneX));

            testBall.DestinationPlaneY = 360;
            Assert.That(360 - diameter, Is.EqualTo(testBall.DestinationPlaneY));

        }

        [Test]
        public void BallUpdateMovementTest()
        {
            int diameter = 20;
            PointF vector = new PointF(0, 0);
            Ball testBall = new Ball(
                    0,
                    0,
                    random.Next(120, 144), diameter,
                     0,
                     0,
                    random.NextDouble() + 0.1,
                    vector);

            int newDestinationPlaneX = 200;
            int newDestinationPlaneY = 300;
            int nrOfFrames = 50;
            PointF newVector = new PointF(10, -5);

            testBall.UpdateMovement(newDestinationPlaneX, newDestinationPlaneY, newVector, nrOfFrames);
            Assert.That(newDestinationPlaneX, Is.EqualTo(testBall.DestinationPlaneX));
            Assert.That(newDestinationPlaneY, Is.EqualTo(testBall.DestinationPlaneY));
            Assert.That(newVector, Is.EqualTo(testBall._vector));
            Assert.That(nrOfFrames, Is.EqualTo(testBall.NrOfFrames));
        }

    }
}
