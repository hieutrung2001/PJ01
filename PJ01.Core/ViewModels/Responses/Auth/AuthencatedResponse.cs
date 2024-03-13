using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ01.Core.ViewModels.Responses.Auth
{
    public class AuthencatedResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
