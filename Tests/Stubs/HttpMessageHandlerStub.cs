using System.Net;
using System.Text;


namespace Tests.Stubs
{
    public class HttpMessageHandlerStub : HttpMessageHandler
    {
        private readonly Exception _exceptionToThrow;
        private readonly HttpStatusCode _statusCode;
        private readonly string _responseContent;

        public HttpMessageHandlerStub(Exception exceptionToThrow, HttpStatusCode statusCode, string responseContent)
        {
            _exceptionToThrow = exceptionToThrow;
            _statusCode = statusCode;
            _responseContent = responseContent;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_exceptionToThrow != null)
                return Task.FromException<HttpResponseMessage>(_exceptionToThrow);

            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_responseContent ?? string.Empty, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }
}
