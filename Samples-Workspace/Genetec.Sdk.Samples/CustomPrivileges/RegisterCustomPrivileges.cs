using Genetec.Sdk;
using Genetec.Sdk.Privileges;
using Genetec.Sdk.Workspace;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows.Media;

namespace CustomPrivileges
{
    public sealed class RegisterCustomPrivileges : IDisposable
    {
        public ImageSource m_customCategoryIcon { get; private set; }
        private Workspace m_workspace;
        private Timer m_timer;
        private DateTime m_oldAccessTime;
        private List<PrivilegesRegistrationInfo> m_oldJsonInfo;
        private List<PrivilegesRegistrationInfo> m_newJsonInfo;
        private List<Guid> m_removedGuids;
        private List<PrivilegeRegistration> m_registrations;
        private Guid m_dataGroupId = new Guid("{C63C0E8C-807D-4652-9877-282F78AA9DDA}");
        private bool m_deleted = false;
        private PrivilegeRegistration m_dataTypeGroup;

        public void Initialize(Workspace workspace)
        {
            m_workspace = workspace;
            RegisterPrivileges();
            GetOldJsonInfo();
            SetTimer();
        }

        //Register the privileges at startup
        public void RegisterPrivileges()
        {
            m_oldAccessTime = GetOldFileAccessTime();
            var registrations = new List<PrivilegeRegistration>();

            // Add the data group
            m_dataTypeGroup = new PrivilegeRegistration(m_dataGroupId, PrivilegeType.Group, "New Privileges");
            m_dataTypeGroup.Details = "Represent the group of correlated data types registrations";
            m_dataTypeGroup.ParentId = SdkPrivilege.Root;
            m_dataTypeGroup.Icon = m_customCategoryIcon;
            registrations.Add(m_dataTypeGroup);

                var json = GetJsonInfo();
                var parentID = json.ParentId;

                foreach (var privileges in json.PrivilegeRegistration)
                {
                    if (privileges.ParentGroupId == Guid.Empty)
                    {
                        var customPrivilege = new PrivilegeRegistration(privileges.PrivilegeId, privileges.PrivilegeType, privileges.Description, String.Empty, 1, null, m_dataGroupId);
                        registrations.Add(customPrivilege);
                    }
                    else
                    {
                        var groupPrivilege = new PrivilegeRegistration(privileges.PrivilegeId, privileges.PrivilegeType, privileges.Description, String.Empty, 1, null, privileges.ParentGroupId);
                        registrations.Add(groupPrivilege);
                    }
                }
                m_workspace.Sdk.SecurityManager.RegisterPrivileges(registrations);
        }

        //Get path to the privileges.json
        private string GetJsonPath()
        {
            string jsonFilePath = null;
            var dllPath = Assembly.GetExecutingAssembly().Location;
            if (!string.IsNullOrEmpty(dllPath))
            {
                var directory = Path.GetDirectoryName(dllPath);
                jsonFilePath = Path.Combine(directory, "privileges.json");
            }    
            return jsonFilePath;
        }

        //Retrieve the access time of the file for comparison
        private DateTime GetOldFileAccessTime()
        {
            m_oldAccessTime = File.GetLastWriteTime(GetJsonPath());
            return m_oldAccessTime;
        }

        //Get and Deserialize JSON info (Ids, Description, Type...)
        private Privileges GetJsonInfo()
        {
            var privileges = new Privileges();
            try
            {
                var json = File.ReadAllText(GetJsonPath());
                privileges = JsonConvert.DeserializeObject<Privileges>(json);
            }
            catch (FileNotFoundException e)
            {
                m_deleted = true;
                UnregisterAll();
                Console.WriteLine("File cannot be found {0}", e.Message);
            }
            return privileges;
        }

        //Store the old json file for comparison
        private List<PrivilegesRegistrationInfo> GetOldJsonInfo()
        {
            var json = File.ReadAllText(GetJsonPath());
            var privileges = JsonConvert.DeserializeObject<Privileges>(json);
            m_oldJsonInfo = privileges.PrivilegeRegistration;

            return m_oldJsonInfo;
        }

        //Timer that resets every n seconds and checks if privileges have been modified
        public void SetTimer()
        {
            m_timer = new Timer
            {
                Interval = 15000
            };

            m_timer.Elapsed += OnTimedEvent;
            m_timer.AutoReset = true;
            m_timer.Enabled = true;
            m_timer.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            CompareJson();
        }

        /// <summary>
        /// Compares the Json
        /// </summary>
        private void CompareJson()
        {
            m_newJsonInfo = GetJsonInfo().PrivilegeRegistration;

            if(m_newJsonInfo != null)
            {
                if (m_deleted)
                {
                    m_deleted = false;
                    RegisterPrivileges();
                }

                var newAccessTime = File.GetLastWriteTime(GetJsonPath());
                m_removedGuids = new List<Guid>();
                m_registrations = new List<PrivilegeRegistration>();

                //Check to verify if file has been accessed
                if (m_oldAccessTime != newAccessTime)
                {
                    FindRemoved();
                    FindAdded();
                }

                m_oldJsonInfo = m_newJsonInfo;
                m_oldAccessTime = newAccessTime;
            }
        }

