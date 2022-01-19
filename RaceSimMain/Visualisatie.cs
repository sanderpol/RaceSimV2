using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class Visualisatie
    {
        private static int Direction;

        public const int TileWidth = 8;
        public const int TileHeight = 5;

        private static int MaxWidth;
        private static int MaxHeight;

        private static int CursorX;
        private static int CursorY;

        private static Race CurrentRace;


        internal static void Initialize(Race currentRace)
        {
            CurrentRace = currentRace;
            CalcMaxXY(currentRace.Track, out MaxWidth, out MaxHeight, out CursorX, out CursorY);
            Console.SetWindowSize(MaxWidth, MaxHeight);
            Console.SetCursorPosition(CursorX, CursorY);
        }

        public static void DrawTrack(Track track)
        {
            foreach (var section in track.Sections)
            {
                var sectionData = CurrentRace.GetSectionData(section);
                string? leftString = GetParticpantStringName(sectionData, isLeft: true);
                string? rightString = GetParticpantStringName(sectionData, isLeft: false);

                var sectionStrings = SetSectionString(GetSingleSectionStringArray(Direction, section.SectionType), leftString, rightString);
                var lineY = CursorY;
                foreach (var line in sectionStrings)
                {
                    Console.SetCursorPosition(CursorX, lineY);
                    Console.WriteLine(line);
                    lineY += 1;
                }
                Direction = SetNew_direction(section.SectionType, Direction);
                SetNewCursorPos(Direction, CursorX, CursorY, out CursorX, out CursorY);
                Console.SetCursorPosition(CursorX, CursorY);
            }
        }

        public static string? GetParticpantStringName(SectionData sectionData, bool isLeft)
        {
            if (isLeft && sectionData.Left != null)
            {
                return sectionData.Left.Equipment.IsBroken ? "xx" : sectionData.Left.Name;
            }
            else if (!isLeft && sectionData.Right != null)
            {
                return sectionData.Right.Equipment.IsBroken ? "xx" : sectionData.Right.Name;
            }
            else
            {
                return null;
            }
        }

        public static string[] GetSingleSectionStringArray(int dir, SectionTypes sectionType)
        {
            return sectionType switch
            {
                SectionTypes.Straight => (dir % 2) switch
                {
                    0 => straightVer,
                    1 => straightHor,
                    _ => throw new NotImplementedException()
                },
                SectionTypes.RightCorner => dir switch
                {
                    0 => LeftTurnDown,
                    1 => RightTurnDown,
                    2 => LeftTurnUp,
                    3 => RightTurnUp,
                    _ => throw new NotImplementedException()
                },
                SectionTypes.LeftCorner => dir switch
                {
                    0 => RightTurnDown,
                    1 => LeftTurnUp,
                    2 => RightTurnUp,
                    3 => LeftTurnDown,
                    _ => throw new NotImplementedException()
                },
                SectionTypes.StartGrid => (dir % 2) switch
                {
                    0 => startVer,
                    1 => startHor,
                    _ => throw new NotImplementedException()
                },
                SectionTypes.Finish => (dir % 2) switch
                {
                    0 => finishVer,
                    1 => finishHor,
                    _ => throw new NotImplementedException()
                },
                _ => throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null),
            };
        }

        public static string[] SetSectionString(string[] section, string? l, string? r)
        {
            var returnStrings = new string[section.Length];

            string leftPart;
            string rightPart;
            if (l != null)
            {
                var ls = l.Split(" ").ToList();
                for (int i = 0; i < ls.Count; i++)
                {
                    ls[i] = ls[i].Substring(0, 1);
                }
                leftPart = String.Concat(ls);
            }
            else
            {
                leftPart = "  ";
            }
            if (r != null)
            {
                var rs = r.Split(" ").ToList();
                for (int i = 0; i < rs.Count; i++)
                {
                    rs[i] = rs[i].Substring(0, 1);
                }
                rightPart = String.Concat(rs);

            }
            else
            {
                rightPart = "  ";
            }

            for (var i = 0; i < section.Length; i++)
            {
                returnStrings[i] = section[i].Replace("LS", leftPart).Replace("RS", rightPart);
            }
            return returnStrings;
        }

        public static void SetNewCursorPos(int dir, int currentX, int currentY, out int cursorX, out int cursorY)
        {
            switch (dir)
            {
                case 0:
                    cursorY = currentY - TileHeight;
                    cursorX = currentX;
                    break;
                case 1:
                    cursorX = currentX + TileWidth;
                    cursorY = currentY;
                    break;
                case 2:
                    cursorY = currentY + TileHeight;
                    cursorX = currentX;
                    break;
                case 3:
                    cursorX = currentX - TileWidth;
                    cursorY = currentY;
                    break;
                default:
                    cursorX = currentX;
                    cursorY = currentY;
                    break;
            }
            
        }

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
            cursorY = -minY + TileHeight + 1;

            maxWidth = maxX - minX;
            maxHeight = maxY - minY;
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

        #region Graphics

        public static readonly string[] finishHor =
        {
            "--------",
            "  LS  # ",
            "      # ",
            "  RS  # ",
            "--------"
        };

        public static readonly string[] finishVer =
        {
            "|      |",
            "|LS  RS|",
            "|      |",
            "|######|",
            "|      |"
        };

        public static readonly string[] straightHor =
        {
            "--------",
            "   LS   ",
            "        ",
            "   RS   ",
            "--------"
        };

        public static readonly string[] straightVer =
        {
            "|      |",
            "|      |",
            "|LS  RS|",
            "|      |",
            "|      |"
        };

        public static readonly string[] startHor =
        {
            "--------",
            "     LS ",
            "        ",
            " RS     ",
            "--------"
        };

        public static readonly string[] startVer =
        {

            "|      |",
            "|LS    |",
            "|      |",
            "|    RS|",
            "|      |"
        };

        public static readonly string[] RightTurnDown =
        {
            "------  ",
            " LS   \\ ",
            "       \\",
            "    RS |",
            "\\      |"
        };

        public static readonly string[] LeftTurnDown =
        {
            "  ------",
            " /   RS ",
            "/       ",
            "| LS    ",
            "|      /"
        };

        public static readonly string[] RightTurnUp =
        {
            "|      \\",
            "| LS    ",
            "\\       ",
            " \\   RS ",
            "  ------"
        };

        public static readonly string[] LeftTurnUp =
        {
            "/      |",
            "    RS |",
            "       /",
            " LS   / ",
            "------"
        };

        #endregion
    }
}
