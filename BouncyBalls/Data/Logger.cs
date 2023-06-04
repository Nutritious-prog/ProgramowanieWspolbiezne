using System.Text.Json;

namespace Data
{
    public class Logger : LoggerApi
    {
        private object _lock = new object();

        public override void SaveLogsToFile(Ball ball)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var objectToSerialize = new
            {
                Timestamp = DateTime.Now,
                Ball = ball
            };

            string json = JsonSerializer.Serialize(objectToSerialize, jsonOptions);

            lock (_lock)
            {
                File.AppendAllText(Path.GetFullPath(@"C:\Users\talla\Desktop\Studia\Rok_2\Semestr_4\wspolbiezne\Wspolbiezne_new\BouncyBalls\Data\logs.json"), json);
                //File.AppendAllText(Path.GetFullPath(@".\logs.json"), json);
            }
        }
    }
}
