using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace FanpageConversationsNotify
{
    class Conversations
    {
        public Conversations()
        {

        }


        private static readonly HttpClient client = new HttpClient();

        const string GRAPH_URL = "https://graph.facebook.com/";
        const string GRAPH_VERSION = "v3.0/";

        const string BASE_URL = GRAPH_URL + GRAPH_VERSION;
        const string CONVERSATIONS_PATH = "/conversations";

        public DateTime lastConversationDateTime = DateTime.Now;

        List<Errors> errorList = new List<Errors>();

        /// <summary>
        /// Get JSON with last conversation(s)
        /// </summary>
        /// <param name="fanpage">Fanpage object</param>
        /// <param name="limitation">Is set limit = 1, to returned JSON</param>
        /// <returns>JSON as string</returns>
        private async Task<string> GetLastConversationJsonAsync(Fanpage fanpage, bool limitation = true)
        {
            string limit = limitation ? "&limit=1" : "";

            var response = await client.GetAsync($"{BASE_URL}{fanpage.Id}{CONVERSATIONS_PATH}?access_token={fanpage.Token}{limit}");

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Check is newer message. If is, return true and save new DateTime into "lastConversationDateTime"
        /// </summary>
        /// <param name="fanpage">Fanpage object</param>
        /// <returns>boolean</returns>
        public async Task<bool> IsNewMessage(Fanpage fanpage)
        {
            string json = await GetLastConversationJsonAsync(fanpage);

            JObject jObject = JObject.Parse(json);

            {
                var jObjectError = jObject["error"];
                
                if (jObjectError != null)
                {
                    string errorType = jObjectError["type"]?.ToString();
                    string errorMessage = jObjectError["message"]?.ToString();
                    MessageBox.Show(errorMessage);

                    Errors error = new Errors(errorType, errorMessage, errorList.Count);
                    errorList.Add(error);
                    await Errors.WriteErrorAsync(error);
                    return false;
                }
            }

            string lastConversationTimeStampString = jObject["data"][0]["updated_time"]?.ToString();
            DateTime? dateTime = await StringToDatetime(lastConversationTimeStampString);

            if(dateTime == null)
                return false;

            if (lastConversationDateTime >= dateTime)
                return false;
            
            lastConversationDateTime = (DateTime) dateTime;

            return true;
        }

        /// <summary>
        /// Convert String to DateTime type, with inserting error into error's array
        /// </summary>
        /// <param name="s">String to convert</param>
        /// <param name="result">Out DateTime type</param>
        /// <returns></returns>
        private async Task<DateTime?> StringToDatetime(string s)
        {
            DateTime? dateTime = null;

            try
            {
                dateTime = DateTime.Parse(s);
            }
            catch(Exception ex)
            {
                Errors error = new Errors("Parse error", $"Problem with string to DateTime format conversion ({ex.Message}) [s = {s}]", errorList.Count);
                errorList.Add(error);
                await Errors.WriteErrorAsync(error);
            }

            return dateTime;
        }

        
    }
}
