using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Models;

[Table("TB_BitacoraMovimiento")]
public partial class TbBitacoraMovimiento
{
    [Key]
    public int IdMovimiento { get; set; }

    public int IdUsuario { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Modulo { get; set; } = null!;

    public int IdRegistroAfectado { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TipoMovimiento { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime FechaHora { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("TbBitacoraMovimientos")]
    public virtual TbUsuario IdUsuarioNavigation { get; set; } = null!;
}
