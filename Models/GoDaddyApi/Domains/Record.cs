using Models.GoDaddyApi.Enums;

namespace Models.GoDaddyApi.Domains
{
    public class Record
    {
        public string Data { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Port { get; set; } = 65535;
        public int Priority { get; set; } = 0;
        public string Protocol { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public int TTL { get; set; } = 600;
        public DnsType Type { get; set; } = DnsType.A;
        public int Weight { get; set; } = 0;
    }
}
