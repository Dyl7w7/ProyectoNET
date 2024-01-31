using System;
using System.Collections.Generic;

namespace Matissa.Model
{
    public partial class Permiso
    {
        public Permiso()
        {
            RolXpermisos = new HashSet<RolXpermiso>();
        }

        public int IdPermiso { get; set; }
        public string Modulo { get; set; } = null!;
        public string? Descripción { get; set; }

        public virtual ICollection<RolXpermiso> RolXpermisos { get; set; }
    }
}
