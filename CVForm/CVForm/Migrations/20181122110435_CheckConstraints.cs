using Microsoft.EntityFrameworkCore.Migrations;

namespace CVForm.Migrations
{
    public partial class CheckConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_JobOfers_JobOfferID",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_JobOfferID",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "JobOfferID",
                table: "JobApplications");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_OfferId",
                table: "JobApplications",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_JobOfers_OfferId",
                table: "JobApplications",
                column: "OfferId",
                principalTable: "JobOfers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql("ALTER TABLE JobOfers ADD CONSTRAINT CK_salaryFromToCheckCheck CHECK (SalaryTo >=  SalaryFrom);");
            migrationBuilder.Sql("ALTER TABLE JobOfers ADD CONSTRAINT CK_salaryToCheck CHECK (SalaryTo > 0);");
            migrationBuilder.Sql("ALTER TABLE JobOfers ADD CONSTRAINT CK_salaryFromCheck CHECK (SalaryFrom > 0);");
            migrationBuilder.Sql("ALTER TABLE JobOfers ADD CONSTRAINT CK_validUntilCheck CHECK (ValidUntil>=GetDate());");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_JobOfers_OfferId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_OfferId",
                table: "JobApplications");

            migrationBuilder.AddColumn<int>(
                name: "JobOfferID",
                table: "JobApplications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobOfferID",
                table: "JobApplications",
                column: "JobOfferID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_JobOfers_JobOfferID",
                table: "JobApplications",
                column: "JobOfferID",
                principalTable: "JobOfers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
