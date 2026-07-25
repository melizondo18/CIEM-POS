using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Models;

[Table("TB_Pago")]
public partial class TbPago
{
    [Key]
    public int IdPago { get; set; }

    public int IdPaciente { get; set; }

    public int IdUsuario { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaPago { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Monto { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string NumeroAutorizacion { get; set; } = null!;

    public bool Estado { get; set; }

    [ForeignKey("IdPaciente")]
    [InverseProperty("TbPagos")]
    public virtual TbPaciente IdPacienteNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("TbPagos")]
    public virtual TbUsuario IdUsuarioNavigation { get; set; } = null!;
}
