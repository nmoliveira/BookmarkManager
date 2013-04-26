namespace BookmarkManager.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookmarkPosition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookmarks", "BookmarkPosition", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookmarks", "BookmarkPosition");
        }
    }
}
