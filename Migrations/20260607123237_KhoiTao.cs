using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Website_QuanLyKhoHangThucPham.Migrations
{
    /// <inheritdoc />
    public partial class KhoiTao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionKey = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Controller = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TransactionRef = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_AppPermissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "AppPermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExportRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProcessedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExportRequests_AspNetUsers_ProcessedById",
                        column: x => x.ProcessedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExportRequests_AspNetUsers_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImportReceipts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiptCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ConfirmedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SupplierId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportReceipts_AspNetUsers_ConfirmedById",
                        column: x => x.ConfirmedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImportReceipts_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImportReceipts_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImportReceipts_Suppliers_SupplierId1",
                        column: x => x.SupplierId1,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinStockLevel = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExportRequestDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExportRequestId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RequestedQuantity = table.Column<int>(type: "int", nullable: false),
                    ActualQuantity = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProductId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportRequestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExportRequestDetails_ExportRequests_ExportRequestId",
                        column: x => x.ExportRequestId,
                        principalTable: "ExportRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExportRequestDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExportRequestDetails_Products_ProductId1",
                        column: x => x.ProductId1,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImportReceiptDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportReceiptId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BatchCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportReceiptDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportReceiptDetails_ImportReceipts_ImportReceiptId",
                        column: x => x.ImportReceiptId,
                        principalTable: "ImportReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportReceiptDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImportReceiptDetails_Products_ProductId1",
                        column: x => x.ProductId1,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InventoryBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    BatchCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryBatches_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreOrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreOrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreOrderItems_StoreOrders_StoreOrderId",
                        column: x => x.StoreOrderId,
                        principalTable: "StoreOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppPermissions",
                columns: new[] { "Id", "Action", "Controller", "Description", "DisplayName", "PermissionKey" },
                values: new object[,]
                {
                    { 1, "Index", "Category", null, "Xem danh muc", "Category.Index" },
                    { 2, "Create", "Category", null, "Tao danh muc", "Category.Create" },
                    { 3, "Edit", "Category", null, "Sua danh muc", "Category.Edit" },
                    { 4, "Index", "Supplier", null, "Xem NCC", "Supplier.Index" },
                    { 5, "Create", "Supplier", null, "Tao NCC", "Supplier.Create" },
                    { 6, "Index", "Product", null, "Xem san pham", "Product.Index" },
                    { 7, "Create", "Product", null, "Tao san pham", "Product.Create" },
                    { 8, "Edit", "Product", null, "Sua san pham", "Product.Edit" },
                    { 9, "Index", "ImportReceipt", null, "Xem phieu nhap", "ImportReceipt.Index" },
                    { 10, "Create", "ImportReceipt", null, "Lap phieu nhap", "ImportReceipt.Create" },
                    { 11, "Index", "ExportRequest", null, "Xem yeu cau xuat", "ExportRequest.Index" },
                    { 12, "Create", "ExportRequest", null, "Tao yeu cau xuat", "ExportRequest.Create" },
                    { 13, "Process", "ExportRequest", null, "Duyet xuat kho", "ExportRequest.Process" },
                    { 14, "Index", "AuditLog", null, "Xem audit log", "AuditLog.Index" },
                    { 15, "History", "Payment", null, "Xem lich su thanh toan", "Payment.History" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Rau cu tuoi" },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Trai cay" },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Thit & Hai san" },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Sua & San pham tu sua" },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Do dong hop" },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Ngu coc & Hat" },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Do uong" },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Gia vi & Nuoc cham" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Address", "Code", "ContactPerson", "CreatedAt", "Email", "IsActive", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, null, "SACH01", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "sach@viet.vn", true, "Cong ty Thuc pham Sach Viet", "0901234567" },
                    { 2, null, "MEK02", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "mek@delta.vn", true, "Cong ty Nong San Mekong", "0912345678" },
                    { 3, null, "BD03", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bd@bien.vn", true, "Cong ty Hai San Bien Dong", "0923456789" },
                    { 4, null, "VNM04", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "vn@milk.vn", true, "Vinamilk Distribution", "1800123456" },
                    { 5, null, "SAB05", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "sab@eco.vn", true, "Sabeco Trading", "0934567890" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CostPrice", "CreatedAt", "Description", "ImageUrl", "IsActive", "MinStockLevel", "Name", "SKU", "SellPrice", "SupplierId", "Unit" },
                values: new object[,]
                {
                    { 1, 1, 8000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 20, "Rau muong tuoi", "SP001", 12000m, 1, "kg" },
                    { 2, 1, 15000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 15, "Ca chua bi do", "SP002", 22000m, 2, "kg" },
                    { 3, 1, 10000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 20, "Bap cai xanh", "SP003", 15000m, 1, "kg" },
                    { 4, 2, 25000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 10, "Chuoi tieu", "SP004", 35000m, 2, "nai" },
                    { 5, 2, 40000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 10, "Xoai cat Hoa Loc", "SP005", 58000m, 2, "kg" },
                    { 6, 3, 130000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 10, "Thit heo ba chi tuoi", "SP006", 160000m, 1, "kg" },
                    { 7, 3, 180000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 5, "Tom su tuoi", "SP007", 230000m, 3, "kg" },
                    { 8, 3, 120000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 5, "Ca phi le dong lanh", "SP008", 155000m, 3, "kg" },
                    { 9, 3, 28000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 30, "Trung ga tuoi (vi 10)", "SP009", 38000m, 1, "vi" },
                    { 10, 4, 28000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 50, "Sua tuoi Vinamilk 1L", "SP010", 36000m, 4, "hop" },
                    { 11, 4, 42000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 20, "Pho mai Con Bo Cuoi", "SP011", 56000m, 4, "goi" },
                    { 12, 5, 35000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 30, "Ca hoi dong hop", "SP012", 48000m, 3, "hop" },
                    { 13, 6, 85000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 20, "Gao ST25 5kg", "SP013", 110000m, 2, "tui" },
                    { 14, 6, 65000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 15, "Hat dieu rang muoi 200g", "SP014", 90000m, 2, "goi" },
                    { 15, 7, 210000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 20, "Nuoc ngot Pepsi 24 lon", "SP015", 265000m, 5, "thung" },
                    { 16, 8, 25000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 30, "Nuoc mam Chinsu 500ml", "SP016", 33000m, 1, "chai" },
                    { 17, 8, 38000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 25, "Dau an Tuong An 1L", "SP017", 50000m, 1, "chai" },
                    { 18, 6, 18000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, 30, "Bot mi Da Lat 1kg", "SP018", 25000m, 2, "goi" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "PermissionId", "RoleName" },
                values: new object[,]
                {
                    { 1, 1, "Admin" },
                    { 2, 2, "Admin" },
                    { 3, 3, "Admin" },
                    { 4, 4, "Admin" },
                    { 5, 5, "Admin" },
                    { 6, 6, "Admin" },
                    { 7, 7, "Admin" },
                    { 8, 8, "Admin" },
                    { 9, 9, "Admin" },
                    { 10, 10, "Admin" },
                    { 11, 11, "Admin" },
                    { 12, 12, "Admin" },
                    { 13, 13, "Admin" },
                    { 14, 14, "Admin" },
                    { 15, 15, "Admin" },
                    { 16, 6, "WarehouseStaff" },
                    { 17, 7, "WarehouseStaff" },
                    { 18, 8, "WarehouseStaff" },
                    { 19, 9, "WarehouseStaff" },
                    { 20, 10, "WarehouseStaff" },
                    { 21, 11, "WarehouseStaff" },
                    { 22, 13, "WarehouseStaff" },
                    { 23, 6, "SalesStaff" },
                    { 24, 11, "SalesStaff" },
                    { 25, 12, "SalesStaff" }
                });

            migrationBuilder.InsertData(
                table: "InventoryBatches",
                columns: new[] { "Id", "BatchCode", "ExpiryDate", "ManufactureDate", "ProductId", "Quantity", "ReceivedDate" },
                values: new object[,]
                {
                    { 1, "B-SP001-A", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 150, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "B-SP002-A", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 80, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "B-SP003-A", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 120, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "B-SP004-A", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, 60, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "B-SP005-A", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, 90, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "B-SP006-A", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 50, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "B-SP007-A", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, 30, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, "B-SP008-A", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, 40, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, "B-SP009-A", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, 200, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, "B-SP010-A", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, 300, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, "B-SP011-A", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, 100, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, "B-SP012-A", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, 150, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, "B-SP013-A", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, 80, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, "B-SP014-A", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14, 120, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, "B-SP015-A", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15, 60, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, "B-SP016-A", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, 180, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, "B-SP017-A", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 17, 100, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, "B-SP018-A", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 18, 140, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ExportRequestDetails_ExportRequestId",
                table: "ExportRequestDetails",
                column: "ExportRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportRequestDetails_ProductId",
                table: "ExportRequestDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportRequestDetails_ProductId1",
                table: "ExportRequestDetails",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_ExportRequests_ProcessedById",
                table: "ExportRequests",
                column: "ProcessedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExportRequests_RequestCode",
                table: "ExportRequests",
                column: "RequestCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExportRequests_RequestedById",
                table: "ExportRequests",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceiptDetails_ImportReceiptId",
                table: "ImportReceiptDetails",
                column: "ImportReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceiptDetails_ProductId",
                table: "ImportReceiptDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceiptDetails_ProductId1",
                table: "ImportReceiptDetails",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceipts_ConfirmedById",
                table: "ImportReceipts",
                column: "ConfirmedById");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceipts_CreatedById",
                table: "ImportReceipts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceipts_ReceiptCode",
                table: "ImportReceipts",
                column: "ReceiptCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceipts_SupplierId",
                table: "ImportReceipts",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceipts_SupplierId1",
                table: "ImportReceipts",
                column: "SupplierId1");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_BatchCode",
                table: "InventoryBatches",
                column: "BatchCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_ProductId",
                table: "InventoryBatches",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true,
                filter: "[SKU] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreOrderItems_ProductId",
                table: "StoreOrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreOrderItems_StoreOrderId",
                table: "StoreOrderItems",
                column: "StoreOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreOrders_OrderCode",
                table: "StoreOrders",
                column: "OrderCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "ExportRequestDetails");

            migrationBuilder.DropTable(
                name: "ImportReceiptDetails");

            migrationBuilder.DropTable(
                name: "InventoryBatches");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "StoreOrderItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ExportRequests");

            migrationBuilder.DropTable(
                name: "ImportReceipts");

            migrationBuilder.DropTable(
                name: "AppPermissions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "StoreOrders");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
