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
        private const int CarWidth = 48, CarHeight = 48;
        private static int Direction { get; set; }
        private static int globalX, globalY, MaxWidth, MaxHeight;
        private static Race CurrentRace;


        public static void init(Race race)
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
                var filledWithDriversTrack = SetDrivers(filledTrack, track);
                var drawedMap = Cache.CreateBitmapSourceFromGdiBitmap(filledWithDriversTrack);
                return drawedMap;
            }
            return null;
        }

        private static Bitmap SetDrivers(Bitmap filledTrack, Track track)
        {
            var currentX = -globalX;
            var currentY = -globalY;
            foreach (Section section in track.Sections)
            {
                filledTrack = DrawDriver(filledTrack, section, currentX, currentY);

                Direction = SetNew_direction(section.SectionType, Direction);
                AdjustXY(Direction, currentY, currentX, out currentY, out currentX);



            }
            return filledTrack;
        }

        public static Bitmap SetTrack(Bitmap bitmap, Track track)
        {
            var currentX = -globalX;
            var currentY = -globalY;
            Graphics g = Graphics.FromImage(bitmap);
            foreach (Section section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.StartGrid:
                        Bitmap startGrid = new Bitmap(Cache.GetBitmapFromCache(_start));
                        g.DrawImage(RotateTrack(startGrid, Direction, section.SectionType), new Point(currentX, currentY));
                        break;
                    case SectionTypes.Finish:
                        Bitmap finish = new Bitmap(Cache.GetBitmapFromCache(_finish));
                        g.DrawImage(RotateTrack(finish, Direction, section.SectionType), new Point(currentX, currentY));
                        break;
                    case SectionTypes.LeftCorner:
                        Bitmap leftCorner = new Bitmap(Cache.GetBitmapFromCache(_corner));
                        g.DrawImage(RotateTrack(leftCorner, Direction, section.SectionType), new Point(currentX, currentY));
                        break;
                    case SectionTypes.RightCorner:
                        Bitmap rightCorner = new Bitmap(Cache.GetBitmapFromCache(_corner));
                        g.DrawImage(RotateTrack(rightCorner, Direction, section.SectionType), new Point(currentX, currentY));
                        break;
                    case SectionTypes.Straight:
                        Bitmap straight = new Bitmap(Cache.GetBitmapFromCache(_straight));
                        g.DrawImage(RotateTrack(straight, Direction, section.SectionType), new Point(currentX, currentY));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                };
                Direction = SetNew_direction(section.SectionType, Direction);

                AdjustXY(Direction, currentY, currentX, out currentY, out currentX);
            }

            return bitmap;
        }

        private static void AdjustXY(int direction, int currentY, int currentX, out int newY, out int newX)
        {
            newY = currentY;
            newX = currentX;
            switch (direction)
            {
                case 0:
                    newY = currentY - TileHeight;
                    break;
                case 1:
                    newX = currentX + TileWidth;
                    break;
                case 2:
                    newY = currentY + TileHeight;
                    break;
                case 3:
                    newX = currentX - TileWidth;
                    break;
            }
        }



        #region Rotate
        public static Bitmap RotateTrack(Bitmap image, int dir, SectionTypes section)
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

        public static Bitmap RotateCar(Bitmap image, int dir)
        {
            switch (dir)
            {
                case 0:
                    image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case 2:
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 3:
                    image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;   
            }
            return image;
        }

        #endregion

        internal static void Initialize(Race currentRace)
        {
            CurrentRace = currentRace;
        }

        private static void CalcMaxXY(Track track)
        {
            int x = TileWidth, y = TileHeight, minX = 0, minY = 0, maxX = 0, maxY = 0;
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



        private static Bitmap DrawDriver(Bitmap filledTrack, Section section, int currentX, int currentY)
        {
            Graphics g = Graphics.FromImage(filledTrack);

            int leftX, leftY, rightX, rightY;
            var sectieData = Data.CurrentRace != null ? Data.CurrentRace.GetSectionData(section) : null;

            if (sectieData != null)
            {
                if (sectieData.Left != null)
                {
                    Bitmap car = new Bitmap(Cache.GetBitmapFromCache(GetCarImage(sectieData.Left)));
                    CalculatePosition(section, sectieData, Direction, isLeft: true, out leftX, out leftY);
                    g.DrawImage(RotateCar(car, Direction), new Point (currentX + leftX, currentY + leftY));
                }
                if (sectieData.Right != null)
                {
                    Bitmap car = new Bitmap(Cache.GetBitmapFromCache(GetCarImage(sectieData.Right)));
                    CalculatePosition(section, sectieData, Direction, isLeft: false, out leftX, out leftY);
                    g.DrawImage(RotateCar(car, Direction), new Point(currentX + leftX, currentY + leftY));
                }
            }
            return filledTrack;
        }

        private static void CalculatePosition(Section section, SectionData sectieData, int direction, bool isLeft, out int pX, out int pY)
        {
            switch (section.SectionType)
            {
                case SectionTypes.StartGrid:
                    if (!Data.CurrentRace.LapsDriven.All(d => d.Value == 0))
                    {
                        goto case SectionTypes.Straight;
                    }
                    CalculateStartPosition(direction, isLeft, out pX, out pY);
                    return;
                case SectionTypes.Straight:
                case SectionTypes.Finish:
                    CalculateStraightPositition(sectieData, direction, isLeft, out pX, out pY);
                    return;
                case SectionTypes.LeftCorner:
                    //CalculateCornerPosition(sectieData, direction, isLeft, out pX, out pY);
                    goto case SectionTypes.Straight;
                    return;
                case SectionTypes.RightCorner:
                    //CalculateCornerPosition(sectieData, direction, isLeft, out pX, out pY);
                    goto case SectionTypes.Straight;
                    return;

            }
            pX = 0;
            pY = 0;
        }

        private static void CalculateStartPosition(int direction, bool isLeft, out int pX, out int pY)
        {
            var width = TileWidth - CarWidth;
            var height = TileHeight - CarHeight;
            switch (direction)
            {
                case 0:
                    pX = isLeft ? width / 3 : width / 3 * 2;
                    pY = isLeft ? height / 3 : height / 3 * 2;
                    return;
                case 1:
                    pX = isLeft ? width / 3 * 2 : width / 3;
                    pY = isLeft ? height / 3 * 2 : height / 3;
                    return;
                case 2:
                    pX = isLeft ? width / 3 * 2 : width / 3;
                    pY = isLeft ? height / 3 * 2 : height / 3;
                    return;
                case 3:
                    pX = isLeft ? width / 3 : width / 3 * 2;
                    pY = isLeft ? height / 3 : height / 3 * 2;
                    return;
            }
            pX = 0;
            pY = 0;
        }

        private static void CalculateStraightPositition(SectionData sectieData, int direction, bool isLeft, out int pX, out int pY)
        {
            var width = TileWidth - CarWidth;
            var height = TileHeight - CarHeight;

            switch (direction)
            {
                case 0:
                    pX = isLeft ? width / 3 : width / 3 * 2;
                    pY = isLeft ? sectieData.DistanceLeft / height * 100 : sectieData.DistanceRight / height * 100;
                    return;
                case 1:
                    pX = isLeft ? sectieData.DistanceLeft / width * 100 : sectieData.DistanceRight / width * 100;
                    pY = isLeft ? height / 3 : height / 3 * 2;
                    return;
                case 2:
                    pX = isLeft ? width / 3 * 2 : width / 3;
                    pY = isLeft ? sectieData.DistanceLeft / height * 100 : sectieData.DistanceRight / height * 100;
                    pY = TileHeight - pY;
                    return;
                case 3:
                    pX = isLeft ? sectieData.DistanceLeft / width * 100 : sectieData.DistanceRight / width * 100;
                    pX = TileWidth - pX;
                    pY = isLeft ? height / 3 * 2 : height / 3;
                    return;
            }
            pX = 0;
            pY = 0;
        }

        private static void CalculateCornerPosition(Section section, SectionData sectieData, int direction, bool isLeft, out int pX, out int pY)
        {
            throw new NotImplementedException();
        }





        private static string GetCarImage(IParticipant p)
        {
            switch (p.TeamColor)
            {
                case TeamColors.RED:
                    return p.Equipment.IsBroken ? Resources.RedCar : Resources.RedCar;
                case TeamColors.GREEN:
                    return p.Equipment.IsBroken ? Resources.GreenCar : Resources.GreenCar;
                case TeamColors.YELLOW:
                    return p.Equipment.IsBroken ? Resources.YellowCar : Resources.YellowCar;
                case TeamColors.BLUE:
                    return p.Equipment.IsBroken ? Resources.BlueCar : Resources.BlueCar;
                case TeamColors.GREY:
                    return p.Equipment.IsBroken ? Resources.GreyCar : Resources.GreyCar;
            }
            return null;
        }
    }
}