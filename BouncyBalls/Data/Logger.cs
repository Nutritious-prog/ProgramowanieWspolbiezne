using System.Collections.ObjectModel;
using System.Text.Json;

namespace Data
{
    public class Logger : LoggerApi
    {
     
        public override void SaveLogsToFile(ObservableCollection<Ball> balls)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var objectToSerialize = new
            {
                Timestamp = DateTime.Now,
                Balls = balls
            };

            string json = JsonSerializer.Serialize(objectToSerialize, jsonOptions);

       
            File.AppendAllText(Path.GetFullPath(@"C:\Users\talla\Desktop\Studia\Rok_2\Semestr_4\wspolbiezne\Wspolbiezne_new\BouncyBalls\Data\logs.json"), json);
            
            
        }
    }
}
