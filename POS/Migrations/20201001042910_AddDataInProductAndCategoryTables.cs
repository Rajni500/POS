using Microsoft.EntityFrameworkCore.Migrations;

namespace POS.Migrations
{
    public partial class AddDataInProductAndCategoryTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
  Set Identity_insert [User] ON 
  Insert into [User] ([Id],[Title],[Email],[Password],[PhoneNumber],[RoleId]) values ( 1, 'Sam', 'Sam@abc.com','abc','9087654321',2)
  
  Insert into [User] ([Id],[Title],[Email],[Password],[PhoneNumber],[RoleId]) values ( 2, 'Rajni', 'Rajni@abc.com','abc','9087654321',1)
  
  Insert into [User] ([Id],[Title],[Email],[Password],[PhoneNumber],[RoleId]) values ( 3, 'Seeta', 'Seeta@abc.com','abc','9087654321',2)
  
  Insert into [User] ([Id],[Title],[Email],[Password],[PhoneNumber],[RoleId]) values ( 4, 'Geeta', 'Geeta@abc.com','abc','9087654321',2)
  Set Identity_insert [User] OFF


  Set Identity_insert [Category] ON 
  Insert into [Category] ([Id],[Title]) values ( 1, 'Computers')
  Insert into [Category] ([Id],[Title]) values ( 2, 'Fruits')
  Insert into [Category] ([Id],[Title]) values ( 3, 'Clothings')
  Insert into [Category] ([Id],[Title]) values ( 4, 'Services')
  Insert into [Category] ([Id],[Title]) values ( 5, 'Burger')
  Insert into [Category] ([Id],[Title]) values ( 6, 'Pizza')
  Set Identity_insert [Category] OFF


  Set Identity_insert [Product] ON
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (1,'clothing',1999,300,'clothing',3)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (2,'computer_repair',3999,5,'computer_repair',4)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (3,'comuter',69999,10,'comuter',1)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (4,'gift_folding',199,100,'gift_folding',4)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (5,'grapes',99,20,'grapes',2)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (6,'headphone',999,150,'headphone',1)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (7,'jacket',3999,50,'jacket',3)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (8,'jacket_men',6999,30,'jacket_men',3)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (9,'keyboard',4999,100,'keyboard',1)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (10,'kiwi',199,50,'kiwi',2)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (11,'motherboard',9999,20,'motherboard',1)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (12,'mouse',199,200,'mouse',1)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (13,'notebook',49999,5,'notebook',1)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (14,'strawberries',499,10,'strawberries',2)
  Insert into [Product]([Id],[Title],[UnitPrice],[AvailableQuantity],[ImageName], [CategoryId])values (15,'tie',299,90,'tie',3)
  Set Identity_insert [Product] OFF
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
