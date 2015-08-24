namespace SACAAE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class periodofixesv2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Periodo", "NumberID", "dbo.NumeroPeriodo");
            DropForeignKey("dbo.NumeroPeriodo", "TypeID", "dbo.TipoPeriodo");
            DropIndex("dbo.Periodo", new[] { "NumberID" });
            DropIndex("dbo.NumeroPeriodo", new[] { "TypeID" });
            AlterColumn("dbo.Periodo", "NumberID", c => c.Int(nullable: false));
            AlterColumn("dbo.NumeroPeriodo", "TypeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Periodo", "NumberID");
            CreateIndex("dbo.NumeroPeriodo", "TypeID");
            AddForeignKey("dbo.Periodo", "NumberID", "dbo.NumeroPeriodo", "ID", cascadeDelete: true);
            AddForeignKey("dbo.NumeroPeriodo", "TypeID", "dbo.TipoPeriodo", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NumeroPeriodo", "TypeID", "dbo.TipoPeriodo");
            DropForeignKey("dbo.Periodo", "NumberID", "dbo.NumeroPeriodo");
            DropIndex("dbo.NumeroPeriodo", new[] { "TypeID" });
            DropIndex("dbo.Periodo", new[] { "NumberID" });
            AlterColumn("dbo.NumeroPeriodo", "TypeID", c => c.Int());
            AlterColumn("dbo.Periodo", "NumberID", c => c.Int());
            CreateIndex("dbo.NumeroPeriodo", "TypeID");
            CreateIndex("dbo.Periodo", "NumberID");
            AddForeignKey("dbo.NumeroPeriodo", "TypeID", "dbo.TipoPeriodo", "ID");
            AddForeignKey("dbo.Periodo", "NumberID", "dbo.NumeroPeriodo", "ID");
        }
    }
}
