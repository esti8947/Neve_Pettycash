using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
using System.Collections;

namespace BL.Services.ActiveDirectoryService
{
    public class ActiveDirectoryService : IDisposable
    {
        private PrincipalContext _AdContext;
        public PrincipalContext AdContext
        {
            get
            {
                if (_AdContext == null)
                {
#if DEBUG
                    ContextType serverType = ContextType.Machine;
                    string serverName = null;
#else
                    ContextType serverType = ContextType.Domain;
                    string serverName = ConfigurationManager.AppSettings["ActiveDirectoryDomain"];
#endif
                    _AdContext = new PrincipalContext(serverType, serverName);
                }

                return _AdContext;
            }
        }

        public IEnumerable GetGroupMembers(string groupName)
        {
            var group = GroupPrincipal.FindByIdentity(AdContext, groupName);
            var members = group.GetMembers(recursive: true);

            foreach (var member in members)
            {
                if (member is UserPrincipal) yield return new
                {
                    member.SamAccountName
                };
            }
        }

        public UserPrincipal SignInAd(string username)
        {
            return new UserPrincipal(AdContext) { DisplayName = username, SamAccountName = username };
            UserPrincipal user = UserPrincipal.FindByIdentity(AdContext, username);
            if (user.IsAccountLockedOut()
                || (user.Enabled.HasValue && !user.Enabled.Value))
            {
                return null;
            }

            return user;
        }


        public bool ValidateCredentials(string username, string password)
        {
            return true;
            return AdContext.ValidateCredentials(username, password);
        }

        public void Dispose()
        {
            if (_AdContext != null)
            {
                _AdContext.Dispose();
            }
        }
    }
}
