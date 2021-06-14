using System;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Library.Values
{
	// Token: 0x020000D7 RID: 215
	public abstract class EdmValue : IEdmValue, IEdmElement, IEdmDelayedValue
	{
		// Token: 0x06000453 RID: 1107 RVA: 0x0000BE22 File Offset: 0x0000A022
		protected EdmValue(IEdmTypeReference type)
		{
			this.type = type;
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0000BE31 File Offset: 0x0000A031
		public IEdmTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000455 RID: 1109
		public abstract EdmValueKind ValueKind { get; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0000BE39 File Offset: 0x0000A039
		IEdmValue IEdmDelayedValue.Value
		{
			get
			{
				return this;
			}
		}

		// Token: 0x040001A9 RID: 425
		private readonly IEdmTypeReference type;
	}
}
