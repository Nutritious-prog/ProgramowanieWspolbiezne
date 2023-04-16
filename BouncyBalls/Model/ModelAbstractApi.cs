using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class ModelAbstractApi
    {

        public abstract ObservableCollection<Ball> GetBalls();

        // isFirst informuje, czy jest to 1 stworzenie kulek. W kolejnych stworzeniach musimy zakończyć wcześniejsze wątki
        public abstract void ApplyNumberOfBalls(int numberOfBalls, bool isFirst = true); 
        public abstract void StartGame();


        public static ModelAbstractApi CreateApi()
        {
            return new PresentationModelApi();
        }

        

    }
}
