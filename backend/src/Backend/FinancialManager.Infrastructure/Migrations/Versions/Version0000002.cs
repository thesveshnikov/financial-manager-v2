using FluentMigrator;

namespace FinancialManager.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_CATEGORIES, "Creating Categories Table")]
public class Version0000002 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Categories")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("UserId").AsGuid().NotNullable();

        Create.ForeignKey("FK_Categories_Users_UserId")
            .FromTable("Categories").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("Id");

        Create.Index("IX_Categories_UserId_Name")
            .OnTable("Categories")
            .OnColumn("UserId").Ascending()
            .OnColumn("Name").Ascending()
            .WithOptions()
            .Unique();
    }
}