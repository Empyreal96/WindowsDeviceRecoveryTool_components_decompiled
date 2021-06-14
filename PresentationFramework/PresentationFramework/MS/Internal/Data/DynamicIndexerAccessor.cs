using System;

namespace MS.Internal.Data
{
	// Token: 0x02000726 RID: 1830
	internal abstract class DynamicIndexerAccessor : DynamicObjectAccessor
	{
		// Token: 0x06007514 RID: 29972 RVA: 0x00217B61 File Offset: 0x00215D61
		protected DynamicIndexerAccessor(Type ownerType, string propertyName) : base(ownerType, propertyName)
		{
		}

		// Token: 0x06007515 RID: 29973
		public abstract object GetValue(object component, object[] args);

		// Token: 0x06007516 RID: 29974
		public abstract void SetValue(object component, object[] args, object value);
	}
}
