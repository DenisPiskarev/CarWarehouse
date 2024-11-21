using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWarehouse.BLL.ViewModel
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            this.Token = String.Empty;
            this.responseMsg =
            new HttpResponseMessage()
            {
                StatusCode =
               System.Net.HttpStatusCode.Unauthorized
            };
        }

        public string Token { get; set; }
        public HttpResponseMessage responseMsg
        {
            get; set;
        }

    }
}
