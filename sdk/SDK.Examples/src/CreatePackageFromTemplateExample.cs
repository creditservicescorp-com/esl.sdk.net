﻿using System;
using System.IO;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;

namespace SDK.Examples
{
    public class CreatePackageFromTemplateExample : SDKSample
    {
        public static void Main(string[] args)
        {
            new CreatePackageFromTemplateExample(Props.GetInstance()).Run();
        }

        public string email1;
        public string email2;
        private Stream fileStream1;

        public readonly string DOCUMENT_NAME = "First Document";
        public readonly string DOCUMENT_ID = "doc1";
        public readonly string PACKAGE_NAME = "CreateTemplateFromPackageExample: " + DateTime.Now;
        public readonly string PACKAGE_DESCRIPTION = "This is a package created using the e-SignLive SDK";
        public readonly string PACKAGE_EMAIL_MESSAGE = "This message should be delivered to all signers";
        public readonly string PACKAGE_EMAIL_MESSAGE2 = "Changed the email message";
        public readonly string PACKAGE_SIGNER1_FIRST = "John";
        public readonly string PACKAGE_SIGNER1_LAST = "Smith";
        public readonly string PACKAGE_SIGNER1_TITLE = "Manager";
        public readonly string PACKAGE_SIGNER1_COMPANY = "Acme Inc.";
        public readonly string PACKAGE_SIGNER1_CUSTOM_ID = "Signer1";

        public readonly string PACKAGE_SIGNER2_FIRST = "Elvis";
        public readonly string PACKAGE_SIGNER2_LAST = "Presley";
        public readonly string PACKAGE_SIGNER2_TITLE = "The King";
        public readonly string PACKAGE_SIGNER2_COMPANY = "Elvis Presley International";
        public readonly string PACKAGE_SIGNER2_CUSTOM_ID = "Signer2";

        public CreatePackageFromTemplateExample(Props props) : this(props.Get("api.url"), props.Get("api.key"), props.Get("1.email"), props.Get("2.email"))
        {
        }

        public CreatePackageFromTemplateExample(string apiKey, string apiUrl, string email1, string email2) : base( apiKey, apiUrl )
        {
            this.email1 = email1;
            this.email2 = email2;
            this.fileStream1 = File.OpenRead(new FileInfo(Directory.GetCurrentDirectory() + "/src/document.pdf").FullName);
        }

        override public void Execute()
        {
            DocumentPackage template = PackageBuilder.NewPackageNamed("Template")
                .DescribedAs("first message")
                .WithEmailMessage(PACKAGE_EMAIL_MESSAGE)
                .WithSigner(SignerBuilder.NewSignerWithEmail(email1)
                    .WithFirstName(PACKAGE_SIGNER1_FIRST)
                    .WithLastName(PACKAGE_SIGNER1_LAST)
                    .WithTitle(PACKAGE_SIGNER1_TITLE)
                    .WithCompany(PACKAGE_SIGNER1_COMPANY)
                    .WithCustomId(PACKAGE_SIGNER1_CUSTOM_ID))
                .WithDocument(DocumentBuilder.NewDocumentNamed(DOCUMENT_NAME)
                    .FromStream(fileStream1, DocumentType.PDF)
                    .WithId(DOCUMENT_ID)
                    .Build())
                .Build();

            template.Id = eslClient.CreateTemplate(template);

            DocumentPackage newPackage = PackageBuilder.NewPackageNamed(PACKAGE_NAME)
                .DescribedAs(PACKAGE_DESCRIPTION)
                .WithEmailMessage(PACKAGE_EMAIL_MESSAGE2)
                .WithSigner(SignerBuilder.NewSignerWithEmail(email2)
                    .WithFirstName(PACKAGE_SIGNER2_FIRST)
                    .WithLastName(PACKAGE_SIGNER2_LAST)
                    .WithTitle(PACKAGE_SIGNER2_TITLE)
                    .WithCompany(PACKAGE_SIGNER2_COMPANY)
                    .WithCustomId(PACKAGE_SIGNER2_CUSTOM_ID))
                .WithSettings(DocumentPackageSettingsBuilder.NewDocumentPackageSettings()
                    .WithoutInPerson()
                    .WithoutDecline()
                    .WithoutOptOut()
                    .WithWatermark()
                    .Build())
                .Build();

            packageId = eslClient.CreatePackageFromTemplate(template.Id, newPackage);

        }
    }
}
