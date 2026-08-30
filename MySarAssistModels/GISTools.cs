using CoordinateSharp;
using CoordinateSharp.Magnetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySarAssistModels
{
    public static class GISTools
    {
        const double PIx = 3.141592653589793;
        const double RADIO = 6371;
        const double EarthRadius = 6378137;

        public static double GetDeclination(this Coordinate c, DateTime OpDate)
        {
            CoordinateSharp.Coordinate coordinate = new CoordinateSharp.Coordinate(c.Latitude, c.Longitude, OpDate);
            //Height is 0km MSL unless specified in overload constructor.
            Magnetic m = new Magnetic(coordinate);

            return m.MagneticFieldElements.Declination;
        }

        public static DateTime GetSunrise(this Coordinate c, DateTime OpDate)
        {
            Celestial cel = Celestial.CalculateCelestialTimes(c.Latitude, c.Longitude, OpDate);
            return cel.SunRise.Value.ToLocalTime();
        }
        public static DateTime GetSunset(this Coordinate c, DateTime OpDate)
        {
            Celestial cel = Celestial.CalculateCelestialTimes(c.Latitude, c.Longitude, OpDate);
            return cel.SunSet.Value.ToLocalTime();
        }

        public static Coordinate FindCentroid(List<Coordinate> coordinates)
        {
            Coordinate centroid = new Coordinate();

            double accumulatedArea = 0.0f;
            double centerX = 0.0f;
            double centerY = 0.0f;

            for (int i = 0, j = coordinates.Count - 1; i < coordinates.Count; j = i++)
            {
                double temp = coordinates[i].Latitude * coordinates[j].Longitude - coordinates[j].Latitude * coordinates[i].Longitude;
                accumulatedArea += temp;
                centerX += (coordinates[i].Latitude + coordinates[j].Latitude) * temp;
                centerY += (coordinates[i].Longitude + coordinates[j].Longitude) * temp;
            }

            if (Math.Abs(accumulatedArea) < 1E-7f)
                return centroid;  // Avoid division by zero

            accumulatedArea *= 3f;


            centroid.Latitude = centerX / accumulatedArea;
            centroid.Longitude = centerY / accumulatedArea;
            return centroid;
        }

        public static double ConvertToRadian(double input)
        {
            return input * Math.PI / 180;
        }

        public static double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2)
        {
            double dlon = ConvertToRadian(lon2 - lon1);
            double dlat = ConvertToRadian(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(ConvertToRadian(lat1)) * Math.Cos(ConvertToRadian(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * RADIO;
        }

        public static double Bearing(double lat1, double long1, double lat2, double long2)

        {
            /*
            double phi1 = ConvertToRadian(lat1);

            double phi2 = ConvertToRadian(lat2);

            double lam1 = ConvertToRadian(long1);

            double lam2 = ConvertToRadian(long2);
            */
            var dLon = ConvertToRadian(long2 - long1);
            var dPhi = Math.Log(
                Math.Tan(ConvertToRadian(lat2) / 2 + Math.PI / 4) / Math.Tan(ConvertToRadian(lat1) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));

            /*
            return Math.Atan2(Math.Sin(lam2 - lam1) * Math.Cos(phi2),

              Math.Cos(phi1) * Math.Sin(phi2) - Math.Sin(phi1) * Math.Cos(phi2) * Math.Cos(lam2 - lam1)

            ) * 180 / Math.PI;
            */
        }

        public static double ToBearing(double radians)
        {
            // convert radians to degrees (as bearing: 0...360)
            return (ToDegrees(radians) + 360) % 360;
        }
        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double CalculatePolygonAreaSquareMeters(List<Coordinate> coordinates)
        {
            double area = 0;

            if (coordinates.Count > 2)
            {
                for (var i = 0; i < coordinates.Count - 1; i++)
                {
                    Coordinate p1, p2;
                    p1 = coordinates[i];
                    p2 = coordinates[i + 1];
                    area += ConvertToRadian(p2.Longitude - p1.Longitude) * (2 + Math.Sin(ConvertToRadian(p1.Latitude)) + Math.Sin(ConvertToRadian(p2.Latitude)));


                }
                area = area * EarthRadius * EarthRadius / 2;
                area = area / 1000; //convert to meters 
            }

            return Math.Abs(area);
        }



        public static bool ShapeContainsLocation(Coordinate location, List<Coordinate> bounds)
        {

            var lastPoint = bounds[bounds.Count - 1];
            var isInside = false;
            var x = location.Longitude;
            foreach (var point in bounds)
            {
                var x1 = lastPoint.Longitude;
                var x2 = point.Longitude;
                var dx = x2 - x1;

                if (Math.Abs(dx) > 180.0)
                {
                    // we have, most likely, just jumped the dateline (could do further validation to this effect if needed).  normalise the numbers.
                    if (x > 0)
                    {
                        while (x1 < 0)
                            x1 += 360;
                        while (x2 < 0)
                            x2 += 360;
                    }
                    else
                    {
                        while (x1 > 0)
                            x1 -= 360;
                        while (x2 > 0)
                            x2 -= 360;
                    }
                    dx = x2 - x1;
                }

                if ((x1 <= x && x2 > x) || (x1 >= x && x2 < x))
                {
                    var grad = (point.Latitude - lastPoint.Latitude) / dx;
                    var intersectAtLat = lastPoint.Latitude + ((x - x1) * grad);

                    if (intersectAtLat > location.Latitude)
                        isInside = !isInside;
                }
                lastPoint = point;
            }

            return isInside;
        }


    }

    public class Coordinate
    {
        private int decimalPlaces = 6;

        public string Label { get; set; }
        public string CardinalLabel { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Accuracy { get; set; } = 0;
        public double? Altitude { get; set; }
        public Coordinate(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }
        public Coordinate(double lat, double lng, string lbl)
        {
            Latitude = lat;
            Longitude = lng;
            Label = lbl;
        }

        public Coordinate() { }


        public string CoordinateOutput(string format)
        {
            StringBuilder coordText = new StringBuilder();
            switch (format)
            {
                case "Decimal Degrees":
                    coordText.Append(Math.Round(Latitude, decimalPlaces)); coordText.Append(", "); coordText.Append(Math.Round(Longitude, decimalPlaces));
                    break;
                case "Degrees Decimal Minutes":
                    coordText.Append(DegreesDecimalMinutes);
                    break;
                case "Degrees Minutes Seconds":
                    coordText.Append(DegreesMinutesSeconds);
                    break;
                case "MGRS":
                    coordText.Append(MGRS);
                    break;
                default: //utm
                    coordText.Append(UTM);
                    break;
            }
            return coordText.ToString();
        }

        public string DecimalDegrees
        {
            get
            {
                return Math.Round(Latitude, decimalPlaces).ToString() + ", " + Math.Round(Longitude, decimalPlaces).ToString();
            }
        }
        public string DecimalDegreesForURL
        {
            get
            {
                return Math.Round(Latitude, decimalPlaces).ToString() + "," + Math.Round(Longitude, decimalPlaces).ToString();
            }
        }
        public string DegreesMinutesSeconds
        {
            get
            {
                CoordinateSharp.Coordinate coordinate = new CoordinateSharp.Coordinate();
                coordinate.Latitude = new CoordinatePart(Latitude, CoordinateType.Lat);
                coordinate.Longitude = new CoordinatePart(Longitude, CoordinateType.Long);
                return coordinate.ToString();
            }
        }

        public string DDMLatitude
        {
            get
            {
                return DegreesDecimalMinutesSep[0];

            }
        }
        public string DDMLongitude
        {
            get
            {
                return DegreesDecimalMinutesSep[1];

            }
        }


        public string DMSLatitude
        {
            get
            {
                CoordinateSharp.Coordinate coordinate = new CoordinateSharp.Coordinate();
                coordinate.Latitude = new CoordinatePart(Latitude, CoordinateType.Lat);
                coordinate.Longitude = new CoordinatePart(Longitude, CoordinateType.Long);
                return coordinate.Latitude.ToString();

            }
        }
        public string DMSLongitude
        {
            get
            {
                CoordinateSharp.Coordinate coordinate = new CoordinateSharp.Coordinate();
                coordinate.Latitude = new CoordinatePart(Latitude, CoordinateType.Lat);
                coordinate.Longitude = new CoordinatePart(Longitude, CoordinateType.Long);
                return coordinate.Longitude.ToString();

            }
        }



        public string[] DegreesDecimalMinutesSep
        {
            get
            {
                string[] parts = new string[2];
                CoordinateSharp.Coordinate coordinate = new CoordinateSharp.Coordinate();
                coordinate.Latitude = new CoordinatePart(Latitude, CoordinateType.Lat);
                coordinate.Longitude = new CoordinatePart(Longitude, CoordinateType.Long);
                parts[0] = coordinate.Latitude.Degrees + "° " + Math.Round(coordinate.Latitude.DecimalMinute, decimalPlaces);
                //if(coordinate.Longitude.)

                if (coordinate.Longitude.DecimalDegree < 0) { parts[1] += "-"; }
                parts[1] += coordinate.Longitude.Degrees + "° " + Math.Round(coordinate.Longitude.DecimalMinute, decimalPlaces);

                return parts;
            }
        }
        public string DegreesDecimalMinutes
        {
            get
            {
                CoordinateSharp.Coordinate coordinate = new CoordinateSharp.Coordinate();
                coordinate.Latitude = new CoordinatePart(Latitude, CoordinateType.Lat);
                coordinate.Longitude = new CoordinatePart(Longitude, CoordinateType.Long);
                string coord = coordinate.Latitude.Degrees + "°" + coordinate.Latitude.DecimalMinute + ", ";
                //if(coordinate.Longitude.)
                if (coordinate.Longitude.DecimalDegree < 0) { coord += "-"; }
                coord += coordinate.Longitude.Degrees + "°" + coordinate.Longitude.DecimalMinute;
                return coord;
            }

        }
        public string UTM
        {
            get
            {
                CoordinateSharp.Coordinate coordinate = new CoordinateSharp.Coordinate();
                coordinate.Latitude = new CoordinatePart(Latitude, CoordinateType.Lat);
                coordinate.Longitude = new CoordinatePart(Longitude, CoordinateType.Long);
                string utm = coordinate.UTM.ToString();
                utm = utm.Replace("m", "");
                return utm;

            }
        }

        public string ShortUTM
        {
            get
            {
                try
                {
                    CoordinateSharp.Coordinate coordinate = new CoordinateSharp.Coordinate();
                    coordinate.Latitude = new CoordinatePart(Latitude, CoordinateType.Lat);
                    coordinate.Longitude = new CoordinatePart(Longitude, CoordinateType.Long);
                    string utm = coordinate.UTM.ToString();

                    utm = utm.Replace("m", "");
                    StringBuilder sh = new StringBuilder();
                    int firstSpace = utm.IndexOf(" ");
                    int secondSpace = utm.IndexOf(" ", firstSpace + 1);
                    //sh.Append(utm.Substring(firstSpace + 3, 3)); sh.Append(" "); sh.Append(utm.Substring(secondSpace + 3, 3));
                    sh.Append(utm.Substring(secondSpace - 6, 3)); sh.Append(" "); sh.Append(utm.Substring(utm.Length-6, 3));
                    return sh.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public string MGRS
        {
            get
            {

                CoordinateSharp.Coordinate coordinate = new CoordinateSharp.Coordinate();
                coordinate.Latitude = new CoordinatePart(Latitude, CoordinateType.Lat);
                coordinate.Longitude = new CoordinatePart(Longitude, CoordinateType.Long);
                string mgrs = coordinate.MGRS.ToString();
                return mgrs;
            }
        }
        public double distanceFromCentre(Coordinate centre)
        {
            return GISTools.DistanceBetweenPlaces(centre.Longitude, centre.Latitude, Longitude, Latitude);
        }

        public double bearingFromCentre(Coordinate centre)
        {
            return GISTools.Bearing(centre.Latitude, centre.Longitude, Latitude, Longitude);
        }
        public double AbsDegreesFromCentre(Coordinate centre, double target)
        {
            //use offset to quickly find the closest to any bearing. 
            //eg to find the point closest to NE, enter 45 for offset.
            double fromCentre = bearingFromCentre(centre);
            double difference = 0;
            if (fromCentre > target) { difference = fromCentre - target; }
            else { difference = target - fromCentre; }
            return Math.Abs(difference);
        }

        public bool TryParseCoordinate(string str, out Coordinate coordinate)
        {
            bool success = false;
            try
            {
                coordinate = new Coordinate();

                if (!string.IsNullOrEmpty(str))
                {
                    CoordinateSharp.Coordinate temp = new CoordinateSharp.Coordinate();
                    success = CoordinateSharp.Coordinate.TryParse(str, out temp);
                    if (!success)
                    {
                        str = str.Replace("E", "mE").Replace("N", "mN");
                        success = CoordinateSharp.Coordinate.TryParse(str, out temp);
                    }
                    if (success)
                    {
                        coordinate.Latitude = temp.Latitude.DecimalDegree;
                        coordinate.Longitude = temp.Longitude.DecimalDegree;
                    }
                }
                return success;
            }
            catch (Exception)
            {
                coordinate = null;
                return success;
            }
        }


    }
}
