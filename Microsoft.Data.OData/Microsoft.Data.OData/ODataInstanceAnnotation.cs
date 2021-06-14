using System;
using System.Xml;
using Microsoft.Data.OData.JsonLight;

namespace Microsoft.Data.OData
{
	// Token: 0x02000130 RID: 304
	public sealed class ODataInstanceAnnotation : ODataAnnotatable
	{
		// Token: 0x060007FC RID: 2044 RVA: 0x0001A716 File Offset: 0x00018916
		public ODataInstanceAnnotation(string name, ODataValue value)
		{
			ODataInstanceAnnotation.ValidateName(name);
			ODataInstanceAnnotation.ValidateValue(value);
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0001A738 File Offset: 0x00018938
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x0001A740 File Offset: 0x00018940
		public string Name { get; private set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0001A749 File Offset: 0x00018949
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x0001A751 File Offset: 0x00018951
		public ODataValue Value { get; private set; }

		// Token: 0x06000801 RID: 2049 RVA: 0x0001A75C File Offset: 0x0001895C
		internal static void ValidateName(string name)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(name, "name");
			if (name.IndexOf('.') < 0 || name[0] == '.' || name[name.Length - 1] == '.')
			{
				throw new ArgumentException(Strings.ODataInstanceAnnotation_NeedPeriodInName(name));
			}
			if (ODataAnnotationNames.IsODataAnnotationName(name))
			{
				throw new ArgumentException(Strings.ODataInstanceAnnotation_ReservedNamesNotAllowed(name, "odata."));
			}
			try
			{
				XmlConvert.VerifyNCName(name);
			}
			catch (XmlException innerException)
			{
				throw new ArgumentException(Strings.ODataInstanceAnnotation_BadTermName(name), innerException);
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001A7E8 File Offset: 0x000189E8
		internal static void ValidateValue(ODataValue value)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataValue>(value, "value");
			if (value is ODataStreamReferenceValue)
			{
				throw new ArgumentException(Strings.ODataInstanceAnnotation_ValueCannotBeODataStreamReferenceValue, "value");
			}
		}
	}
}
