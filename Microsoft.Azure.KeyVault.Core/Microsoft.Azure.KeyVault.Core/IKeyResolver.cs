using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.KeyVault.Core
{
	// Token: 0x02000003 RID: 3
	public interface IKeyResolver
	{
		// Token: 0x0600000B RID: 11
		Task<IKey> ResolveKeyAsync(string kid, CancellationToken token);
	}
}
