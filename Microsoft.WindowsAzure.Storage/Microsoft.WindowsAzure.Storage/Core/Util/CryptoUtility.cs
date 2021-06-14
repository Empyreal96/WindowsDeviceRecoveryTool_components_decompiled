using System;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000096 RID: 150
	internal static class CryptoUtility
	{
		// Token: 0x06000FF8 RID: 4088 RVA: 0x0003CB2C File Offset: 0x0003AD2C
		internal static string ComputeHmac256(byte[] key, string message)
		{
			string result;
			using (HashAlgorithm hashAlgorithm = new HMACSHA256(key))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(message);
				result = Convert.ToBase64String(hashAlgorithm.ComputeHash(bytes));
			}
			return result;
		}
	}
}
