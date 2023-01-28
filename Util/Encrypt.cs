using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace AKI.Util.Encrypt
{
    public class Aes
    {
        private static string g_strPassword = null;
        private static byte[] g_aPassword = null;

        public static string Password { set { g_strPassword = value; g_aPassword = GetPasswordBytes(); } }

        public static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes = null)
        {
            byte[] encryptedBytes = null;
            if (null == passwordBytes)
                passwordBytes = g_aPassword;
            byte[] saltBytes = passwordBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes = null)
        {
            byte[] decryptedBytes = null;
            if (null == passwordBytes)
                passwordBytes = g_aPassword;
            byte[] saltBytes = passwordBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public static string Encrypt(string text, byte[] passwordBytes = null)
        {
            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            if (null == passwordBytes)
                passwordBytes = g_aPassword;
            byte[] encryptedBytes = null;

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            int saltSize = GetSaltSize(passwordBytes);
            byte[] saltBytes = GetRandomBytes(saltSize);
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];

            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < originalBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            }

            encryptedBytes = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string decryptedText, byte[] passwordBytes = null)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptedText);
            if (null == passwordBytes)
                passwordBytes = g_aPassword;
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] decryptedBytes = Decrypt(bytesToBeDecrypted, passwordBytes);
            int saltSize = GetSaltSize(passwordBytes);
            byte[] originalBytes = new byte[decryptedBytes.Length - saltSize];

            for (int i = saltSize; i < decryptedBytes.Length; i++)
            {
                originalBytes[i - saltSize] = decryptedBytes[i];
            }

            return Encoding.UTF8.GetString(originalBytes);
        }

        public static int GetSaltSize(byte[] passwordBytes = null)
        {
            if (null == passwordBytes)
                passwordBytes = g_aPassword;
            var key = new Rfc2898DeriveBytes(passwordBytes, passwordBytes, 1000);
            byte[] ba = key.GetBytes(2);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ba.Length; i++)
            {
                sb.Append(Convert.ToInt32(ba[i]).ToString());
            }
            int saltSize = 0;
            string s = sb.ToString();
            foreach (char c in s)
            {
                int intc = Convert.ToInt32(c.ToString());
                saltSize = saltSize + intc;
            }

            return saltSize;
        }

        public static byte[] GetRandomBytes(int length)
        {
            byte[] ba = new byte[length];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

        private static byte[] GetPasswordBytes()
        {
            byte[] ba = null;

            if (g_strPassword.Length == 0)
                ba = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            else
            {
                SecureString sstrPassword = new SecureString();
                foreach(char c in g_strPassword.ToArray())
                    sstrPassword.AppendChar(c);

                IntPtr unmanagedBytes = Marshal.SecureStringToGlobalAllocAnsi(sstrPassword);
                try
                {
                    unsafe
                    {
                        byte* byteArray = (byte*)unmanagedBytes.ToPointer();

                        byte* pEnd = byteArray;
                        while (*pEnd++ != 0) { }
                        int length = (int)((pEnd - byteArray) - 1);

                        ba = new byte[length];

                        for (int i = 0; i < length; ++i)
                        {
                            byte dataAtIndex = *(byteArray + i);
                            ba[i] = dataAtIndex;
                        }
                    }
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocAnsi(unmanagedBytes);
                }
            }

            return System.Security.Cryptography.SHA256.Create().ComputeHash(ba);
        }

    }
}
