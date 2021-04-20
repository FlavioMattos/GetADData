using System.DirectoryServices;
using GetADData.models;
using GetADData.queries;
using GetADData.utilities;

namespace GetADData
{
    class Program
    {
        static void Main(string[] args)
        {
            var entry = new EntryModel();
            var queries = new Getters();

            ArgsValidations.IsArgsValid(args);

            var searchResultCol = queries.GetEntriesByEmail(args);

            if (null != searchResultCol)
            {
                foreach (SearchResult searchResult in searchResultCol)
                {
                    entry.UserName = queries.GetUserCN(searchResult);
                    entry.Emails = queries.GetEmails(searchResult);
                    entry.Department = queries.GetDepartment(searchResult);
                    entry.ActiveDirectoryGroups = queries.GetADGroups(searchResult);
                }
            }

            JsonUtilities.JsonFileExporter(args, entry);
        }
    }
}
