using System.Numerics;

namespace Data
{
    public class Ball
    {
        //aktualna pozycja kulki aktualizowana w każdym odświeżeniu
        Vector2 _currentCoordinates = new Vector2();

        //losowo generowane położenie do którego kulka chce się dostać, kiedy się tam dostanie generowane na nowo
        Vector2 _generatedCoordinates = new Vector2();

        //ilość pikseli którą kulka będzie przebywać w każdym odświeżeniu
        private double _speed { get; set; }

        private int _radius { get; set; }

        PositionsGenerator pg;

        public Ball(float currentX, float currentY, double speed, int radius)
        {
            _currentCoordinates.X = currentX;
            _currentCoordinates.Y = currentY;
            _speed = speed;
            _radius = radius;

            pg = new PositionsGenerator(radius);
        }       

        public void Move()
        {

        }
        //metoda sprawdza czy pozycja piłeczki pokrywa się z współrzędnymi celu
        //jeśli tak generuje nowy cel
        private bool checkIfReachedDestination()
        {
            return true;
        }
    }
}
