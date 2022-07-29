using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using VRC.Core;


namespace Heavenly.VRChat.Utilities
{
    public static class APIU
    {

        public static int GetOnlineVRChatPlayersCount()
        {
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.164 Safari/537.36 OPR/77.0.4054.298");
            client.Headers.Add("Cookie", "auth=" + ApiCredentials.authToken);
            string countString = client.DownloadString("https://vrchat.com/api/1/visits");
            return int.Parse(countString);
        }

    }
}
