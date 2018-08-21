using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GameGuy.GGCrypt {
    public class Encrypt {
        private DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        private ICryptoTransform desencrypt;

        public Encrypt(string key) {
            DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(key);
            desencrypt = DES.CreateEncryptor();
        }

        public Encrypt() {
            string key = GenerateKey();

            DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(key);

            desencrypt = DES.CreateEncryptor();
        }

        public string GenerateKey() {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }

        public void encryptFile(string fileName, string key) {
            DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(key);

            encryptFile(fileName);
        }

        public void encryptFile(string fileName) {
            FileStream fsInput = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            fileName = fileName.Substring(0, fileName.IndexOf('.'));
            FileStream fileCrypt = new FileStream(fileName + ".gg", FileMode.Create, FileAccess.Write);

            CryptoStream cryptostream = new CryptoStream(fileCrypt, desencrypt, CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[fsInput.Length - 1];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);

            fsInput.Close();
            cryptostream.Close();
        }
    }
}