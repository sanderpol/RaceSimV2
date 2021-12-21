namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }
        public int StartingDirection { get; set; }
        public int TotalLaps { get; set; }

        public Track(string name, int dir, int totalLap, SectionTypes[] sections)
        {
            Name = name;
            StartingDirection = dir;
            TotalLaps = totalLap;
            Sections = SectionTypesToSections(sections);
        }

        private LinkedList<Section> SectionTypesToSections(SectionTypes[] sectionTypes)
        {
            var sections = new LinkedList<Section>();
            if (sectionTypes== null)
                return sections;

            sectionTypes.ToList().ForEach(x => sections.AddLast(new Section(x)));
            return sections;
        }
    }
}