using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DAL.Models;

namespace BL.NdbManager
{
    public class NdbUserManager : UserManager<NdbUser>
    {
        public NdbUserManager(IUserStore<NdbUser> store,
                              IOptions<IdentityOptions> optionsAccessor,
                              IPasswordHasher<NdbUser> passwordHasher,
                              IEnumerable<IUserValidator<NdbUser>> userValidators,
                              IEnumerable<IPasswordValidator<NdbUser>> passwordValidators,
                              ILookupNormalizer keyNormalizer,
                              IdentityErrorDescriber errors,
                              IServiceProvider services,
                              ILogger<UserManager<NdbUser>> logger)
                              : base(store, optionsAccessor, passwordHasher,
                                    userValidators, passwordValidators, keyNormalizer,
                                    errors, services, logger)
        { }
    }
}
