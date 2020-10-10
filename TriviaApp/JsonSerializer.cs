using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TriviaApp
{

    public static class JsonSerializer
    {
        public static ICryptoAlgorithm cryptoAlgorithm { get; set; }
        public static Byte[] serializeRequest<T>(T request,int requestCode)
        {
            Byte[] code = BitConverter.GetBytes(requestCode);
            string json = JsonConvert.SerializeObject(request);
            Byte[] content = Encoding.ASCII.GetBytes(cryptoAlgorithm.encrypt(json));
            Byte[] size = BitConverter.GetBytes(content.Length);
            Byte[] msg = new Byte[code.Length + size.Length + content.Length];
            code.CopyTo(msg, 0);
            size.CopyTo(msg, code.Length);
            content.CopyTo(msg, code.Length + size.Length);
            return msg;
        }

    }
   
}
