using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaApp
{
    class OTPCryptoAlgorithm : ICryptoAlgorithm
    {
        public string decrypt(string message)
        {
            char[] output = message.Substring(0, message.Length/2).ToCharArray();
            char[] key = message.Substring(message.Length / 2).ToCharArray();

            for (int i = 0; i < output.Length; i++)
                output[i] = (char)(output[i] ^ key[i]);

            return new string(output);
        }

        public string encrypt(string message)
        {
            char[] output = message.ToCharArray();
            char[] key = randomString(message.Length).ToCharArray();
            for (int i = 0; i < output.Length; i++)
                output[i] = (char)(output[i] ^ key[i]);

            return (new string(output)) + (new string(key));
        }
        private string randomString(int len)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[len];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
    }
}
