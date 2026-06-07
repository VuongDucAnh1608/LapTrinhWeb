namespace Website_QuanLyKhoHangThucPham.Models
{
    public enum ReceiptStatus
    {
        Draft = 0,
        Confirmed = 1,
        Cancelled = 2
    }

    public enum ExportStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Completed = 3
    }

    public enum UserRole
    {
        Admin = 0,
        WarehouseStaff = 1,
        SalesStaff = 2
    }
}