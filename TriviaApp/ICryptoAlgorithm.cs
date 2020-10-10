using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaApp
{
    public interface ICryptoAlgorithm
    {
        string encrypt(string message);
        string decrypt(string message);
    }
}
