using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ01.Core.ViewModels.Responses.Accounts
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
