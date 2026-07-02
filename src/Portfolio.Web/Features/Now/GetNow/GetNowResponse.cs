namespace Portfolio.Web.Features.Now.GetNow;

public sealed record NowResponse(string StatusText, string Mood, DateTimeOffset UpdatedAt);
