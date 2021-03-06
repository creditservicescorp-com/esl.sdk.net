﻿using NUnit.Framework;
using System;
using Silanis.ESL.SDK;

namespace SDK.Examples
{
    [TestFixture()]
    public class MobileCaptureSignatureStyleExampleTest
    {
        [Test()]
        public void VerifyResult()
        {
            MobileCaptureSignatureStyleExample example = new MobileCaptureSignatureStyleExample(Props.GetInstance());
            example.Run();

            DocumentPackage documentPackage = example.EslClient.GetPackage(example.PackageId);

            foreach (Signature signature in documentPackage.Documents[example.DOCUMENT_NAME].Signatures)
            {
				if ((int)(signature.X + 0.1) == example.MOBILE_CAPTURE_SIGNATURE_POSITION_X && (int)(signature.Y + 0.1) == example.MOBILE_CAPTURE_SIGNATURE_POSITION_Y)
                {
					Assert.AreEqual(signature.Style, SignatureStyle.MOBILE_CAPTURE);
					Assert.AreEqual(signature.Page, example.MOBILE_CAPTURE_SIGNATURE_PAGE);
                }
            }
        }
    }
}

