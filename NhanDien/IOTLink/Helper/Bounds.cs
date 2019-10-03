﻿using NhanDien.IOTLink.Process.Model;
using System;
using System.Collections.Generic;

namespace NhanDien.IOTLink.Helper
{
    public class Bounds
    {
        /// <summary>
        /// Box nam tây bắc đông  cách nhau bởi dấu phẩy
        /// </summary>
        public string Box { get; private set; }

        /// <summary>
        /// Nam tây bắc đông (South, West, North, East) ~ (minLat, minLng, maxLat, maxLng)
        /// </summary>
        public IList<double> SWNE { get; private set; }

        /// <summary>
        /// Tọa độ nhỏ
        /// </summary>
        public Location Min { get; private set; }

        /// <summary>
        /// Tọa độ trung tâm
        /// </summary>
        public Location Middle { get; private set; }

        /// <summary>
        /// Tọa độ lớn
        /// </summary>
        public Location Max { get; private set; }

        /// <summary>
        /// Mức zoom
        /// </summary>
        public int Zoom { get; private set; }

        /// <summary>
        /// X
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Maximum longtitude
        /// </summary>
        public double MaxLng { get; set; }

        /// <summary>
        /// Maximum latitude
        /// </summary>
        public double MaxLat { get; set; }

        /// <summary>
        /// Minimum longtitude
        /// </summary>
        public double MinLng { get; set; }

        /// <summary>
        /// Minimum latitude
        /// </summary>
        public double MinLat { get; set; }

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Bounds(int x, int y, int z)
        {
            Zoom = z;
            X = x;
            Y = y;
            var south = Tile2Lat(y, z);
            var north = Tile2Lat(y + 1, z);
            var west = Tile2Lng(x, z);
            var east = Tile2Lng(x + 1, z);
            SetMinMiddleMax(south, west, north, east);
        }

        /// <summary>
        /// Box
        /// </summary>
        /// <param name="box"></param>
        public Bounds(string box)
        {
            Box = box;
            var temp = box.Split(",");
            var south = double.Parse(temp[0]);
            var west = double.Parse(temp[1]);
            var north = double.Parse(temp[2]);
            var east = double.Parse(temp[3]);
            SetMinMiddleMax(south, west, north, east);
        }

        /// <summary>
        /// Set min middle max
        /// </summary>
        /// <param name="south"></param>
        /// <param name="west"></param>
        /// <param name="north"></param>
        /// <param name="east"></param>
        private void SetMinMiddleMax(double south, double west, double north, double east)
        {
            Min = new Location
            {
                Lat = south,
                Lng = west
            };
            Max = new Location
            {
                Lat = north,
                Lng = east
            };
            SWNE = new List<double> { south, west, north, east };
            Box = string.Join(",", SWNE);
            Middle = new Location
            {
                Lat = (Min.Lat + Max.Lat) / 2,
                Lng = (Min.Lng + Max.Lng) / 2
            };
            MinLat = south;
            MinLng = west;
            MaxLat = north;
            MaxLng = east;
        }

        /// <summary>
        /// Tile to latitude
        /// </summary>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public double Tile2Lat(int y, int z)
        {
            double n = Math.PI - (2.0 * Math.PI * y / Math.Pow(2.0, z));
            return 180.0 / Math.PI * Math.Atan(Math.Sinh(n));
        }

        /// <summary>
        /// Tile to longitude
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public double Tile2Lng(int x, int z)
        {
            return (x / Math.Pow(2.0, z) * 360.0) - 180;
        }

        /// <summary>
        /// Pixels to meters
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public IList<double> PixelsToMeters(int x, int y, int z)
        {
            var res = 2 * Math.PI * Constant.R / 256 / Math.Pow(2, z);
            var mx = (x * res) - (2 * Math.PI * Constant.R / 2.0);
            var my = (y * res) - (2 * Math.PI * Constant.R / 2.0);
            my = -my;
            return new List<double> { mx, my };
        }

        /// <summary>
        /// Point to latitude longitutde
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public Location MetersToLatLng(IList<double> coord)
        {
            var lng = coord[0] / (2 * Math.PI * Constant.R / 2.0) * 180.0;
            var lat = coord[1] / (2 * Math.PI * Constant.R / 2.0) * 180.0;
            lat = 180 / Math.PI * ((2 * Math.Atan(Math.Exp(lat * Math.PI / 180.0))) - (Math.PI / 2.0));
            return new Location
            {
                Lat = lat,
                Lng = lng
            };
        }
    }
}