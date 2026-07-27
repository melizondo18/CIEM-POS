/*
 * Nombre del archivo: TbUsuario.cs
 * Descripción: Modelo correspondiente a la tabla TB_Usuario.
 */

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIEMPOS.Models;

[Table("TB_Usuario")]
[Index("IdPersona", Name = "UQ_TB_Usuario_IdPersona", IsUnique = true)]
public partial class TbUsuario
{
    [Key]
    public int IdUsuario { get; set; }

    [Display(Name = "Persona")]
    [Required(ErrorMessage = "La persona es obligatoria.")]
    public int IdPersona { get; set; }

    [Display(Name = "Rol")]
    [Required(ErrorMessage = "El rol es obligatorio.")]
    public int IdRol { get; set; }

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(255, ErrorMessage = "La contraseña no puede exceder los 255 caracteres.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    [DataType(DataType.Password)]
    [Unicode(false)]
    public string Contrasena { get; set; } = null!;

    [NotMapped]
    [Display(Name = "Confirmar contraseña")]
    [Required(ErrorMessage = "Debe confirmar la contraseña.")]
    [DataType(DataType.Password)]
    [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmarContrasena { get; set; } = string.Empty;

    [Display(Name = "Debe cambiar contraseña")]
    public bool DebeCambiarContrasena { get; set; }

    [Display(Name = "Estado")]
    public bool Estado { get; set; }

    [ValidateNever]
    [ForeignKey("IdPersona")]
    [InverseProperty("TbUsuario")]
    public virtual TbPersona IdPersonaNavigation { get; set; } = null!;

    [ValidateNever]
    [ForeignKey("IdRol")]
    [InverseProperty("TbUsuarios")]
    public virtual TbRol IdRolNavigation { get; set; } = null!;

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<TbBitacoraIngreso> TbBitacoraIngresos { get; set; } = new List<TbBitacoraIngreso>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<TbBitacoraMovimiento> TbBitacoraMovimientos { get; set; } = new List<TbBitacoraMovimiento>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<TbEvaluacionFisica> TbEvaluacionFisicas { get; set; } = new List<TbEvaluacionFisica>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<TbPago> TbPagos { get; set; } = new List<TbPago>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<TbPrescripcion> TbPrescripcions { get; set; } = new List<TbPrescripcion>();
}