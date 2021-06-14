using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A1 RID: 161
	internal struct ResolverContractKey : IEquatable<ResolverContractKey>
	{
		// Token: 0x0600083C RID: 2108 RVA: 0x00020263 File Offset: 0x0001E463
		public ResolverContractKey(Type resolverType, Type contractType)
		{
			this._resolverType = resolverType;
			this._contractType = contractType;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00020273 File Offset: 0x0001E473
		public override int GetHashCode()
		{
			return this._resolverType.GetHashCode() ^ this._contractType.GetHashCode();
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0002028C File Offset: 0x0001E48C
		public override bool Equals(object obj)
		{
			return obj is ResolverContractKey && this.Equals((ResolverContractKey)obj);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x000202A4 File Offset: 0x0001E4A4
		public bool Equals(ResolverContractKey other)
		{
			return this._resolverType == other._resolverType && this._contractType == other._contractType;
		}

		// Token: 0x040002C1 RID: 705
		private readonly Type _resolverType;

		// Token: 0x040002C2 RID: 706
		private readonly Type _contractType;
	}
}
