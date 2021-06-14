using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x02000086 RID: 134
	public interface IEdmPropertyConstructor : IEdmElement
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000226 RID: 550
		string Name { get; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000227 RID: 551
		IEdmExpression Value { get; }
	}
}
