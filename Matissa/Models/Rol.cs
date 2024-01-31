using System.ComponentModel.DataAnnotations;
using Matissa.Model;

namespace Matissa.Models
{
    public partial class Rol
    {
        public Rol()
        {
            RolXpermisos = new HashSet<RolXpermiso>();
            Usuarios = new HashSet<Usuario>();
        }

        public int IdRol { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        public string NombreRol { get; set; } = null!;

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Range(0, 1, ErrorMessage = "El estado debe ser 0 o 1.")]
        public byte? Estado { get; set; }

        public virtual ICollection<RolXpermiso> RolXpermisos { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}