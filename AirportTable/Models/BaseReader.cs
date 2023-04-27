using AirportTable.Models;
using AirportTable.ViewModels;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Linq;

namespace AirportTimeTable.Models {
    public class BaseReader {
        public BaseReader() {
            string path = (Directory.GetCurrentDirectory().Contains("AirportTable") ? "../../.." : "AirportTable") + "/Assets/Misc/storage.db";
            using var con = new SqliteConnection("Data Source=" + path);
            con.Open();
            using var reader = new SqliteCommand("SELECT * FROM content", con).ExecuteReader();
            if (!reader.HasRows) return;
            while (reader.Read()) {
                var row = Enumerable.Range(0, reader.VisibleFieldCount).Select(x => reader[x]).ToArray();
                // Log.Write("YEAH " + Utils.Obj2json(row));
                long day = (long) row[1];
                Log.Write("Yeah: " + day + " " + row[2] + " " + Utils.Json2obj((string) row[2]));
            }
        }
    }
}
