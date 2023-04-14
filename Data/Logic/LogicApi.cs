using Data;
using System.Collections.ObjectModel;


namespace Logic
{
    public abstract class LogicAPI
    {
        public static LogicAPI CreateAPI()
        {
            return new BallManager();
        }
        public abstract ObservableCollection<Ball> getBalls();
        public abstract void CreateBalls(int NrOfBalls);
        public abstract void IsCollisionAndHandleCollision(ObservableCollection<Ball> CurrentBalls);
        public abstract void MoveBall(Ball ball);
        public abstract void BounceBall(Ball ball1, Ball ball2);
        public abstract void FindNewBallPosition(Ball ball);
        public abstract void RunBalls();
    }
}
