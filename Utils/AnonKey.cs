using Microsoft.AspNetCore.Http;

namespace MetasAhorro.Utils;

public static class AnonKey
{
    public const string CookieName = "ma_anon";

    public static string GetOrCreate(HttpContext ctx)
    {
        if (ctx.Request.Cookies.TryGetValue(CookieName, out var val) && !string.IsNullOrWhiteSpace(val))
            return val;

        var key = Guid.NewGuid().ToString("N");

        var secure = ctx.Request.IsHttps;
        ctx.Response.Cookies.Append(CookieName, key, new CookieOptions
        {
            Expires   = DateTimeOffset.UtcNow.AddYears(2),
            IsEssential = true,
            HttpOnly  = true,
            SameSite  = SameSiteMode.Lax,
            Secure    = secure
        });

        return key;
    }
}
