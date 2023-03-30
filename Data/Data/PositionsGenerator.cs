using System.Numerics;


namespace Data
{
    public class PositionsGenerator
    {
        private Random generator = new Random();
        private int _passedRadius;

        public PositionsGenerator() { }

        public PositionsGenerator(int radius)
        {
            _passedRadius = radius;
        }

        //generowanie losowej pozycji z pełnego zakresu jednak zwężonego o promień, żeby kulka nie wchodziła poza niego 
        public Vector2 GenerateCoordinates()
        {
            Vector2 coordinates = new Vector2();

            coordinates.X = generator.Next(_passedRadius, (int)(BallPlane._planeWidth - _passedRadius));
            coordinates.Y = generator.Next(_passedRadius, (int)(BallPlane._planeHeight - _passedRadius));

            return coordinates;
        }
    }
}
