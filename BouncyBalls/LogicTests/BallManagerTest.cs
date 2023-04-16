using Logic;
using System.Collections.ObjectModel;

namespace LogicTests
{
    internal class BallManagerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MangerCollectionSizeTest()
        {
            int numberOfBalls = 5;
            LogicAbstractApi api = LogicAbstractApi.CreateAPI();
            api.CreateBalls(numberOfBalls);
            ObservableCollection<Ball> balls = api.getBalls();

            Assert.That(numberOfBalls, Is.EqualTo(balls.Count));

        }

        [Test]
        public void BallMovementTest()
        {
            int numberOfBalls = 3;
            LogicAbstractApi api = LogicAbstractApi.CreateAPI();
            api.CreateBalls(numberOfBalls);
            ObservableCollection<Ball> balls = api.getBalls();

            double initBallPosX = balls.First().XCoordinate;
            double initBallPosY = balls.First().YCoordinate;

            api.FindInitBallDestination(balls.First());

            api.MoveBall(balls.First());

            Assert.That(balls.First().XCoordinate, Is.Not.EqualTo(initBallPosX));
            Assert.That(balls.First().YCoordinate, Is.Not.EqualTo(initBallPosY));


        }

        [Test]
        public void BallBounceTest()
        {
            int numberOfBalls = 3;
            LogicAbstractApi api = LogicAbstractApi.CreateAPI();
            api.CreateBalls(numberOfBalls);
            ObservableCollection<Ball> balls = api.getBalls();

            double destBallPosX = balls.First().DestinationPlaneX;
            double destBallPosY = balls.First().DestinationPlaneY;
            double secondDestBallPosX = balls.Last().DestinationPlaneX;
            double secondDestBallPosY = balls.Last().DestinationPlaneY;

            api.FindInitBallDestination(balls.First());
            api.FindInitBallDestination(balls.Last());

            api.BounceBall(balls.First(), balls.Last());

            Assert.That(balls.First().XCoordinate, Is.Not.EqualTo(destBallPosX));
            Assert.That(balls.First().YCoordinate, Is.Not.EqualTo(destBallPosY));
            Assert.That(balls.Last().XCoordinate, Is.Not.EqualTo(secondDestBallPosX));
            Assert.That(balls.Last().YCoordinate, Is.Not.EqualTo(secondDestBallPosY));


        }

    }
}
