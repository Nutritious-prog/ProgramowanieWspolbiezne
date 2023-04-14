using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private object _lockObject = new object();

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
                RaisePropertyChanged("XCoordinate");
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
                RaisePropertyChanged("YCoordinate");
            }
        }

        //ilość pikseli którą kulka będzie przebywać w każdym odświeżeniu
        private double _speed { get; set; }

        private int _radius { get; set; }

        private double _destinationPlaneX;
        public double DestinationPlaneX
        {
            get => _destinationPlaneX;

            set
            {
                if (value > 640 - _radius * 2)
                {
                    _destinationPlaneX = 640 - _radius * 2;
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
                if (value > 360 - _radius * 2)
                {
                    _destinationPlaneY = 360 - _radius * 2;
                }
                else _destinationPlaneY = value;
            }
        }

        public double _mass { get; set; }
        public PointF _vector { get; set; }

        public Ball(double XCoordinate, double YCoordinate, double Speed, int Radius, double DestinationPlaneX, double DestinationPlaneY, double Mass, PointF Vector)
        {
            this._radius = Radius;
            this.XCoordinate = XCoordinate;
            this.YCoordinate = YCoordinate;
            this._speed = Speed;
            this.DestinationPlaneX = DestinationPlaneX;
            this.DestinationPlaneY = DestinationPlaneY;
            this._mass = Mass;
            this._vector = Vector;
        }

        public void Move(double nrOfFrames, double duration)
        {
            if ((_vector.X > 0 && XCoordinate + _vector.X > DestinationPlaneX)
                || (_vector.X < 0 && XCoordinate + _vector.X < DestinationPlaneX))
                XCoordinate = DestinationPlaneX;
            else
                XCoordinate += _vector.X;

            if ((_vector.Y > 0 && YCoordinate + _vector.Y > DestinationPlaneY)
                || (_vector.Y < 0 && YCoordinate + _vector.Y < DestinationPlaneY))
                YCoordinate = DestinationPlaneY;
            else
                YCoordinate += _vector.Y;

            _speed = (int)(duration / nrOfFrames * 100);
        }
        public void UpdateMovement(double x, double y, PointF vector, double speed)
        {
            //var previousDestX = DestinationPlaneX;
            //var previousDestY = DestinationPlaneY;
            //var previousVector = Vector;

            // sekcja krytyczna - tylko 1 watek na raz moze wykonac te logike
            lock (_lockObject)
            {
                DestinationPlaneX = x;
                DestinationPlaneY = y;
                _vector = vector;
                _speed = speed;
                //Console.WriteLine($"MOVEMENT UPDATED for Ball with id {Id}:\n" +
                //$"destination X,Y: {previousDestX}, {previousDestY} => {DestinationPlaneX}, {DestinationPlaneY}\n" +
                //$"Vector X,Y: {previousVector.X}, {previousVector.Y} => {vector.X}, {vector.Y}\n");
            }
        }
    }
}
