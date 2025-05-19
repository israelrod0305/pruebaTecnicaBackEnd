using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PruebaTecnica.Dto
{
    public class UserDto
    {
        public int Id { get; set; }

        public bool? ChangePassword { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string Apellido { get; set; }

        // No marques como [Required] directamente aquí
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Aquí va la lógica de validación condicional
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ChangePassword == true)
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    yield return new ValidationResult(
                        "La contraseña es obligatoria cuando se requiere cambiar la contraseña.",
                        new[] { nameof(Password) }
                    );
                }
            }
        }
    }
    
}
