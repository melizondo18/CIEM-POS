// Esta clase representa la entidad Persona del sistema CIEMPOS.
// Contiene la definición de los atributos, las validaciones de los datos
// y las relaciones con otras entidades de la base de datos.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Models;

[Table("TB_Persona")]
[Index("Email", Name = "UQ_TB_Persona_Email", IsUnique = true)]
[Index("Identificacion", Name = "UQ_TB_Persona_Identificacion", IsUnique = true)]
public partial class TbPersona
{
    [Key]
    public int IdPersona { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Display(Name = "Apellido")]
    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
    [Unicode(false)]
    public string Apellido { get; set; } = null!;

    [Display(Name = "Identificación")]
    [Required(ErrorMessage = "La identificación es obligatoria.")]
    [StringLength(20, ErrorMessage = "La identificación no puede superar los 20 caracteres.")]
    [Unicode(false)]
    public string Identificacion { get; set; } = null!;

    [Display(Name = "Fecha de nacimiento")]
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    public DateOnly FechaNacimiento { get; set; }

    [Display(Name = "Sexo")]
    [Required(ErrorMessage = "El sexo es obligatorio.")]
    [StringLength(50, ErrorMessage = "El sexo no puede superar los 50 caracteres.")]
    [Unicode(false)]
    public string Sexo { get; set; } = null!;

    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
    [StringLength(100, ErrorMessage = "El correo electrónico no puede superar los 100 caracteres.")]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Display(Name = "Teléfono")]
    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
    [Unicode(false)]
    public string Telefono { get; set; } = null!;

    [Display(Name = "Dirección")]
    [Required(ErrorMessage = "La dirección es obligatoria.")]
    [StringLength(255, ErrorMessage = "La dirección no puede superar los 255 caracteres.")]
    [Unicode(false)]
    public string Direccion { get; set; } = null!;

    [Display(Name = "Contacto de emergencia")]
    [StringLength(100, ErrorMessage = "El contacto de emergencia no puede superar los 100 caracteres.")]
    [Unicode(false)]
    public string? ContactoEmergencia { get; set; }

    [Display(Name = "Teléfono de emergencia")]
    [StringLength(20, ErrorMessage = "El teléfono de emergencia no puede superar los 20 caracteres.")]
    [Unicode(false)]
    public string? TelefonoEmergencia { get; set; }

    [Display(Name = "Fecha de registro")]
    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [Display(Name = "Estado")]
    public bool Estado { get; set; }

    // Relación con la entidad Paciente
    [InverseProperty("IdPersonaNavigation")]
    public virtual TbPaciente? TbPaciente { get; set; }

    // Relación con la entidad Usuario
    [InverseProperty("IdPersonaNavigation")]
    public virtual TbUsuario? TbUsuario { get; set; }
}