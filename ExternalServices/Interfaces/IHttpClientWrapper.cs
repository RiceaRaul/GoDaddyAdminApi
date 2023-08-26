namespace ExternalServices.Interfaces
{
    public interface IHttpClientWrapper
    {
        void addHeader(string key, string value);
        void setBaseUrl(string url);
        Task<RModel> PerformAction<REModel, RModel>(string url, REModel payload, HttpMethod method) where RModel : new();
        Task<RModel> PerformAction<RModel>(string url, HttpMethod method) where RModel : new();
    }
}
