using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class authResponse
    {

        String token_type;

    String access_token;

    String scope;

    Int32 expires_in;

        public string Token_type { get => token_type; set => token_type = value; }
        public string Access_token { get => access_token; set => access_token = value; }
        public string Scope { get => scope; set => scope = value; }
        public Int32 Expires_in { get => expires_in; set => expires_in = value; }
    }
}
