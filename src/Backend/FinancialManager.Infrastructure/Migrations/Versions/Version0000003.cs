using FluentMigrator;

namespace FinancialManager.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_EXPENSES, "Creating Expenses Table")]
public class Version0000003 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Expenses")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("ExpenseDate").AsDateTime().NotNullable()
            .WithColumn("Amount").AsDecimal().NotNullable()
            .WithColumn("PaymentMethod").AsInt32().NotNullable()
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("CategoryId").AsGuid().Nullable()
            .WithColumn("UserId").AsGuid().NotNullable();

        Create.ForeignKey("FK_Expenses_Categories_CategoryId")
            .FromTable("Expenses").ForeignColumn("CategoryId")
            .ToTable("Categories").PrimaryColumn("Id");

        Create.ForeignKey("FK_Expenses_Users_UserId")   
            .FromTable("Expenses").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("Id");

        Create.Index("IX_Expenses_UserId_ExpenseDate")
            .OnTable("Expenses")
            .OnColumn("UserId").Ascending()
            .OnColumn("ExpenseDate").Descending();
    }
}