using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace BackEndApplication.Utils
{
    public class JsonContent : StringContent
    {
        private const string MEDIA_TYPE = "application/json";

        public JsonContent(object data)
            : base(GetStringValue(data), Encoding.Unicode, MEDIA_TYPE)
        {
        }

        private static string GetStringValue(object data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
    }
}
