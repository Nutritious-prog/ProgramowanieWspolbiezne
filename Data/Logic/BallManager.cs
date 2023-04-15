﻿using System.Collections.ObjectModel;
using System.Drawing;
using Data;


namespace Logic
{
    public class BallManager : LogicAPI
    {
        private ObservableCollection<Ball> _currentBalls = new ObservableCollection<Ball>();
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
            _currentBalls.Clear();
            Random random = new Random();
            for (int i = 0; i < NrOfBalls; i++)
            {
                PointF vector = new PointF(0, 0);
                int diameter = random.Next(40) + 20;
                Ball ball = new Ball(
                    random.Next(0, 640 - diameter),
                    random.Next(2, 360 - diameter),
                    random.Next(20, 30), diameter,
                    0,
                    0,
                    random.NextDouble() + 0.1,
                    vector);        
                _currentBalls.Add(ball);    
            }

        }

        public override void MoveBall(Ball ball)
        {
            ball.Move();

            Thread.Sleep(50);
        }

        public override void BounceBall(Ball ball1, Ball ball2)  // TODO: odbijanie pilki od sciany
        {
            PointF tmp = ball1._vector;  // czy ten wektor jest gdziekolwiek uzywany? chyba nie

            double tmpX = ball1.DestinationPlaneX;
            double tmpY = ball1.DestinationPlaneY;

            double temp = ball1.NrOfFrames + ((2 * ball2._mass) / (ball1._mass + ball2._mass));

            double temp2 = ball2.NrOfFrames + ((2 * ball1._mass) / (ball1._mass + ball2._mass));

            ball1.UpdateMovement(ball2.DestinationPlaneX, ball2.DestinationPlaneY, ball2._vector, temp);
            ball2.UpdateMovement(tmpX, tmpY, tmp, temp2);
        }

        public override /*async*/ void IsCollisionAndHandleCollision(ObservableCollection<Ball> CurrentBalls) // czy pilka zderza sie z inna pilka
        {
            double distanceX;
            double distanceY;

            Dictionary<(int, int), bool> bouncesDict = new Dictionary<(int, int), bool>();
            // na poczatku nie mamy zadnych zarejestrowanych odbic - wrzucamy wszedzie false, zeby nam potem nie krzyczal, że Key does not exist
            for (int i = 0; i < CurrentBalls.Count; i++)
            {
                for (int j = i + 1; j < CurrentBalls.Count; j++)
                {
                    bouncesDict[(i, j)] = false;
                }
            }

            while (true) // wykrywamy zderzenia przez caly czas dzialania programu
            {
                for (int i = 0; i < CurrentBalls.Count; i++)
                {
                    for (int j = i + 1; j < CurrentBalls.Count; j++)
                    {
                        distanceX = CurrentBalls[i].XCoordinate - CurrentBalls[j].XCoordinate;
                        distanceY = CurrentBalls[i].YCoordinate - CurrentBalls[j].YCoordinate;
                        if (Math.Sqrt(distanceX * distanceX + distanceY * distanceY) <= CurrentBalls[i].Radius + CurrentBalls[j].Radius)        
                        {
                            // jezeli obsluzylismy juz odbicie dla tej pary kulek, to pomijamy Bounce
                            if (bouncesDict[(i, j)]) continue;

                            //Console.WriteLine($"COLLISION DETECTED between:\n{CurrentBalls[i].Details}\nand\n\n{CurrentBalls[j].Details}\n");
                            BounceBall(CurrentBalls[i], CurrentBalls[j]);
                            bouncesDict[(i, j)] = true; // jezeli zrobilismy Bounce, to ustawiamy flage na true, zeby wiedziec, ze to odbicie juz zostalo obsluzone
                        }
                        else bouncesDict[(i, j)] = false; // jezeli kulki sie nie stykaja to ustawiamy flage na false, zeby bylo mozna obsluzyc kolejne zderzenie dla tej pary kulek
                    }
                }
            }
        }

        public override void FindNewBallPosition(Ball ball)
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

        //metoda odpowiedzialna za poruszanie piłkami każdą w osobnym wątku
        public override void RunBalls()
        {
            foreach (Ball ball in _currentBalls)
            {
                Task task = new Task(() =>
                {
                    bool hitWall = false;
                    FindNewBallPosition(ball);
                    while (true)
                    {

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
                });
                task.Start();
            }
            Task task1 = new Task(() => IsCollisionAndHandleCollision(_currentBalls));
            task1.Start();
        }
    }
}