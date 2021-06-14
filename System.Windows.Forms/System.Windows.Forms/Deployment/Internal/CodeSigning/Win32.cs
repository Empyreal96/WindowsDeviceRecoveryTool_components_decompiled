using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x0200000A RID: 10
	internal static class Win32
	{
		// Token: 0x06000017 RID: 23
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetProcessHeap();

		// Token: 0x06000018 RID: 24
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool HeapFree([In] IntPtr hHeap, [In] uint dwFlags, [In] IntPtr lpMem);

		// Token: 0x06000019 RID: 25
		[DllImport("clr.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertTimestampAuthenticodeLicense([In] ref Win32.CRYPT_DATA_BLOB pSignedLicenseBlob, [In] string pwszTimestampURI, [In] [Out] ref Win32.CRYPT_DATA_BLOB pTimestampSignatureBlob);

		// Token: 0x0600001A RID: 26
		[DllImport("clr.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertVerifyAuthenticodeLicense([In] ref Win32.CRYPT_DATA_BLOB pLicenseBlob, [In] uint dwFlags, [In] [Out] ref Win32.AXL_SIGNER_INFO pSignerInfo, [In] [Out] ref Win32.AXL_TIMESTAMPER_INFO pTimestamperInfo);

		// Token: 0x0600001B RID: 27
		[DllImport("clr.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertFreeAuthenticodeSignerInfo([In] ref Win32.AXL_SIGNER_INFO pSignerInfo);

		// Token: 0x0600001C RID: 28
		[DllImport("clr.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertFreeAuthenticodeTimestamperInfo([In] ref Win32.AXL_TIMESTAMPER_INFO pTimestamperInfo);

		// Token: 0x0600001D RID: 29
		[DllImport("clr.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int _AxlGetIssuerPublicKeyHash([In] IntPtr pCertContext, [In] [Out] ref IntPtr ppwszPublicKeyHash);

		// Token: 0x0600001E RID: 30
		[DllImport("clr.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int _AxlRSAKeyValueToPublicKeyToken([In] ref Win32.CRYPT_DATA_BLOB pModulusBlob, [In] ref Win32.CRYPT_DATA_BLOB pExponentBlob, [In] [Out] ref IntPtr ppwszPublicKeyToken);

		// Token: 0x0600001F RID: 31
		[DllImport("clr.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int _AxlPublicKeyBlobToPublicKeyToken([In] ref Win32.CRYPT_DATA_BLOB pCspPublicKeyBlob, [In] [Out] ref IntPtr ppwszPublicKeyToken);

		// Token: 0x06000020 RID: 32
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("crypt32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptRetrieveTimeStamp([MarshalAs(UnmanagedType.LPWStr)] [In] string wszUrl, [In] uint dwRetrievalFlags, [In] int dwTimeout, [MarshalAs(UnmanagedType.LPStr)] [In] string pszHashId, [In] [Out] ref Win32.CRYPT_TIMESTAMP_PARA pPara, [In] byte[] pbData, [In] int cbData, [In] [Out] ref IntPtr ppTsContext, [In] [Out] ref IntPtr ppTsSigner, [In] [Out] ref IntPtr phStore);

		// Token: 0x06000021 RID: 33
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("crypt32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptVerifyTimeStampSignature([In] byte[] pbTSContentInfo, [In] int cbTSContentInfo, [In] byte[] pbData, [In] int cbData, [In] IntPtr hAdditionalStore, [In] [Out] ref IntPtr ppTsContext, [In] [Out] ref IntPtr ppTsSigner, [In] [Out] ref IntPtr phStore);

		// Token: 0x06000022 RID: 34
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("crypt32.dll", SetLastError = true)]
		internal static extern bool CertFreeCertificateContext(IntPtr pCertContext);

		// Token: 0x06000023 RID: 35
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("crypt32.dll", SetLastError = true)]
		internal static extern bool CertCloseStore(IntPtr pCertContext, int dwFlags);

		// Token: 0x06000024 RID: 36
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("crypt32.dll")]
		internal static extern void CryptMemFree(IntPtr pv);

		// Token: 0x0400008A RID: 138
		internal const string CRYPT32 = "crypt32.dll";

		// Token: 0x0400008B RID: 139
		internal const string KERNEL32 = "kernel32.dll";

		// Token: 0x0400008C RID: 140
		internal const string MSCORWKS = "clr.dll";

		// Token: 0x0400008D RID: 141
		internal const int S_OK = 0;

		// Token: 0x0400008E RID: 142
		internal const int NTE_BAD_HASH = -2146893822;

		// Token: 0x0400008F RID: 143
		internal const int NTE_BAD_KEY = -2146893821;

		// Token: 0x04000090 RID: 144
		internal const int TRUST_E_SYSTEM_ERROR = -2146869247;

		// Token: 0x04000091 RID: 145
		internal const int TRUST_E_NO_SIGNER_CERT = -2146869246;

		// Token: 0x04000092 RID: 146
		internal const int TRUST_E_COUNTER_SIGNER = -2146869245;

		// Token: 0x04000093 RID: 147
		internal const int TRUST_E_CERT_SIGNATURE = -2146869244;

		// Token: 0x04000094 RID: 148
		internal const int TRUST_E_TIME_STAMP = -2146869243;

		// Token: 0x04000095 RID: 149
		internal const int TRUST_E_BAD_DIGEST = -2146869232;

		// Token: 0x04000096 RID: 150
		internal const int TRUST_E_BASIC_CONSTRAINTS = -2146869223;

		// Token: 0x04000097 RID: 151
		internal const int TRUST_E_FINANCIAL_CRITERIA = -2146869218;

		// Token: 0x04000098 RID: 152
		internal const int TRUST_E_PROVIDER_UNKNOWN = -2146762751;

		// Token: 0x04000099 RID: 153
		internal const int TRUST_E_ACTION_UNKNOWN = -2146762750;

		// Token: 0x0400009A RID: 154
		internal const int TRUST_E_SUBJECT_FORM_UNKNOWN = -2146762749;

		// Token: 0x0400009B RID: 155
		internal const int TRUST_E_SUBJECT_NOT_TRUSTED = -2146762748;

		// Token: 0x0400009C RID: 156
		internal const int TRUST_E_NOSIGNATURE = -2146762496;

		// Token: 0x0400009D RID: 157
		internal const int CERT_E_UNTRUSTEDROOT = -2146762487;

		// Token: 0x0400009E RID: 158
		internal const int TRUST_E_FAIL = -2146762485;

		// Token: 0x0400009F RID: 159
		internal const int TRUST_E_EXPLICIT_DISTRUST = -2146762479;

		// Token: 0x040000A0 RID: 160
		internal const int CERT_E_CHAINING = -2146762486;

		// Token: 0x040000A1 RID: 161
		internal const int AXL_REVOCATION_NO_CHECK = 1;

		// Token: 0x040000A2 RID: 162
		internal const int AXL_REVOCATION_CHECK_END_CERT_ONLY = 2;

		// Token: 0x040000A3 RID: 163
		internal const int AXL_REVOCATION_CHECK_ENTIRE_CHAIN = 4;

		// Token: 0x040000A4 RID: 164
		internal const int AXL_URL_CACHE_ONLY_RETRIEVAL = 8;

		// Token: 0x040000A5 RID: 165
		internal const int AXL_LIFETIME_SIGNING = 16;

		// Token: 0x040000A6 RID: 166
		internal const int AXL_TRUST_MICROSOFT_ROOT_ONLY = 32;

		// Token: 0x040000A7 RID: 167
		internal const int WTPF_IGNOREREVOKATION = 512;

		// Token: 0x040000A8 RID: 168
		internal const string szOID_KP_LIFETIME_SIGNING = "1.3.6.1.4.1.311.10.3.13";

		// Token: 0x040000A9 RID: 169
		internal const string szOID_RSA_signingTime = "1.2.840.113549.1.9.5";

		// Token: 0x040000AA RID: 170
		internal const string szOID_OIWSEC_sha1 = "1.3.14.3.2.26";

		// Token: 0x040000AB RID: 171
		internal const string szOID_NIST_sha256 = "2.16.840.1.101.3.4.2.1";

		// Token: 0x040000AC RID: 172
		internal const string szOID_RSA_messageDigest = "1.2.840.113549.1.9.4";

		// Token: 0x040000AD RID: 173
		internal const string szOID_PKIX_KP_TIMESTAMP_SIGNING = "1.3.6.1.5.5.7.3.8";

		// Token: 0x0200050F RID: 1295
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_DATA_BLOB
		{
			// Token: 0x040036C1 RID: 14017
			internal uint cbData;

			// Token: 0x040036C2 RID: 14018
			internal IntPtr pbData;
		}

		// Token: 0x02000510 RID: 1296
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct AXL_SIGNER_INFO
		{
			// Token: 0x040036C3 RID: 14019
			internal uint cbSize;

			// Token: 0x040036C4 RID: 14020
			internal uint dwError;

			// Token: 0x040036C5 RID: 14021
			internal uint algHash;

			// Token: 0x040036C6 RID: 14022
			internal IntPtr pwszHash;

			// Token: 0x040036C7 RID: 14023
			internal IntPtr pwszDescription;

			// Token: 0x040036C8 RID: 14024
			internal IntPtr pwszDescriptionUrl;

			// Token: 0x040036C9 RID: 14025
			internal IntPtr pChainContext;
		}

		// Token: 0x02000511 RID: 1297
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct AXL_TIMESTAMPER_INFO
		{
			// Token: 0x040036CA RID: 14026
			internal uint cbSize;

			// Token: 0x040036CB RID: 14027
			internal uint dwError;

			// Token: 0x040036CC RID: 14028
			internal uint algHash;

			// Token: 0x040036CD RID: 14029
			internal System.Runtime.InteropServices.ComTypes.FILETIME ftTimestamp;

			// Token: 0x040036CE RID: 14030
			internal IntPtr pChainContext;
		}

		// Token: 0x02000512 RID: 1298
		internal struct CRYPT_TIMESTAMP_CONTEXT
		{
			// Token: 0x040036CF RID: 14031
			internal uint cbEncoded;

			// Token: 0x040036D0 RID: 14032
			internal IntPtr pbEncoded;

			// Token: 0x040036D1 RID: 14033
			internal IntPtr pTimeStamp;
		}

		// Token: 0x02000513 RID: 1299
		internal struct CRYPT_TIMESTAMP_INFO
		{
			// Token: 0x040036D2 RID: 14034
			internal int dwVersion;

			// Token: 0x040036D3 RID: 14035
			internal IntPtr pszTSAPolicyId;

			// Token: 0x040036D4 RID: 14036
			internal Win32.CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;

			// Token: 0x040036D5 RID: 14037
			internal Win32.CRYPTOAPI_BLOB HashedMessage;

			// Token: 0x040036D6 RID: 14038
			internal Win32.CRYPTOAPI_BLOB SerialNumber;

			// Token: 0x040036D7 RID: 14039
			internal System.Runtime.InteropServices.ComTypes.FILETIME ftTime;

			// Token: 0x040036D8 RID: 14040
			internal IntPtr pvAccuracy;

			// Token: 0x040036D9 RID: 14041
			[MarshalAs(UnmanagedType.Bool)]
			internal bool fOrdering;

			// Token: 0x040036DA RID: 14042
			internal Win32.CRYPTOAPI_BLOB Nonce;

			// Token: 0x040036DB RID: 14043
			internal Win32.CRYPTOAPI_BLOB Tsa;

			// Token: 0x040036DC RID: 14044
			internal int cExtension;

			// Token: 0x040036DD RID: 14045
			internal IntPtr rgExtension;
		}

		// Token: 0x02000514 RID: 1300
		internal struct CRYPT_ALGORITHM_IDENTIFIER
		{
			// Token: 0x040036DE RID: 14046
			internal IntPtr pszOid;

			// Token: 0x040036DF RID: 14047
			internal Win32.CRYPTOAPI_BLOB Parameters;
		}

		// Token: 0x02000515 RID: 1301
		internal struct CRYPTOAPI_BLOB
		{
			// Token: 0x040036E0 RID: 14048
			internal uint cbData;

			// Token: 0x040036E1 RID: 14049
			internal IntPtr pbData;
		}

		// Token: 0x02000516 RID: 1302
		internal struct CRYPT_TIMESTAMP_PARA
		{
			// Token: 0x040036E2 RID: 14050
			internal IntPtr pszTSAPolicyId;

			// Token: 0x040036E3 RID: 14051
			internal bool fRequestCerts;

			// Token: 0x040036E4 RID: 14052
			internal Win32.CRYPTOAPI_BLOB Nonce;

			// Token: 0x040036E5 RID: 14053
			internal int cExtension;

			// Token: 0x040036E6 RID: 14054
			internal IntPtr rgExtension;
		}
	}
}
