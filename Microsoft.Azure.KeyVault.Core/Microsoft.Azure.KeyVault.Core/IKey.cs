using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.KeyVault.Core
{
	// Token: 0x02000002 RID: 2
	public interface IKey : IDisposable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		string DefaultEncryptionAlgorithm { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2
		string DefaultKeyWrapAlgorithm { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3
		string DefaultSignatureAlgorithm { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000004 RID: 4
		string Kid { get; }

		// Token: 0x06000005 RID: 5
		Task<byte[]> DecryptAsync(byte[] ciphertext, byte[] iv, byte[] authenticationData, byte[] authenticationTag, string algorithm, CancellationToken token);

		// Token: 0x06000006 RID: 6
		Task<Tuple<byte[], byte[], string>> EncryptAsync(byte[] plaintext, byte[] iv, byte[] authenticationData, string algorithm, CancellationToken token);

		// Token: 0x06000007 RID: 7
		Task<Tuple<byte[], string>> WrapKeyAsync(byte[] key, string algorithm, CancellationToken token);

		// Token: 0x06000008 RID: 8
		Task<byte[]> UnwrapKeyAsync(byte[] encryptedKey, string algorithm, CancellationToken token);

		// Token: 0x06000009 RID: 9
		Task<Tuple<byte[], string>> SignAsync(byte[] digest, string algorithm, CancellationToken token);

		// Token: 0x0600000A RID: 10
		Task<bool> VerifyAsync(byte[] digest, byte[] signature, string algorithm, CancellationToken token);
	}
}
