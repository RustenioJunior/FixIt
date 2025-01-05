using System;

    namespace UserManagement.Models
    {
        public class TokenValidationRequest
        {
            public string Token { get; set; } = string.Empty;
            public Guid CorrelationId { get; set; }
        }
    }