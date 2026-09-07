using EnterpriseCouponEngine.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

app.MapPost("/api/v1/coupons/redeem", async (RedeemRequest request, IDistributedCache cache) =>
{
    if (string.IsNullOrEmpty(request.CouponCode) || request.OrderTotal <= 0)
    {
        return Results.BadRequest(new { error = "Invalid request payload" });
    }

    var lockKey = $"lock:{request.CouponCode}";
    var isLocked = await cache.GetStringAsync(lockKey);
    
    if (!string.IsNullOrEmpty(isLocked))
    {
        return Results.StatusCode(423); 
    }

    await cache.SetStringAsync(lockKey, "locked", new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2)
    });

    try
    {
        var cachedCoupon = await cache.GetStringAsync(request.CouponCode);
        if (string.IsNullOrEmpty(cachedCoupon))
        {
            return Results.NotFound(new { error = "Coupon code not found or expired" });
        }

        var coupon = JsonSerializer.Deserialize<Coupon>(cachedCoupon);
        if (coupon == null || !coupon.IsActive || coupon.ExpirationDate < DateTime.UtcNow)
        {
            return Results.BadRequest(new { error = "Coupon is inactive or expired" });
        }

        if (coupon.CurrentUsageCount >= coupon.MaxUsageLimit)
        {
            return Results.BadRequest(new { error = "Coupon usage limit has been reached" });
        }

        coupon.CurrentUsageCount++;
        
        var updatedCouponJson = JsonSerializer.Serialize(coupon);
        await cache.SetStringAsync(request.CouponCode, updatedCouponJson);

        var discount = coupon.DiscountAmount;
        var finalPrice = Math.Max(0, request.OrderTotal - discount);

        return Results.Ok(new 
        { 
            message = "Coupon applied successfully",
            discountAmount = discount,
            finalPrice = finalPrice
        });
    }
    finally
    {
        await cache.RemoveAsync(lockKey);
    }
});

app.MapPost("/api/v1/coupons", async (Coupon coupon, IDistributedCache cache) =>
{
    if (string.IsNullOrEmpty(coupon.Code) || coupon.DiscountAmount <= 0)
    {
        return Results.BadRequest(new { error = "Invalid coupon configuration" });
    }

    var couponJson = JsonSerializer.Serialize(coupon);
    await cache.SetStringAsync(coupon.Code, couponJson);

    return Results.Created($"/api/v1/coupons/{coupon.Code}", coupon);
});

app.Run();
