using System.Net;

namespace Models.ErrorHandling
{
    public class ApiErrorException : Exception
    {
        public HttpStatusCode statusCode; 
        public ApiErrorException() {
            this.statusCode = HttpStatusCode.InternalServerError;
        }
        public ApiErrorException(string message) : base(message) { 
            this.statusCode= HttpStatusCode.InternalServerError;
        }
        public ApiErrorException(string message, HttpStatusCode status) : base(message) { 
            this.statusCode = status;
        }
    }
}
