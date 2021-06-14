using System;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x02000096 RID: 150
	public interface IEdmStringValue : IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600026F RID: 623
		string Value { get; }
	}
}
