﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SACAAE.Data_Access
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SACAAE_SP : DbContext
    {
        public SACAAE_SP()
            : base("name=SACAAE_SP")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual int SP_CreateGroupsinNewSemester(Nullable<int> pIdPeriod)
        {
            var pIdPeriodParameter = pIdPeriod.HasValue ?
                new ObjectParameter("pIdPeriod", pIdPeriod) :
                new ObjectParameter("pIdPeriod", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_CreateGroupsinNewSemester", pIdPeriodParameter);
        }
    }
}