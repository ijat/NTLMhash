using System;
using System.Security.Cryptography;
using System.Text;

namespace PassHash
{
class ByteEncoding
    {

        public ByteEncoding()
        {

        }

        //bytes to b64 string
        public static string base64FromBytes(byte[] bytesIn)
        {
            return Convert.ToBase64String(bytesIn);
        }

        //bytes to hex string
        public static string hexFromBytes(byte[] bytesIn)
        {
            StringBuilder sOut = new StringBuilder();

            foreach (byte x in bytesIn)
            {
                sOut.Append(String.Format("{0:X2}", x));
                //sOut.Append(" ");
            }


            return sOut.ToString().TrimEnd(' ');
        }

        /// <summary>
        /// Convert hex string to bytes
        /// </summary>
        /// <param name="hexString">hex string In</param>
        /// <returns>bytes out</returns>
        public static byte[] hextoBytes(string hexString)
        {



            //check for null
            if (hexString == null) return null;
            //get length
            int len = hexString.Length;
            if (len % 2 == 1) return null;
            int len_half = len / 2;
            //create a byte array
            byte[] bs = new byte[len_half];
            try
            {
                //convert the hexstring to bytes
                for (int i = 0; i != len_half; i++)
                {
                    bs[i] = (byte)Int32.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch
            {
                //MessageBox.Show("Exception : " + ex.Message);
                throw new Exception("Your Hex may not all be hex");
            }
            //return the byte array
            return bs;
        }



        //used to turn bytes to a string of bits.  Used for easy visualization. 
        public static string binFromBytes(byte[] bytesIn)
        {

            StringBuilder newString = new StringBuilder();

            foreach (byte x in bytesIn)
            {
                string sOut = Convert.ToString(x, 2);
                while (sOut.Length < 8)
                {
                    sOut = "0" + sOut;
                }

                newString.Append(sOut);
                newString.Append(" ");

            }


            return newString.ToString().TrimEnd(' ');
        }


        /// <summary>
        /// Take in a hex string and retrun an MD5 hash as hex
        /// </summary>
        /// <param name="data">hex string data in</param>
        /// <returns>MD5 out as hex string</returns>
        public static string MD5StrToStr(string data)
        {

            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            byte[] byData = hextoBytes(data);
            md.ComputeHash(byData);

            return hexFromBytes(md.Hash);
            
        }

        
        /// <summary>
        /// addjust parity as needed.  The key byte is only 7 bits, the 8th is an odd parity bit.
        /// </summary>
        /// <param name="bytes"></param>
        public static void oddParity(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                bool needsParity = (((b >> 7) ^ (b >> 6) ^ (b >> 5) ^ (b >> 4) ^ (b >> 3) ^ (b >> 2) ^ (b >> 1)) & 0x01) == 0;
                if (needsParity)
                {
                    bytes[i] |= (byte)0x01;
                }
                else
                {
                    bytes[i] &= (byte)0xfe;
                }
            }
        }
    }
}