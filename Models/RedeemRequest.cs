namespace EnterpriseCouponEngine.Models;

public class RedeemRequest
{
    public string CouponCode { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public decimal OrderTotal { get; set; }
}
