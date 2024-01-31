using System.ComponentModel.DataAnnotations;
using Matissa.Models;

namespace Matissa.Model
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string NombreUsuario { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 5 caracteres.")]
        public string Contraseña { get; set; } = null!;

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Range(0, 1, ErrorMessage = "El estado debe ser 0 o 1.")]
        public byte Estado { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public int? IdRol { get; set; }

        public virtual Rol? IdRolNavigation { get; set; }
    }
}
