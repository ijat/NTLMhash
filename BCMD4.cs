using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Digests;

namespace PassHash
{
    class BCMD4
    {
        public BCMD4()
        {


        }


        /// <summary>
        /// Convert Password to NT Hash.  Convert to unicode and MD4
        /// </summary>
        /// <param name="passwordIn">password In</param>
        /// <returns>NT Hash as string</returns>
        public static string NTHash(string passwordIn)
        {
            MD4Digest md = new MD4Digest();
            byte[] unicodePassword = Encoding.Convert(Encoding.ASCII, Encoding.Unicode, Encoding.ASCII.GetBytes(passwordIn));


            md.BlockUpdate(unicodePassword, 0, unicodePassword.Length);
            byte[] hash = new byte[16];
            md.DoFinal(hash, 0);


            return ByteEncoding.hexFromBytes(hash); ;
        }

        /// <summary>
        /// Convert Password to NT Hash.  Convert to unicode and MD4
        /// </summary>
        /// <param name="passwordIn">password In</param>
        /// <returns>NT Hash as byte[]</returns>
        public static byte[] NTHashAsBytes(string passwordIn)
        {
            MD4Digest md = new MD4Digest();
            byte[] unicodePassword = Encoding.Convert(Encoding.ASCII, Encoding.Unicode, Encoding.ASCII.GetBytes(passwordIn));


            md.BlockUpdate(unicodePassword, 0, unicodePassword.Length);
            byte[] hash = new byte[16];
            md.DoFinal(hash, 0);


            return hash;
        }


    }
}