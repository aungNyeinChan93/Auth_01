using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Database_01.Entities
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        public required string PermissionName { get; set; }

        public List<RolePermission>? RolePermissions { get; set; }

    }
}
