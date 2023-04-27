using AirportTable.ViewModels;
using Avalonia.Controls;

namespace AirportTable.Views {
    public partial class MainWindow: Window {
        public MainWindow() {
            InitializeComponent();
            var mwvm = new MainWindowViewModel();
            DataContext = mwvm;
            mwvm.AddWindow(this);
        }
    }
}