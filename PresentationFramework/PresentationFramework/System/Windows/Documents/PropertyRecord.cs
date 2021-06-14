using System;

namespace System.Windows.Documents
{
	// Token: 0x020003A2 RID: 930
	internal struct PropertyRecord
	{
		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x06003288 RID: 12936 RVA: 0x000DD235 File Offset: 0x000DB435
		// (set) Token: 0x06003289 RID: 12937 RVA: 0x000DD23D File Offset: 0x000DB43D
		internal DependencyProperty Property
		{
			get
			{
				return this._property;
			}
			set
			{
				this._property = value;
			}
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x0600328A RID: 12938 RVA: 0x000DD246 File Offset: 0x000DB446
		// (set) Token: 0x0600328B RID: 12939 RVA: 0x000DD24E File Offset: 0x000DB44E
		internal object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x04001ED0 RID: 7888
		private DependencyProperty _property;

		// Token: 0x04001ED1 RID: 7889
		private object _value;
	}
}
