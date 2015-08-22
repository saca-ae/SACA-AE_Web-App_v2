namespace SACAAE.Migrations
{
    using SACAAE.Data_Access;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SACAAE.Data_Access.SACAAEContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SACAAE.Data_Access.SACAAEContext context)
        {
            DBInitializer.init(context);
        }
    }
}
