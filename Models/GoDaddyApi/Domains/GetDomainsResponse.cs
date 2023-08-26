namespace Models.GoDaddyApi.Domains
{
    public class GetDomainsResponse
    {
        public IEnumerable<Domain> Domains { get; set; } = new List<Domain>();
    }
}
