using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Models;

public partial class ZooProjectContext : DbContext
{
    public ZooProjectContext()
    {
    }

    public ZooProjectContext(DbContextOptions<ZooProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<ZooAnimal> ZooAnimals { get; set; }

    public virtual DbSet<ZooTable> ZooTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-4G7SNHH9,1433;Database=ZooProject;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.ToTable("Animal");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ZooAnimal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ZooAnima__3214EC071517DE3C");

            entity.ToTable("ZooAnimal");

            entity.HasOne(d => d.Animal).WithMany(p => p.ZooAnimals)
                .HasForeignKey(d => d.AnimalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ZooAnimal_Animal");

            entity.HasOne(d => d.Zoo).WithMany(p => p.ZooAnimals)
                .HasForeignKey(d => d.ZooId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ZooAnimal_ZooTable");
        });

        modelBuilder.Entity<ZooTable>(entity =>
        {
            entity.ToTable("ZooTable");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
