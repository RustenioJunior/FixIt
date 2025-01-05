using System;

namespace Authentication.Models
{
    public class TokenValidationRequest
    {
        public string Token { get; set; }
        public Guid CorrelationId { get; set; }
    }
}