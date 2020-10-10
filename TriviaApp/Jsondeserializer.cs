using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TriviaApp
{
    class JsonDeserializer
    {
        public static ICryptoAlgorithm cryptoAlgorithm { get; set; }
        public static T deserializeResponse<T>(Byte[] content)
        {
            return JsonConvert.DeserializeObject<T>(cryptoAlgorithm.decrypt(Encoding.ASCII.GetString(content)));
        }
    }
}
