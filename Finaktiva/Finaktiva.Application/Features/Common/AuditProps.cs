using System.Text.Json.Serialization;

namespace Finaktiva.Application.Features.Common
{
    public class AuditProps
    {
        [JsonIgnore]
        public string? CurrentUserName { get; set; }
        [JsonIgnore]
        public Guid? CurrentUserId { get; set; }
        [JsonIgnore]
        public string? CurrentUserEmail { get; set; }
    }
}
