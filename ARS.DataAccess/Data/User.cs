using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ARS.DataAccess
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        public string Password { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

    }
}
