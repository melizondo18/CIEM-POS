using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Models;

[Table("TB_Permiso")]
[Index("Nombre", Name = "UQ_TB_Permiso_Nombre", IsUnique = true)]
public partial class TbPermiso
{
    [Key]
    public int IdPermiso { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    public bool Estado { get; set; }

    [ForeignKey("IdPermiso")]
    [InverseProperty("IdPermisos")]
    public virtual ICollection<TbRol> IdRols { get; set; } = new List<TbRol>();
}
