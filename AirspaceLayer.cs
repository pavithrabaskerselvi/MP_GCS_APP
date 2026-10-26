using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using GMap.NET;
using GMap.NET.WindowsForms;
using Newtonsoft.Json.Linq;

namespace MissionPlanner
{
    public static class AirspaceLayer
    {
        static readonly Dictionary<GMapControl, GMapOverlay> overlays =
            new Dictionary<GMapControl, GMapOverlay>();

        public static bool IsOn(GMapControl map) => overlays.ContainsKey(map);

        public static void Toggle(GMapControl map)
        {
            if (IsOn(map))
            {
                map.Overlays.Remove(overlays[map]);
                overlays.Remove(map);
            }
            else
            {
                string file = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "airspace.geojson");
                if (!File.Exists(file))
                {
                    System.Windows.Forms.MessageBox.Show("airspace.geojson not found:\n" + file);
                    return;
                }
                var ov = new GMapOverlay("airspace");
                Load(file, ov);
                map.Overlays.Insert(0, ov);   // under waypoints/drone icon
                overlays[map] = ov;
            }
            map.Refresh();
        }

        static void Load(string file, GMapOverlay ov)
        {
            var root = JObject.Parse(File.ReadAllText(file));
            // draw order: green (bottom), yellow, red (top)
            foreach (string zone in new[] { "green", "yellow", "red" })
            {
                foreach (var f in root["features"])
                {
                    string z = ((string)f["properties"]["zone"] ?? "green").ToLower();
                    if (z != zone) continue;

                    var g = f["geometry"];
                    string type = (string)g["type"];
                    string name = (string)f["properties"]["name"];
                    if (type == "Polygon")
                        AddPoly(ov, g["coordinates"], zone, name);
                    else if (type == "MultiPolygon")
                        foreach (var poly in g["coordinates"])
                            AddPoly(ov, poly, zone, name);
                }
            }
        }

        static void AddPoly(GMapOverlay ov, JToken rings, string zone, string name)
        {
            Color c; int alpha; float width;
            if (zone == "red")         { c = Color.Red;       alpha = 110; width = 1.5f; }
            else if (zone == "yellow") { c = Color.Gold;      alpha = 80;  width = 1.5f; }
            else                       { c = Color.LimeGreen; alpha = 55;  width = 1f;   }

            var pts = new List<PointLatLng>();
            foreach (var p in rings[0])                                   // outer ring
                pts.Add(new PointLatLng((double)p[1], (double)p[0]));     // lat, lon

            ov.Polygons.Add(new GMapPolygon(pts, name ?? "zone")
            {
                Fill = new SolidBrush(Color.FromArgb(alpha, c)),
                Stroke = new Pen(c, width)
            });
        }
    }
}
