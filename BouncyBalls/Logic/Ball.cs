using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Logic
{
    public class Ball : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private object _lockObject = new object();
        private bool _canMove = true;
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

        public Ball(double XCoordinate, double YCoordinate, double NrOfFrames, int Diameter, double DestinationPlaneX, double DestinationPlaneY, double Mass, PointF Vector)
        {
            this.Diameter = Diameter;
            this.XCoordinate = XCoordinate;
            this.YCoordinate = YCoordinate;
            this.NrOfFrames = NrOfFrames;
            this.DestinationPlaneX = DestinationPlaneX;
            this.DestinationPlaneY = DestinationPlaneY;
            this._mass = Mass;
            this._vector = Vector;
        }

        public void Move()
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
        }
        public void UpdateMovement(double x, double y, PointF vector)
        {
            _canMove = false;

            // sekcja krytyczna - tylko 1 watek na raz moze wykonac te logike
            lock (_lockObject)
            {
                DestinationPlaneX = x;
                DestinationPlaneY = y;
                _vector = vector;         
            }
            _canMove = true;    
        }
    }
}
