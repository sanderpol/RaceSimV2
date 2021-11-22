namespace Model
{
    public class Section
    {
        public SectionTypes SectionType { get; set; }
        public readonly int SectionLength = 100;
        public Section(SectionTypes sectionType)
        {
            SectionType = sectionType;
        }

    }
}