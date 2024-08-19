using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Encrypter
{
    public class KeyElements
    {
        public string text { get; set; }
        public KeyElements(string text)
        {

            this.text = text;
        }
        public int[] f64Getter (string text)
        {
            string first64 = text.Substring(0, 64);
            int[] f64 = new int[64];
            for (int i = 0; i < 64; i++)
            {
                f64[i] = int.Parse(first64[i].ToString());
            }
            return f64;
        }

        public int[] l128Getter (string text)
        {
            string last128 = text.Substring(text.Length - 128, 128);
            int[] l128 = new int[128];
            for (int i = 0; i < 128; i++)
            {
                l128[i] = int.Parse(last128[i].ToString());
            }
            return l128;
        }

        public byte[] IVGenerator(string text)
        {
            string first64 = text.Substring(0, 64);
            int[] f64 = new int[64];
            for (int i = 0; i < 64; i++)
            {
                f64[i] = int.Parse(first64[i].ToString());
            }
            
            char[] ch32 = new char[32];
            for (int i = 0;i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    if (f64[i]%2 == 0)
                    {
                        ch32[j] = (char)f64[63-j];
                    }
                    else
                    {
                     switch(f64[63-j]%6)
                        {
                            case '0':
                                ch32[j] = 'f'; break;
                            case '1':
                                ch32[j] = 'e'; break;
                            case '2':
                                ch32[j] = 'd'; break;
                            case '3':
                                ch32[j] = 'c'; break;
                            case '4':
                                ch32[j] = 'b'; break;
                            case '5':
                                ch32[j] = 'a'; break;
                        }   
                    }
                }
            }
            byte[] iv = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                iv[i] = (byte)ch32[i * 2];
            }
            return iv;
        }

        public byte[] KeyGenerator(string text)
        {
            int[] l128 = l128Getter(text);
            char[] ch64 = new char[64];
            for(int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    if (l128[i] % 2 == 0)
                    {
                        ch64[j] = (char)l128[127 - j];
                    }
                    else
                    {
                        switch (l128[127 - j] % 6)
                        {
                            case '0':
                                ch64[j] = 'f'; break;
                            case '1':
                                ch64[j] = 'e'; break;
                            case '2':
                                ch64[j] = 'd'; break;
                            case '3':
                                ch64[j] = 'c'; break;
                            case '4':
                                ch64[j] = 'b'; break;
                            case '5':
                                ch64[j] = 'a'; break;
                        }
                    }
                }
            }
            byte[] key = new byte[32];

            for (int i = 0; i < 32; i++)
            {
                key[i] = (byte)ch64[i * 2];
            }

            return key;
        }

    }

}
