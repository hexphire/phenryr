using Microsoft.EntityFrameworkCore.Migrations;

namespace Phenryr.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EightBallAnswer",
                columns: table => new
                {
                    AnswerId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnswerText = table.Column<string>(nullable: true),
                    AnswerColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EightBallAnswer", x => x.AnswerId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EightBallAnswer");
        }
    }
}
