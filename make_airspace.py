import csv, json, math, urllib.request, io

URL = "https://davidmegginson.github.io/ourairports-data/airports.csv"
rows = csv.DictReader(io.StringIO(urllib.request.urlopen(URL).read().decode("utf-8")))

def circle(lat, lon, km, n=48):
    pts = []
    for i in range(n + 1):
        a = 2 * math.pi * i / n
        dlat = (km / 111.32) * math.sin(a)
        dlon = (km / (111.32 * math.cos(math.radians(lat)))) * math.cos(a)
        pts.append([round(lon + dlon, 5), round(lat + dlat, 5)])
    return pts

def feat(zone, name, ring):
    return {"type": "Feature",
            "properties": {"zone": zone, "name": name},
            "geometry": {"type": "Polygon", "coordinates": [ring]}}

yellow, red = [], []
for r in rows:
    if r["iso_country"] == "IN" and r["type"] in ("large_airport", "medium_airport"):
        lat, lon = float(r["latitude_deg"]), float(r["longitude_deg"])
        yellow.append(feat("yellow", r["name"], circle(lat, lon, 12)))
        red.append(feat("red", r["name"], circle(lat, lon, 5)))

# yellow first, so red draws on top
json.dump({"type": "FeatureCollection", "features": yellow + red},
          open("airspace.geojson", "w"))
print(len(red), "airports written")