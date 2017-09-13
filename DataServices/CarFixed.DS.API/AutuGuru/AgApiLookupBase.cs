using System;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CarFixed.DS.API.AutuGuru
{
    public class AgApiLookupBase
    {
        protected HttpResponseMessage CallAPI(string baseUrl, string requestUrl, string apiKey, string privateKey)
        {
            HttpResponseMessage response = null;

            string signature = GenerateSignature(apiKey, privateKey);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromHours(1);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("AUTOGURU_API_KEY", apiKey);
            client.DefaultRequestHeaders.Add("AUTOGURU_API_SIGNATURE", signature);

            response = client.GetAsync(requestUrl).Result;

            return response;
        }

        private string GenerateSignature(string apiKey, string privateKey)
        {
            int timeStamp = GetCurrentTimeStamp();

            return GenerateSignature(apiKey, privateKey, timeStamp);
        }

        private string GenerateSignature(string apiKey, string privateKey, int timeStamp)
        {
            string concat = apiKey + privateKey + timeStamp.ToString();

            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(concat));

            string hashString = string.Empty;
            foreach (byte x in hashBytes)
            {
                hashString += String.Format("{0:x2}", x);
            }

            string signature = hashString + timeStamp.ToString("x2");

            return signature;
        }


        private int GetCurrentTimeStamp()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));

            return (int)t.TotalSeconds;
        }
    }
}
