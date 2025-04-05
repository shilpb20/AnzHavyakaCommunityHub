using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Core.Helpers
{
    public class JsonHelper
    {
        public static bool IsEmptyJson<T>(T data)
        {
            if (data == null)
                return false;

            try
            {
                var json = JsonConvert.SerializeObject(data);
                var parsed = JObject.Parse(json);
                return !parsed.Properties().Any();
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }
    }
}
