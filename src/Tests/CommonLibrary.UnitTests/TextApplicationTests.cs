using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class TextApplicationTests
    {
        [Test]
        public void CanPrependText()
        {
            string[] args = new string[] { "-checklicense:true", @"-licensefile:F:\dev\Workshops\KnowledgeDrink\doc\license.txt",
                                           "-recurse:true", "-pattern:**/*.cs", @"-rootdir:F:\dev\Workshops\KnowledgeDrink\src\Lib\GenericCode",
                                           "-filetype:file", @"-outfile:F:\Dev\Workshops\KnowledgeDrink\doc\filesToLicense.txt"};

            //LicenseApplier applier = new LicenseApplier();
            //applier.Execute(args);
        }
    }
}
