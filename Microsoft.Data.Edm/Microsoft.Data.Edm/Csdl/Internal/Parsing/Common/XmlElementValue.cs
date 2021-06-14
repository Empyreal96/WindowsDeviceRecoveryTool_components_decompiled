using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x0200015B RID: 347
	internal abstract class XmlElementValue
	{
		// Token: 0x060006D2 RID: 1746 RVA: 0x000117F6 File Offset: 0x0000F9F6
		internal XmlElementValue(string elementName, CsdlLocation elementLocation)
		{
			this.Name = elementName;
			this.Location = elementLocation;
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0001180C File Offset: 0x0000FA0C
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x00011814 File Offset: 0x0000FA14
		internal string Name { get; private set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0001181D File Offset: 0x0000FA1D
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00011825 File Offset: 0x0000FA25
		internal CsdlLocation Location { get; private set; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060006D7 RID: 1751
		internal abstract object UntypedValue { get; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060006D8 RID: 1752
		internal abstract bool IsUsed { get; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0001182E File Offset: 0x0000FA2E
		internal virtual bool IsText
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x00011831 File Offset: 0x0000FA31
		internal virtual string TextValue
		{
			get
			{
				return this.ValueAs<string>();
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00011839 File Offset: 0x0000FA39
		internal virtual TValue ValueAs<TValue>() where TValue : class
		{
			return this.UntypedValue as TValue;
		}
	}
}
