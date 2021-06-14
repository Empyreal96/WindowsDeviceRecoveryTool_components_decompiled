using System;

namespace System.Windows
{
	/// <summary>Represents the resource key for the <see cref="T:System.Windows.DataTemplate" /> class.</summary>
	// Token: 0x020000AA RID: 170
	public class DataTemplateKey : TemplateKey
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.DataTemplateKey" /> class.</summary>
		// Token: 0x0600038F RID: 911 RVA: 0x0000A323 File Offset: 0x00008523
		public DataTemplateKey() : base(TemplateKey.TemplateType.DataTemplate)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.DataTemplateKey" /> class with the specified type.</summary>
		/// <param name="dataType">The type for which this template is designed. This is either a <see cref="T:System.Type" /> (to indicate that the <see cref="T:System.Windows.DataTemplate" /> is used to display items of the given type), or a string (to indicate that the <see cref="T:System.Windows.DataTemplate" /> is used to display <see cref="T:System.Xml.XmlNode" /> elements with the given tag name).</param>
		// Token: 0x06000390 RID: 912 RVA: 0x0000A32C File Offset: 0x0000852C
		public DataTemplateKey(object dataType) : base(TemplateKey.TemplateType.DataTemplate, dataType)
		{
		}
	}
}
