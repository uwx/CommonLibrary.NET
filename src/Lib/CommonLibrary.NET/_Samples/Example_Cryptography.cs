using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;

//<doc:using>
using ComLib;
using ComLib.Cryptography;
using ComLib.Cryptography.DES;
//</doc:using>
using ComLib.Application;


namespace ComLib.Samples
{	
    /// <summary>
    /// Example for the Cryptography namespace.
    /// </summary>
    public class Example_Cryptography : App
    {
		/// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            //<doc:example>
			// 1. Encrypt using default provider. ( Symmetric TripleDes )
            string plainText = "www.knowledgedrink.com";
            string encrypted = Crypto.Encrypt(plainText);
            string decrypted = Crypto.Decrypt(encrypted);

            Console.WriteLine("====================================================");
            Console.WriteLine("CRYPTOGRAPHY ");
            Console.WriteLine("Encrypted : " + plainText + " to " + encrypted);
            Console.WriteLine("Decrypted : " + encrypted + " to " + decrypted);
            Console.WriteLine(Environment.NewLine);

            // 2. Use non-static encryption provider.
            ICrypto crypto = new CryptoHash("commonlib.net", new MD5CryptoServiceProvider());
            string hashed = crypto.Encrypt("my baby - 2002 honda accord ex coupe");
            Console.WriteLine(hashed);

            // 3. Change the crypto provider on the static helper.
            ICrypto crypto2 = new CryptoSym("new key", new TripleDESCryptoServiceProvider());
            Crypto.Init(crypto2);
            string encryptedWithNewKey = Crypto.Encrypt("www.knowledgedrink.com");
            Console.WriteLine(string.Format("Encrypted text : using old key - {0}, using new key - {1}", encrypted, encryptedWithNewKey));

            // 4. Generate the check value of a 3DES key by encrypting 16 hexadecimal zeroes.
            DESKey randomKey = new DESKey(DesKeyType.TripleLength);
            string keyCheckValue = ComLib.Cryptography.DES.TripleDES.Encrypt(randomKey, "0000000000000000");
            Console.WriteLine(string.Format("3DES key: {0} with check value {1}", randomKey.ToString(), keyCheckValue));
			
			//</doc:example>
            return BoolMessageItem.True;
        }		
    }
}
