using NUnit.Framework;
using System;
using Silanis.ESL.SDK;
using System.Collections.Generic;
using Silanis.ESL.SDK.Builder;

namespace SDK.Examples
{
    [TestFixture()]
    public class BasicPackageCreationExampleTest
    {
        [Test()]
        public void VerifyResult()
        {
            BasicPackageCreationExample example = new BasicPackageCreationExample( Props.GetInstance() );
            example.Run();

            DocumentPackage documentPackage = example.EslClient.GetPackage(example.PackageId);

            // Verify if the package is created correctly
            Assert.AreEqual("This is a package created using the e-SignLive SDK", documentPackage.Description);
            Assert.AreEqual("This message should be delivered to all signers", documentPackage.EmailMessage);

            // Verify if the sdk version is set correctly
            Assert.IsTrue(documentPackage.Attributes.Contents.ContainsKey( "sdk" ));
            Assert.IsTrue(documentPackage.Attributes.Contents["sdk"].ToString().Contains(".NET"));

            // Signer 1
            Signer signer = documentPackage.Signers[example.email1.ToLower()];
            Assert.AreEqual("Client1", signer.Id);
            Assert.AreEqual("John", signer.FirstName);
            Assert.AreEqual("Smith", signer.LastName);
            Assert.AreEqual("Managing Director", signer.Title);
            Assert.AreEqual("Acme Inc.", signer.Company);

            // Signer 2
            signer = documentPackage.Signers[example.email2.ToLower()];
            Assert.AreEqual("Patty", signer.FirstName);
            Assert.AreEqual("Galant", signer.LastName);

            // Document 1
            Document document = documentPackage.Documents["First Document"];
            List<Field> fields = document.Signatures[0].Fields;
            Field field = fields[0];

            Assert.AreEqual(FieldStyle.UNBOUND_CHECK_BOX, field.Style);
            Assert.AreEqual(0, field.Page);
            Assert.AreEqual(FieldBuilder.CHECKBOX_CHECKED, field.Value);

            // Document 2
            document = documentPackage.Documents["Second Document"];
            fields = document.Signatures[0].Fields;

            field = fields[0];
            Assert.AreEqual(FieldStyle.UNBOUND_RADIO_BUTTON, field.Style);
            Assert.AreEqual(0, field.Page);
            Assert.AreEqual("", field.Value);
            Assert.AreEqual("group", field.Validator.Options[0]);

            field = fields[1];
            Assert.AreEqual(FieldStyle.UNBOUND_RADIO_BUTTON, field.Style);
            Assert.AreEqual(0, field.Page);
            Assert.AreEqual(FieldBuilder.RADIO_SELECTED, field.Value);
            Assert.AreEqual("group", field.Validator.Options[0]);

            field = fields[2];
            Assert.AreEqual(FieldStyle.UNBOUND_RADIO_BUTTON, field.Style);
            Assert.AreEqual(0, field.Page);
            Assert.AreEqual("", field.Value);
            Assert.AreEqual("group", field.Validator.Options[0]);

        }
    }
}

