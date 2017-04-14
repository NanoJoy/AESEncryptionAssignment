using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace cs625asnmt1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder randomStringBuilder = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                randomStringBuilder.Append(chars[random.Next(chars.Length)]);
            }
            string randomString = randomStringBuilder.ToString();
            Console.WriteLine("Original String: " + randomString);

            using (Aes myAes = Aes.Create())
            {
                Console.Write("Key: ");
                for (int i = 0; i < myAes.Key.Length; i++)
                {
                    Console.Write(myAes.Key[i]);
                }
                Console.WriteLine();
                
                ICryptoTransform encryptor = myAes.CreateEncryptor();
                using (FileStream file = new FileStream("encrypted.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(file, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            Console.WriteLine("Encrypting message...");
                            swEncrypt.Write(randomString);
                        }
                    }
                }
                
                ICryptoTransform decryptor = myAes.CreateDecryptor();
                using (FileStream file = new FileStream("encrypted.txt", FileMode.Open, FileAccess.Read))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(file, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            Console.WriteLine("Decrypting message...");
                            string plaintext = srDecrypt.ReadToEnd();
                            Console.WriteLine("Decrypted Message: " + plaintext);
                        }

                    }
                }
            }
            Console.ReadLine();
        }
    }
}
