using System.Collections.ObjectModel;
using System.Drawing;
using Data;


namespace Logic
{
    public class BallManager
    {
        private ObservableCollection<Ball> _currentBalls = new ObservableCollection<Ball>();
        public ObservableCollection<Ball> CurrentBalls
        {
            get
            {
                return _currentBalls;
            }
        }
        public ObservableCollection<Ball> getBalls()
        {
            return CurrentBalls;
        }
        public void CreateBalls(int NrOfBalls)
        {
            _currentBalls.Clear();
            Random random = new Random();
            for (int i = 0; i < NrOfBalls; i++)
            {
                Ball ball = new Ball(random.Next(0, BallPlane._planeWidth), random.Next(2, BallPlane._planeHeight), random.NextDouble() / 10, (int)random.NextDouble() * 50 + 10);
                _currentBalls.Add(ball);
            }

        }

        public void MoveBall(Ball ball, double nrOfFrames, double duration)
        {
            ball.Move();
            Thread.Sleep((int)((duration / nrOfFrames) * 100));
        }

        public Ball BounceBall(Ball ball)  // TODO: odbijanie pilki od sciany
        {
            return ball;
        }

        //metoda odpowiedzialna za poruszanie piłkami każdą w osobnym wątku
        public void RunBalls()
        {
            /*foreach (Ball ball in _currentBalls)
            {
                Thread thread = new Thread(() =>
                {
                    PointF vector = new PointF(0, 0);
                    vector = FindNewBallPosition(ball, 25, vector);
                    while (true)
                    {
                        // todo: pilka znajduje nowy wektor w momencie gdy sie odbije od sciany lub innej pilki
                        if ((vector.X > 0 && vector.Y > 0 && ball.XCoordinate >= ball.DestinationPlaneX && ball.YCoordinate >= ball.DestinationPlaneY) ||
                        (vector.X > 0 && vector.Y < 0 && ball.XCoordinate >= ball.DestinationPlaneX && ball.YCoordinate <= ball.DestinationPlaneY) ||
                        (vector.X < 0 && vector.Y < 0 && ball.XCoordinate <= ball.DestinationPlaneX && ball.YCoordinate <= ball.DestinationPlaneY) ||
                        (vector.X < 0 && vector.Y > 0 && ball.XCoordinate <= ball.DestinationPlaneX && ball.YCoordinate >= ball.DestinationPlaneY))
                        {
                            vector = FindNewBallPosition(ball, 25, vector);
                        }
                        MoveBall(ball, 7, 4, vector);
                    }
                });
                thread.Start();
            }*/
        }
    }
}
