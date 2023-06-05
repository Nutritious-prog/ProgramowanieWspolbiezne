using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
   public abstract class BallApi
    {
        public abstract void Move();
        public abstract void UpdateMovement(double x, double y, PointF vector, double nrOfFrames);
        public abstract bool isCollision(Ball ball2);

        public static BallApi CreateNewBall(int id, double XCoordinate, double YCoordinate, double NrOfFrames, int Diameter, double DestinationPlaneX, double DestinationPlaneY, double Mass, PointF Vector, LoggerApi logger)
        {
            return new Ball(id, XCoordinate, YCoordinate, NrOfFrames, Diameter, DestinationPlaneX, DestinationPlaneY, Mass, Vector, logger);
        }
    }

    public abstract class LoggerApi
    {
        public abstract void SaveLogsToFile(ObservableCollection<Ball> balls);

        public static LoggerApi CreateLogger()
        {
            return new Logger();
        }
    }
}
