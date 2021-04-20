using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Principal;

namespace GetADData.queries
{
    public class Getters
    {
        public SortedList GetADGroups(SearchResult searchResult)
        {
            var activeDirectoryGroups = new SortedList();

            if (searchResult == null) return activeDirectoryGroups;

            var userActiveDirectoryEntry = new DirectoryEntry(searchResult.Path);
            var i = 0;

            // Invoke Groups method.
            var userActiveDirectoryGroups = userActiveDirectoryEntry.Invoke("Groups");
            foreach (var obj in (IEnumerable)userActiveDirectoryGroups)
            {
                // Create object for each group.
                var groupDirectoryEntry = new DirectoryEntry(obj);
                var groupName = groupDirectoryEntry.Name.Replace("cn=", string.Empty);
                groupName = groupName.Replace("CN=", string.Empty);
                if (activeDirectoryGroups.ContainsKey(groupName)) continue;

                i++;
                if (i<10)
                    activeDirectoryGroups.Add($"group00{i}", groupName);
                else if (i<100)
                    activeDirectoryGroups.Add($"group0{i}", groupName);
                else
                    activeDirectoryGroups.Add($"group{i}", groupName);
            }
            return activeDirectoryGroups;
        }

        public string GetUserCN(SearchResult searchResult)
        {
            return searchResult.GetDirectoryEntry().Name.Replace("CN=", string.Empty);
        }

        public SearchResultCollection GetEntriesByEmail(string[] args)
        {
            var email = args[0];
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("The email should not be empty");

            var directoryEntry = new DirectoryEntry();
            var directorySearcher = new DirectorySearcher(directoryEntry, "(mail=" + email + ")");

            return directorySearcher.FindAll();
        }

        public SearchResult GetEntryByUser(string loginName)
        {
            if (string.IsNullOrEmpty(loginName))
                throw new ArgumentException("The loginName should not be empty");

            var backSlash = loginName.IndexOf("\\");
            var userName = backSlash > 0 ? loginName.Substring(backSlash + 1) : loginName;

            var directoryEntry = new DirectoryEntry();
            var directorySearcher = new DirectorySearcher(directoryEntry, "(sAMAccountName=" + userName + ")");

            return directorySearcher.FindOne();
        }

        public string[] UsersInGroup(string groupName)
        {
            var users = new List<string>();

            // First, find the group:
            var query = string.Format("(CN={0})", groupName);
            var searchResult = new DirectorySearcher(query).FindOne();
            var group = new DirectoryEntry(searchResult.Path);

            // Find all the members
            foreach (var rawMember in (IEnumerable)group.Invoke("members"))
            {
                // Grab this member's SID
                var member = new DirectoryEntry(rawMember);
                byte[] sid = null;
                foreach (var o in member.Properties["objectSid"]) sid = o as byte[];

                // Convert it to a domain\user string
                try
                {
                    users.Add(
                      new SecurityIdentifier(sid, 0).Translate(typeof(NTAccount)).ToString());
                }
                catch { } // Some SIDs cannot be discovered - ignore these
            }

            return users.ToArray();
        }

        public string GetDepartment(SearchResult searchResult)
        {
            return GetProperty(searchResult, "department");
        }

        public SortedList GetEmails(SearchResult searchResult)
        {
            return new SortedList {{"mail", GetProperty(searchResult, "mail")}};
        }

        private string GetProperty(SearchResult searchResult, string property)
        {
            if (searchResult == null) return string.Empty;

            var directoryEntry = searchResult.GetDirectoryEntry();
            return directoryEntry.Properties.Contains(property) ? directoryEntry.Properties[property].Value.ToString() : string.Empty;
        }
    }
}
