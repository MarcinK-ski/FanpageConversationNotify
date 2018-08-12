using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanpageConversationsNotify
{
    class Errors
    {
        public int ErrorNo { get; set; }
        public string ErrorName { get; set; }
        public string ErrorInfo { get; set; }

        public Errors(string name, string description, int no = -1)
        {
            ErrorNo = no;
            ErrorName = name;
            ErrorInfo = description;
        }

        public static void WriteError(Errors error)
        {
            using (StreamWriter writer = new StreamWriter("errorinfo.log", true))
            {
                writer.WriteLine($"{error.ErrorNo} -> {error.ErrorNo}: {error.ErrorInfo}");
            }
        }

        public static async Task WriteErrorAsync(Errors error)
        {
            using (StreamWriter writer = new StreamWriter("errorinfo.log", true))
            {
                await writer.WriteLineAsync($"{error.ErrorNo} -> {error.ErrorNo}: {error.ErrorInfo}");
            }
        }

    }
}
