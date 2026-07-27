using System;
using System.Collections.Generic;
using CIEMPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbBitacoraIngreso> TbBitacoraIngresos { get; set; }

    public virtual DbSet<TbBitacoraMovimiento> TbBitacoraMovimientos { get; set; }

    public virtual DbSet<TbEvaluacionFisica> TbEvaluacionFisicas { get; set; }

    public virtual DbSet<TbPaciente> TbPacientes { get; set; }

    public virtual DbSet<TbPago> TbPagos { get; set; }

    public virtual DbSet<TbPermiso> TbPermisos { get; set; }

    public virtual DbSet<TbPersona> TbPersonas { get; set; }

    public virtual DbSet<TbPrescripcion> TbPrescripcions { get; set; }

    public virtual DbSet<TbRol> TbRols { get; set; }

    public virtual DbSet<TbUsuario> TbUsuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbBitacoraIngreso>(entity =>
        {
            entity.Property(e => e.FechaHoraIngreso).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TbBitacoraIngresos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_BitacoraIngreso_TB_Usuario");
        });

        modelBuilder.Entity<TbBitacoraMovimiento>(entity =>
        {
            entity.Property(e => e.FechaHora).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TbBitacoraMovimientos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_BitacoraMovimiento_TB_Usuario");
        });

        modelBuilder.Entity<TbEvaluacionFisica>(entity =>
        {
            entity.Property(e => e.FechaEvaluacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.TbEvaluacionFisicas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_EvaluacionFisica_TB_Paciente");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TbEvaluacionFisicas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_EvaluacionFisica_TB_Usuario");
        });

        modelBuilder.Entity<TbPaciente>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasOne(d => d.IdPersonaNavigation).WithOne(p => p.TbPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_Paciente_TB_Persona");
        });

        modelBuilder.Entity<TbPago>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaPago).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.TbPagos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_Pago_TB_Paciente");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TbPagos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_Pago_TB_Usuario");
        });

        modelBuilder.Entity<TbPermiso>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue(true);
        });

        modelBuilder.Entity<TbPersona>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<TbPrescripcion>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaPrescripcion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.TbPrescripcions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_Prescripcion_TB_Paciente");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TbPrescripcions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_Prescripcion_TB_Usuario");
        });

        modelBuilder.Entity<TbRol>(entity =>
        {
            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasMany(d => d.IdPermisos).WithMany(p => p.IdRols)
                .UsingEntity<Dictionary<string, object>>(
                    "TbRolPermiso",
                    r => r.HasOne<TbPermiso>().WithMany()
                        .HasForeignKey("IdPermiso")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TB_RolPermiso_TB_Permiso"),
                    l => l.HasOne<TbRol>().WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TB_RolPermiso_TB_Rol"),
                    j =>
                    {
                        j.HasKey("IdRol", "IdPermiso");
                        j.ToTable("TB_RolPermiso");
                    });
        });

        modelBuilder.Entity<TbUsuario>(entity =>
        {
            entity.Property(e => e.DebeCambiarContrasena).HasDefaultValue(true);
            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasOne(d => d.IdPersonaNavigation).WithOne(p => p.TbUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_Usuario_TB_Persona");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.TbUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_Usuario_TB_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
