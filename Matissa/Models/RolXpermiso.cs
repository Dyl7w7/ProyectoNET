using System;
using System.Collections.Generic;
using Matissa.Models;

namespace Matissa.Model
{
    public partial class RolXpermiso
    {
        public int IdRolXpermiso { get; set; }
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }

        public virtual Permiso IdPermisoNavigation { get; set; } = null!;
        public virtual Rol IdRolNavigation { get; set; } = null!;
    }
}
