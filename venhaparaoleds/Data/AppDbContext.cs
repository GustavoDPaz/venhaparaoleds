using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using venhaparaoleds.Models;

namespace Venhaparaoleds.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Candidato> Candidatos { get; set; }
    public DbSet<Concurso> Concursos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder.UseSqlite("Data Source=projeto.db");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
   
        modelBuilder.Entity<Candidato>()
            .Property(e => e.Profissoes)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        modelBuilder.Entity<Concurso>()
            .Property(e => e.Vagas) 
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        
        base.OnModelCreating(modelBuilder);
        
    }
}