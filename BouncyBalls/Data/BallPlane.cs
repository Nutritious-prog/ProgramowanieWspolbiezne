using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class BallPlane
    {
        private Point _leftUpCorner;
        private Point _rightDownCorner;

        public Point LeftUpCorner { get; set; }
        public Point RightDownCorner { get; set; }

        public BallPlane(Point LeftUpCorner, Point RightDownCorner)
        {
            this.LeftUpCorner = LeftUpCorner;
            this.RightDownCorner = RightDownCorner;
        }
    }
}
