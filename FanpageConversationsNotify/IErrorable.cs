using System.Collections.Generic;

namespace FanpageConversationsNotify
{
    internal interface IErrorable
    {
        IEnumerable<Errors> ErrorsArray { get; set; }
    }
}