namespace Models.GoDaddyApi.Domains
{
    public class Domain
    {
        public DateTime createdAt { get; set; }
        public string domain { get; set; }
        public int domainId { get; set; }
        public bool expirationProtected { get; set; }
        public DateTime expires { get; set; }
        public bool exposeWhois { get; set; }
        public bool holdRegistrar { get; set; }
        public bool locked { get; set; }
        public object nameServers { get; set; }
        public bool privacy { get; set; }
        public DateTime registrarCreatedAt { get; set; }
        public bool renewAuto { get; set; }
        public DateTime renewDeadline { get; set; }
        public bool renewable { get; set; }
        public string status { get; set; }
        public DateTime transferAwayEligibleAt { get; set; }
        public bool transferProtected { get; set; }
    }
}
