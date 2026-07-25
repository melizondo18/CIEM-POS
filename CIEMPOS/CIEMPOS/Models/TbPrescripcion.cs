using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Models;

[Table("TB_Prescripcion")]
public partial class TbPrescripcion
{
    [Key]
    public int IdPrescripcion { get; set; }

    public int IdPaciente { get; set; }

    public int IdUsuario { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaPrescripcion { get; set; }

    [Unicode(false)]
    public string? Cardio { get; set; }

    [Unicode(false)]
    public string? Fuerza { get; set; }

    [Unicode(false)]
    public string? Estiramiento { get; set; }

    [Unicode(false)]
    public string? Observaciones { get; set; }

    public bool Estado { get; set; }

    [ForeignKey("IdPaciente")]
    [InverseProperty("TbPrescripcions")]
    public virtual TbPaciente IdPacienteNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("TbPrescripcions")]
    public virtual TbUsuario IdUsuarioNavigation { get; set; } = null!;
}
