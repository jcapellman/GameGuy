using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace GGCrypt {
    public class Decrypt {
        static public string decryptFile(string sInputFilename, string sKey) {
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
