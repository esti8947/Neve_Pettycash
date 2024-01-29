//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using System.Data;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using System.Data;
//using System.DirectoryServices.AccountManagement;
//using System.Security.Claims;
//using DAL.Data;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using BL.Services.ActiveDirectoryService;
//using DAL.Models;


//namespace BL.NdbManager
//{
//    public class NdbSignInManager : SignInManager<NdbUser>
//    {
//        private readonly NdbUserManager _userManager;
//        private readonly PettyCashNeveDbContext _dbContext;
//        private readonly IHttpContextAccessor _contextAccessor;
//        private readonly ILogger<SignInManager<NdbUser>> _logger;

//        //private List<NdbRole> _RolesCache;
//        //private List<NdbRole> RolesCache
//        //{
//        //    get
//        //    {
//        //        if(_RolesCache == null)
//        //        {
//        //            _RolesCache = _roleManager.Roles.ToList();
//        //        }
//        //        return _RolesCache;
//        //    }
//        //}

//        private ActiveDirectoryService _ActiveDirectoryService;
//        private ActiveDirectoryService ActiveDirectoryService
//        {
//            get
//            {
//                if (_ActiveDirectoryService == null)
//                {
//                    _ActiveDirectoryService = new ActiveDirectoryService();
//                }
//                return _ActiveDirectoryService;
//            }
//        }

//        public NdbSignInManager(NdbUserManager userManager,  PettyCashNeveDbContext dbContext, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<NdbUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<NdbUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<NdbUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
//        {
//            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
//            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
//            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
//        }

//        //public NdbSignInManager(
//        //    NdbUserManager userManager,
//        //    NdbRoleManager roleManager,
//        //    PettyCashNeveDbContext dbContext,
//        //    IHttpContextAccessor contextAccessor,
//        //    IUserClaimsPrincipalFactory<NdbUser> claimsFactory,
//        //    IOptions<IdentityOptions> optionsAccessor,
//        //    ILogger<NdbSignInManager> logger,
//        //    IAuthenticationSchemeProvider schemes
//        //) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
//        //{
//        //    _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
//        //    _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
//        //    _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
//        //    _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
//        //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        //}


//        public override async Task<SignInResult> PasswordSignInAsync(NdbUser user, string password, bool isPersistent, bool lockoutOnFailure)
//        {
//            UserPrincipal userP = PasswordSignInAd(user.UserName, password);
//            if(userP == null)
//            {
//                return SignInResult.Failed;
//            }
//            NdbUser dbUser = await VerifyUserExistsInDb(userP);
//            await SignInAsync(dbUser, isPersistent);

//            return SignInResult.Success;
//        }

//        public override async Task SignInAsync(NdbUser user, bool isPersistent, string authenticationMethod = null)
//        {
//            UserPrincipal adPrincipal = ActiveDirectoryService.SignInAd(user.UserName);

//            await LoadRolesFromAd(user, adPrincipal);
//            await base.SignInAsync(user, isPersistent, authenticationMethod);
//        }

//        private List<Principal> GetGroupsWithParents(UserPrincipal principal)
//        {
//            List<Principal> buffer = new List<Principal>();

//            PrincipalSearchResult<Principal> directGroups = principal.GetGroups();
//            RecursiveGetGroupsWithParents(buffer, directGroups);

//            return buffer;
//        }

//        private void RecursiveGetGroupsWithParents(List<Principal> buffer, PrincipalSearchResult<Principal> result)
//        {
//            foreach(Principal group in result)
//            {
//                if(!buffer.Contains(group)) buffer.Add(group);

//                PrincipalSearchResult<Principal> parentGroups = group.GetGroups();
//                if(parentGroups != null) RecursiveGetGroupsWithParents(buffer, parentGroups);
//            }
//        }

//        //private async Task LoadRolesFromAd(NdbUser user, UserPrincipal adPrincipal)
//        //{
//        //    List<Principal> groups = new List<Principal>();
//        //    groups.Add(new GroupPrincipal(ActiveDirectoryService.AdContext) { Name = "Maalot Beit Chana" });

//        //    foreach(Principal group in groups)
//        //    {
//        //        NdbRole role = await VerifyRoleExistsInDb(group.Name);
//        //        var res = await _userManager.IsInRoleAsync(user, role.Name);
//        //        if (!res)
//        //           await _userManager.AddToRoleAsync(user,role.Name);
//        //            //user.UserRoles.Add(new ApplicationUserRole() { User = user, Role = role });
//        //    }
//        //    await _userManager.UpdateAsync(user);
//        }

//        //private static SemaphoreSlim verifyRoleExistsInDbSemaphore = new SemaphoreSlim(1,1);
//        //private async Task<NdbRole> VerifyRoleExistsInDb(string roleName)
//        //{
//        //    NdbRole role = _roleManager.Roles.FirstOrDefault(r => r.Name==roleName);
//        //    if(role == null)
//        //    {
//        //        await verifyRoleExistsInDbSemaphore.WaitAsync();
//        //        try
//        //        {

//        //                role = new NdbRole { Name = roleName };

//        //                var result = await _roleManager.CreateAsync(role);
//        //                if (!result.Succeeded)
//        //                {
//        //                    _logger.LogError($"Failed to create role '{roleName}'.");
//        //                }
                    
//        //        }
//        //        finally
//        //        {
//        //            verifyRoleExistsInDbSemaphore.Release();
//        //        }
//        //    }
//        //    return role;
//        //}

//        private async Task<NdbUser> VerifyUserExistsInDb(UserPrincipal adPrincipal)
//        {
//            NdbUser ndbUser = await _userManager.FindByNameAsync(adPrincipal.SamAccountName);
//            if(ndbUser == null)
//            {
//                ndbUser = new NdbUser
//                {
//                    PhoneNumber = adPrincipal.VoiceTelephoneNumber,
//                    UserName = adPrincipal.SamAccountName,
//                    Email = adPrincipal.EmailAddress,
//                    //UserRoles = new List<ApplicationUserRole>()
//                };
//                var result = await _userManager.CreateAsync(ndbUser);
//                if (!result.Succeeded)
//                {
//                    _logger.LogError($"Failed to create user '{adPrincipal.SamAccountName}'.");
//                }
//            }
//            return ndbUser;
//        }

//        private UserPrincipal PasswordSignInAd(string username, string password)
//        {
//            if(!ActiveDirectoryService.ValidateCredentials(username, password))
//            {
//                return null;
//            }
//            return ActiveDirectoryService.SignInAd(username);
//        }
//    }
//}
