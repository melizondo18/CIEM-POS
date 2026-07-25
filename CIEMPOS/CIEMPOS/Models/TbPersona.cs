using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Models;

[Table("TB_Persona")]
[Index("Email", Name = "UQ_TB_Persona_Email", IsUnique = true)]
[Index("Identificacion", Name = "UQ_TB_Persona_Identificacion", IsUnique = true)]
public partial class TbPersona
{
    [Key]
    public int IdPersona { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Apellido { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Identificacion { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Sexo { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Telefono { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Direccion { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? ContactoEmergencia { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TelefonoEmergencia { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    public bool Estado { get; set; }

    [InverseProperty("IdPersonaNavigation")]
    public virtual TbPaciente? TbPaciente { get; set; }

    [InverseProperty("IdPersonaNavigation")]
    public virtual TbUsuario? TbUsuario { get; set; }
}
