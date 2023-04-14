using System.Collections.ObjectModel;
using System.Drawing;
using Data;


namespace Logic
{
    public   class BallManager
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
                PointF vector = new PointF(0, 0);
                int diameter = 30;// random.Next(50) + 10;
                Ball ball = new Ball(random.Next(0, 640 - diameter), random.Next(2, 360 - diameter), random.NextDouble() / 10, diameter, 0, 0, random.Next(2, 300), vector);
                _currentBalls.Add(ball);    
            }

        }

        public void MoveBall(Ball ball, double nrOfFrames, double duration)
        {
            ball.Move(nrOfFrames, duration);
            Thread.Sleep((int)((duration / nrOfFrames) * 100));
        }

        public void BounceBall(Ball ball1, Ball ball2)  // TODO: odbijanie pilki od sciany
        {
            PointF tmp = ball1._vector;  // czy ten wektor jest gdziekolwiek uzywany? chyba nie

            double tmpX = ball1.DestinationPlaneX;
            double tmpY = ball1.DestinationPlaneY;

            ball1.UpdateMovement(ball2.DestinationPlaneX, ball2.DestinationPlaneY, ball2._vector, ball1._speed - ((2 * ball2._mass) / ball1._mass + ball2._mass));
            ball2.UpdateMovement(tmpX, tmpY, tmp, ball2._speed - ((2 * ball1._mass) / ball1._mass + ball2._mass));
        }

        public /*async*/ void IsCollisionAndHandleCollision(ObservableCollection<Ball> CurrentBalls) // czy pilka zderza sie z inna pilka
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
                        if (Math.Sqrt(distanceX * distanceX + distanceY * distanceY) <= CurrentBalls[i]._radius + CurrentBalls[j]._radius)
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

        public PointF FindNewBallPosition(Ball ball, int nrOfFrames)
        {
            // losowe miejsce na ktorejs ze scianek jako destination point
            Random random = new Random();
            var values = new[] { 0, 1, 2, 3 };
            int result = values[random.Next(values.Length)];        // to zwroci cyfre od 1 do 4?
            switch (result)
            {
                case 0: //sciana lewa
                    ball.DestinationPlaneX = 0;
                    ball.DestinationPlaneY = random.Next(0, 360 - (int)(ball._radius * 2));
                    break;
                case 1: //sciana prawa
                    ball.DestinationPlaneX = 640 - (int)(ball._radius * 2);
                    ball.DestinationPlaneY = random.Next(0, 360 - (int)(ball._radius * 2));
                    break;
                case 2: //sciana gorna
                    ball.DestinationPlaneX = random.Next(0, 640 - (int)(ball._radius * 2));
                    ball.DestinationPlaneY = 0;
                    break;
                case 3: //sciana dolna
                    ball.DestinationPlaneX = random.Next(0, 640 - (int)(ball._radius * 2));
                    ball.DestinationPlaneY = 360 - (int)(ball._radius * 2);
                    break;
            }

            // jeżeli wylosujemy wspolrzedna, w ktorej juz znajduje sie kulka, to przerzucamy cel na przeciwlegla sciane
            if (ball.XCoordinate == ball.DestinationPlaneX)
            {
                if (ball.DestinationPlaneX == 0)
                    ball.DestinationPlaneX = 640 - ball._radius * 2;
                else if (ball.DestinationPlaneX == 640 - ball._radius * 2)
                    ball.DestinationPlaneX = 0;
            }

            if (ball.YCoordinate == ball.DestinationPlaneY)
            {
                if (ball.DestinationPlaneY == 0)
                    ball.DestinationPlaneY = 360 - ball._radius * 2;
                else if (ball.DestinationPlaneY == 360 - ball._radius * 2)
                    ball.DestinationPlaneY = 0;
            }

            return new PointF
            {
                X = (float)((ball.DestinationPlaneX - ball.XCoordinate) / nrOfFrames),
                Y = (float)((ball.DestinationPlaneY - ball.YCoordinate) / nrOfFrames)
            };
        }

        //metoda odpowiedzialna za poruszanie piłkami każdą w osobnym wątku
        public void RunBalls()
        {
            foreach (Ball ball in _currentBalls)
            {
                Task task = new Task(() =>
                {
                    ball._vector = FindNewBallPosition(ball, 25);
                    while (true)
                    {
                        // todo: pilka znajduje nowy wektor w momencie gdy sie odbije od sciany lub innej pilki
                        if ((ball._vector.X > 0 && ball._vector.Y > 0 && ball.XCoordinate >= ball.DestinationPlaneX && ball.YCoordinate >= ball.DestinationPlaneY) ||
                        (ball._vector.X > 0 && ball._vector.Y < 0 && ball.XCoordinate >= ball.DestinationPlaneX && ball.YCoordinate <= ball.DestinationPlaneY) ||
                        (ball._vector.X < 0 && ball._vector.Y < 0 && ball.XCoordinate <= ball.DestinationPlaneX && ball.YCoordinate <= ball.DestinationPlaneY) ||
                        (ball._vector.X < 0 && ball._vector.Y > 0 && ball.XCoordinate <= ball.DestinationPlaneX && ball.YCoordinate >= ball.DestinationPlaneY))
                        {
                            ball._vector = FindNewBallPosition(ball, 25);
                        }

                        MoveBall(ball, 7, 4);
                    }
                });
                task.Start();
            }
            Task task1 = new Task(() => IsCollisionAndHandleCollision(_currentBalls));
            task1.Start();
        }
    }
}
