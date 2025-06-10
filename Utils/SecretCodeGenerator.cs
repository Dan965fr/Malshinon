using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Utils
{
    internal static class SecretCodeGenerator
    {
        public static string GenerateSecretCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }


    }
}
