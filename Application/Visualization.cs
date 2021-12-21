using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Application
{
    public class Visualization
    {
        #region graphics

        private static readonly string _start = Resources.Start;
        private static readonly string _finish = Resources.Finish;
        private static readonly string _straight = Resources.Straight;
        private static readonly string _corner = Resources.Corner;
        private static readonly string _greenCar = Resources.GreenCar;
        private static readonly string _redCar = Resources.RedCar;
        private static readonly string _blueCar = Resources.BlueCar;
        private static readonly string _yellowCar = Resources.YellowCar;
        private static readonly string _greyCar = Resources.GreyCar;
        #endregion

        private const int TileHeight = 128, TileWidth = 128;
        private static int Direction { get; set; }
        private static int globalX, globalY, MaxWidth, MaxHeight;
        private static Race CurrentRace;


        public static void init (Race race)
        { 
            CurrentRace = race;
        }
        public static BitmapSource DrawTrack(Track track)
        {
            if (track != null)
            {
                CalcMaxXY(track);
                var emptyTrack = Cache.CreateEmptyBitmap(MaxWidth, MaxHeight);
                var filledTrack = SetTrack(emptyTrack, track);
                var drawedMap = Cache.CreateBitmapSourceFromGdiBitmap(filledTrack);
                return drawedMap;
            }
            return null;
        }

        public static Bitmap SetTrack(Bitmap bitmap, Track track)
        {

            int currentX, currentY;
            currentX = -globalX;
            currentY = -globalY;
            Graphics g = Graphics.FromImage(bitmap);
            foreach (Section section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.StartGrid:
                        Bitmap startGrid = new Bitmap(Cache.GetBitmapFromCache(_start));
                        g.DrawImage(Rotate(startGrid, Direction, section.SectionType), new Point(currentX, currentY));
                        break;
                    case SectionTypes.Finish:
                        Bitmap finish = new Bitmap(Cache.GetBitmapFromCache(_finish));
                        g.DrawImage(Rotate(finish, Direction, section.SectionType), new Point(currentX, currentY));
                        break;
                    case SectionTypes.LeftCorner:
                        Bitmap leftCorner = new Bitmap(Cache.GetBitmapFromCache(_corner));
                        g.DrawImage(Rotate(leftCorner, Direction, section.SectionType), new Point(currentX, currentY)); 
                        break;
                    case SectionTypes.RightCorner:
                        Bitmap rightCorner = new Bitmap(Cache.GetBitmapFromCache(_corner));
                        g.DrawImage(Rotate(rightCorner, Direction, section.SectionType), new Point(currentX, currentY));
                        break;
                    case SectionTypes.Straight:
                        Bitmap straight = new Bitmap(Cache.GetBitmapFromCache(_straight));
                        g.DrawImage(Rotate(straight, Direction, section.SectionType), new Point(currentX, currentY));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                };
                Direction = SetNew_direction(section.SectionType, Direction);
                switch (Direction)
                {
                    case 0:
                        currentY -= TileHeight;
                        break;
                    case 1:
                        currentX += TileWidth;
                        break;
                    case 2:
                        currentY += TileHeight;
                        break;
                    case 3:
                        currentX -= TileWidth;
                        break;
                }
            }

            return bitmap;
        }

        public static Bitmap Rotate(Bitmap image, int dir, SectionTypes section)
        {
            switch (section)
            {
                case SectionTypes.Straight:
                    switch (dir)
                    {
                        case 0:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                        case 1:
                            break;
                        case 2:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 3:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                    }
                    break;
                case SectionTypes.LeftCorner:
                    switch (dir)
                    {
                        case 0:
                            break;
                        case 1:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 2:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 3:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
                    break;
                case SectionTypes.RightCorner:
                    switch (dir)
                    {
                        case 0:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                        case 1:
                            break;
                        case 2:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 3:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                    }
                    break;
                case SectionTypes.StartGrid:
                    switch (dir)
                    {
                        case 0:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                        case 1:
                            break;
                        case 2:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 3:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                    }
                    break;
                case SectionTypes.Finish:
                    switch (dir)
                    {
                        case 0:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                        case 1:
                            break;
                        case 2:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 3:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                    }
                    break;
            }
            return image;
        }



            internal static void Initialize(Race currentRace)
        {
            CurrentRace = currentRace;
        }

        private static void CalcMaxXY(Track track)
        {
            int x = TileWidth, y = TileHeight, minX = 0, minY = 0, maxX = 0 , maxY = 0;
            Direction = track.StartingDirection;
            foreach (var section in track.Sections.ToList())
            {
                Direction = SetNew_direction(section.SectionType, Direction);
                switch (Direction)
                {
                    case 0:
                        y -= TileHeight;
                        if (y < minY) minY = y;
                        break;
                    case 1:
                        x += TileWidth;
                        if (x > maxX) maxX = x;
                        break;
                    case 2:
                        y += TileHeight;
                        if (y > maxY) maxY = y;
                        break;
                    case 3:
                        x -= TileWidth;
                        if (x < minX) minX = x;
                        break;
                }
            }

            globalX = -minX;
            globalY = minY - TileHeight;

            MaxWidth = maxX - minX;
            MaxHeight = maxY - minY + TileHeight;
        }
        public static int SetNew_direction(SectionTypes sectionType, int dir)
        {
            switch (sectionType)
            {
                case SectionTypes.LeftCorner:
                    if (dir == 0) return 3;
                    dir -= 1;
                    break;
                case SectionTypes.RightCorner:
                    dir += 1;
                    break;
            }

            return dir %= 4;
        }
        private static void OnDriverChanged(object sender, DriversChangedEventArgs e)
        {
            DrawTrack(e.Track);
        }

        public static void OnNextRace(object sender, NextRaceEventArgs e)
        {
            Initialize(e.Race);

            CurrentRace.DriverChanged += OnDriverChanged;

        }

        }
    }