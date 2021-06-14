using System;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x02000099 RID: 153
	public interface IEdmTimeValue : IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000279 RID: 633
		TimeSpan Value { get; }
	}
}
