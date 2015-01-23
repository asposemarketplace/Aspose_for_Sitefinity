using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Aspose.GoogleSync.Components
{
    public class Crypto
    {
        private static byte[] salt = Encoding.ASCII.GetBytes("asf345234$32sff");

        public static string Encrypt(string plaintextString)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(plaintextString));
        }

        public static string Decrypt(string encryptedTextBytes)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String(encryptedTextBytes));
        }
    }
}