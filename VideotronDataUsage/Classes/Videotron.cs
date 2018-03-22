using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VideotronDataUsage.Classes
{
    public class Videotron
    {
        public string pourcentageTotal { get; set; }

        public string pourcentageUpload { get; set; }

        public string octetsTotal { get; set; }

        public string octetsUpload { get; set; }

        public string octetsDownload { get; set; }

        public string octetsMax { get; set; }

        public string dateReset { get; set; }

        public string dateDebut { get; set; }

        public string joursRestants { get; set; }

        public string jours { get; set; }

        /// <summary>
        /// Effectue la requête à l'API de consomation de Videotron
        /// </summary>
        /// <param name="userkey">https://www.videotron.com/client/user-management/residentiel/secur/InitProfile.do?dispatch=initProfile&appId=EC#</param>
        public void getDataUsage(string userkey)
        {
            if (userkey != null && userkey != "")
            {
                string response;
                HttpWebResponse ret = HttpRequest.GetRequest("https://www.videotron.com/api/1.0/internet/usage/wired/" + userkey + ".json?lang=fr&caller=google.com");
                response = new StreamReader(ret.GetResponseStream()).ReadToEnd();
                dynamic toParse = JObject.Parse(response);

                if (toParse.messages[0].severity == "error")
                    throw new Exception("Une erreur est survenue");
                jours = toParse.daysFromStart;
                dateReset = toParse.periodEndDate;
                joursRestants = toParse.daysToEnd;
                dateDebut = toParse.periodStartDate;
                pourcentageTotal = toParse.internetAccounts[0].combinedPercent;
                octetsMax = toParse.internetAccounts[0].maxCombinedBytes;
                octetsDownload = toParse.internetAccounts[0].downloadedBytes;
                octetsUpload = toParse.internetAccounts[0].uploadedBytes;
                octetsTotal = (Int64.Parse(octetsDownload) + Int64.Parse(octetsUpload)).ToString();
            }
        }
    }
}
