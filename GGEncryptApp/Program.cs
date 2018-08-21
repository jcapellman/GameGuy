using System.IO;
using GameGuy.GGCrypt;

namespace GGEncrypt2 {
    class Program {
        static void Main(string[] args) {
            Encrypt enc = new Encrypt("45^^!209");

            DirectoryInfo dInfo = new DirectoryInfo(".");
           
            FileInfo[] files = dInfo.GetFiles();
            
    
            foreach (FileInfo file in files) {
                if (file.Extension == ".ggs") {
                    enc.encryptFile(file.Name);
                }
            }
        }
    }
}