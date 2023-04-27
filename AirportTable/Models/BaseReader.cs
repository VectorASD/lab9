using AirportTable.Models;
using Avalonia.Media.Imaging;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AirportTimeTable.Models {
    public class BaseReader {
        public TableItem[][] data = Array.Empty<TableItem[]>();
        public Dictionary<string, Bitmap> images = new();

        public BaseReader() {
            string path = (Directory.GetCurrentDirectory().Contains("AirportTable") ? "../../.." : "AirportTable") + "/Assets/Misc/storage.db";
            using var con = new SqliteConnection("Data Source=" + path);
            con.Open();

            using (var reader2 = new SqliteCommand("SELECT * FROM images", con).ExecuteReader()) {
                if (!reader2.HasRows) return;

                while (reader2.Read()) {
                    var row = Enumerable.Range(0, reader2.VisibleFieldCount).Select(x => reader2[x]).ToArray();
                    var id = (string) row[0];
                    images[id] = ((string) row[1]).Base64toBitmap();
                }
            }

            using var reader = new SqliteCommand("SELECT * FROM content", con).ExecuteReader();
            if (!reader.HasRows) return;

            var res = new List<TableItem>[4];
            for (int i = 0; i < 4; i++) res[i] = new();

            while (reader.Read()) {
                var row = Enumerable.Range(0, reader.VisibleFieldCount).Select(x => reader[x]).ToArray();
                long day = (long) row[1];
                var obj = Utils.Json2obj((string) row[2]) ?? throw new System.Exception("Чё?!");
                res[day].Add(new TableItem((object[]) obj, this));
            }
            data = res.Select(x => x.ToArray()).ToArray();
        }

        // Рандомит некоторые вещи, связанные с текущим временем ;'-}
        public void TimeBomb() {

        }
    }
}
