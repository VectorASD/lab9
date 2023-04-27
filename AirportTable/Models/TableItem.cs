using AirportTable.ViewModels;
using Avalonia.Media.Imaging;

namespace AirportTimeTable.Models {
    public class TableItem {
        public Bitmap Image { get; }
        public string Flight { get; }
        public string Destination { get; }
        public string Time { get; }
        public string TimeCount { get; }
        public string Terminal { get; }
        public string Status { get; }

        public Bitmap BigImage { get; }
        public string Path { get; }
        public bool IsDeparture { get; }

        public TableItem(object[] data, BaseReader br) {
            Image = br.images[(string) data[0]];
            Flight = (string) data[1];
            Destination = (string) data[2];
            Time = (string) data[3];
            TimeCount = (string) data[4];
            Terminal = (string) data[5];
            Status = (string) data[6];

            BigImage = br.images[(string) data[7]];
            Path = (string) data[8];
            IsDeparture = Path.StartsWith("Новосибирск");
        }
    }
}
