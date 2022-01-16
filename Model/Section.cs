namespace Model
{
    public class Section
    {
        public SectionTypes SectionType { get; set; }

        public static int SectionLength = 1500;
        public Section(SectionTypes sectionType)
        {
            SectionType = sectionType;
        }

    }
}