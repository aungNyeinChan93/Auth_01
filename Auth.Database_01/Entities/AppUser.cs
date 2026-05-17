using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Database_01.Entities
{
    public class AppUser
    {
        [Key]
        public int AppUserId { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }

        public Role? Role { get; set; }
    }
}
