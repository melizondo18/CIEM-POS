using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Models;

[Table("TB_Usuario")]
[Index("IdPersona", Name = "UQ_TB_Usuario_IdPersona", IsUnique = true)]
public partial class TbUsuario
{
    [Key]
    public int IdUsuario { get; set; }

    public int IdPersona { get; set; }

    public int IdRol { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Contrasena { get; set; } = null!;

    public bool Estado { get; set; }

    [ForeignKey("IdPersona")]
    [InverseProperty("TbUsuario")]
    public virtual TbPersona IdPersonaNavigation { get; set; } = null!;

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
