using System;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x02000156 RID: 342
	public sealed class ODataPrimitiveValue : ODataValue
	{
		// Token: 0x06000942 RID: 2370 RVA: 0x0001D2C7 File Offset: 0x0001B4C7
		public ODataPrimitiveValue(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(Strings.ODataPrimitiveValue_CannotCreateODataPrimitiveValueFromNull, null);
			}
			if (!EdmLibraryExtensions.IsPrimitiveType(value.GetType()))
			{
				throw new ODataException(Strings.ODataPrimitiveValue_CannotCreateODataPrimitiveValueFromUnsupportedValueType(value.GetType()));
			}
			this.Value = value;
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x0001D303 File Offset: 0x0001B503
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x0001D30B File Offset: 0x0001B50B
		public object Value { get; private set; }
	}
}
