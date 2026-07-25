using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Models;

[Table("TB_Paciente")]
[Index("IdPersona", Name = "UQ_TB_Paciente_IdPersona", IsUnique = true)]
public partial class TbPaciente
{
    [Key]
    public int IdPaciente { get; set; }

    public int IdPersona { get; set; }

    [Unicode(false)]
    public string? InformacionClinica { get; set; }

    public bool Estado { get; set; }

    [ForeignKey("IdPersona")]
    [InverseProperty("TbPaciente")]
    public virtual TbPersona IdPersonaNavigation { get; set; } = null!;

    [InverseProperty("IdPacienteNavigation")]
    public virtual ICollection<TbEvaluacionFisica> TbEvaluacionFisicas { get; set; } = new List<TbEvaluacionFisica>();

    [InverseProperty("IdPacienteNavigation")]
    public virtual ICollection<TbPago> TbPagos { get; set; } = new List<TbPago>();

    [InverseProperty("IdPacienteNavigation")]
    public virtual ICollection<TbPrescripcion> TbPrescripcions { get; set; } = new List<TbPrescripcion>();
}
