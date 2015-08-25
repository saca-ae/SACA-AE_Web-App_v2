namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamegroup : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.GrupoAula", name: "Group", newName: "GroupID");
            RenameIndex(table: "dbo.GrupoAula", name: "IX_Group", newName: "IX_GroupID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.GrupoAula", name: "IX_GroupID", newName: "IX_Group");
            RenameColumn(table: "dbo.GrupoAula", name: "GroupID", newName: "Group");
        }
    }
}
