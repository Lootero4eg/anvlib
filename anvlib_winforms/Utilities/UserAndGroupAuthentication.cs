using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Principal;

namespace anvlib.Utilities
{
    public static class UserAndGroupAuthentication
    {
        public static string GetCurrentUserFullName()
        {
            var cuser = WindowsIdentity.GetCurrent();
            return cuser.Name;
        }

        public static string GetCurrentUserNameWithoutDomainRealm()
        {
            var cuser = WindowsIdentity.GetCurrent();
            if (cuser.Name.Contains('\\'))
                return cuser.Name.Split('\\')[1];
            else
                return cuser.Name;
        }

        public static WindowsIdentity GetCurrentUser()
        {
            return WindowsIdentity.GetCurrent();            
        }

        public static bool IsCurrentUserInGroup(string group_name)
        {
            var cuser = WindowsIdentity.GetCurrent();
            foreach (var group in cuser.Groups)
            {
                if (group.Value.ToLower() == group_name.ToLower())
                    return true;
            }

            return false;
        }
    }
}
