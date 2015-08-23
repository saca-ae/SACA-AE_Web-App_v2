using Microsoft.AspNet.Identity.EntityFramework;
using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SACAAE.Data_Access
{
    public class SACAAEContext : IdentityDbContext<User>
    {
        public SACAAEContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static SACAAEContext Create()
        {
            return new SACAAEContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<BillingDetail>().HasKey(h => new { h.BillingDetailID, h.BillingID });
            //modelBuilder.Entity<TicketMessage>().HasKey(h => new { h.TicketMessageID, h.TicketID });

            base.OnModelCreating(modelBuilder);
        }

        //Tables
        public DbSet<Aula> Aulas { get; set; }
        public DbSet<BloqueAcademico> BloquesAcademicos { get; set; }
        public DbSet<BloqueAcademicoXPlanDeEstudio> BloquesAcademicosXPlanesDeEstudio { get; set; }
        public DbSet<BloqueXPlanXCurso> BloquesXPlanesXCursos { get; set; }
        public DbSet<Comision> Comisiones { get; set; }
        public DbSet<ComisionXProfesor> ComisionesXProfesores { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Modalidad> Modalidades { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<PlanDeEstudio> PlanesDeEstudio { get; set; }
        public DbSet<PlanDeEstudioXSede> PlanesDeEstudioXSedes { get; set; }
        public DbSet<ProfesorXGrupo> ProfesoresXCursos { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<ProyectoXProfesor> ProyectosXProfesores { get; set; }
        public DbSet<Sede> Sedes { get; set; }
        public DbSet<Plaza> Plazas { get; set; }
        public DbSet<PlazaXProfesor> PlazasXProfesores { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<TipoEntidad> TipoEntidades { get; set; }

        public DbSet<GrupoAula> GrupoAula { get; set; }
    }
}
/*
 * Rollback database
 * update-database -target:0
 */