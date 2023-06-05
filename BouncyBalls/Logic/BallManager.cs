using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using Data;


namespace Logic
{
    internal class BallManager : LogicAbstractApi
    {
        private ObservableCollection<Ball> _currentBalls = new ObservableCollection<Ball>();
        Dictionary<(Ball, Ball), DateTime> bouncesDict = new Dictionary<(Ball, Ball), DateTime>();
        private object dictLock = new object();
        private const double MIN_COLLISION_INTERVAL = 3;
        LoggerApi logger = LoggerApi.CreateLogger();
        public ObservableCollection<Ball> CurrentBalls
        {
            get
            {
                return _currentBalls;
            }
        }
        public override ObservableCollection<Ball> getBalls()
        {
            return CurrentBalls;
        }
        public override void CreateBalls(int NrOfBalls)
        {
            if (File.Exists(@"C:\Users\talla\Desktop\Studia\Rok_2\Semestr_4\wspolbiezne\Wspolbiezne_new\BouncyBalls\Data\logs.json")) File.Delete(@"C:\Users\talla\Desktop\Studia\Rok_2\Semestr_4\wspolbiezne\Wspolbiezne_new\BouncyBalls\Data\logs.json");


            Random random = new Random();
           
            bool collision = false;
            int i = 0;
            while (_currentBalls.Count != NrOfBalls)
            {
                PointF vector = new PointF(0, 0);
                int diameter = random.Next(30) + 35;
                Ball ball = new Ball(
                    i,
                    random.Next(0, 640 - diameter),
                    random.Next(2, 360 - diameter),
                    random.Next(120, 144), diameter,
                    0,
                    0,
                    diameter * diameter,
                    vector,
                    logger);
                foreach (Ball different_ball in CurrentBalls)
                {
                    if (ball.isCollision(different_ball))
                    {
                        collision = true;
                        break;
                    }

                }
                if (!collision) _currentBalls.Add(ball);
                else collision = false;
                i++;

            }
        }

        private async void enableBallBouncing(Ball ball)
        {
            await Task.Delay(3);
            ball.CanBounce = true;

        }

        public override void MoveBall(Ball ball)
        {
            ball.Move();

            Thread.Sleep(5);
        }

        public override void BounceBall(Ball ball1, Ball ball2)  // funkcja reaguje na odbicie piłki od innej piłki
        {
            PointF tmp = ball1._vector; 

            double tmpX = ball1.DestinationPlaneX;
            double tmpY = ball1.DestinationPlaneY;

            double temp = ball1.NrOfFrames + ((2 * ball2._mass) / (ball1._mass + ball2._mass));

            double temp2 = ball2.NrOfFrames + ((2 * ball1._mass) / (ball1._mass + ball2._mass));

            ball1.UpdateMovement(ball2.DestinationPlaneX, ball2.DestinationPlaneY, ball2._vector, temp);
            ball2.UpdateMovement(tmpX, tmpY, tmp, temp2);
        }

        public override /*async*/ void HandleCollision(Ball ball1, Ball ball2) // czy pilka zderza sie z inna pilka
        {
            lock (dictLock)
            {
                if (!bouncesDict.ContainsKey((ball1, ball2)))
                {
                    Ball temp = ball1;
                    ball1 = ball2;
                    ball2 = temp;
                }
                DateTime lastCollisionTime = bouncesDict[(ball1, ball2)];
                TimeSpan timeSinceLastCollision = DateTime.Now - lastCollisionTime;
                if (timeSinceLastCollision.TotalMilliseconds < MIN_COLLISION_INTERVAL)
                {
                    // Do not handle collision yet
                    return;
                }
                BounceBall(ball1, ball2);
                bouncesDict[(ball1, ball2)] = DateTime.Now;

                //enableBallsDictBouncing(ball1, ball2);
            }

        }

