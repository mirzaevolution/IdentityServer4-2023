using System.Security.Claims;

namespace Basics.Intr.Client.Models
{
    public class ProtectedModel
    {
        public string UserName { get; set; }
        public List<Claim> Claims { get; set; } = new List<Claim>();
    }
}
