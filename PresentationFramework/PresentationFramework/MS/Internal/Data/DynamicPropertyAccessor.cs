using System;

namespace MS.Internal.Data
{
	// Token: 0x02000725 RID: 1829
	internal abstract class DynamicPropertyAccessor : DynamicObjectAccessor
	{
		// Token: 0x06007511 RID: 29969 RVA: 0x00217B61 File Offset: 0x00215D61
		protected DynamicPropertyAccessor(Type ownerType, string propertyName) : base(ownerType, propertyName)
		{
		}

		// Token: 0x06007512 RID: 29970
		public abstract object GetValue(object component);

		// Token: 0x06007513 RID: 29971
		public abstract void SetValue(object component, object value);
	}
}
