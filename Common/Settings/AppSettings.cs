namespace Common.Settings
{
    public class AppSettings
    {
        public string CorsOrigin { get; set; } = default!;
        public JWT JWT { get; set; } = default!;
        public Salsa20 Salsa20 { get; set; } = default!;
        public GoDaddySettings GoDaddySettings { get; set; } = default!;
    }
}
