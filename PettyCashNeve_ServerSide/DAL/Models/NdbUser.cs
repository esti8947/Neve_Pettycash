using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public class NdbUser : IdentityUser
    {
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }

}
