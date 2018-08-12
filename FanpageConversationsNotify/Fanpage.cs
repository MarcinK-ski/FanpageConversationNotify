using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanpageConversationsNotify
{
    class Fanpage
    {
        public Fanpage(string id, string token)
        {
            Id = id;
            _token = new AccessToken();
            Token = token;
        }


        public string Id { get; set; }

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = GetFanpageNameById();
            }
        }

        private string GetFanpageNameById()
        {
            throw new NotImplementedException();
        }

        AccessToken _token;

        public string Token
        {
            get
            {
                return _token.Token;
            }

            set
            {
                _token.Token = value;
            }
        }

        public DateTime ValiidTo
        {
            get
            {
                return _token.ValidTo;
            }

            private set
            {
                _token.ValidTo = value;
            }
        }

    }
}
