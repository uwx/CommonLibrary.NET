using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using ComLib;
using ComLib.Cryptography;
using NUnit.Framework;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class Cryptography
    {        
        [Test]
        public void CanEncryptSymmetricTripleDes()
        {
            ICrypto crypto = new CryptoSym("commonlib.net", new TripleDESCryptoServiceProvider());
            string encrypted = crypto.Encrypt("horizonguy");
            Assert.AreNotEqual("horizonguy", encrypted);
        }


        [Test]
        public void IsEncryptSymmetricTripleDesDifferentByInput()
        {
            ICrypto crypto = new CryptoSym("commonlib.net", new TripleDESCryptoServiceProvider());
            string encrypted = crypto.Encrypt("horizonguy");
            string encrypted2 = crypto.Encrypt("bourneIdentity");
            Assert.AreNotEqual("horizonguy", encrypted);
            Assert.AreNotEqual(encrypted, encrypted2);
        }


        [Test]
        public void CanDecryptSymmetricTripleDes()
        {
            string encrypted = "OTrZQMzbEM2QTfH7vJyaDg==";
            ICrypto crypto = new CryptoSym("commonlib.net", new TripleDESCryptoServiceProvider());
            string decrypted = crypto.Decrypt(encrypted);
            Assert.AreEqual("horizonguy", decrypted);
        }


        [Test]
        public void CanEncryptHash()
        {
            string plainText = "horizonguy";
            ICrypto crypto = new CryptoHash("commonlib.net", new MD5CryptoServiceProvider());
            string encrypted = crypto.Encrypt(plainText);
            Assert.AreNotEqual("horizonguy", encrypted);
        }


        [Test]
        public void CanDecryptHash()
        {
            string encrypted = "44bf9ba3479261e365c6389bc03bf497";
            ICrypto crypto = new CryptoHash("commonlib.net", new MD5CryptoServiceProvider());
            bool ismatch = crypto.IsMatch(encrypted, "horizonguy");
            Assert.AreEqual(ismatch, true);
        }


        [Test]
        public void CanEncryptWithSpecialCharsTripleDes()
        {
            string plainText = "~`!@#$%^&*()_+{}|:\"<>?[]\\,./;'-=";
            ICrypto crypto = new CryptoSym("commonlib.net", new TripleDESCryptoServiceProvider());
            string encrypted = crypto.Encrypt(plainText);

            Assert.AreNotEqual(plainText, encrypted);

            // Now decrypt.
            string decrypted = crypto.Decrypt(encrypted);
            Assert.AreEqual("~`!@#$%^&*()_+{}|:\"<>?[]\\,./;'-=", decrypted);
        }

        [Test]
        public void TestTripleDESEncryptionDecryption()
        {
            // Test single-length key.
            Assert.AreEqual("D5D44FF720683D0D", ComLib.Cryptography.DES.TripleDES.Encrypt(new ComLib.Cryptography.DES.DESKey("0123456789ABCDEF"), "0000000000000000"));
            Assert.AreEqual("0000000000000000", ComLib.Cryptography.DES.TripleDES.Decrypt(new ComLib.Cryptography.DES.DESKey("0123456789ABCDEF"), "D5D44FF720683D0D"));

            // Test double-length key.
            Assert.AreEqual("9335C20E81AA38EA", ComLib.Cryptography.DES.TripleDES.Encrypt(new ComLib.Cryptography.DES.DESKey("C1EFB589A26DA4E62A8032A10B626E79"), "0000000000000000"));
            Assert.AreEqual("0000000000000000", ComLib.Cryptography.DES.TripleDES.Decrypt(new ComLib.Cryptography.DES.DESKey("C1EFB589A26DA4E62A8032A10B626E79"), "9335C20E81AA38EA"));

            // Test triple-length key.
            Assert.AreEqual("5BD812B06E29CE8C", ComLib.Cryptography.DES.TripleDES.Encrypt(new ComLib.Cryptography.DES.DESKey("BA80F4DFCBDCFE3EFB9119E96DE0E52ABC1FCE7CD9376E10"), "0000000000000000"));
            Assert.AreEqual("0000000000000000", ComLib.Cryptography.DES.TripleDES.Decrypt(new ComLib.Cryptography.DES.DESKey("BA80F4DFCBDCFE3EFB9119E96DE0E52ABC1FCE7CD9376E10"), "5BD812B06E29CE8C"));
        }
    }
}
