using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace POS.Migrations
{
    public partial class EncryptedPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "User",
                nullable: true);

            migrationBuilder.Sql(@"

              Update [User] set PasswordHash = 0x1337F80FA3D7245DBCE8E302E01BB3B5AD2761CC8FCB34D251CC171AD5D21CA642F6112BFC973569E5096B421F51C9EEB46A18D03218B30FEE034F8CA46FD158,
              PasswordSalt = 0x41F4EB604D652C8CED4AC1052D9527633F32A3A4FF8CA5E1D35AED58C2ADF7E2934C9DB63DB44F582B7A7F8C9AF7A1338F7D687BC5B3219D47D95E0AFA620704B06B222659F282DB3E534E3A6E8C9D011444BAE59EACAC366939DA5D1A62A0758C323BC9E70805BB2E0F2705B71F17FE38A1F9F682F9D3B0BEC178ED41F11AC2 
              where Id in (1,2,3,4)

            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                nullable: true);
        }
    }
}
