using System;

namespace Authentication.Models
{
    public class TokenValidationResponse
    {
        public bool IsValid { get; set; }
         public Guid CorrelationId { get; set; }
    }
}