using Data;
using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class PresentationModelApi : ModelAbstractApi
    {

        private LogicAbstractApi logicApi;
        public PresentationModelApi() {


            logicApi = LogicAbstractApi.CreateAPI();

        }

        public override void ApplyNumberOfBalls(int numberOfBalls, bool isFirst = true)
        {
            if(!isFirst)
            {
                logicApi.StopBalls();
                Debug.WriteLine("Current BouncyBalls tasks have been stopped");
            }

            logicApi.CreateBalls(numberOfBalls);
        }

        public override ObservableCollection<Ball> GetBalls()
        {
            return logicApi.getBalls();
        }

        public override void StartGame()
        {
            logicApi.RunBalls();
        }

    }
}
