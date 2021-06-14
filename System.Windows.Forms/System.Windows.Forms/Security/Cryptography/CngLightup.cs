using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography
{
	// Token: 0x0200050B RID: 1291
	internal static class CngLightup
	{
		// Token: 0x06005480 RID: 21632 RVA: 0x00162E18 File Offset: 0x00161018
		internal static RSA GetRSAPublicKey(X509Certificate2 cert)
		{
			if (CngLightup.s_getRsaPublicKey == null)
			{
				if (CngLightup.s_preferRsaCng.Value)
				{
					CngLightup.s_getRsaPublicKey = (CngLightup.BindCoreDelegate<RSA>("RSA", true) ?? CngLightup.BindGetCapiPublicKey<RSA, RSACryptoServiceProvider>("1.2.840.113549.1.1.1"));
				}
				else
				{
					CngLightup.s_getRsaPublicKey = CngLightup.BindGetCapiPublicKey<RSA, RSACryptoServiceProvider>("1.2.840.113549.1.1.1");
				}
			}
			return CngLightup.s_getRsaPublicKey(cert);
		}

		// Token: 0x06005481 RID: 21633 RVA: 0x00162E7C File Offset: 0x0016107C
		internal static RSA GetRSAPrivateKey(X509Certificate2 cert)
		{
			if (CngLightup.s_getRsaPrivateKey == null)
			{
				if (CngLightup.s_preferRsaCng.Value)
				{
					Func<X509Certificate2, RSA> func;
					if ((func = CngLightup.BindCoreDelegate<RSA>("RSA", false)) == null)
					{
						func = CngLightup.BindGetCapiPrivateKey<RSA>("1.2.840.113549.1.1.1", (CspParameters csp) => new RSACryptoServiceProvider(csp));
					}
					CngLightup.s_getRsaPrivateKey = func;
				}
				else
				{
					CngLightup.s_getRsaPrivateKey = CngLightup.BindGetCapiPrivateKey<RSA>("1.2.840.113549.1.1.1", (CspParameters csp) => new RSACryptoServiceProvider(csp));
				}
			}
			return CngLightup.s_getRsaPrivateKey(cert);
		}

		// Token: 0x06005482 RID: 21634 RVA: 0x00162F1C File Offset: 0x0016111C
		internal static DSA GetDSAPublicKey(X509Certificate2 cert)
		{
			if (CngLightup.s_getDsaPublicKey == null)
			{
				CngLightup.s_getDsaPublicKey = (CngLightup.BindCoreDelegate<DSA>("DSA", true) ?? CngLightup.BindGetCapiPublicKey<DSA, DSACryptoServiceProvider>("1.2.840.10040.4.1"));
			}
			return CngLightup.s_getDsaPublicKey(cert);
		}

		// Token: 0x06005483 RID: 21635 RVA: 0x00162F54 File Offset: 0x00161154
		internal static DSA GetDSAPrivateKey(X509Certificate2 cert)
		{
			if (CngLightup.s_getDsaPrivateKey == null)
			{
				Func<X509Certificate2, DSA> func;
				if ((func = CngLightup.BindCoreDelegate<DSA>("DSA", false)) == null)
				{
					func = CngLightup.BindGetCapiPrivateKey<DSA>("1.2.840.10040.4.1", (CspParameters csp) => new DSACryptoServiceProvider(csp));
				}
				CngLightup.s_getDsaPrivateKey = func;
			}
			return CngLightup.s_getDsaPrivateKey(cert);
		}

		// Token: 0x06005484 RID: 21636 RVA: 0x00162FB8 File Offset: 0x001611B8
		internal static ECDsa GetECDsaPublicKey(X509Certificate2 cert)
		{
			if (CngLightup.s_getECDsaPublicKey == null)
			{
				Func<X509Certificate2, ECDsa> func;
				if ((func = CngLightup.BindCoreDelegate<ECDsa>("ECDsa", true)) == null && (func = CngLightup.<>c.<>9__30_0) == null)
				{
					func = (CngLightup.<>c.<>9__30_0 = ((X509Certificate2 c) => null));
				}
				CngLightup.s_getECDsaPublicKey = func;
			}
			return CngLightup.s_getECDsaPublicKey(cert);
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x00163010 File Offset: 0x00161210
		internal static ECDsa GetECDsaPrivateKey(X509Certificate2 cert)
		{
			if (CngLightup.s_getECDsaPrivateKey == null)
			{
				Func<X509Certificate2, ECDsa> func;
				if ((func = CngLightup.BindCoreDelegate<ECDsa>("ECDsa", false)) == null && (func = CngLightup.<>c.<>9__31_0) == null)
				{
					func = (CngLightup.<>c.<>9__31_0 = ((X509Certificate2 c) => null));
				}
				CngLightup.s_getECDsaPrivateKey = func;
			}
			return CngLightup.s_getECDsaPrivateKey(cert);
		}

		// Token: 0x06005486 RID: 21638 RVA: 0x00163068 File Offset: 0x00161268
		internal static byte[] Pkcs1SignData(RSA rsa, byte[] data, string hashAlgorithmName)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = rsa as RSACryptoServiceProvider;
			if (rsacryptoServiceProvider != null)
			{
				return rsacryptoServiceProvider.SignData(data, hashAlgorithmName);
			}
			if (CngLightup.s_rsaPkcs1SignMethod == null)
			{
				Type[] types = new Type[]
				{
					typeof(byte[]),
					CngLightup.s_hashAlgorithmNameType,
					CngLightup.s_rsaSignaturePaddingType
				};
				MethodInfo method = typeof(RSA).GetMethod("SignData", BindingFlags.Instance | BindingFlags.Public, null, types, null);
				Type type = typeof(Func<, , , , >).MakeGenericType(new Type[]
				{
					typeof(RSA),
					typeof(byte[]),
					CngLightup.s_hashAlgorithmNameType,
					CngLightup.s_rsaSignaturePaddingType,
					typeof(byte[])
				});
				Delegate openDelegate = Delegate.CreateDelegate(type, method);
				CngLightup.s_rsaPkcs1SignMethod = delegate(RSA delegateRsa, byte[] delegateData, string delegateAlgorithm)
				{
					object obj = Activator.CreateInstance(CngLightup.s_hashAlgorithmNameType, new object[]
					{
						delegateAlgorithm
					});
					object[] args = new object[]
					{
						delegateRsa,
						delegateData,
						obj,
						CngLightup.s_pkcs1SignaturePadding
					};
					return (byte[])openDelegate.DynamicInvoke(args);
				};
			}
			return CngLightup.s_rsaPkcs1SignMethod(rsa, data, hashAlgorithmName);
		}

		// Token: 0x06005487 RID: 21639 RVA: 0x00163158 File Offset: 0x00161358
		internal static bool Pkcs1VerifyData(RSA rsa, byte[] data, byte[] signature, string hashAlgorithmName)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = rsa as RSACryptoServiceProvider;
			if (rsacryptoServiceProvider != null)
			{
				return rsacryptoServiceProvider.VerifyData(data, hashAlgorithmName, signature);
			}
			if (CngLightup.s_rsaPkcs1VerifyMethod == null)
			{
				Type[] types = new Type[]
				{
					typeof(byte[]),
					typeof(byte[]),
					CngLightup.s_hashAlgorithmNameType,
					CngLightup.s_rsaSignaturePaddingType
				};
				MethodInfo method = typeof(RSA).GetMethod("VerifyData", BindingFlags.Instance | BindingFlags.Public, null, types, null);
				Type type = typeof(Func<, , , , , >).MakeGenericType(new Type[]
				{
					typeof(RSA),
					typeof(byte[]),
					typeof(byte[]),
					CngLightup.s_hashAlgorithmNameType,
					CngLightup.s_rsaSignaturePaddingType,
					typeof(bool)
				});
				Delegate openDelegate = Delegate.CreateDelegate(type, method);
				CngLightup.s_rsaPkcs1VerifyMethod = delegate(RSA delegateRsa, byte[] delegateData, byte[] delegateSignature, string delegateAlgorithm)
				{
					object obj = Activator.CreateInstance(CngLightup.s_hashAlgorithmNameType, new object[]
					{
						delegateAlgorithm
					});
					object[] args = new object[]
					{
						delegateRsa,
						delegateData,
						delegateSignature,
						obj,
						CngLightup.s_pkcs1SignaturePadding
					};
					return (bool)openDelegate.DynamicInvoke(args);
				};
			}
			return CngLightup.s_rsaPkcs1VerifyMethod(rsa, data, signature, hashAlgorithmName);
		}

		// Token: 0x06005488 RID: 21640 RVA: 0x00163264 File Offset: 0x00161464
		internal static byte[] Pkcs1Encrypt(RSA rsa, byte[] data)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = rsa as RSACryptoServiceProvider;
			if (rsacryptoServiceProvider != null)
			{
				return rsacryptoServiceProvider.Encrypt(data, false);
			}
			if (CngLightup.s_rsaPkcs1EncryptMethod == null)
			{
				Delegate openDelegate = CngLightup.BindRsaCryptMethod("Encrypt");
				CngLightup.s_rsaPkcs1EncryptMethod = ((RSA delegateRsa, byte[] delegateData) => (byte[])openDelegate.DynamicInvoke(new object[]
				{
					delegateRsa,
					delegateData,
					CngLightup.s_pkcs1EncryptionPadding
				}));
			}
			return CngLightup.s_rsaPkcs1EncryptMethod(rsa, data);
		}

		// Token: 0x06005489 RID: 21641 RVA: 0x001632C4 File Offset: 0x001614C4
		internal static byte[] Pkcs1Decrypt(RSA rsa, byte[] data)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = rsa as RSACryptoServiceProvider;
			if (rsacryptoServiceProvider != null)
			{
				return rsacryptoServiceProvider.Decrypt(data, false);
			}
			if (CngLightup.s_rsaPkcs1DecryptMethod == null)
			{
				Delegate openDelegate = CngLightup.BindRsaCryptMethod("Decrypt");
				CngLightup.s_rsaPkcs1DecryptMethod = ((RSA delegateRsa, byte[] delegateData) => (byte[])openDelegate.DynamicInvoke(new object[]
				{
					delegateRsa,
					delegateData,
					CngLightup.s_pkcs1EncryptionPadding
				}));
			}
			return CngLightup.s_rsaPkcs1DecryptMethod(rsa, data);
		}

		// Token: 0x0600548A RID: 21642 RVA: 0x00163324 File Offset: 0x00161524
		internal static byte[] OaepSha1Encrypt(RSA rsa, byte[] data)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = rsa as RSACryptoServiceProvider;
			if (rsacryptoServiceProvider != null)
			{
				return rsacryptoServiceProvider.Encrypt(data, true);
			}
			if (CngLightup.s_rsaOaepSha1EncryptMethod == null)
			{
				Delegate openDelegate = CngLightup.BindRsaCryptMethod("Encrypt");
				CngLightup.s_rsaOaepSha1EncryptMethod = ((RSA delegateRsa, byte[] delegateData) => (byte[])openDelegate.DynamicInvoke(new object[]
				{
					delegateRsa,
					delegateData,
					CngLightup.s_oaepSha1EncryptionPadding
				}));
			}
			return CngLightup.s_rsaOaepSha1EncryptMethod(rsa, data);
		}

		// Token: 0x0600548B RID: 21643 RVA: 0x00163384 File Offset: 0x00161584
		internal static byte[] OaepSha1Decrypt(RSA rsa, byte[] data)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = rsa as RSACryptoServiceProvider;
			if (rsacryptoServiceProvider != null)
			{
				return rsacryptoServiceProvider.Decrypt(data, true);
			}
			if (CngLightup.s_rsaOaepSha1DecryptMethod == null)
			{
				Delegate openDelegate = CngLightup.BindRsaCryptMethod("Decrypt");
				CngLightup.s_rsaOaepSha1DecryptMethod = ((RSA delegateRsa, byte[] delegateData) => (byte[])openDelegate.DynamicInvoke(new object[]
				{
					delegateRsa,
					delegateData,
					CngLightup.s_oaepSha1EncryptionPadding
				}));
			}
			return CngLightup.s_rsaOaepSha1DecryptMethod(rsa, data);
		}

		// Token: 0x0600548C RID: 21644 RVA: 0x001633E4 File Offset: 0x001615E4
		private static Delegate BindRsaCryptMethod(string methodName)
		{
			Type[] types = new Type[]
			{
				typeof(byte[]),
				CngLightup.s_rsaEncryptionPaddingType
			};
			MethodInfo method = typeof(RSA).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public, null, types, null);
			Type type = typeof(Func<, , , >).MakeGenericType(new Type[]
			{
				typeof(RSA),
				typeof(byte[]),
				CngLightup.s_rsaEncryptionPaddingType,
				typeof(byte[])
			});
			return Delegate.CreateDelegate(type, method);
		}

		// Token: 0x0600548D RID: 21645 RVA: 0x00163470 File Offset: 0x00161670
		private static bool DetectRsaCngSupport()
		{
			Type systemCoreType = CngLightup.GetSystemCoreType("System.Security.Cryptography.RSACng", false);
			if (systemCoreType == null)
			{
				return false;
			}
			Type systemCoreType2 = CngLightup.GetSystemCoreType("System.Security.Cryptography.DSACng", false);
			if (systemCoreType2 == null)
			{
				return false;
			}
			Type[] types = new Type[]
			{
				typeof(byte[]),
				CngLightup.s_hashAlgorithmNameType
			};
			MethodInfo method = typeof(DSA).GetMethod("SignData", BindingFlags.Instance | BindingFlags.Public, null, types, null);
			return !(method == null);
		}

		// Token: 0x0600548E RID: 21646 RVA: 0x001634F0 File Offset: 0x001616F0
		private static Func<X509Certificate2, T> BindGetCapiPublicKey<T, TCryptoServiceProvider>(string algorithmOid) where T : AsymmetricAlgorithm where TCryptoServiceProvider : T, ICspAsymmetricAlgorithm, new()
		{
			return delegate(X509Certificate2 cert)
			{
				PublicKey publicKey = cert.PublicKey;
				if (publicKey.Oid.Value != algorithmOid)
				{
					return default(T);
				}
				AsymmetricAlgorithm key = publicKey.Key;
				ICspAsymmetricAlgorithm cspAsymmetricAlgorithm = (ICspAsymmetricAlgorithm)key;
				byte[] rawData = cspAsymmetricAlgorithm.ExportCspBlob(false);
				TCryptoServiceProvider tcryptoServiceProvider = Activator.CreateInstance<TCryptoServiceProvider>();
				tcryptoServiceProvider.ImportCspBlob(rawData);
				return (T)((object)tcryptoServiceProvider);
			};
		}

		// Token: 0x0600548F RID: 21647 RVA: 0x00163518 File Offset: 0x00161718
		private static Func<X509Certificate2, T> BindGetCapiPrivateKey<T>(string algorithmOid, Func<CspParameters, T> instanceFactory) where T : AsymmetricAlgorithm
		{
			return delegate(X509Certificate2 cert)
			{
				if (!cert.HasPrivateKey)
				{
					return default(T);
				}
				PublicKey publicKey = cert.PublicKey;
				if (publicKey.Oid.Value != algorithmOid)
				{
					return default(T);
				}
				AsymmetricAlgorithm privateKey = cert.PrivateKey;
				ICspAsymmetricAlgorithm cspAlgorithm = (ICspAsymmetricAlgorithm)privateKey;
				CspParameters arg = CngLightup.CopyCspParameters(cspAlgorithm);
				return instanceFactory(arg);
			};
		}

		// Token: 0x06005490 RID: 21648 RVA: 0x00163548 File Offset: 0x00161748
		private static Func<X509Certificate2, T> BindCoreDelegate<T>(string algorithmName, bool isPublic)
		{
			string namespaceQualifiedTypeName = "System.Security.Cryptography.X509Certificates." + algorithmName + "CertificateExtensions";
			Type systemCoreType = CngLightup.GetSystemCoreType(namespaceQualifiedTypeName, false);
			if (systemCoreType == null)
			{
				return null;
			}
			string name = "Get" + algorithmName + (isPublic ? "Public" : "Private") + "Key";
			MethodInfo method = systemCoreType.GetMethod(name, BindingFlags.Static | BindingFlags.Public, null, new Type[]
			{
				typeof(X509Certificate2)
			}, null);
			return (Func<X509Certificate2, T>)method.CreateDelegate(typeof(Func<X509Certificate2, T>));
		}

		// Token: 0x06005491 RID: 21649 RVA: 0x001635D0 File Offset: 0x001617D0
		private static CspParameters CopyCspParameters(ICspAsymmetricAlgorithm cspAlgorithm)
		{
			CspKeyContainerInfo cspKeyContainerInfo = cspAlgorithm.CspKeyContainerInfo;
			CspParameters cspParameters = new CspParameters(cspKeyContainerInfo.ProviderType, cspKeyContainerInfo.ProviderName, cspKeyContainerInfo.KeyContainerName)
			{
				Flags = CspProviderFlags.UseExistingKey,
				KeyNumber = (int)cspKeyContainerInfo.KeyNumber
			};
			if (cspKeyContainerInfo.MachineKeyStore)
			{
				cspParameters.Flags |= CspProviderFlags.UseMachineKeyStore;
			}
			return cspParameters;
		}

		// Token: 0x06005492 RID: 21650 RVA: 0x00163628 File Offset: 0x00161828
		private static Type GetSystemCoreType(string namespaceQualifiedTypeName, bool throwOnError = true)
		{
			Assembly assembly = typeof(CngKey).Assembly;
			return assembly.GetType(namespaceQualifiedTypeName, throwOnError);
		}

		// Token: 0x0400368A RID: 13962
		private const string DsaOid = "1.2.840.10040.4.1";

		// Token: 0x0400368B RID: 13963
		private const string RsaOid = "1.2.840.113549.1.1.1";

		// Token: 0x0400368C RID: 13964
		private const string HashAlgorithmNameTypeName = "System.Security.Cryptography.HashAlgorithmName";

		// Token: 0x0400368D RID: 13965
		private const string RSASignaturePaddingTypeName = "System.Security.Cryptography.RSASignaturePadding";

		// Token: 0x0400368E RID: 13966
		private const string RSAEncryptionPaddingTypeName = "System.Security.Cryptography.RSAEncryptionPadding";

		// Token: 0x0400368F RID: 13967
		private const string RSACngTypeName = "System.Security.Cryptography.RSACng";

		// Token: 0x04003690 RID: 13968
		private const string DSACngTypeName = "System.Security.Cryptography.DSACng";

		// Token: 0x04003691 RID: 13969
		private static readonly Type s_hashAlgorithmNameType = typeof(object).Assembly.GetType("System.Security.Cryptography.HashAlgorithmName", false);

		// Token: 0x04003692 RID: 13970
		private static readonly Type s_rsaSignaturePaddingType = typeof(object).Assembly.GetType("System.Security.Cryptography.RSASignaturePadding", false);

		// Token: 0x04003693 RID: 13971
		private static readonly Type s_rsaEncryptionPaddingType = typeof(object).Assembly.GetType("System.Security.Cryptography.RSAEncryptionPadding", false);

		// Token: 0x04003694 RID: 13972
		private static readonly object s_pkcs1SignaturePadding = (CngLightup.s_rsaSignaturePaddingType == null) ? null : CngLightup.s_rsaSignaturePaddingType.GetProperty("Pkcs1", BindingFlags.Static | BindingFlags.Public).GetValue(null);

		// Token: 0x04003695 RID: 13973
		private static readonly object s_pkcs1EncryptionPadding = (CngLightup.s_rsaEncryptionPaddingType == null) ? null : CngLightup.s_rsaEncryptionPaddingType.GetProperty("Pkcs1", BindingFlags.Static | BindingFlags.Public).GetValue(null);

		// Token: 0x04003696 RID: 13974
		private static readonly object s_oaepSha1EncryptionPadding = (CngLightup.s_rsaEncryptionPaddingType == null) ? null : CngLightup.s_rsaEncryptionPaddingType.GetProperty("OaepSHA1", BindingFlags.Static | BindingFlags.Public).GetValue(null);

		// Token: 0x04003697 RID: 13975
		private static readonly Lazy<bool> s_preferRsaCng = new Lazy<bool>(new Func<bool>(CngLightup.DetectRsaCngSupport));

		// Token: 0x04003698 RID: 13976
		private static volatile Func<X509Certificate2, DSA> s_getDsaPublicKey;

		// Token: 0x04003699 RID: 13977
		private static volatile Func<X509Certificate2, DSA> s_getDsaPrivateKey;

		// Token: 0x0400369A RID: 13978
		private static volatile Func<X509Certificate2, RSA> s_getRsaPublicKey;

		// Token: 0x0400369B RID: 13979
		private static volatile Func<X509Certificate2, RSA> s_getRsaPrivateKey;

		// Token: 0x0400369C RID: 13980
		private static volatile Func<RSA, byte[], string, byte[]> s_rsaPkcs1SignMethod;

		// Token: 0x0400369D RID: 13981
		private static volatile Func<RSA, byte[], byte[], string, bool> s_rsaPkcs1VerifyMethod;

		// Token: 0x0400369E RID: 13982
		private static volatile Func<RSA, byte[], byte[]> s_rsaPkcs1EncryptMethod;

		// Token: 0x0400369F RID: 13983
		private static volatile Func<RSA, byte[], byte[]> s_rsaPkcs1DecryptMethod;

		// Token: 0x040036A0 RID: 13984
		private static volatile Func<RSA, byte[], byte[]> s_rsaOaepSha1EncryptMethod;

		// Token: 0x040036A1 RID: 13985
		private static volatile Func<RSA, byte[], byte[]> s_rsaOaepSha1DecryptMethod;

		// Token: 0x040036A2 RID: 13986
		private static volatile Func<X509Certificate2, ECDsa> s_getECDsaPublicKey;

		// Token: 0x040036A3 RID: 13987
		private static volatile Func<X509Certificate2, ECDsa> s_getECDsaPrivateKey;
	}
}
