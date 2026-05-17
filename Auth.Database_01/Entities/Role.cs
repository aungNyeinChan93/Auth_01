using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Database_01.Entities
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public required string RoleName { get; set; }

        public List<RolePermission>? RolePermissions { get; set; }
    }
}
