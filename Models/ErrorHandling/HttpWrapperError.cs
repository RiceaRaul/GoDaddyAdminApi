namespace Models.ErrorHandling
{
    public class HttpWrapperError
    {
        public int HttpStatusCode { get; set; }
        public string HttpErrorMessage { get; set; } = String.Empty;
    }
}
