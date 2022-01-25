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
        public static int GetRemainingTileHeightSpace { get => TileHeight - CarHeight; }
        public static int GetRemainingTileWidthSpace { get => TileHeight - CarHeight; }
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
                CalcMaxXY(track, out MaxWidth, out MaxHeight, out globalX, out globalY); ;
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
            var currentX = globalX;
            var currentY = globalY;
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
            var currentX = globalX;
            var currentY = globalY;
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
        private static Bitmap ExtraCarRotation(bool isLeft, Section section, SectionData sectionData, Bitmap car)
        {
            var returnCar = new Bitmap(car.Width, car.Height);
            if (section.SectionType == SectionTypes.LeftCorner || section.SectionType == SectionTypes.RightCorner)
            {
                float distance = isLeft ? sectionData.DistanceLeft : sectionData.DistanceRight;
                var angle = distance / Section.SectionLength * 90;
                if (section.SectionType == SectionTypes.LeftCorner)
                {
                    angle = -angle;
                }
                Graphics g = Graphics.FromImage(returnCar);

                g.TranslateTransform((float)returnCar.Width / 2, (float)returnCar.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)returnCar.Width / 2, -(float)returnCar.Height / 2);
                g.DrawImage(car, new Point(0, 0));

                return returnCar;
            }
            else
            {
                return car;
            }
        }

        #endregion

        public static void CalcMaxXY(Track track, out int maxWidth, out int maxHeight, out int cursorX, out int cursorY)
        {
            int x = 0, y = 0, minX = 0, minY = 0, maxX = 0, maxY = 0;
            Direction = track.StartingDirection;

            foreach (var section in track.Sections.ToList())
            {
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

                Direction = SetNew_direction(section.SectionType, Direction);
            }

            switch (track.StartingDirection)
            {
                case 0:
                    maxX += TileWidth;
                    break;
                case 1:
                    minY -= TileHeight;
                    break;
                case 2:
                    maxX += TileWidth;
                    break;
                case 3:
                    minY -= TileHeight;
                    break;
            }

            cursorX = -minX;
            cursorY = -minY - TileHeight;

            maxWidth = maxX - minX;
            maxHeight = maxY - minY + TileHeight;
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

        private static Bitmap DrawDriver(Bitmap filledTrack, Section section, int currentX, int currentY)
        {
            Graphics g = Graphics.FromImage(filledTrack);

            int leftX, leftY, rightX, rightY;
            var sectieData = Data.CurrentRace != null ? Data.CurrentRace.GetSectionData(section) : null;
            var isStart = !Data.CurrentRace.LapsDriven.All(d => d.Value == 0);

            if (sectieData != null)
            {
                if (sectieData.Left != null)
                {
                    Bitmap car = RotateCar(new Bitmap(Cache.GetBitmapFromCache(GetCarImage(sectieData.Left)), new Size(CarWidth, CarHeight)), Direction);
                    CalculatePosition(section, sectieData, Direction, isLeft: true, isStart, out leftX, out leftY);

                    car = ExtraCarRotation(true, section, sectieData, car);

                    g.DrawImage(car, new Point(currentX + leftX, currentY + leftY));
                }
                if (sectieData.Right != null)
                {

                    Bitmap car = RotateCar(new Bitmap(Cache.GetBitmapFromCache(GetCarImage(sectieData.Right)), new Size(CarWidth, CarHeight)), Direction);
                    CalculatePosition(section, sectieData, Direction, isLeft: false, isStart, out rightX, out rightY);
                    car = ExtraCarRotation(false, section, sectieData, car);

                    g.DrawImage(car, new Point(currentX + rightX, currentY + rightY));
                }
            }
            g.Dispose();
            return filledTrack;
        }

        #region CalcPosition
        public static void CalculatePosition(Section section, SectionData sectieData, int direction, bool isLeft, bool isStart, out int pX, out int pY)
        {
            switch (section.SectionType)
            {
                case SectionTypes.StartGrid:
                    if (!isStart)
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
                    CalculateLeftCornerPosition(sectieData, direction, isLeft, out pX, out pY);
                    return;
                case SectionTypes.RightCorner:
                    CalculateRightCornerPosition(sectieData, direction, isLeft, out pX, out pY);
                    return;
                default:
                    pX = 0;
                    pY = 0;
                    break;
            }
        }

        public static void CalculateStartPosition(int direction, bool isLeft, out int pX, out int pY)
        {
            var width = GetRemainingTileWidthSpace;
            var height = GetRemainingTileHeightSpace;
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
                default:
                    pX = 0;
                    pY = 0;
                    break;
            }
        }

        public static void CalculateStraightPositition(SectionData sectieData, int direction, bool isLeft, out int pX, out int pY)
        {
            var width = GetRemainingTileWidthSpace;
            var height = GetRemainingTileHeightSpace;
            double distance = isLeft ? (sectieData.DistanceLeft == 0 ? 1 : sectieData.DistanceLeft) : (sectieData.DistanceRight == 0 ? 1 : sectieData.DistanceRight);
            switch (direction)
            {
                case 0:
                    pX = isLeft ? width / 3 : width / 3 * 2;
                    pY = (int)(height * distance / Section.SectionLength);
                    pY = height - pY;
                    return;
                case 1:
                    pX = (int)(width * distance / Section.SectionLength);
                    pY = isLeft ? height / 3 : height / 3 * 2;
                    return;
                case 2:
                    pX = isLeft ? width / 3 * 2 : width / 3;
                    pY = (int)(height * distance / Section.SectionLength);
                    return;
                case 3:
                    pX = (int)(width * distance / Section.SectionLength);
                    pX = width - pX;
                    pY = isLeft ? height / 3 * 2 : height / 3;
                    return;
                default:
                    pX = 0;
                    pY = 0;
                    break;
            }
        }

        public static void CalculateLeftCornerPosition(SectionData sectieData, int direction, bool isLeft, out int pX, out int pY)
        {
            var width = GetRemainingTileWidthSpace;
            var height = GetRemainingTileHeightSpace;
            int radius, x, y;
            double distance = isLeft ? (sectieData.DistanceLeft == 0 ? 1 : sectieData.DistanceLeft) : (sectieData.DistanceRight == 0 ? 1 : sectieData.DistanceRight);
            double angle = -(distance / Section.SectionLength * 90);

            switch (direction)
            {
                case 0:
                    radius = isLeft ? width / 3 : width / 3 * 2;
                    x = (int)(radius * Math.Cos(ToRad(angle)));
                    y = (int)(radius * Math.Sin(ToRad(angle)));
                    pX = x + 0;
                    pY = y + height;
                    return;
                case 1:
                    angle = 90 + angle;
                    radius = isLeft ? height / 3 : height / 3 * 2;
                    x = (int)(radius * Math.Cos(ToRad(angle)));
                    y = (int)(radius * Math.Sin(ToRad(angle)));
                    pX = x + 0;
                    pY = y + 0;
                    return;
                case 2:
                    angle = 180 + angle;
                    radius = isLeft ? width / 3 * 2 : width / 3;
                    x = (int)(radius * Math.Cos(ToRad(angle)));
                    y = (int)(radius * Math.Sin(ToRad(angle)));
                    pX = x + width;
                    pY = y + 0;
                    return;
                case 3:
                    angle = 270 + angle;
                    radius = isLeft ? height / 3 * 2 : height / 3;
                    x = (int)(radius * Math.Cos(ToRad(angle)));
                    y = (int)(radius * Math.Sin(ToRad(angle)));
                    pX = x + width;
                    pY = y + height;
                    return;
                default:
                    pX = 0;
                    pY = 0;
                    break;

            }

        }

        public static void CalculateRightCornerPosition(SectionData sectieData, int direction, bool isLeft, out int pX, out int pY)
        {
            var width = GetRemainingTileWidthSpace;
            var height = GetRemainingTileHeightSpace;
            int radius, x, y;
            double distance = isLeft ? (sectieData.DistanceLeft == 0 ? 1 : sectieData.DistanceLeft) : (sectieData.DistanceRight == 0 ? 1 : sectieData.DistanceRight);
            double angle = distance / Section.SectionLength * 90;

            switch (direction)
            {
                case 0:
                    angle = 180 + angle;
                    radius = isLeft ? width / 3 * 2 : width / 3;
                    x = (int)(radius * Math.Cos(ToRad(angle)));
                    y = (int)(radius * Math.Sin(ToRad(angle)));
                    pX = x + width;
                    pY = y + height;
                    return;
                case 1:
                    angle = 270 + angle;
                    radius = isLeft ? height / 3 * 2 : height / 3;
                    x = (int)(radius * Math.Cos(ToRad(angle)));
                    y = (int)(radius * Math.Sin(ToRad(angle)));
                    pX = x + 0;
                    pY = y + height;
                    return;
                case 2:
                    radius = isLeft ? width / 3 : width / 3 * 2;
                    x = (int)(radius * Math.Cos(ToRad(angle)));
                    y = (int)(radius * Math.Sin(ToRad(angle)));
                    pX = x + 0;
                    pY = y + 0;
                    return;
                case 3:
                    angle = 90 + angle;
                    radius = isLeft ? height / 3 : height / 3 * 2;
                    x = (int)(radius * Math.Cos(ToRad(angle)));
                    y = (int)(radius * Math.Sin(ToRad(angle)));
                    pX = x + width;
                    pY = y + 0;
                    return;
                default:
                    pX = 0;
                    pY = 0;
                    break;
            }

        }

        private static double ToRad(double angle)
        {
            return (Math.PI / 180) * angle;
        }


        #endregion


        private static string GetCarImage(IParticipant p)
        {
            switch (p.TeamColor)
            {
                case TeamColors.RED:
                    return p.Equipment.IsBroken ? Resources.RedCarBroken : Resources.RedCar;
                case TeamColors.GREEN:
                    return p.Equipment.IsBroken ? Resources.GreenCarBroken : Resources.GreenCar;
                case TeamColors.YELLOW:
                    return p.Equipment.IsBroken ? Resources.YellowCarBroken : Resources.YellowCar;
                case TeamColors.BLUE:
                    return p.Equipment.IsBroken ? Resources.BlueCarBroken : Resources.BlueCar;
                case TeamColors.GREY:
                    return p.Equipment.IsBroken ? Resources.GreyCarBroken : Resources.GreyCar;
                default:
                    return null;
            }
        }
    }
}