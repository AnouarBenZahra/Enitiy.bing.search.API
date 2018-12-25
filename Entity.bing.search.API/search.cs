using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Entity.bing.search.API
{
    public class Search
    {

        public async Task<string> QueryAPI(string textQuery)
        {
            string result = string.Empty;
            string host = "https://api.cognitive.microsoft.com";
            string basePath = "/bing/v7.0/entities";
            string mkt = "en-US";            
            string subKey = "ENTER KEY HERE";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subKey);

            string uri = host + basePath + "?mkt=" + mkt + "&q=" + System.Net.WebUtility.UrlEncode(textQuery);

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            string stringAsyncResponse = await response.Content.ReadAsStringAsync();
            result = JsonPrintResult(stringAsyncResponse);
            return result;
        }

        static string JsonPrintResult(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {

                json = json.Replace(Environment.NewLine, "").Replace("\t", "");

                StringBuilder stringBuilder = new StringBuilder();
                bool quote = false;
                bool ignore = false;
                int offset = 0;
                int indentLength = 3;

                foreach (char item in json)
                {
                    switch (item)
                    {
                        case '"':
                            if (!ignore) quote = !quote;
                            break;
                        case '\'':
                            if (quote) ignore = !ignore;
                            break;
                    }

                    if (quote)
                        stringBuilder.Append(item);
                    else
                    {
                        switch (item)
                        {
                            case '{':
                            case '[':
                                stringBuilder.Append(item);
                                stringBuilder.Append(Environment.NewLine);
                                stringBuilder.Append(new string(' ', ++offset * indentLength));
                                break;
                            case '}':
                            case ']':
                                stringBuilder.Append(Environment.NewLine);
                                stringBuilder.Append(new string(' ', --offset * indentLength));
                                stringBuilder.Append(item);
                                break;
                            case ',':
                                stringBuilder.Append(item);
                                stringBuilder.Append(Environment.NewLine);
                                stringBuilder.Append(new string(' ', offset * indentLength));
                                break;
                            case ':':
                                stringBuilder.Append(item);
                                stringBuilder.Append(' ');
                                break;
                            default:
                                if (item != ' ') stringBuilder.Append(item);
                                break;
                        }
                    }
                }

                return stringBuilder.ToString().Trim();
            }
            else
                return string.Empty;
        }

    }
}
