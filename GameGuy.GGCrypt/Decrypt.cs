using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GameGuy.GGCrypt {
    public class Decrypt {
        public static string decryptFile(string sInputFilename, string sKey) {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);


            FileStream fsread = new FileStream(sInputFilename,
                                           FileMode.Open,
                                           FileAccess.Read);

            ICryptoTransform desdecrypt = DES.CreateDecryptor();

            CryptoStream cryptostreamDecr = new CryptoStream(fsread,
                                                         desdecrypt,
                                                         CryptoStreamMode.Read);
            string tmpStr = new StreamReader(cryptostreamDecr).ReadToEnd();

            fsread.Close();
            cryptostreamDecr.Close();

            return tmpStr;
        }
    }
}
