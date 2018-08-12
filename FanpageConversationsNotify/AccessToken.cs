using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanpageConversationsNotify
{
    class AccessToken
    {
        public string Token { get; set; }

        DateTime _validTo;
        public DateTime ValidTo
        {
            get
            {
                return _validTo;
            }

            set
            {
                _validTo = value;
            }
        }

        public AccessToken()
        {

        }

    }
}
