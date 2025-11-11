using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceOne.Migrations
{
    /// <inheritdoc />
    public partial class addpublisherpropertyforblogtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "Blogs");
        }
    }
}
