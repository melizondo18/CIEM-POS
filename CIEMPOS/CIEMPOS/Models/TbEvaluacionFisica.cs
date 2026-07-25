using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Models;

[Table("TB_EvaluacionFisica")]
public partial class TbEvaluacionFisica
{
    [Key]
    public int IdEvaluacion { get; set; }

    public int IdPaciente { get; set; }

    public int IdUsuario { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaEvaluacion { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal Peso { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal Estatura { get; set; }

    [Column("IMC", TypeName = "decimal(5, 2)")]
    public decimal Imc { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal PorcentajeGrasa { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal MasaMuscular { get; set; }

    [Unicode(false)]
    public string? Observaciones { get; set; }

    [ForeignKey("IdPaciente")]
    [InverseProperty("TbEvaluacionFisicas")]
    public virtual TbPaciente IdPacienteNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("TbEvaluacionFisicas")]
    public virtual TbUsuario IdUsuarioNavigation { get; set; } = null!;
}
