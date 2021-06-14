using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x020001BD RID: 445
	internal sealed class FeedWithoutExpectedTypeValidator
	{
		// Token: 0x06000DCF RID: 3535 RVA: 0x0002FF48 File Offset: 0x0002E148
		internal FeedWithoutExpectedTypeValidator()
		{
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0002FF50 File Offset: 0x0002E150
		internal void ValidateEntry(IEdmEntityType entityType)
		{
			if (this.itemType == null)
			{
				this.itemType = entityType;
			}
			if (this.itemType.IsEquivalentTo(entityType))
			{
				return;
			}
			IEdmType commonBaseType = this.itemType.GetCommonBaseType(entityType);
			if (commonBaseType == null)
			{
				throw new ODataException(Strings.FeedWithoutExpectedTypeValidator_IncompatibleTypes(entityType.ODataFullName(), this.itemType.ODataFullName()));
			}
			this.itemType = (IEdmEntityType)commonBaseType;
		}

		// Token: 0x040004A8 RID: 1192
		private IEdmEntityType itemType;
	}
}
