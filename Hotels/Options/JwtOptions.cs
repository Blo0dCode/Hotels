namespace Hotels.Options;

public class JwtOptions
{
    [Required] public string Key { get; set; } = null!;
    [Required] public string Issuer { get; set; } = null!;
    [Required] public string Audience { get; set; } = null!;
    [Required] public TimeSpan ExpiryDuration { get; set; }
}