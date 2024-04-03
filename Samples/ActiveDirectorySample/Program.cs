using System;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;

namespace ActiveDirectorySample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string DomainName = "genetec.com";

            Console.WriteLine("### FindUsers ###");
            FindUsers(DomainName, 10);
            Console.WriteLine();

            Console.WriteLine("### GetForestDomain ###");
            GetForestDomain(DomainName);
            Console.WriteLine();

            Console.WriteLine("### GetDomainInfo ###");
            GetDomainInfo(DomainName);
        }

        public static void FindUsers(string domainName, int maxCount)
        {
            void PrintUser(SearchResult result, int count=1)
            {
                var name = result.Properties.GetValueOrDefault<string>("name");
                var distinguishedName = result.Properties.GetValueOrDefault<string>("DistinguishedName");
                var accountName = result.Properties.GetValueOrDefault<string>("sAMAccountName");
                var displayName = result.Properties.GetValueOrDefault<string>("displayName");
                var email = result.Properties.GetValueOrDefault<string>("email");
                var userPrincipalName = result.Properties.GetValueOrDefault<string>("userPrincipalName");
                var description = result.Properties.GetValueOrDefault<string>("description");
                var firstName = result.Properties.GetValueOrDefault<string>("givenName");
                var lastName = result.Properties.GetValueOrDefault<string>("sn");

                var adDateObject = result.Properties.GetValueOrDefault<Int64>("accountExpires");
                var accountExpires = adDateObject == long.MaxValue || adDateObject <= 0 || DateTime.MaxValue.ToFileTime() <= adDateObject ?
                    DateTime.MaxValue :
                    DateTime.FromFileTimeUtc(adDateObject);

                var filteredOctetString = result.Properties.GetValue<byte[]>("objectSid").Aggregate(string.Empty, (c, b) => c += $"\\{b.ToString("X2")}");
                var sid = filteredOctetString.IndexOf('-') != -1 ?
                    new SecurityIdentifier(filteredOctetString) :
                    new SecurityIdentifier(filteredOctetString.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)
                                                              .Select(token => byte.Parse(token, System.Globalization.NumberStyles.HexNumber))
                                                              .ToArray(), 0);

                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine($"User #{count}");
                Console.WriteLine($"Name {name}");
                Console.WriteLine($"distinguishedName {distinguishedName}");
                Console.WriteLine($"accountName {accountName}");
                Console.WriteLine($"displayName {displayName}");
                Console.WriteLine($"email {email}");
                Console.WriteLine($"userPrincipalName {userPrincipalName}");
                Console.WriteLine($"description {description}");
                Console.WriteLine($"firstName {firstName}");
                Console.WriteLine($"lastName {lastName}");
                Console.WriteLine($"accountExpires {accountExpires}");
                Console.WriteLine($"sid {sid}");
                Console.WriteLine("-------------------------------------------------------");
            }

            using (var entry = new DirectoryEntry($"LDAP://{domainName}", null, null, AuthenticationTypes.Secure))
            using (var directorySearcher = new DirectorySearcher(entry, "(objectClass=user)"))
            {
                switch (maxCount)
                {
                    case 1:
                        PrintUser(directorySearcher.FindOne());
                        break;
                    default:
                        using (var results = directorySearcher.FindAll())
                        {
                            int count = 0;
                            foreach (SearchResult result in results)
                            {
                                if (count++ >= maxCount) break;
                                PrintUser(result, count);
                            }
                        }
                        break;
                }
            }
        }

        public static void GetForestDomain(string domainName)
        {
            using (var rootDSE = new DirectoryEntry($"LDAP://{domainName}/RootDSE", null, null, AuthenticationTypes.Secure))
            {
                // Get the CN name for the condiguration folder
                var configurationNamingContext = rootDSE.Properties["configurationNamingContext"][0].ToString();

                // Get the rootDSE of the directory
                using (var searchRoot = new DirectoryEntry($"LDAP://{domainName}/cn=Partitions,{configurationNamingContext}", null, null, AuthenticationTypes.Secure))
                using (var searcher = new DirectorySearcher(searchRoot))
                {
                    searcher.SearchScope = SearchScope.OneLevel;
                    searcher.PropertiesToLoad.Add("netbiosname");
                    searcher.PropertiesToLoad.Add("dnsroot");
                    searcher.Filter = "(&(objectcategory=Crossref)(netBIOSName=*))";

                    using (var searchResults = searcher.FindAll())
                    {
                        foreach (SearchResult result in searchResults)
                        {
                            if (result.Properties.Count == 3)
                            {
                                var fullyQualifiedDomainName = result.Properties["dnsroot"][0].ToString();
                                var netBiosDomainName = result.Properties["netbiosname"][0].ToString();

                                Console.WriteLine("-------------------------------------------------------");
                                Console.WriteLine($"fullyQualifiedDomainName {fullyQualifiedDomainName}");
                                Console.WriteLine($"netBiosDomainName {netBiosDomainName}");
                                Console.WriteLine("-------------------------------------------------------");
                            }
                        }
                    }
                }
            }
        }

        public static void GetDomainInfo(string domainName)
        {
            using (var entry = new DirectoryEntry($"LDAP://{domainName}", null, null, AuthenticationTypes.Secure))
            using (var searcher = new DirectorySearcher(entry, "(objectClass=domain)",
                new[] {
                    "distinguishedName",
                    "objectGUID",
                    "objectClass",
                    "cn",
                    "displayName",
                    "name",
                    "mail",
                    "description",
                    "objectSid",
                    "sAMAccountName",
                    "userPrincipalName",
                    "givenName",
                    "sn",
                    "memberOf"}){
                SearchScope = SearchScope.Subtree
            })
            {
                var ret = searcher.FindOne();
                Guid id = new Guid(ret.Properties.GetValue<byte[]>("objectGUID"));
                string name = ret.Properties.GetValue<string>("name");
                string distinguishedName = ret.Properties.GetValue<string>("distinguishedName");

                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine($"name {name}");
                Console.WriteLine($"distinguishedName {distinguishedName}");
                Console.WriteLine("-------------------------------------------------------");
            }
        }
    }
}
