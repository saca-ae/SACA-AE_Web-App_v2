using SACAAE.Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class Sede
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int GroupEnum { get; set; }

        public virtual ICollection<Classroom> Classrooms { get; set; }
        public virtual ICollection<StudyPlanXSede> StudyPlansXSedes { get; set; }

        private const string MuchoSede = "Sede ya existe";

        private SACAAEContext gvDatabase = new SACAAEContext();
       
        public IQueryable tomarTodasLasSedes()
        {
            return from sede in gvDatabase.Sedes
                   orderby sede.Name
                   select sede;
        }

        public IQueryable<Sede> ObtenerTodosSedes()
        {
            return from Sede in gvDatabase.Sedes
                   orderby Sede.Name
                   select Sede;
        }


        public Sede ObtenerSede(int id)
        {
            return gvDatabase.Sedes.SingleOrDefault(sede => sede.ID == id);

        }

        public Sede ObtenerSede(String nombre) {

            return gvDatabase.Sedes.SingleOrDefault(sede => sede.Name == nombre);
        }

        public void crearSede(Sede sede) 
        {
            if (ExisteSede(sede))
                throw new ArgumentException(MuchoSede);

            gvDatabase.Sedes.Add(sede);
            gvDatabase.SaveChanges();
        }

        public void borrarSede(Sede pSede) {

            var vSede = gvDatabase.Sedes.SingleOrDefault(sede => sede.ID == pSede.ID);
            if (vSede != null)
            {
                gvDatabase.Sedes.Remove(vSede);
                gvDatabase.SaveChanges();
            }
            else
                return;
     
        }

        public void borrarSede(String nombre) {

            borrarSede(ObtenerSede(nombre));
        }

        public bool ExisteSede(Sede sede)
        {
            if (sede == null)
                return false;
            return (gvDatabase.Sedes.SingleOrDefault(s => s.ID == sede.ID ||
                s.ID == sede.ID) != null);
        }

        public void Actualizar(Sede sede)
        {
            if (!ExisteSede(sede))
                crearSede(sede);

            var temp = gvDatabase.Sedes.Find(sede.ID);

            if (temp != null)
            {
                gvDatabase.Entry(temp).Property(s => s.Name).CurrentValue = sede.Name;
            }

            gvDatabase.SaveChanges(); 
        }
    
    }
}