        public void FindAdded()
        {
            m_newJsonInfo = GetJsonInfo().PrivilegeRegistration;
            var addGroupRegistrations = new List<PrivilegeRegistration>();
            var missingGroupRegistrations = new List<PrivilegeRegistration>();

            var definitions = m_workspace.Sdk.SecurityManager.GetPrivilegeDefinitions();

            var result = definitions.First(x => x.Id == m_dataGroupId);

            var addDifference = m_newJsonInfo.Select(x => new { x.Description, x.PrivilegeId, x.PrivilegeType, x.ParentGroupId })
                .Except(m_oldJsonInfo.Select(x => new { x.Description, x.PrivilegeId, x.PrivilegeType, x.ParentGroupId })).ToList();

            var addGroup = m_newJsonInfo.Select(x => x.ParentGroupId).Except(m_oldJsonInfo.Select(x => x.ParentGroupId)).ToList();

            if (result.Children.Count == 0)
            {
                RegisterPrivileges();
            }
            else
            {
                foreach (var toBeAdded in addDifference)
                {
                    var findMissingGroupLead = m_oldJsonInfo.Where(x => x.ParentGroupId == toBeAdded.PrivilegeId).ToList()
                        .Any(missingGroup => toBeAdded.PrivilegeId == missingGroup.ParentGroupId);

                    var findMissingGroup = m_oldJsonInfo.Where(x => x.PrivilegeId == toBeAdded.ParentGroupId).ToList()
                        .Any(missingGroup => toBeAdded.ParentGroupId == missingGroup.PrivilegeId);

                    if ((findMissingGroupLead && addGroup.Count == 0) || (findMissingGroup && addGroup.Count  > 0))
                    {
                        UnregisterAll();
                        RegisterPrivileges();
                        break;
                    }
                    if (toBeAdded.ParentGroupId == Guid.Empty)
                    {
                        var customPrivilege = new PrivilegeRegistration(toBeAdded.PrivilegeId, toBeAdded.PrivilegeType, toBeAdded.Description, String.Empty, 1, null, m_dataGroupId);
                        m_registrations.Add(customPrivilege);
                    }
                    else
                    {
                        var addGroupPrivilege = new PrivilegeRegistration(toBeAdded.PrivilegeId, toBeAdded.PrivilegeType, toBeAdded.Description, String.Empty, 1, null, toBeAdded.ParentGroupId);
                        addGroupRegistrations.Add(addGroupPrivilege);
                    }
                    m_workspace.Sdk.SecurityManager.RegisterPrivileges(addGroupRegistrations);
                    m_workspace.Sdk.SecurityManager.RegisterPrivileges(m_registrations);
                }
            }
        }

        public void FindRemoved()
        {
            m_newJsonInfo = GetJsonInfo().PrivilegeRegistration;
            var removeGroupPrivilege = new List<Guid>();
            var removeDifference = m_oldJsonInfo.Select(x => new { x.Description, x.PrivilegeId, x.PrivilegeType, x.ParentGroupId })
                                   .Except(m_newJsonInfo.Select(x => new { x.Description, x.PrivilegeId, x.PrivilegeType, x.ParentGroupId })).ToList();

            foreach(var toBeRemoved in removeDifference)
            {
                if(toBeRemoved.ParentGroupId == Guid.Empty)
                {
                    m_removedGuids.Add(toBeRemoved.PrivilegeId);
                }
                else
                {
                    removeGroupPrivilege.Add(toBeRemoved.PrivilegeId);       
                }
                m_workspace.Sdk.SecurityManager.UnregisterPrivileges(m_removedGuids);
                m_workspace.Sdk.SecurityManager.UnregisterPrivileges(removeGroupPrivilege);
            }  
        }

        public void UnregisterAll()
        {
            foreach (var oldInfo in m_oldJsonInfo)
            {
                var removedPrivilege = new PrivilegeRegistration(oldInfo.PrivilegeId, oldInfo.PrivilegeType, oldInfo.Description, String.Empty, 1, null, m_dataGroupId);
                m_removedGuids.Add(removedPrivilege.Id);
                m_workspace.Sdk.SecurityManager.UnregisterPrivileges(m_removedGuids);
            }
        }

        public void Dispose()
        {
            m_timer.Stop();
            m_timer.Elapsed -= OnTimedEvent;
            m_timer.Dispose();
        }

        //For Json Object
        public class Privileges
        {
            public Guid ParentId { get; set; }
            public List<PrivilegesRegistrationInfo> PrivilegeRegistration { get; set; }
        }

        public class PrivilegesRegistrationInfo
        {
            public Guid PrivilegeId { get; set; }
            public string Description { get; set; }
            public PrivilegeType PrivilegeType { get; set; }
            public Guid ParentGroupId { get; set; }
        }
    }
}
