using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Logic;
using Model;

namespace ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {


        private ModelAbstractApi _modelAPI;

        private bool first = true;

        public ICommand Apply { get; set; }
        public ICommand Start { get; set; }
        public ObservableCollection<Ball> ObsCollBall => _modelAPI.GetBalls();

        public MainWindowViewModel()
        {

            _modelAPI = ModelAbstractApi.CreateApi();
            Apply = new RelayCommand(() =>
            {            
                _modelAPI.ApplyNumberOfBalls(NrOfBalls, first);
                first = false;
            });
            Start = new RelayCommand(() => _modelAPI.StartGame());
        }



        private int _numberOfBalls;
        public int NrOfBalls
        {
            get { return _numberOfBalls; }
            set
            {
                if (value != _numberOfBalls)
                {
                    _numberOfBalls = value;
                    OnPropertyChanged("NrOfBalls");
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {

            throw new NotImplementedException(); 
        }
    }
}