        public override void FindInitBallDestination(Ball ball)
        {
            // losowe miejsce na ktorejs ze scianek jako destination point

            double lastDestinationX = ball.DestinationPlaneX;
            double lastDestinationY = ball.DestinationPlaneY;


            Random random = new Random();
            var values = new[] { 0, 1, 2, 3 };
            int result = values[random.Next(values.Length)];        // to zwroci cyfre od 0 do 3
            switch (result)
            {
                case 0: //sciana lewa
                    ball.DestinationPlaneX = 0;
                    ball.DestinationPlaneY = random.Next(0, 360 - (int)ball.Diameter);
                    ball._vector = new PointF
                    {
                        X = -1 * Math.Abs(ball._vector.X),
                        Y = ball._vector.Y
                    };
                    break;
                case 1: //sciana prawa
                    ball.DestinationPlaneX = 640 - (int)ball.Diameter;
                    ball.DestinationPlaneY = random.Next(0, 360 - (int)ball.Diameter);
                    ball._vector = new PointF
                    {
                        X = Math.Abs(ball._vector.X),
                        Y = ball._vector.Y
                    };
                    break;
                case 2: //sciana gorna
                    ball.DestinationPlaneX = random.Next(0, 640 - (int)ball.Diameter);
                    ball.DestinationPlaneY = 0;
                    ball._vector = new PointF
                    {
                        X = ball._vector.X,
                        Y = -1 * Math.Abs(ball._vector.Y)
                    };
                    break;
                case 3: //sciana dolna
                    ball.DestinationPlaneX = random.Next(0, 640 - (int)ball.Diameter);
                    ball.DestinationPlaneY = 360 - (int)ball.Diameter;
                    ball._vector = new PointF
                    {
                        X = ball._vector.X,
                        Y = Math.Abs(ball._vector.Y)
                    };
                    break;
            }

            // jeżeli wylosujemy wspolrzedna, w ktorej juz znajduje sie kulka, to przerzucamy cel na przeciwlegla sciane
            if (lastDestinationX == ball.DestinationPlaneX)
            {
                if (ball.DestinationPlaneX == 0)
                    ball.DestinationPlaneX = 640 - ball.Diameter;
                else if (ball.DestinationPlaneX == 640 - ball.Diameter)
                    ball.DestinationPlaneX = 0;
            }

            if (lastDestinationY == ball.DestinationPlaneY)
            {
                if (ball.DestinationPlaneY == 0)
                    ball.DestinationPlaneY = 360 - ball.Diameter;
                else if (ball.DestinationPlaneY == 360 - ball.Diameter)
                    ball.DestinationPlaneY = 0;
            }

            // wtedy kiedy vektor jest (0, 0), czyli jeszcze przed rozpoczeciem ruchu kulek tworzymy wektor
            if (ball._vector.X == 0 && ball._vector.Y == 0)
            {
                ball._vector = new PointF
                {
                    X = (float)((ball.DestinationPlaneX - ball.XCoordinate) / ball.NrOfFrames),
                    Y = (float)((ball.DestinationPlaneY - ball.YCoordinate) / ball.NrOfFrames)
                };
            }
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


        public void BallTaskMethod(Ball ball, CancellationToken cancellationToken)
        {

            bool hitWall = false;
            FindInitBallDestination(ball);
            while (!cancellationToken.IsCancellationRequested)
            {

                foreach (Ball diffrent_ball in _currentBalls)
                {
                    if (diffrent_ball == ball)
                    {
                        continue;
                    }


                    double distanceX = ball.XCoordinate - diffrent_ball.XCoordinate;
                    double distanceY = ball.YCoordinate - diffrent_ball.YCoordinate;
                    if (ball.CanBounce && diffrent_ball.CanBounce && Math.Sqrt(distanceX * distanceX + distanceY * distanceY) <= ball.Radius + diffrent_ball.Radius)
                    {
                        HandleCollision(ball, diffrent_ball);
                        ball.CanBounce = false;
                        diffrent_ball.CanBounce = false;
                        enableBallBouncing(ball);
                        enableBallBouncing(diffrent_ball);




                    }



                }



                if (!hitWall && (ball.XCoordinate <= 0 || ball.XCoordinate >= 640 - ball.Diameter))
                {
                    hitWall = true;
                    ball._vector = new PointF
                    {
                        X = -ball._vector.X,
                        Y = ball._vector.Y
                    };
                }

                if (!hitWall && (ball.YCoordinate <= 0 || ball.YCoordinate >= 360 - ball.Diameter))
                {
                    hitWall = true;
                    ball._vector = new PointF
                    {
                        X = ball._vector.X,
                        Y = -ball._vector.Y
                    };
                }
                MoveBall(ball);
                hitWall = false;


            }


        }

        //metoda odpowiedzialna za poruszanie piłkami każdą w osobnym wątku
        public override void RunBalls()
        {
            for (int i = 0; i < CurrentBalls.Count; i++)
            {
                for (int j = i + 1; j < CurrentBalls.Count; j++)
                {
                    bouncesDict[(CurrentBalls[i], CurrentBalls[j])] = DateTime.Now;
                }
            }

            foreach (Ball ball in _currentBalls)
            {
                Task task = Task.Run(() =>
                {
                    BallTaskMethod(ball, cancellationTokenSource.Token);
                });

            }

            Task loggerTask = Task.Run(() =>
            {

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Elapsed += new ElapsedEventHandler(OnTimerRun);
                timer.Interval = 1000;
                timer.Enabled = true;

            });
        }

        public void OnTimerRun(object sender, EventArgs e)
        {
            logger.SaveLogsToFile(_currentBalls);
            
        }

        public override void StopBalls()
        {
            _currentBalls.Clear();
            bouncesDict.Clear();
            CancelCurrentThreads();
        }

        internal void CancelCurrentThreads()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
        }
    }
}
