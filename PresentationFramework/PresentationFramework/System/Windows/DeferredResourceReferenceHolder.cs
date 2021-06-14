using System;

namespace System.Windows
{
	// Token: 0x02000119 RID: 281
	internal class DeferredResourceReferenceHolder : DeferredResourceReference
	{
		// Token: 0x06000BB8 RID: 3000 RVA: 0x0002B124 File Offset: 0x00029324
		internal DeferredResourceReferenceHolder(object resourceKey, object value) : base(null, null)
		{
			this._keyOrValue = new object[]
			{
				resourceKey,
				value
			};
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0002B142 File Offset: 0x00029342
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			return this.Value;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0002B14C File Offset: 0x0002934C
		internal override Type GetValueType()
		{
			object value = this.Value;
			if (value == null)
			{
				return null;
			}
			return value.GetType();
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0002B16B File Offset: 0x0002936B
		internal override object Key
		{
			get
			{
				return ((object[])this._keyOrValue)[0];
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x0002B17A File Offset: 0x0002937A
		// (set) Token: 0x06000BBD RID: 3005 RVA: 0x0002B189 File Offset: 0x00029389
		internal override object Value
		{
			get
			{
				return ((object[])this._keyOrValue)[1];
			}
			set
			{
				((object[])this._keyOrValue)[1] = value;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0002B199 File Offset: 0x00029399
		internal override bool IsUnset
		{
			get
			{
				return this.Value == DependencyProperty.UnsetValue;
			}
		}
	}
}
