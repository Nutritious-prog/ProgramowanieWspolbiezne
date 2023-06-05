using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : BallApi, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private object _lockObject = new object();
        private bool _canMove = true;
        private bool _canBounce = true;
        public int Id { get; init; }
        private double _xCoordinate;
        private double _yCoordinate;
        public double XCoordinate
        {
            get
            {
                return _xCoordinate;
            }
            set
            {
                _xCoordinate = value;
                RaisePropertyChanged(nameof(XCoordinate));
            }
        }
        public double YCoordinate
        {
            get
            {
                return _yCoordinate;
            }
            set
            {
                _yCoordinate = value;
                RaisePropertyChanged(nameof(YCoordinate));
            }
        }

        public bool CanBounce
        {
            get { return _canBounce; }
            set { _canBounce = value; }
        }

        //ilość pikseli którą kulka będzie przebywać w każdym odświeżeniu
        public double NrOfFrames { get; set; }

        public int Diameter { get; private set; }
        public int Radius => Diameter / 2;

        private double _destinationPlaneX;
        public double DestinationPlaneX
        {
            get => _destinationPlaneX;

            set
            {
                if (value > 640 - Diameter)
                {
                    _destinationPlaneX = 640 - Diameter;
                }
                else if (value < 0 + Diameter)
                {
                    _destinationPlaneX = 0 + Diameter;
                }
                else _destinationPlaneX = value;
            }
        }
        private double _destinationPlaneY;
        public double DestinationPlaneY
        {
            get => _destinationPlaneY;

            set
            {
                if (value > 360 - Diameter)
                {
                    _destinationPlaneY = 360 - Diameter;
                }
                else if (value < 0 + Diameter)
                {
                    _destinationPlaneY = 0 + Diameter;
                }
                else _destinationPlaneY = value;
            }
        }

        public double _mass { get; set; }
        public PointF _vector { get; set; }

        private LoggerApi _logger;

        
        public Ball(int id, double XCoordinate, double YCoordinate, double NrOfFrames, int Diameter, double DestinationPlaneX, double DestinationPlaneY, double Mass, PointF Vector, LoggerApi logger)
        {
            this.Diameter = Diameter;
            this.XCoordinate = XCoordinate;
            this.YCoordinate = YCoordinate;
            this.NrOfFrames = NrOfFrames;
            this.DestinationPlaneX = DestinationPlaneX;
            this.DestinationPlaneY = DestinationPlaneY;
            this._mass = Mass;
            this._vector = Vector;
            _logger = logger;
            this.Id = id;
        }

        public override void Move()
        {
            if (!_canMove) return;

            if (_vector.X > 0 && XCoordinate + _vector.X > 640 - Diameter)
                XCoordinate = 640 - Diameter;
            else if (_vector.X < 0 && XCoordinate + _vector.X < 0)
                XCoordinate = 0;
            else
                XCoordinate += _vector.X;   

            if (_vector.Y > 0 && YCoordinate + _vector.Y > 360 - Diameter)
                YCoordinate = 360 - Diameter;
            else if (_vector.Y < 0 && YCoordinate + _vector.Y < 0)
                YCoordinate = 0;
            else
                YCoordinate += _vector.Y;
            //_logger.SaveLogsToFile(this);
        }
        public override void UpdateMovement(double x, double y, PointF vector, double nrOfFrames)
        {
            _canMove = false;

            // sekcja krytyczna - tylko 1 watek na raz moze wykonac te logike
            lock (_lockObject)
            {
                DestinationPlaneX = x;
                DestinationPlaneY = y;
                _vector = vector;
                NrOfFrames = nrOfFrames;
            }
            _canMove = true;
            //_logger.SaveLogsToFile(this);
        }
        public override bool isCollision(Ball ball2)
        {
            return Math.Sqrt(Math.Pow(this.XCoordinate - ball2.XCoordinate, 2) + Math.Pow(this.YCoordinate - ball2.YCoordinate, 2)) <= this.Radius + ball2.Radius;
        }
    }
}
