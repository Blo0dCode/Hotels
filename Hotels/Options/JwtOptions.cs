namespace Hotels.Options;

public class JwtOptions
{
    public string Key { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public TimeSpan ExpiryDuration { get; init; }
}