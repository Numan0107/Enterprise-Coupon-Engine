namespace EnterpriseCouponEngine.Models;

public class Coupon
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public decimal DiscountAmount { get; set; }
    public int MaxUsageLimit { get; set; }
    public int CurrentUsageCount { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; } = true;
}
