using FinancialManager.Domain.Enums;
using FluentMigrator;

namespace FinancialManager.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_USERS, "Creating Users Table")]
public class Version0000001 : ForwardOnlyMigration
{
  public override void Up()
  {
    Create.Table("Users")
      .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
      .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
      .WithColumn("Name").AsString(255).NotNullable()
      .WithColumn("Email").AsString(255).NotNullable()
      .WithColumn("Password").AsString(2000).NotNullable()
      .WithColumn("DateOfBirthday").AsDateTime().NotNullable()
      .WithColumn("Role").AsInt32().NotNullable()
      .WithDefaultValue((int)Role.Admin);

    Execute.Sql(@"
            ALTER TABLE Users
            ADD COLUMN ActiveEmail VARCHAR(255)
            GENERATED ALWAYS AS (
                CASE
                    WHEN Active = 1 THEN Email
                    ELSE NULL
                END
            ) STORED;
        ");

    Create.Index("IX_Users_Email")
      .OnTable("Users")
      .OnColumn("ActiveEmail")
      .Unique();
  }
}
