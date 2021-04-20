using System.Collections;

namespace GetADData.models
{
    public class EntryModel
    {
        public string UserName { get; set; }

        public SortedList Emails { get; set; }

        public string Department { get; set; }

        public SortedList ActiveDirectoryGroups { get; set; }
    }
}
