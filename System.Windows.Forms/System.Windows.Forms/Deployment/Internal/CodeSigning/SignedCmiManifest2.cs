using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Microsoft.Win32;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x02000015 RID: 21
	internal class SignedCmiManifest2
	{
		// Token: 0x06000077 RID: 119 RVA: 0x000027DB File Offset: 0x000009DB
		private SignedCmiManifest2()
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003FEF File Offset: 0x000021EF
		internal SignedCmiManifest2(XmlDocument manifestDom, bool useSha256)
		{
			if (manifestDom == null)
			{
				throw new ArgumentNullException("manifestDom");
			}
			this.m_manifestDom = manifestDom;
			this.m_useSha256 = useSha256;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004013 File Offset: 0x00002213
		internal void Sign(CmiManifestSigner2 signer)
		{
			this.Sign(signer, null);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004020 File Offset: 0x00002220
		internal void Sign(CmiManifestSigner2 signer, string timeStampUrl)
		{
			this.m_strongNameSignerInfo = null;
			this.m_authenticodeSignerInfo = null;
			if (signer == null || signer.StrongNameKey == null)
			{
				throw new ArgumentNullException("signer");
			}
			SignedCmiManifest2.RemoveExistingSignature(this.m_manifestDom);
			if ((signer.Flag & CmiManifestSignerFlag.DontReplacePublicKeyToken) == CmiManifestSignerFlag.None)
			{
				SignedCmiManifest2.ReplacePublicKeyToken(this.m_manifestDom, signer.StrongNameKey, this.m_useSha256);
			}
			XmlDocument licenseDom = null;
			if (signer.Certificate != null)
			{
				SignedCmiManifest2.InsertPublisherIdentity(this.m_manifestDom, signer.Certificate);
				licenseDom = SignedCmiManifest2.CreateLicenseDom(signer, this.ExtractPrincipalFromManifest(), SignedCmiManifest2.ComputeHashFromManifest(this.m_manifestDom, this.m_useSha256));
				SignedCmiManifest2.AuthenticodeSignLicenseDom(licenseDom, signer, timeStampUrl, this.m_useSha256);
			}
			SignedCmiManifest2.StrongNameSignManifestDom(this.m_manifestDom, licenseDom, signer, this.m_useSha256);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000040D8 File Offset: 0x000022D8
		internal void Verify(CmiManifestVerifyFlags verifyFlags)
		{
			this.m_strongNameSignerInfo = null;
			this.m_authenticodeSignerInfo = null;
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(this.m_manifestDom.NameTable);
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(this.m_manifestDom, "//ds:Signature[@Id=\"StrongNameSignature\"]", xmlNamespaceManager) as XmlElement;
			if (xmlElement == null)
			{
				throw new CryptographicException(-2146762496);
			}
			this.VerifySignatureForm(xmlElement, "StrongNameSignature", xmlNamespaceManager);
			string publicKeyToken = this.VerifyPublicKeyToken();
			this.m_strongNameSignerInfo = new CmiStrongNameSignerInfo(-2146762485, publicKeyToken);
			ManifestSignedXml2 manifestSignedXml = new ManifestSignedXml2(this.m_manifestDom, true);
			manifestSignedXml.LoadXml(xmlElement);
			if (this.m_useSha256)
			{
				manifestSignedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha256";
			}
			AsymmetricAlgorithm publicKey = null;
			bool flag = manifestSignedXml.CheckSignatureReturningKey(out publicKey);
			this.m_strongNameSignerInfo.PublicKey = publicKey;
			if (!flag)
			{
				this.m_strongNameSignerInfo.ErrorCode = -2146869232;
				throw new CryptographicException(-2146869232);
			}
			if ((verifyFlags & CmiManifestVerifyFlags.StrongNameOnly) != CmiManifestVerifyFlags.StrongNameOnly)
			{
				if (this.m_useSha256)
				{
					this.VerifyLicenseNew(verifyFlags);
					return;
				}
				this.VerifyLicense(verifyFlags);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007C RID: 124 RVA: 0x000041E8 File Offset: 0x000023E8
		internal CmiStrongNameSignerInfo StrongNameSignerInfo
		{
			get
			{
				return this.m_strongNameSignerInfo;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000041F0 File Offset: 0x000023F0
		internal CmiAuthenticodeSignerInfo AuthenticodeSignerInfo
		{
			get
			{
				return this.m_authenticodeSignerInfo;
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000041F8 File Offset: 0x000023F8
		private unsafe void VerifyLicense(CmiManifestVerifyFlags verifyFlags)
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(this.m_manifestDom.NameTable);
			xmlNamespaceManager.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");
			xmlNamespaceManager.AddNamespace("asm2", "urn:schemas-microsoft-com:asm.v2");
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			xmlNamespaceManager.AddNamespace("msrel", "http://schemas.microsoft.com/windows/rel/2005/reldata");
			xmlNamespaceManager.AddNamespace("r", "urn:mpeg:mpeg21:2003:01-REL-R-NS");
			xmlNamespaceManager.AddNamespace("as", "http://schemas.microsoft.com/windows/pki/2005/Authenticode");
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(this.m_manifestDom, "asm:assembly/ds:Signature/ds:KeyInfo/msrel:RelData/r:license", xmlNamespaceManager) as XmlElement;
			if (xmlElement == null)
			{
				return;
			}
			if (!this.OverrideTimestampImprovements(xmlNamespaceManager))
			{
				XmlNodeList xmlNodeList = xmlElement.SelectNodes("r:issuer/ds:Signature", xmlNamespaceManager);
				if (xmlNodeList == null || xmlNodeList.Count != 1)
				{
					this.m_authenticodeSignerInfo.ErrorCode = -2146762496;
					throw new CryptographicException(-2146762496);
				}
				DateTime dateTime;
				this.VerifySignatureTimestampNew(xmlNodeList[0] as XmlElement, xmlNamespaceManager, out dateTime);
			}
			this.VerifyAssemblyIdentity(xmlNamespaceManager);
			this.m_authenticodeSignerInfo = new CmiAuthenticodeSignerInfo(-2146762485);
			byte[] bytes = Encoding.UTF8.GetBytes(xmlElement.OuterXml);
			fixed (byte* ptr = bytes)
			{
				Win32.AXL_SIGNER_INFO axl_SIGNER_INFO = default(Win32.AXL_SIGNER_INFO);
				axl_SIGNER_INFO.cbSize = (uint)Marshal.SizeOf(typeof(Win32.AXL_SIGNER_INFO));
				Win32.AXL_TIMESTAMPER_INFO timestamperInfo = default(Win32.AXL_TIMESTAMPER_INFO);
				timestamperInfo.cbSize = (uint)Marshal.SizeOf(typeof(Win32.AXL_TIMESTAMPER_INFO));
				Win32.CRYPT_DATA_BLOB crypt_DATA_BLOB = default(Win32.CRYPT_DATA_BLOB);
				IntPtr pbData = new IntPtr((void*)ptr);
				crypt_DATA_BLOB.cbData = (uint)bytes.Length;
				crypt_DATA_BLOB.pbData = pbData;
				int num = Win32.CertVerifyAuthenticodeLicense(ref crypt_DATA_BLOB, (uint)verifyFlags, ref axl_SIGNER_INFO, ref timestamperInfo);
				if (2148204800U != axl_SIGNER_INFO.dwError)
				{
					this.m_authenticodeSignerInfo = new CmiAuthenticodeSignerInfo(axl_SIGNER_INFO, timestamperInfo);
				}
				Win32.CertFreeAuthenticodeSignerInfo(ref axl_SIGNER_INFO);
				Win32.CertFreeAuthenticodeTimestamperInfo(ref timestamperInfo);
				if (num != 0)
				{
					throw new CryptographicException(num);
				}
			}
			this.VerifyPublisherIdentity(xmlNamespaceManager);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000043E0 File Offset: 0x000025E0
		private void VerifyLicenseNew(CmiManifestVerifyFlags verifyFlags)
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(this.m_manifestDom.NameTable);
			xmlNamespaceManager.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");
			xmlNamespaceManager.AddNamespace("asm2", "urn:schemas-microsoft-com:asm.v2");
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			xmlNamespaceManager.AddNamespace("msrel", "http://schemas.microsoft.com/windows/rel/2005/reldata");
			xmlNamespaceManager.AddNamespace("r", "urn:mpeg:mpeg21:2003:01-REL-R-NS");
			xmlNamespaceManager.AddNamespace("as", "http://schemas.microsoft.com/windows/pki/2005/Authenticode");
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(this.m_manifestDom, "asm:assembly/ds:Signature/ds:KeyInfo/msrel:RelData/r:license", xmlNamespaceManager) as XmlElement;
			if (xmlElement == null)
			{
				return;
			}
			this.VerifyAssemblyIdentity(xmlNamespaceManager);
			this.m_authenticodeSignerInfo = new CmiAuthenticodeSignerInfo(-2146762485);
			XmlElement xmlElement2 = SignedCmiManifest2.GetSingleNode(xmlElement, "//r:issuer/ds:Signature", xmlNamespaceManager) as XmlElement;
			if (xmlElement2 == null)
			{
				throw new CryptographicException(-2146762496);
			}
			this.VerifySignatureForm(xmlElement2, "AuthenticodeSignature", xmlNamespaceManager);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xmlElement.OuterXml);
			xmlElement2 = (SignedCmiManifest2.GetSingleNode(xmlDocument, "//r:issuer/ds:Signature", xmlNamespaceManager) as XmlElement);
			ManifestSignedXml2 manifestSignedXml = new ManifestSignedXml2(xmlDocument);
			manifestSignedXml.LoadXml(xmlElement2);
			if (this.m_useSha256)
			{
				manifestSignedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha256";
			}
			if (!manifestSignedXml.CheckSignature())
			{
				this.m_authenticodeSignerInfo = null;
				throw new CryptographicException(-2146869244);
			}
			X509Certificate2 signingCertificate = this.GetSigningCertificate(manifestSignedXml, xmlNamespaceManager);
			X509Store x509Store = new X509Store(StoreName.Disallowed, StoreLocation.CurrentUser);
			x509Store.Open(OpenFlags.OpenExistingOnly);
			try
			{
				X509Certificate2Collection certificates = x509Store.Certificates;
				if (certificates == null)
				{
					this.m_authenticodeSignerInfo.ErrorCode = -2146762485;
					throw new CryptographicException(-2146762485);
				}
				if (certificates.Contains(signingCertificate))
				{
					this.m_authenticodeSignerInfo.ErrorCode = -2146762479;
					throw new CryptographicException(-2146762479);
				}
			}
			finally
			{
				x509Store.Close();
			}
			string hash;
			string description;
			string descriptionUrl;
			if (!this.GetManifestInformation(xmlElement, xmlNamespaceManager, out hash, out description, out descriptionUrl))
			{
				this.m_authenticodeSignerInfo.ErrorCode = -2146762749;
				throw new CryptographicException(-2146762749);
			}
			this.m_authenticodeSignerInfo.Hash = hash;
			this.m_authenticodeSignerInfo.Description = description;
			this.m_authenticodeSignerInfo.DescriptionUrl = descriptionUrl;
			DateTime verificationTime;
			bool flag = this.OverrideTimestampImprovements(xmlNamespaceManager) ? this.VerifySignatureTimestamp(xmlElement2, xmlNamespaceManager, out verificationTime) : this.VerifySignatureTimestampNew(xmlElement2, xmlNamespaceManager, out verificationTime);
			bool flag2 = false;
			if (flag)
			{
				flag2 = ((verifyFlags & CmiManifestVerifyFlags.LifetimeSigning) == CmiManifestVerifyFlags.LifetimeSigning);
				if (!flag2)
				{
					flag2 = this.GetLifetimeSigning(signingCertificate);
				}
			}
			uint authenticodePolicies = this.GetAuthenticodePolicies();
			X509Chain x509Chain = new X509Chain();
			x509Chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
			x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
			if ((CmiManifestVerifyFlags.RevocationCheckEndCertOnly & verifyFlags) == CmiManifestVerifyFlags.RevocationCheckEndCertOnly)
			{
				x509Chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EndCertificateOnly;
			}
			else if ((CmiManifestVerifyFlags.RevocationCheckEntireChain & verifyFlags) == CmiManifestVerifyFlags.RevocationCheckEntireChain)
			{
				x509Chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
			}
			else if ((CmiManifestVerifyFlags.RevocationNoCheck & verifyFlags) == CmiManifestVerifyFlags.RevocationNoCheck || (512U & authenticodePolicies) == 512U)
			{
				x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
			}
			x509Chain.ChainPolicy.VerificationTime = verificationTime;
			if (flag && flag2)
			{
				x509Chain.ChainPolicy.ApplicationPolicy.Add(new Oid("1.3.6.1.4.1.311.10.3.13"));
			}
			x509Chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
			x509Chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
			if (!x509Chain.Build(signingCertificate))
			{
				this.AuthenticodeSignerInfo.ErrorCode = -2146762748;
				throw new CryptographicException(-2146762748);
			}
			this.m_authenticodeSignerInfo.SignerChain = x509Chain;
			x509Store = new X509Store(StoreName.TrustedPublisher, StoreLocation.CurrentUser);
			x509Store.Open(OpenFlags.OpenExistingOnly);
			try
			{
				X509Certificate2Collection certificates = x509Store.Certificates;
				if (certificates == null)
				{
					this.m_authenticodeSignerInfo.ErrorCode = -2146762485;
					throw new CryptographicException(-2146762485);
				}
				if (!certificates.Contains(signingCertificate))
				{
					this.AuthenticodeSignerInfo.ErrorCode = -2146762748;
					throw new CryptographicException(-2146762748);
				}
			}
			finally
			{
				x509Store.Close();
			}
			XmlElement xmlElement3 = SignedCmiManifest2.GetSingleNode(xmlElement, "r:grant/as:AuthenticodePublisher/as:X509SubjectName", xmlNamespaceManager) as XmlElement;
			if (xmlElement3 == null || string.Compare(signingCertificate.Subject, xmlElement3.InnerText, StringComparison.Ordinal) != 0)
			{
				this.AuthenticodeSignerInfo.ErrorCode = -2146869244;
				throw new CryptographicException(-2146869244);
			}
			this.VerifyPublisherIdentity(xmlNamespaceManager);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004800 File Offset: 0x00002A00
		private X509Certificate2 GetSigningCertificate(ManifestSignedXml2 signedXml, XmlNamespaceManager nsm)
		{
			X509Certificate2 x509Certificate = null;
			KeyInfo keyInfo = signedXml.KeyInfo;
			KeyInfoX509Data keyInfoX509Data = null;
			RSAKeyValue rsakeyValue = null;
			foreach (object obj in keyInfo)
			{
				KeyInfoClause keyInfoClause = (KeyInfoClause)obj;
				if (rsakeyValue == null)
				{
					rsakeyValue = (keyInfoClause as RSAKeyValue);
					if (rsakeyValue == null)
					{
						break;
					}
				}
				if (keyInfoX509Data == null)
				{
					keyInfoX509Data = (keyInfoClause as KeyInfoX509Data);
				}
				if (rsakeyValue != null && keyInfoX509Data != null)
				{
					break;
				}
			}
			if (rsakeyValue == null || keyInfoX509Data == null)
			{
				this.m_authenticodeSignerInfo.ErrorCode = -2146762749;
				throw new CryptographicException(-2146762749);
			}
			RSA key = rsakeyValue.Key;
			if (key == null)
			{
				this.m_authenticodeSignerInfo.ErrorCode = -2146869244;
				throw new CryptographicException(-2146869244);
			}
			RSAParameters rsaparameters = key.ExportParameters(false);
			foreach (object obj2 in keyInfoX509Data.Certificates)
			{
				X509Certificate2 x509Certificate2 = (X509Certificate2)obj2;
				if (x509Certificate2 != null)
				{
					bool flag = false;
					foreach (X509Extension x509Extension in x509Certificate2.Extensions)
					{
						X509BasicConstraintsExtension x509BasicConstraintsExtension = x509Extension as X509BasicConstraintsExtension;
						if (x509BasicConstraintsExtension != null)
						{
							flag = x509BasicConstraintsExtension.CertificateAuthority;
							if (flag)
							{
								break;
							}
						}
					}
					if (!flag)
					{
						RSA rsapublicKey = CngLightup.GetRSAPublicKey(x509Certificate2);
						RSAParameters rsaparameters2 = rsapublicKey.ExportParameters(false);
						if (StructuralComparisons.StructuralEqualityComparer.Equals(rsaparameters.Exponent, rsaparameters2.Exponent) && StructuralComparisons.StructuralEqualityComparer.Equals(rsaparameters.Modulus, rsaparameters2.Modulus))
						{
							x509Certificate = x509Certificate2;
							break;
						}
					}
				}
			}
			if (x509Certificate == null)
			{
				this.m_authenticodeSignerInfo.ErrorCode = -2146869244;
				throw new CryptographicException(-2146869244);
			}
			return x509Certificate;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000049E0 File Offset: 0x00002BE0
		private void VerifySignatureForm(XmlElement signatureNode, string signatureKind, XmlNamespaceManager nsm)
		{
			string name = "Id";
			if (!signatureNode.HasAttribute(name))
			{
				name = "id";
				if (!signatureNode.HasAttribute(name))
				{
					name = "ID";
					if (!signatureNode.HasAttribute(name))
					{
						throw new CryptographicException(-2146762749);
					}
				}
			}
			string attribute = signatureNode.GetAttribute(name);
			if (attribute == null || string.Compare(attribute, signatureKind, StringComparison.Ordinal) != 0)
			{
				throw new CryptographicException(-2146762749);
			}
			bool flag = false;
			XmlNodeList xmlNodeList = signatureNode.SelectNodes("ds:SignedInfo/ds:Reference", nsm);
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlElement xmlElement = xmlNode as XmlElement;
				if (xmlElement != null && xmlElement.HasAttribute("URI"))
				{
					string attribute2 = xmlElement.GetAttribute("URI");
					if (attribute2 != null && attribute2.Length == 0)
					{
						XmlNode singleNode = SignedCmiManifest2.GetSingleNode(xmlElement, "ds:Transforms", nsm);
						if (singleNode == null)
						{
							throw new CryptographicException(-2146762749);
						}
						XmlNodeList xmlNodeList2 = singleNode.SelectNodes("ds:Transform", nsm);
						if (xmlNodeList2.Count < 2)
						{
							throw new CryptographicException(-2146762749);
						}
						bool flag2 = false;
						bool flag3 = false;
						for (int i = 0; i < xmlNodeList2.Count; i++)
						{
							XmlElement xmlElement2 = xmlNodeList2[i] as XmlElement;
							string attribute3 = xmlElement2.GetAttribute("Algorithm");
							if (attribute3 == null)
							{
								break;
							}
							if (string.Compare(attribute3, "http://www.w3.org/2001/10/xml-exc-c14n#", StringComparison.Ordinal) != 0)
							{
								flag2 = true;
								if (flag3)
								{
									flag = true;
									break;
								}
							}
							else if (string.Compare(attribute3, "http://www.w3.org/2000/09/xmldsig#enveloped-signature", StringComparison.Ordinal) != 0)
							{
								flag3 = true;
								if (flag2)
								{
									flag = true;
									break;
								}
							}
						}
					}
				}
			}
			if (!flag)
			{
				throw new CryptographicException(-2146762749);
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004BB4 File Offset: 0x00002DB4
		private bool GetManifestInformation(XmlElement licenseNode, XmlNamespaceManager nsm, out string hash, out string description, out string url)
		{
			hash = "";
			description = "";
			url = "";
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(licenseNode, "r:grant/as:ManifestInformation", nsm) as XmlElement;
			if (xmlElement == null)
			{
				return false;
			}
			if (!xmlElement.HasAttribute("Hash"))
			{
				return false;
			}
			hash = xmlElement.GetAttribute("Hash");
			if (string.IsNullOrEmpty(hash))
			{
				return false;
			}
			foreach (char val in hash)
			{
				if (255 == SignedCmiManifest2.HexToByte(val))
				{
					return false;
				}
			}
			if (xmlElement.HasAttribute("Description"))
			{
				description = xmlElement.GetAttribute("Description");
			}
			if (xmlElement.HasAttribute("Url"))
			{
				url = xmlElement.GetAttribute("Url");
			}
			return true;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004C78 File Offset: 0x00002E78
		private bool VerifySignatureTimestamp(XmlElement signatureNode, XmlNamespaceManager nsm, out DateTime verificationTime)
		{
			verificationTime = DateTime.Now;
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(signatureNode, "ds:Object/as:Timestamp", nsm) as XmlElement;
			if (xmlElement != null)
			{
				string innerText = xmlElement.InnerText;
				if (!string.IsNullOrEmpty(innerText))
				{
					byte[] array = null;
					try
					{
						array = Convert.FromBase64String(innerText);
					}
					catch (FormatException)
					{
						this.m_authenticodeSignerInfo.ErrorCode = -2146869243;
						throw new CryptographicException(-2146869243);
					}
					if (array != null)
					{
						SignedCms signedCms = new SignedCms();
						signedCms.Decode(array);
						signedCms.CheckSignature(true);
						CryptographicAttributeObjectCollection signedAttributes = signedCms.SignerInfos[0].SignedAttributes;
						foreach (CryptographicAttributeObject cryptographicAttributeObject in signedAttributes)
						{
							if (string.Compare(cryptographicAttributeObject.Oid.Value, "1.2.840.113549.1.9.5", StringComparison.Ordinal) == 0)
							{
								foreach (AsnEncodedData asnEncodedData in cryptographicAttributeObject.Values)
								{
									if (string.Compare(asnEncodedData.Oid.Value, "1.2.840.113549.1.9.5", StringComparison.Ordinal) == 0)
									{
										byte[] rawData = asnEncodedData.RawData;
										Pkcs9SigningTime pkcs9SigningTime = new Pkcs9SigningTime(rawData);
										verificationTime = pkcs9SigningTime.SigningTime;
										return true;
									}
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004DB8 File Offset: 0x00002FB8
		private bool VerifySignatureTimestampNew(XmlElement signatureNode, XmlNamespaceManager nsm, out DateTime verificationTime)
		{
			verificationTime = DateTime.Now;
			DateTime? dateTime = null;
			string a = null;
			byte[] array = null;
			XmlNodeList xmlNodeList = signatureNode.SelectNodes("ds:Object/as:Timestamp", nsm);
			XmlNodeList xmlNodeList2 = signatureNode.SelectNodes("ds:SignatureValue", nsm);
			if (xmlNodeList == null || xmlNodeList.Count == 0 || xmlNodeList2 == null || xmlNodeList2.Count == 0)
			{
				return false;
			}
			if (xmlNodeList.Count > 1 || xmlNodeList2.Count > 1 || string.IsNullOrEmpty(xmlNodeList[0].InnerText) || string.IsNullOrEmpty(xmlNodeList2[0].InnerText))
			{
				this.m_authenticodeSignerInfo.ErrorCode = -2146869243;
				throw new CryptographicException(-2146869243);
			}
			byte[] array2 = null;
			byte[] array3 = null;
			try
			{
				array2 = Convert.FromBase64String(xmlNodeList[0].InnerText);
				array3 = Convert.FromBase64String(xmlNodeList2[0].InnerText);
			}
			catch (FormatException)
			{
				this.m_authenticodeSignerInfo.ErrorCode = -2146869243;
				throw new CryptographicException(-2146869243);
			}
			if (array2 == null || array3 == null)
			{
				return false;
			}
			SignedCms signedCms = new SignedCms();
			signedCms.Decode(array2);
			signedCms.CheckSignature(true);
			if (signedCms.SignerInfos.Count != 1)
			{
				this.m_authenticodeSignerInfo.ErrorCode = -2146869246;
				throw new CryptographicException(-2146869246);
			}
			a = signedCms.SignerInfos[0].DigestAlgorithm.Value;
			CryptographicAttributeObjectCollection signedAttributes = signedCms.SignerInfos[0].SignedAttributes;
			foreach (CryptographicAttributeObject cryptographicAttributeObject in signedAttributes)
			{
				if (dateTime == null && string.Compare(cryptographicAttributeObject.Oid.Value, "1.2.840.113549.1.9.5", StringComparison.Ordinal) == 0)
				{
					foreach (AsnEncodedData asnEncodedData in cryptographicAttributeObject.Values)
					{
						if (string.Compare(asnEncodedData.Oid.Value, "1.2.840.113549.1.9.5", StringComparison.Ordinal) == 0)
						{
							dateTime = new DateTime?(new Pkcs9SigningTime(asnEncodedData.RawData).SigningTime);
							break;
						}
					}
				}
				else if (array == null && string.Compare(cryptographicAttributeObject.Oid.Value, "1.2.840.113549.1.9.4", StringComparison.Ordinal) == 0)
				{
					foreach (AsnEncodedData asnEncodedData2 in cryptographicAttributeObject.Values)
					{
						if (string.Compare(asnEncodedData2.Oid.Value, "1.2.840.113549.1.9.4", StringComparison.Ordinal) == 0)
						{
							byte[] rawData = asnEncodedData2.RawData;
							array = new Pkcs9MessageDigest
							{
								RawData = rawData
							}.MessageDigest;
							break;
						}
					}
				}
			}
			bool flag = false;
			bool flag2 = false;
			try
			{
				dateTime = new DateTime?(this.VerifyRFC3161Timestamp(array2, array3));
				flag = true;
				flag2 = true;
			}
			catch (Exception ex)
			{
				if ((!(ex is CryptographicException) || ex.HResult != -2146893822) && array != null)
				{
					HashAlgorithm hashAlgorithm = null;
					if (a == "2.16.840.1.101.3.4.2.1")
					{
						hashAlgorithm = SHA256.Create();
					}
					else if (a == "1.3.14.3.2.26")
					{
						hashAlgorithm = SHA1.Create();
					}
					if (hashAlgorithm != null)
					{
						byte[] array4 = hashAlgorithm.ComputeHash(array3);
						if (array4 != null && array4.Length == array.Length)
						{
							flag = array4.SequenceEqual(array);
						}
					}
				}
			}
			if (!flag)
			{
				this.m_authenticodeSignerInfo.ErrorCode = -2146893822;
				throw new CryptographicException(-2146893822);
			}
			if (dateTime == null)
			{
				return false;
			}
			if (!flag2)
			{
				X509Certificate2 certificate = signedCms.SignerInfos[0].Certificate;
				if (certificate.NotAfter < dateTime || certificate.NotBefore > dateTime)
				{
					this.m_authenticodeSignerInfo.ErrorCode = -2146869243;
					throw new CryptographicException(-2146869243);
				}
			}
			bool flag3 = false;
			try
			{
				using (X509Chain x509Chain = new X509Chain())
				{
					x509Chain.ChainPolicy.ExtraStore.AddRange(signedCms.Certificates);
					x509Chain.ChainPolicy.VerificationTime = dateTime.Value;
					x509Chain.ChainPolicy.ApplicationPolicy.Add(new Oid("1.3.6.1.5.5.7.3.8"));
					flag3 = x509Chain.Build(signedCms.SignerInfos[0].Certificate);
				}
			}
			catch (Exception ex2)
			{
				if (ex2 is ArgumentException || ex2 is CryptographicException)
				{
					this.m_authenticodeSignerInfo.ErrorCode = -2146869243;
					throw new CryptographicException(-2146869243);
				}
			}
			if (!flag3)
			{
				this.m_authenticodeSignerInfo.ErrorCode = -2146762748;
				throw new CryptographicException(-2146762748);
			}
			verificationTime = dateTime.Value;
			return true;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005290 File Offset: 0x00003490
		private DateTime VerifyRFC3161Timestamp(byte[] base64DecodedMessage, byte[] base64DecodedSignatureValue)
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			IntPtr zero3 = IntPtr.Zero;
			DateTime result;
			try
			{
				if (!Win32.CryptVerifyTimeStampSignature(base64DecodedMessage, base64DecodedMessage.Length, base64DecodedSignatureValue, base64DecodedSignatureValue.Length, IntPtr.Zero, ref zero, ref zero2, ref zero3) || zero == IntPtr.Zero)
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				try
				{
					result = this.ReadRfc3161TimestampTokenTime(zero);
				}
				catch (Exception ex)
				{
					throw new CryptographicException(ex.Message);
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Win32.CryptMemFree(zero);
				}
				if (zero2 != IntPtr.Zero)
				{
					Win32.CertFreeCertificateContext(zero2);
				}
				if (zero3 != IntPtr.Zero)
				{
					Win32.CertCloseStore(zero3, 0);
				}
			}
			return result;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005358 File Offset: 0x00003558
		private DateTime ReadRfc3161TimestampTokenTime(IntPtr pTsContext)
		{
			Win32.CRYPT_TIMESTAMP_CONTEXT crypt_TIMESTAMP_CONTEXT = (Win32.CRYPT_TIMESTAMP_CONTEXT)Marshal.PtrToStructure(pTsContext, typeof(Win32.CRYPT_TIMESTAMP_CONTEXT));
			Win32.CRYPT_TIMESTAMP_INFO crypt_TIMESTAMP_INFO = (Win32.CRYPT_TIMESTAMP_INFO)Marshal.PtrToStructure(crypt_TIMESTAMP_CONTEXT.pTimeStamp, typeof(Win32.CRYPT_TIMESTAMP_INFO));
			long num = (long)crypt_TIMESTAMP_INFO.ftTime.dwHighDateTime;
			num <<= 32;
			num |= ((long)crypt_TIMESTAMP_INFO.ftTime.dwLowDateTime & (long)((ulong)-1));
			return DateTime.FromFileTimeUtc(num);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000053C0 File Offset: 0x000035C0
		private bool OverrideTimestampImprovements(XmlNamespaceManager nsm)
		{
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(this.m_manifestDom, "asm:assembly/asm2:publisherIdentity", nsm) as XmlElement;
			if (xmlElement == null || !xmlElement.HasAttributes)
			{
				return true;
			}
			if (!xmlElement.HasAttribute("issuerKeyHash"))
			{
				return true;
			}
			string attribute = xmlElement.GetAttribute("issuerKeyHash");
			try
			{
				string text = ConfigurationManager.AppSettings.Get("ClickOnceTimeStampImprovementsOverride");
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(new char[]
					{
						';'
					});
					foreach (string text2 in array)
					{
						if (text2.Trim().Equals(attribute, StringComparison.OrdinalIgnoreCase))
						{
							return true;
						}
					}
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005484 File Offset: 0x00003684
		private bool GetLifetimeSigning(X509Certificate2 signingCertificate)
		{
			foreach (X509Extension x509Extension in signingCertificate.Extensions)
			{
				X509EnhancedKeyUsageExtension x509EnhancedKeyUsageExtension = x509Extension as X509EnhancedKeyUsageExtension;
				if (x509EnhancedKeyUsageExtension != null)
				{
					OidCollection enhancedKeyUsages = x509EnhancedKeyUsageExtension.EnhancedKeyUsages;
					foreach (Oid oid in enhancedKeyUsages)
					{
						if (string.Compare("1.3.6.1.4.1.311.10.3.13", oid.Value, StringComparison.Ordinal) == 0)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000054F4 File Offset: 0x000036F4
		private uint GetAuthenticodePolicies()
		{
			uint result = 0U;
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\WinTrust\\Trust Providers\\Software Publishing");
				if (registryKey != null)
				{
					RegistryValueKind valueKind = registryKey.GetValueKind("State");
					if (valueKind == RegistryValueKind.DWord || valueKind == RegistryValueKind.Binary)
					{
						object value = registryKey.GetValue("State");
						if (value != null)
						{
							result = Convert.ToUInt32(value);
						}
					}
					registryKey.Close();
				}
			}
			catch (SecurityException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (IOException)
			{
			}
			return result;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000558C File Offset: 0x0000378C
		private XmlElement ExtractPrincipalFromManifest()
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(this.m_manifestDom.NameTable);
			xmlNamespaceManager.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");
			XmlNode singleNode = SignedCmiManifest2.GetSingleNode(this.m_manifestDom, "asm:assembly/asm:assemblyIdentity", xmlNamespaceManager);
			if (singleNode == null)
			{
				throw new CryptographicException(-2146762749);
			}
			return singleNode as XmlElement;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000055E0 File Offset: 0x000037E0
		private void VerifyAssemblyIdentity(XmlNamespaceManager nsm)
		{
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(this.m_manifestDom, "asm:assembly/asm:assemblyIdentity", nsm) as XmlElement;
			XmlElement xmlElement2 = SignedCmiManifest2.GetSingleNode(this.m_manifestDom, "asm:assembly/ds:Signature/ds:KeyInfo/msrel:RelData/r:license/r:grant/as:ManifestInformation/as:assemblyIdentity", nsm) as XmlElement;
			if (xmlElement == null || xmlElement2 == null || !xmlElement.HasAttributes || !xmlElement2.HasAttributes)
			{
				throw new CryptographicException(-2146762749);
			}
			XmlAttributeCollection attributes = xmlElement.Attributes;
			if (attributes.Count == 0 || attributes.Count != xmlElement2.Attributes.Count)
			{
				throw new CryptographicException(-2146762749);
			}
			foreach (object obj in attributes)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)obj;
				if (!xmlElement2.HasAttribute(xmlAttribute.LocalName) || xmlAttribute.Value != xmlElement2.GetAttribute(xmlAttribute.LocalName))
				{
					throw new CryptographicException(-2146762749);
				}
			}
			this.VerifyHash(nsm);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000056EC File Offset: 0x000038EC
		private void VerifyPublisherIdentity(XmlNamespaceManager nsm)
		{
			if (this.m_authenticodeSignerInfo.ErrorCode == -2146762496)
			{
				return;
			}
			X509Certificate2 certificate = this.m_authenticodeSignerInfo.SignerChain.ChainElements[0].Certificate;
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(this.m_manifestDom, "asm:assembly/asm2:publisherIdentity", nsm) as XmlElement;
			if (xmlElement == null || !xmlElement.HasAttributes)
			{
				throw new CryptographicException(-2146762749);
			}
			if (!xmlElement.HasAttribute("name") || !xmlElement.HasAttribute("issuerKeyHash"))
			{
				throw new CryptographicException(-2146762749);
			}
			string attribute = xmlElement.GetAttribute("name");
			string attribute2 = xmlElement.GetAttribute("issuerKeyHash");
			IntPtr intPtr = 0;
			int num = Win32._AxlGetIssuerPublicKeyHash(certificate.Handle, ref intPtr);
			if (num != 0)
			{
				throw new CryptographicException(num);
			}
			string strB = Marshal.PtrToStringUni(intPtr);
			Win32.HeapFree(Win32.GetProcessHeap(), 0U, intPtr);
			if (string.Compare(attribute, certificate.SubjectName.Name, StringComparison.Ordinal) != 0 || string.Compare(attribute2, strB, StringComparison.Ordinal) != 0)
			{
				throw new CryptographicException(-2146762485);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000057F8 File Offset: 0x000039F8
		private void VerifyHash(XmlNamespaceManager nsm)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument = (XmlDocument)this.m_manifestDom.Clone();
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(xmlDocument, "asm:assembly/ds:Signature/ds:KeyInfo/msrel:RelData/r:license/r:grant/as:ManifestInformation", nsm) as XmlElement;
			if (xmlElement == null)
			{
				throw new CryptographicException(-2146762749);
			}
			if (!xmlElement.HasAttribute("Hash"))
			{
				throw new CryptographicException(-2146762749);
			}
			string attribute = xmlElement.GetAttribute("Hash");
			if (attribute == null || attribute.Length == 0)
			{
				throw new CryptographicException(-2146762749);
			}
			XmlElement xmlElement2 = SignedCmiManifest2.GetSingleNode(xmlDocument, "asm:assembly/ds:Signature", nsm) as XmlElement;
			if (xmlElement2 == null)
			{
				throw new CryptographicException(-2146762749);
			}
			xmlElement2.ParentNode.RemoveChild(xmlElement2);
			byte[] array = SignedCmiManifest2.HexStringToBytes(xmlElement.GetAttribute("Hash"));
			byte[] array2 = SignedCmiManifest2.ComputeHashFromManifest(xmlDocument, this.m_useSha256);
			if (array.Length == 0 || array.Length != array2.Length)
			{
				throw new CryptographicException(-2146869232);
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != array2[i])
				{
					throw new CryptographicException(-2146869232);
				}
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005910 File Offset: 0x00003B10
		private unsafe string VerifyPublicKeyToken()
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(this.m_manifestDom.NameTable);
			xmlNamespaceManager.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(this.m_manifestDom, "asm:assembly/ds:Signature/ds:KeyInfo/ds:KeyValue/ds:RSAKeyValue/ds:Modulus", xmlNamespaceManager) as XmlElement;
			XmlElement xmlElement2 = SignedCmiManifest2.GetSingleNode(this.m_manifestDom, "asm:assembly/ds:Signature/ds:KeyInfo/ds:KeyValue/ds:RSAKeyValue/ds:Exponent", xmlNamespaceManager) as XmlElement;
			if (xmlElement == null || xmlElement2 == null)
			{
				throw new CryptographicException(-2146762749);
			}
			byte[] bytes = Encoding.UTF8.GetBytes(xmlElement.InnerXml);
			byte[] bytes2 = Encoding.UTF8.GetBytes(xmlElement2.InnerXml);
			string publicKeyToken = SignedCmiManifest2.GetPublicKeyToken(this.m_manifestDom);
			byte[] array = SignedCmiManifest2.HexStringToBytes(publicKeyToken);
			byte[] array2;
			fixed (byte* ptr = bytes)
			{
				fixed (byte* ptr2 = bytes2)
				{
					Win32.CRYPT_DATA_BLOB crypt_DATA_BLOB = default(Win32.CRYPT_DATA_BLOB);
					Win32.CRYPT_DATA_BLOB crypt_DATA_BLOB2 = default(Win32.CRYPT_DATA_BLOB);
					IntPtr intPtr = 0;
					crypt_DATA_BLOB.cbData = (uint)bytes.Length;
					crypt_DATA_BLOB.pbData = new IntPtr((void*)ptr);
					crypt_DATA_BLOB2.cbData = (uint)bytes2.Length;
					crypt_DATA_BLOB2.pbData = new IntPtr((void*)ptr2);
					int num = Win32._AxlRSAKeyValueToPublicKeyToken(ref crypt_DATA_BLOB, ref crypt_DATA_BLOB2, ref intPtr);
					if (num != 0)
					{
						throw new CryptographicException(num);
					}
					array2 = SignedCmiManifest2.HexStringToBytes(Marshal.PtrToStringUni(intPtr));
					Win32.HeapFree(Win32.GetProcessHeap(), 0U, intPtr);
				}
			}
			if (array.Length == 0 || array.Length != array2.Length)
			{
				throw new CryptographicException(-2146762485);
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != array2[i])
				{
					throw new CryptographicException(-2146762485);
				}
			}
			return publicKeyToken;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005AD0 File Offset: 0x00003CD0
		private static void InsertPublisherIdentity(XmlDocument manifestDom, X509Certificate2 signerCert)
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(manifestDom.NameTable);
			xmlNamespaceManager.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");
			xmlNamespaceManager.AddNamespace("asm2", "urn:schemas-microsoft-com:asm.v2");
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(manifestDom, "asm:assembly", xmlNamespaceManager) as XmlElement;
			if (!(SignedCmiManifest2.GetSingleNode(manifestDom, "asm:assembly/asm:assemblyIdentity", xmlNamespaceManager) is XmlElement))
			{
				throw new CryptographicException(-2146762749);
			}
			XmlElement xmlElement2 = SignedCmiManifest2.GetSingleNode(manifestDom, "asm:assembly/asm2:publisherIdentity", xmlNamespaceManager) as XmlElement;
			if (xmlElement2 == null)
			{
				xmlElement2 = manifestDom.CreateElement("publisherIdentity", "urn:schemas-microsoft-com:asm.v2");
			}
			IntPtr intPtr = 0;
			int num = Win32._AxlGetIssuerPublicKeyHash(signerCert.Handle, ref intPtr);
			if (num != 0)
			{
				throw new CryptographicException(num);
			}
			string value = Marshal.PtrToStringUni(intPtr);
			Win32.HeapFree(Win32.GetProcessHeap(), 0U, intPtr);
			xmlElement2.SetAttribute("name", signerCert.SubjectName.Name);
			xmlElement2.SetAttribute("issuerKeyHash", value);
			XmlElement xmlElement3 = SignedCmiManifest2.GetSingleNode(manifestDom, "asm:assembly/ds:Signature", xmlNamespaceManager) as XmlElement;
			if (xmlElement3 != null)
			{
				xmlElement.InsertBefore(xmlElement2, xmlElement3);
				return;
			}
			xmlElement.AppendChild(xmlElement2);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005BF8 File Offset: 0x00003DF8
		private static void RemoveExistingSignature(XmlDocument manifestDom)
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(manifestDom.NameTable);
			xmlNamespaceManager.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			XmlNode singleNode = SignedCmiManifest2.GetSingleNode(manifestDom, "asm:assembly/ds:Signature", xmlNamespaceManager);
			if (singleNode != null)
			{
				singleNode.ParentNode.RemoveChild(singleNode);
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005C50 File Offset: 0x00003E50
		internal static RSACryptoServiceProvider GetFixedRSACryptoServiceProvider(RSACryptoServiceProvider oldCsp, bool useSha256)
		{
			if (!useSha256)
			{
				return oldCsp;
			}
			if (!oldCsp.CspKeyContainerInfo.ProviderName.StartsWith("Microsoft", StringComparison.Ordinal))
			{
				return oldCsp;
			}
			CspParameters cspParameters = new CspParameters();
			cspParameters.ProviderType = 24;
			cspParameters.KeyContainerName = oldCsp.CspKeyContainerInfo.KeyContainerName;
			cspParameters.KeyNumber = (int)oldCsp.CspKeyContainerInfo.KeyNumber;
			if (oldCsp.CspKeyContainerInfo.MachineKeyStore)
			{
				cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
			}
			return new RSACryptoServiceProvider(cspParameters);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005CC8 File Offset: 0x00003EC8
		private unsafe static void ReplacePublicKeyToken(XmlDocument manifestDom, AsymmetricAlgorithm snKey, bool useSha256)
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(manifestDom.NameTable);
			xmlNamespaceManager.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(manifestDom, "asm:assembly/asm:assemblyIdentity", xmlNamespaceManager) as XmlElement;
			if (xmlElement == null)
			{
				throw new CryptographicException(-2146762749);
			}
			if (!xmlElement.HasAttribute("publicKeyToken"))
			{
				throw new CryptographicException(-2146762749);
			}
			byte[] array;
			if (snKey is RSACryptoServiceProvider)
			{
				array = SignedCmiManifest2.GetFixedRSACryptoServiceProvider((RSACryptoServiceProvider)snKey, useSha256).ExportCspBlob(false);
				if (array == null || array.Length == 0)
				{
					throw new CryptographicException(-2146893821);
				}
			}
			else
			{
				using (RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider())
				{
					rsacryptoServiceProvider.ImportParameters(((RSA)snKey).ExportParameters(false));
					array = rsacryptoServiceProvider.ExportCspBlob(false);
				}
			}
			fixed (byte* ptr = array)
			{
				Win32.CRYPT_DATA_BLOB crypt_DATA_BLOB = default(Win32.CRYPT_DATA_BLOB);
				crypt_DATA_BLOB.cbData = (uint)array.Length;
				crypt_DATA_BLOB.pbData = new IntPtr((void*)ptr);
				IntPtr intPtr = 0;
				int num = Win32._AxlPublicKeyBlobToPublicKeyToken(ref crypt_DATA_BLOB, ref intPtr);
				if (num != 0)
				{
					throw new CryptographicException(num);
				}
				string value = Marshal.PtrToStringUni(intPtr);
				Win32.HeapFree(Win32.GetProcessHeap(), 0U, intPtr);
				xmlElement.SetAttribute("publicKeyToken", value);
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005E18 File Offset: 0x00004018
		private static string GetPublicKeyToken(XmlDocument manifestDom)
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(manifestDom.NameTable);
			xmlNamespaceManager.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(manifestDom, "asm:assembly/asm:assemblyIdentity", xmlNamespaceManager) as XmlElement;
			if (xmlElement == null || !xmlElement.HasAttribute("publicKeyToken"))
			{
				throw new CryptographicException(-2146762749);
			}
			return xmlElement.GetAttribute("publicKeyToken");
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005E8C File Offset: 0x0000408C
		private static byte[] ComputeHashFromManifest(XmlDocument manifestDom, bool useSha256)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			using (TextReader textReader = new StringReader(manifestDom.OuterXml))
			{
				XmlReader reader = XmlReader.Create(textReader, new XmlReaderSettings
				{
					DtdProcessing = DtdProcessing.Parse
				}, manifestDom.BaseURI);
				xmlDocument.Load(reader);
			}
			XmlDsigExcC14NTransform xmlDsigExcC14NTransform = new XmlDsigExcC14NTransform();
			xmlDsigExcC14NTransform.LoadInput(xmlDocument);
			if (useSha256)
			{
				using (SHA256CryptoServiceProvider sha256CryptoServiceProvider = new SHA256CryptoServiceProvider())
				{
					byte[] array = sha256CryptoServiceProvider.ComputeHash(xmlDsigExcC14NTransform.GetOutput() as MemoryStream);
					if (array == null)
					{
						throw new CryptographicException(-2146869232);
					}
					return array;
				}
			}
			byte[] result;
			using (SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider())
			{
				byte[] array2 = sha1CryptoServiceProvider.ComputeHash(xmlDsigExcC14NTransform.GetOutput() as MemoryStream);
				if (array2 == null)
				{
					throw new CryptographicException(-2146869232);
				}
				result = array2;
			}
			return result;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005F94 File Offset: 0x00004194
		private static XmlDocument CreateLicenseDom(CmiManifestSigner2 signer, XmlElement principal, byte[] hash)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.LoadXml("<r:license xmlns:r=\"urn:mpeg:mpeg21:2003:01-REL-R-NS\" xmlns:as=\"http://schemas.microsoft.com/windows/pki/2005/Authenticode\"><r:grant><as:ManifestInformation><as:assemblyIdentity /></as:ManifestInformation><as:SignedBy/><as:AuthenticodePublisher><as:X509SubjectName>CN=dummy</as:X509SubjectName></as:AuthenticodePublisher></r:grant><r:issuer></r:issuer></r:license>");
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			xmlNamespaceManager.AddNamespace("r", "urn:mpeg:mpeg21:2003:01-REL-R-NS");
			xmlNamespaceManager.AddNamespace("as", "http://schemas.microsoft.com/windows/pki/2005/Authenticode");
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(xmlDocument, "r:license/r:grant/as:ManifestInformation/as:assemblyIdentity", xmlNamespaceManager) as XmlElement;
			xmlElement.RemoveAllAttributes();
			foreach (object obj in principal.Attributes)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)obj;
				xmlElement.SetAttribute(xmlAttribute.Name, xmlAttribute.Value);
			}
			XmlElement xmlElement2 = SignedCmiManifest2.GetSingleNode(xmlDocument, "r:license/r:grant/as:ManifestInformation", xmlNamespaceManager) as XmlElement;
			xmlElement2.SetAttribute("Hash", (hash.Length == 0) ? "" : SignedCmiManifest2.BytesToHexString(hash, 0, hash.Length));
			xmlElement2.SetAttribute("Description", (signer.Description == null) ? "" : signer.Description);
			xmlElement2.SetAttribute("Url", (signer.DescriptionUrl == null) ? "" : signer.DescriptionUrl);
			XmlElement xmlElement3 = SignedCmiManifest2.GetSingleNode(xmlDocument, "r:license/r:grant/as:AuthenticodePublisher/as:X509SubjectName", xmlNamespaceManager) as XmlElement;
			xmlElement3.InnerText = signer.Certificate.SubjectName.Name;
			return xmlDocument;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000060FC File Offset: 0x000042FC
		private static void AuthenticodeSignLicenseDom(XmlDocument licenseDom, CmiManifestSigner2 signer, string timeStampUrl, bool useSha256)
		{
			using (RSA rsaprivateKey = CngLightup.GetRSAPrivateKey(signer.Certificate))
			{
				if (rsaprivateKey == null)
				{
					throw new NotSupportedException();
				}
				ManifestSignedXml2 manifestSignedXml = new ManifestSignedXml2(licenseDom);
				manifestSignedXml.SigningKey = rsaprivateKey;
				manifestSignedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
				if (signer.UseSha256)
				{
					manifestSignedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha256";
				}
				manifestSignedXml.KeyInfo.AddClause(new RSAKeyValue(rsaprivateKey));
				manifestSignedXml.KeyInfo.AddClause(new KeyInfoX509Data(signer.Certificate, signer.IncludeOption));
				Reference reference = new Reference();
				reference.Uri = "";
				if (signer.UseSha256)
				{
					reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha256";
				}
				reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
				reference.AddTransform(new XmlDsigExcC14NTransform());
				manifestSignedXml.AddReference(reference);
				manifestSignedXml.ComputeSignature();
				XmlElement xml = manifestSignedXml.GetXml();
				xml.SetAttribute("Id", "AuthenticodeSignature");
				XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(licenseDom.NameTable);
				xmlNamespaceManager.AddNamespace("r", "urn:mpeg:mpeg21:2003:01-REL-R-NS");
				XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(licenseDom, "r:license/r:issuer", xmlNamespaceManager) as XmlElement;
				xmlElement.AppendChild(licenseDom.ImportNode(xml, true));
				if (timeStampUrl != null && timeStampUrl.Length != 0)
				{
					SignedCmiManifest2.TimestampSignedLicenseDom(licenseDom, timeStampUrl, useSha256);
				}
				licenseDom.DocumentElement.ParentNode.InnerXml = "<msrel:RelData xmlns:msrel=\"http://schemas.microsoft.com/windows/rel/2005/reldata\">" + licenseDom.OuterXml + "</msrel:RelData>";
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006284 File Offset: 0x00004484
		private unsafe static string ObtainRFC3161Timestamp(string timeStampUrl, string signatureValue, bool useSha256)
		{
			byte[] array = Convert.FromBase64String(signatureValue);
			string result = string.Empty;
			string pszHashId = useSha256 ? "2.16.840.1.101.3.4.2.1" : "1.3.14.3.2.26";
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			IntPtr zero3 = IntPtr.Zero;
			try
			{
				byte[] array2 = new byte[24];
				using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
				{
					randomNumberGenerator.GetBytes(array2);
				}
				Win32.CRYPT_TIMESTAMP_PARA crypt_TIMESTAMP_PARA = new Win32.CRYPT_TIMESTAMP_PARA
				{
					fRequestCerts = true,
					pszTSAPolicyId = IntPtr.Zero
				};
				try
				{
					fixed (byte* ptr = array2)
					{
						crypt_TIMESTAMP_PARA.Nonce.cbData = (uint)array2.Length;
						crypt_TIMESTAMP_PARA.Nonce.pbData = (IntPtr)((void*)ptr);
						if (!Win32.CryptRetrieveTimeStamp(timeStampUrl, 0U, 60000, pszHashId, ref crypt_TIMESTAMP_PARA, array, array.Length, ref zero, ref zero2, ref zero3))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
					}
				}
				finally
				{
					byte* ptr = null;
				}
				Win32.CRYPT_TIMESTAMP_CONTEXT crypt_TIMESTAMP_CONTEXT = (Win32.CRYPT_TIMESTAMP_CONTEXT)Marshal.PtrToStructure(zero, typeof(Win32.CRYPT_TIMESTAMP_CONTEXT));
				byte[] array3 = new byte[crypt_TIMESTAMP_CONTEXT.cbEncoded];
				Marshal.Copy(crypt_TIMESTAMP_CONTEXT.pbEncoded, array3, 0, (int)crypt_TIMESTAMP_CONTEXT.cbEncoded);
				result = Convert.ToBase64String(array3);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Win32.CryptMemFree(zero);
				}
				if (zero2 != IntPtr.Zero)
				{
					Win32.CertFreeCertificateContext(zero2);
				}
				if (zero3 != IntPtr.Zero)
				{
					Win32.CertCloseStore(zero3, 0);
				}
			}
			return result;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00006428 File Offset: 0x00004628
		private unsafe static void TimestampSignedLicenseDom(XmlDocument licenseDom, string timeStampUrl, bool useSha256)
		{
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(licenseDom.NameTable);
			xmlNamespaceManager.AddNamespace("r", "urn:mpeg:mpeg21:2003:01-REL-R-NS");
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			xmlNamespaceManager.AddNamespace("as", "http://schemas.microsoft.com/windows/pki/2005/Authenticode");
			string innerText = string.Empty;
			try
			{
				XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(licenseDom, "r:license/r:issuer/ds:Signature/ds:SignatureValue", xmlNamespaceManager) as XmlElement;
				string innerText2 = xmlElement.InnerText;
				innerText = SignedCmiManifest2.ObtainRFC3161Timestamp(timeStampUrl, innerText2, useSha256);
			}
			catch
			{
				Win32.CRYPT_DATA_BLOB crypt_DATA_BLOB = default(Win32.CRYPT_DATA_BLOB);
				byte[] bytes = Encoding.UTF8.GetBytes(licenseDom.OuterXml);
				fixed (byte* ptr = bytes)
				{
					Win32.CRYPT_DATA_BLOB crypt_DATA_BLOB2 = default(Win32.CRYPT_DATA_BLOB);
					IntPtr pbData = new IntPtr((void*)ptr);
					crypt_DATA_BLOB2.cbData = (uint)bytes.Length;
					crypt_DATA_BLOB2.pbData = pbData;
					int num = Win32.CertTimestampAuthenticodeLicense(ref crypt_DATA_BLOB2, timeStampUrl, ref crypt_DATA_BLOB);
					if (num != 0)
					{
						throw new CryptographicException(num);
					}
				}
				byte[] array = new byte[crypt_DATA_BLOB.cbData];
				Marshal.Copy(crypt_DATA_BLOB.pbData, array, 0, array.Length);
				Win32.HeapFree(Win32.GetProcessHeap(), 0U, crypt_DATA_BLOB.pbData);
				innerText = Encoding.UTF8.GetString(array);
			}
			XmlElement xmlElement2 = licenseDom.CreateElement("as", "Timestamp", "http://schemas.microsoft.com/windows/pki/2005/Authenticode");
			xmlElement2.InnerText = innerText;
			XmlElement xmlElement3 = licenseDom.CreateElement("Object", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement3.AppendChild(xmlElement2);
			XmlElement xmlElement4 = SignedCmiManifest2.GetSingleNode(licenseDom, "r:license/r:issuer/ds:Signature", xmlNamespaceManager) as XmlElement;
			xmlElement4.AppendChild(xmlElement3);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000065C4 File Offset: 0x000047C4
		private static void StrongNameSignManifestDom(XmlDocument manifestDom, XmlDocument licenseDom, CmiManifestSigner2 signer, bool useSha256)
		{
			RSA rsa = signer.StrongNameKey as RSA;
			if (rsa == null)
			{
				throw new NotSupportedException();
			}
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(manifestDom.NameTable);
			xmlNamespaceManager.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");
			XmlElement xmlElement = SignedCmiManifest2.GetSingleNode(manifestDom, "asm:assembly", xmlNamespaceManager) as XmlElement;
			if (xmlElement == null)
			{
				throw new CryptographicException(-2146762749);
			}
			if (!(signer.StrongNameKey is RSA))
			{
				throw new NotSupportedException();
			}
			ManifestSignedXml2 manifestSignedXml = new ManifestSignedXml2(xmlElement);
			if (signer.StrongNameKey is RSACryptoServiceProvider)
			{
				manifestSignedXml.SigningKey = SignedCmiManifest2.GetFixedRSACryptoServiceProvider(signer.StrongNameKey as RSACryptoServiceProvider, useSha256);
			}
			else
			{
				manifestSignedXml.SigningKey = signer.StrongNameKey;
			}
			manifestSignedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
			if (signer.UseSha256)
			{
				manifestSignedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha256";
			}
			manifestSignedXml.KeyInfo.AddClause(new RSAKeyValue(rsa));
			if (licenseDom != null)
			{
				manifestSignedXml.KeyInfo.AddClause(new KeyInfoNode(licenseDom.DocumentElement));
			}
			manifestSignedXml.KeyInfo.Id = "StrongNameKeyInfo";
			Reference reference = new Reference();
			reference.Uri = "";
			if (signer.UseSha256)
			{
				reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha256";
			}
			reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
			reference.AddTransform(new XmlDsigExcC14NTransform());
			manifestSignedXml.AddReference(reference);
			manifestSignedXml.ComputeSignature();
			XmlElement xml = manifestSignedXml.GetXml();
			xml.SetAttribute("Id", "StrongNameSignature");
			xmlElement.AppendChild(xml);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00006740 File Offset: 0x00004940
		private static string BytesToHexString(byte[] array, int start, int end)
		{
			string result = null;
			if (array != null)
			{
				char[] array2 = new char[(end - start) * 2];
				int num = end;
				int num2 = 0;
				while (num-- > start)
				{
					int num3 = (array[num] & 240) >> 4;
					array2[num2++] = SignedCmiManifest2.hexValues[num3];
					num3 = (int)(array[num] & 15);
					array2[num2++] = SignedCmiManifest2.hexValues[num3];
				}
				result = new string(array2);
			}
			return result;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000067A8 File Offset: 0x000049A8
		private static byte[] HexStringToBytes(string hexString)
		{
			uint num = (uint)(hexString.Length / 2);
			byte[] array = new byte[num];
			int num2 = hexString.Length - 2;
			int num3 = 0;
			while ((long)num3 < (long)((ulong)num))
			{
				array[num3] = (byte)((int)SignedCmiManifest2.HexToByte(hexString[num2]) << 4 | (int)SignedCmiManifest2.HexToByte(hexString[num2 + 1]));
				num2 -= 2;
				num3++;
			}
			return array;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003BB6 File Offset: 0x00001DB6
		private static byte HexToByte(char val)
		{
			if (val <= '9' && val >= '0')
			{
				return (byte)(val - '0');
			}
			if (val >= 'a' && val <= 'f')
			{
				return (byte)(val - 'a' + '\n');
			}
			if (val >= 'A' && val <= 'F')
			{
				return (byte)(val - 'A' + '\n');
			}
			return byte.MaxValue;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00006804 File Offset: 0x00004A04
		private static XmlNode GetSingleNode(XmlNode parentNode, string xPath, XmlNamespaceManager namespaceManager = null)
		{
			XmlNodeList xmlNodeList = (namespaceManager != null) ? parentNode.SelectNodes(xPath, namespaceManager) : parentNode.SelectNodes(xPath);
			if (xmlNodeList == null)
			{
				return null;
			}
			if (xmlNodeList.Count > 1)
			{
				throw new CryptographicException(-2146869247);
			}
			return xmlNodeList[0];
		}

		// Token: 0x040000E2 RID: 226
		private XmlDocument m_manifestDom;

		// Token: 0x040000E3 RID: 227
		private CmiStrongNameSignerInfo m_strongNameSignerInfo;

		// Token: 0x040000E4 RID: 228
		private CmiAuthenticodeSignerInfo m_authenticodeSignerInfo;

		// Token: 0x040000E5 RID: 229
		private bool m_useSha256;

		// Token: 0x040000E6 RID: 230
		private const string Sha256SignatureMethodUri = "http://www.w3.org/2000/09/xmldsig#rsa-sha256";

		// Token: 0x040000E7 RID: 231
		private const string Sha256DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha256";

		// Token: 0x040000E8 RID: 232
		private const string wintrustPolicyFlagsRegPath = "Software\\Microsoft\\Windows\\CurrentVersion\\WinTrust\\Trust Providers\\Software Publishing";

		// Token: 0x040000E9 RID: 233
		private const string wintrustPolicyFlagsRegName = "State";

		// Token: 0x040000EA RID: 234
		private const string AssemblyNamespaceUri = "urn:schemas-microsoft-com:asm.v1";

		// Token: 0x040000EB RID: 235
		private const string AssemblyV2NamespaceUri = "urn:schemas-microsoft-com:asm.v2";

		// Token: 0x040000EC RID: 236
		private const string MSRelNamespaceUri = "http://schemas.microsoft.com/windows/rel/2005/reldata";

		// Token: 0x040000ED RID: 237
		private const string LicenseNamespaceUri = "urn:mpeg:mpeg21:2003:01-REL-R-NS";

		// Token: 0x040000EE RID: 238
		private const string AuthenticodeNamespaceUri = "http://schemas.microsoft.com/windows/pki/2005/Authenticode";

		// Token: 0x040000EF RID: 239
		private const string licenseTemplate = "<r:license xmlns:r=\"urn:mpeg:mpeg21:2003:01-REL-R-NS\" xmlns:as=\"http://schemas.microsoft.com/windows/pki/2005/Authenticode\"><r:grant><as:ManifestInformation><as:assemblyIdentity /></as:ManifestInformation><as:SignedBy/><as:AuthenticodePublisher><as:X509SubjectName>CN=dummy</as:X509SubjectName></as:AuthenticodePublisher></r:grant><r:issuer></r:issuer></r:license>";

		// Token: 0x040000F0 RID: 240
		private static readonly char[] hexValues = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f'
		};
	}
}
