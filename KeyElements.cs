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

        public byte[] IVkeygenerator(string text)
        {
            byte[] iv = new byte[32];
            return iv;
        }

    }

}
