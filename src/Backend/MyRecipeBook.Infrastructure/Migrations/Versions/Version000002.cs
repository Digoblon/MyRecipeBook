using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_RECIPES,"Create a table to save the recipes' information")]
public class Version000002 : VersionBase
{
    private const string RECIPE_TABLE_NAME = "Recipes";
    
    public override void Up()
    {
        CreateTable(RECIPE_TABLE_NAME)
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("CookingTime").AsInt32().Nullable()
            .WithColumn("Difficulty").AsInt32().Nullable()
            .WithColumn("UserID").AsInt64().NotNullable().ForeignKey("FK_Recipe_User_Id","Users","Id");

        CreateTable("Ingredients")
            .WithColumn("Item").AsString().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredient_Recipe_Id", RECIPE_TABLE_NAME, "Id")
            .OnDelete(System.Data.Rule.Cascade);
        
        CreateTable("Instructions")
            .WithColumn("Step").AsInt32().NotNullable()
            .WithColumn("Text").AsString(2000).NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Instruction_Recipe_Id", RECIPE_TABLE_NAME, "Id")
            .OnDelete(System.Data.Rule.Cascade);
        
        CreateTable("DishTypes")
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_DishType_Recipe_Id", RECIPE_TABLE_NAME, "Id")
            .OnDelete(System.Data.Rule.Cascade);
    }
}