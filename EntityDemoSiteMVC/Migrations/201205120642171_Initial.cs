namespace EntityDemoSiteMVC.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropColumn("Customer", "EmailAddress");
        }
        
        public override void Down()
        {
            AddColumn("Customer", "EmailAddress", c => c.String());
        }
    }
}
