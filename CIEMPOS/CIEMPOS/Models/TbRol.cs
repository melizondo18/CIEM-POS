using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Models;

[Table("TB_Rol")]
[Index("Nombre", Name = "UQ_TB_Rol_Nombre", IsUnique = true)]
public partial class TbRol
{
    [Key]
    public int IdRol { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    public bool Estado { get; set; }

    [InverseProperty("IdRolNavigation")]
    public virtual ICollection<TbUsuario> TbUsuarios { get; set; } = new List<TbUsuario>();

    [ForeignKey("IdRol")]
    [InverseProperty("IdRols")]
    public virtual ICollection<TbPermiso> IdPermisos { get; set; } = new List<TbPermiso>();
}
