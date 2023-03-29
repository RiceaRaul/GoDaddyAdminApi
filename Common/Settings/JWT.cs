namespace Common.Settings
{
    public class JWT
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string Secret { get; set; } = default!;
        public int ExpireHours { get; set; } = default!;
    }
}
