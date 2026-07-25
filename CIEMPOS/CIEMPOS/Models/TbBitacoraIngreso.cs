using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Models;

[Table("TB_BitacoraIngreso")]
public partial class TbBitacoraIngreso
{
    [Key]
    public int IdBitacora { get; set; }

    public int IdUsuario { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaHoraIngreso { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaHoraSalida { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("TbBitacoraIngresos")]
    public virtual TbUsuario IdUsuarioNavigation { get; set; } = null!;
}
