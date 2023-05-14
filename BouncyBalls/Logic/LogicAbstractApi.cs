using Data;
using System.Collections.ObjectModel;


namespace Logic
{
    public abstract class LogicAbstractApi
    {
        public static LogicAbstractApi CreateAPI()
        {
            return new BallManager();
        }
        public abstract ObservableCollection<Ball> getBalls();
        public abstract void CreateBalls(int NrOfBalls);
        public abstract void HandleCollision(Ball ball1, Ball ball2);
        public abstract void MoveBall(Ball ball);
        public abstract void BounceBall(Ball ball1, Ball ball2);
        public abstract void FindInitBallDestination(Ball ball);
        public abstract void RunBalls();
        public abstract void StopBalls();
    }
}
