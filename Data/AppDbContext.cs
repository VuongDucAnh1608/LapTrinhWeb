using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Website_QuanLyKhoHangThucPham.Models;

namespace Website_QuanLyKhoHangThucPham.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryBatch> InventoryBatches { get; set; }
        public DbSet<ImportReceipt> ImportReceipts { get; set; }
        public DbSet<ImportReceiptDetail> ImportReceiptDetails { get; set; }
        public DbSet<ExportRequest> ExportRequests { get; set; }
        public DbSet<ExportRequestDetail> ExportRequestDetails { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<StoreOrder> StoreOrders { get; set; }
        public DbSet<StoreOrderItem> StoreOrderItems { get; set; }
        public DbSet<AppPermission> AppPermissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Product>().HasIndex(p => p.SKU).IsUnique().HasFilter("[SKU] IS NOT NULL");
            b.Entity<InventoryBatch>().HasIndex(x => x.BatchCode).IsUnique();
            b.Entity<ImportReceipt>().HasIndex(x => x.ReceiptCode).IsUnique();
            b.Entity<ExportRequest>().HasIndex(x => x.RequestCode).IsUnique();
            b.Entity<StoreOrder>().HasIndex(x => x.OrderCode).IsUnique();

            // Restrict cascades to avoid multiple cascade path error
            b.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
            b.Entity<Product>().HasOne(p => p.Supplier).WithMany(s => s.Products).HasForeignKey(p => p.SupplierId).OnDelete(DeleteBehavior.Restrict);
            b.Entity<InventoryBatch>().HasOne(x => x.Product).WithMany(p => p.Batches).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
            b.Entity<ImportReceipt>().HasOne(r => r.Supplier).WithMany().HasForeignKey(r => r.SupplierId).OnDelete(DeleteBehavior.Restrict);
            b.Entity<ImportReceipt>().HasOne(r => r.CreatedBy).WithMany().HasForeignKey(r => r.CreatedById).OnDelete(DeleteBehavior.Restrict);
            b.Entity<ImportReceipt>().HasOne(r => r.ConfirmedBy).WithMany().HasForeignKey(r => r.ConfirmedById).OnDelete(DeleteBehavior.Restrict);
            b.Entity<ImportReceiptDetail>().HasOne(d => d.Product).WithMany().HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.Restrict);
            b.Entity<ExportRequest>().HasOne(r => r.RequestedBy).WithMany().HasForeignKey(r => r.RequestedById).OnDelete(DeleteBehavior.Restrict);
            b.Entity<ExportRequest>().HasOne(r => r.ProcessedBy).WithMany().HasForeignKey(r => r.ProcessedById).OnDelete(DeleteBehavior.Restrict);
            b.Entity<ExportRequestDetail>().HasOne(d => d.Product).WithMany().HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.Restrict);
            b.Entity<StoreOrderItem>().HasOne(i => i.Product).WithMany().HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.Restrict);

            var d0 = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var exp1 = new DateTime(2026, 9, 30, 0, 0, 0, DateTimeKind.Utc);
            var exp2 = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc);

            b.Entity<Category>().HasData(
                new Category { Id=1, Name="Rau cu tuoi",          IsActive=true, CreatedAt=d0 },
                new Category { Id=2, Name="Trai cay",             IsActive=true, CreatedAt=d0 },
                new Category { Id=3, Name="Thit & Hai san",       IsActive=true, CreatedAt=d0 },
                new Category { Id=4, Name="Sua & San pham tu sua",IsActive=true, CreatedAt=d0 },
                new Category { Id=5, Name="Do dong hop",          IsActive=true, CreatedAt=d0 },
                new Category { Id=6, Name="Ngu coc & Hat",        IsActive=true, CreatedAt=d0 },
                new Category { Id=7, Name="Do uong",              IsActive=true, CreatedAt=d0 },
                new Category { Id=8, Name="Gia vi & Nuoc cham",   IsActive=true, CreatedAt=d0 }
            );

            b.Entity<Supplier>().HasData(
                new Supplier { Id=1, Name="Cong ty Thuc pham Sach Viet", Code="SACH01", Phone="0901234567", Email="sach@viet.vn",  IsActive=true, CreatedAt=d0 },
                new Supplier { Id=2, Name="Cong ty Nong San Mekong",     Code="MEK02",  Phone="0912345678", Email="mek@delta.vn", IsActive=true, CreatedAt=d0 },
                new Supplier { Id=3, Name="Cong ty Hai San Bien Dong",   Code="BD03",   Phone="0923456789", Email="bd@bien.vn",   IsActive=true, CreatedAt=d0 },
                new Supplier { Id=4, Name="Vinamilk Distribution",       Code="VNM04",  Phone="1800123456", Email="vn@milk.vn",   IsActive=true, CreatedAt=d0 },
                new Supplier { Id=5, Name="Sabeco Trading",              Code="SAB05",  Phone="0934567890", Email="sab@eco.vn",   IsActive=true, CreatedAt=d0 }
            );

            b.Entity<Product>().HasData(
                new Product { Id=1,  Name="Rau muong tuoi",         SKU="SP001", CategoryId=1, SupplierId=1, Unit="kg",    CostPrice=8000,   SellPrice=12000,  MinStockLevel=20, IsActive=true, CreatedAt=d0 },
                new Product { Id=2,  Name="Ca chua bi do",          SKU="SP002", CategoryId=1, SupplierId=2, Unit="kg",    CostPrice=15000,  SellPrice=22000,  MinStockLevel=15, IsActive=true, CreatedAt=d0 },
                new Product { Id=3,  Name="Bap cai xanh",           SKU="SP003", CategoryId=1, SupplierId=1, Unit="kg",    CostPrice=10000,  SellPrice=15000,  MinStockLevel=20, IsActive=true, CreatedAt=d0 },
                new Product { Id=4,  Name="Chuoi tieu",             SKU="SP004", CategoryId=2, SupplierId=2, Unit="nai",   CostPrice=25000,  SellPrice=35000,  MinStockLevel=10, IsActive=true, CreatedAt=d0 },
                new Product { Id=5,  Name="Xoai cat Hoa Loc",       SKU="SP005", CategoryId=2, SupplierId=2, Unit="kg",    CostPrice=40000,  SellPrice=58000,  MinStockLevel=10, IsActive=true, CreatedAt=d0 },
                new Product { Id=6,  Name="Thit heo ba chi tuoi",   SKU="SP006", CategoryId=3, SupplierId=1, Unit="kg",    CostPrice=130000, SellPrice=160000, MinStockLevel=10, IsActive=true, CreatedAt=d0 },
                new Product { Id=7,  Name="Tom su tuoi",            SKU="SP007", CategoryId=3, SupplierId=3, Unit="kg",    CostPrice=180000, SellPrice=230000, MinStockLevel=5,  IsActive=true, CreatedAt=d0 },
                new Product { Id=8,  Name="Ca phi le dong lanh",    SKU="SP008", CategoryId=3, SupplierId=3, Unit="kg",    CostPrice=120000, SellPrice=155000, MinStockLevel=5,  IsActive=true, CreatedAt=d0 },
                new Product { Id=9,  Name="Trung ga tuoi (vi 10)",  SKU="SP009", CategoryId=3, SupplierId=1, Unit="vi",    CostPrice=28000,  SellPrice=38000,  MinStockLevel=30, IsActive=true, CreatedAt=d0 },
                new Product { Id=10, Name="Sua tuoi Vinamilk 1L",   SKU="SP010", CategoryId=4, SupplierId=4, Unit="hop",   CostPrice=28000,  SellPrice=36000,  MinStockLevel=50, IsActive=true, CreatedAt=d0 },
                new Product { Id=11, Name="Pho mai Con Bo Cuoi",    SKU="SP011", CategoryId=4, SupplierId=4, Unit="goi",   CostPrice=42000,  SellPrice=56000,  MinStockLevel=20, IsActive=true, CreatedAt=d0 },
                new Product { Id=12, Name="Ca hoi dong hop",        SKU="SP012", CategoryId=5, SupplierId=3, Unit="hop",   CostPrice=35000,  SellPrice=48000,  MinStockLevel=30, IsActive=true, CreatedAt=d0 },
                new Product { Id=13, Name="Gao ST25 5kg",           SKU="SP013", CategoryId=6, SupplierId=2, Unit="tui",   CostPrice=85000,  SellPrice=110000, MinStockLevel=20, IsActive=true, CreatedAt=d0 },
                new Product { Id=14, Name="Hat dieu rang muoi 200g",SKU="SP014", CategoryId=6, SupplierId=2, Unit="goi",   CostPrice=65000,  SellPrice=90000,  MinStockLevel=15, IsActive=true, CreatedAt=d0 },
                new Product { Id=15, Name="Nuoc ngot Pepsi 24 lon", SKU="SP015", CategoryId=7, SupplierId=5, Unit="thung", CostPrice=210000, SellPrice=265000, MinStockLevel=20, IsActive=true, CreatedAt=d0 },
                new Product { Id=16, Name="Nuoc mam Chinsu 500ml",  SKU="SP016", CategoryId=8, SupplierId=1, Unit="chai",  CostPrice=25000,  SellPrice=33000,  MinStockLevel=30, IsActive=true, CreatedAt=d0 },
                new Product { Id=17, Name="Dau an Tuong An 1L",     SKU="SP017", CategoryId=8, SupplierId=1, Unit="chai",  CostPrice=38000,  SellPrice=50000,  MinStockLevel=25, IsActive=true, CreatedAt=d0 },
                new Product { Id=18, Name="Bot mi Da Lat 1kg",      SKU="SP018", CategoryId=6, SupplierId=2, Unit="goi",   CostPrice=18000,  SellPrice=25000,  MinStockLevel=30, IsActive=true, CreatedAt=d0 }
            );

            b.Entity<InventoryBatch>().HasData(
                new InventoryBatch { Id=1,  ProductId=1,  BatchCode="B-SP001-A", Quantity=150, ExpiryDate=exp2, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=2,  ProductId=2,  BatchCode="B-SP002-A", Quantity=80,  ExpiryDate=exp2, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=3,  ProductId=3,  BatchCode="B-SP003-A", Quantity=120, ExpiryDate=exp2, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=4,  ProductId=4,  BatchCode="B-SP004-A", Quantity=60,  ExpiryDate=exp2, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=5,  ProductId=5,  BatchCode="B-SP005-A", Quantity=90,  ExpiryDate=exp2, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=6,  ProductId=6,  BatchCode="B-SP006-A", Quantity=50,  ExpiryDate=exp2, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=7,  ProductId=7,  BatchCode="B-SP007-A", Quantity=30,  ExpiryDate=exp2, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=8,  ProductId=8,  BatchCode="B-SP008-A", Quantity=40,  ExpiryDate=exp2, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=9,  ProductId=9,  BatchCode="B-SP009-A", Quantity=200, ExpiryDate=exp2, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=10, ProductId=10, BatchCode="B-SP010-A", Quantity=300, ExpiryDate=exp1, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=11, ProductId=11, BatchCode="B-SP011-A", Quantity=100, ExpiryDate=exp1, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=12, ProductId=12, BatchCode="B-SP012-A", Quantity=150, ExpiryDate=exp1, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=13, ProductId=13, BatchCode="B-SP013-A", Quantity=80,  ExpiryDate=exp1, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=14, ProductId=14, BatchCode="B-SP014-A", Quantity=120, ExpiryDate=exp1, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=15, ProductId=15, BatchCode="B-SP015-A", Quantity=60,  ExpiryDate=exp1, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=16, ProductId=16, BatchCode="B-SP016-A", Quantity=180, ExpiryDate=exp1, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=17, ProductId=17, BatchCode="B-SP017-A", Quantity=100, ExpiryDate=exp1, ManufactureDate=d0, ReceivedDate=d0 },
                new InventoryBatch { Id=18, ProductId=18, BatchCode="B-SP018-A", Quantity=140, ExpiryDate=exp1, ManufactureDate=d0, ReceivedDate=d0 }
            );

            // Seed permissions cho dynamic role management
            b.Entity<AppPermission>().HasData(
                new AppPermission { Id=1,  PermissionKey="Category.Index",         DisplayName="Xem danh muc",         Controller="Category",       Action="Index"    },
                new AppPermission { Id=2,  PermissionKey="Category.Create",        DisplayName="Tao danh muc",         Controller="Category",       Action="Create"   },
                new AppPermission { Id=3,  PermissionKey="Category.Edit",          DisplayName="Sua danh muc",         Controller="Category",       Action="Edit"     },
                new AppPermission { Id=4,  PermissionKey="Supplier.Index",         DisplayName="Xem NCC",              Controller="Supplier",        Action="Index"    },
                new AppPermission { Id=5,  PermissionKey="Supplier.Create",        DisplayName="Tao NCC",              Controller="Supplier",        Action="Create"   },
                new AppPermission { Id=6,  PermissionKey="Product.Index",          DisplayName="Xem san pham",         Controller="Product",         Action="Index"    },
                new AppPermission { Id=7,  PermissionKey="Product.Create",         DisplayName="Tao san pham",         Controller="Product",         Action="Create"   },
                new AppPermission { Id=8,  PermissionKey="Product.Edit",           DisplayName="Sua san pham",         Controller="Product",         Action="Edit"     },
                new AppPermission { Id=9,  PermissionKey="ImportReceipt.Index",    DisplayName="Xem phieu nhap",       Controller="ImportReceipt",   Action="Index"    },
                new AppPermission { Id=10, PermissionKey="ImportReceipt.Create",   DisplayName="Lap phieu nhap",       Controller="ImportReceipt",   Action="Create"   },
                new AppPermission { Id=11, PermissionKey="ExportRequest.Index",    DisplayName="Xem yeu cau xuat",     Controller="ExportRequest",   Action="Index"    },
                new AppPermission { Id=12, PermissionKey="ExportRequest.Create",   DisplayName="Tao yeu cau xuat",     Controller="ExportRequest",   Action="Create"   },
                new AppPermission { Id=13, PermissionKey="ExportRequest.Process",  DisplayName="Duyet xuat kho",       Controller="ExportRequest",   Action="Process"  },
                new AppPermission { Id=14, PermissionKey="AuditLog.Index",         DisplayName="Xem audit log",        Controller="AuditLog",        Action="Index"    },
                new AppPermission { Id=15, PermissionKey="Payment.History",        DisplayName="Xem lich su thanh toan",Controller="Payment",        Action="History"  }
            );

            // Admin co tat ca quyen mac dinh
            b.Entity<RolePermission>().HasData(
                new RolePermission { Id=1,  RoleName="Admin", PermissionId=1  },
                new RolePermission { Id=2,  RoleName="Admin", PermissionId=2  },
                new RolePermission { Id=3,  RoleName="Admin", PermissionId=3  },
                new RolePermission { Id=4,  RoleName="Admin", PermissionId=4  },
                new RolePermission { Id=5,  RoleName="Admin", PermissionId=5  },
                new RolePermission { Id=6,  RoleName="Admin", PermissionId=6  },
                new RolePermission { Id=7,  RoleName="Admin", PermissionId=7  },
                new RolePermission { Id=8,  RoleName="Admin", PermissionId=8  },
                new RolePermission { Id=9,  RoleName="Admin", PermissionId=9  },
                new RolePermission { Id=10, RoleName="Admin", PermissionId=10 },
                new RolePermission { Id=11, RoleName="Admin", PermissionId=11 },
                new RolePermission { Id=12, RoleName="Admin", PermissionId=12 },
                new RolePermission { Id=13, RoleName="Admin", PermissionId=13 },
                new RolePermission { Id=14, RoleName="Admin", PermissionId=14 },
                new RolePermission { Id=15, RoleName="Admin", PermissionId=15 },
                // WarehouseStaff
                new RolePermission { Id=16, RoleName="WarehouseStaff", PermissionId=6  },
                new RolePermission { Id=17, RoleName="WarehouseStaff", PermissionId=7  },
                new RolePermission { Id=18, RoleName="WarehouseStaff", PermissionId=8  },
                new RolePermission { Id=19, RoleName="WarehouseStaff", PermissionId=9  },
                new RolePermission { Id=20, RoleName="WarehouseStaff", PermissionId=10 },
                new RolePermission { Id=21, RoleName="WarehouseStaff", PermissionId=11 },
                new RolePermission { Id=22, RoleName="WarehouseStaff", PermissionId=13 },
                // SalesStaff
                new RolePermission { Id=23, RoleName="SalesStaff", PermissionId=6  },
                new RolePermission { Id=24, RoleName="SalesStaff", PermissionId=11 },
                new RolePermission { Id=25, RoleName="SalesStaff", PermissionId=12 }
            );
        }
    }
}
