﻿using AirportTimeTable.Models;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;

namespace AirportTable.ViewModels {
    public class Log {
        static readonly bool use_file = false;

        static readonly List<string> logs = new();
        static readonly string path = "../../../Log.txt";
        static bool first = true;

        public static MainWindowViewModel? Mwvm { private get; set; }
        public static void Write(string message, bool without_update = false) {
            if (!without_update) {
                foreach (var mess in message.Split('\n')) logs.Add(mess);
                while (logs.Count > 55) logs.RemoveAt(0);

                if (Mwvm != null) Mwvm.Logg = string.Join('\n', logs);
            }

            if (use_file) {
                if (first) File.WriteAllText(path, message + "\n");
                else File.AppendAllText(path, message + "\n");
                first = false;
            }
        }
    }

    public class MainWindowViewModel: ViewModelBase {
        private string log = "";
        public string Logg { get => log; set { this.RaiseAndSetIfChanged(ref log, value); } }

        readonly BaseReader br = new();

        public MainWindowViewModel() {
            Log.Mwvm = this;
            SelectA = ReactiveCommand.Create<Unit, Unit>(_ => { FuncSelectA(); return new Unit(); });
            SelectB = ReactiveCommand.Create<Unit, Unit>(_ => { FuncSelectB(); return new Unit(); });
        }

        Button? button_a, button_b;
        public void AddWindow(Window mw) {
            button_a = mw.Find<Button>("Button_A");
            button_b = mw.Find<Button>("Button_B");
            SetButtonState(0, true);
        }

        private void SetButtonState(int num, bool state) {
            var button = num == 0 ? button_a : button_b;
            if (button == null) return;
            button.Background = new SolidColorBrush(Color.Parse(state ? "#EB7501" : "#323B44"));

            var canvas = (Canvas) ((AvaloniaList<ILogical>) button.GetLogicalChildren())[0];
            var app = Application.Current ?? throw new System.Exception("Чё?!");
            var ress = app.Resources;

            var res = ress[num == 0 ? (state ? "departure_B" : "departure_A") : (state ? "landing_B" : "landing_A")];
            var img2 = (Image) (res ?? throw new System.Exception("Чё?!"));
            var img = (Image) canvas.Children[0];
            img.Source = img2.Source;

            var tb = (TextBlock) canvas.Children[1];
            tb.Foreground = new SolidColorBrush(Color.Parse(state ? "#1C242B" : "#6F788B"));
        }

        public TableItem[] Items { get => br.data[0].Select(x => new TableItem((object[]) x, br)).ToArray(); }



        bool selected = false;
        void SelectButton(bool newy) {
            if (selected == newy) return;
            selected = newy;
            SetButtonState(0, !newy);
            SetButtonState(1, newy);
        }
        void FuncSelectA() => SelectButton(false);
        void FuncSelectB() => SelectButton(true);
        public ReactiveCommand<Unit, Unit> SelectA { get; }
        public ReactiveCommand<Unit, Unit> SelectB { get; }
    }
}