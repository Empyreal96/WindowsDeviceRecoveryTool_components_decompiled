using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x0200015D RID: 349
	internal class XmlElementValue<TValue> : XmlElementValue
	{
		// Token: 0x060006E0 RID: 1760 RVA: 0x00011867 File Offset: 0x0000FA67
		internal XmlElementValue(string name, CsdlLocation location, TValue newValue) : base(name, location)
		{
			this.value = newValue;
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00011878 File Offset: 0x0000FA78
		internal override bool IsText
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0001187B File Offset: 0x0000FA7B
		internal override bool IsUsed
		{
			get
			{
				return this.isUsed;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x00011883 File Offset: 0x0000FA83
		internal override object UntypedValue
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00011890 File Offset: 0x0000FA90
		internal TValue Value
		{
			get
			{
				this.isUsed = true;
				return this.value;
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001189F File Offset: 0x0000FA9F
		internal override T ValueAs<T>()
		{
			return this.Value as T;
		}

		// Token: 0x0400038E RID: 910
		private readonly TValue value;

		// Token: 0x0400038F RID: 911
		private bool isUsed;
	}
}
