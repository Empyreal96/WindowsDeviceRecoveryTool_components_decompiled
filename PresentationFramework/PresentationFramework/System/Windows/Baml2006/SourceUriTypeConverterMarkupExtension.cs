using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Navigation;

namespace System.Windows.Baml2006
{
	// Token: 0x02000177 RID: 375
	internal class SourceUriTypeConverterMarkupExtension : TypeConverterMarkupExtension
	{
		// Token: 0x060015EB RID: 5611 RVA: 0x0006B19C File Offset: 0x0006939C
		public SourceUriTypeConverterMarkupExtension(TypeConverter converter, object value, Assembly assemblyInfo) : base(converter, value)
		{
			this._assemblyInfo = assemblyInfo;
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x0006B1B0 File Offset: 0x000693B0
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			object obj = base.ProvideValue(serviceProvider);
			Uri uri = obj as Uri;
			if (uri != null)
			{
				Uri uri2 = BaseUriHelper.AppendAssemblyVersion(uri, this._assemblyInfo);
				if (uri2 != null)
				{
					return new ResourceDictionary.ResourceDictionarySourceUriWrapper(uri, uri2);
				}
			}
			return obj;
		}

		// Token: 0x0400127D RID: 4733
		private Assembly _assemblyInfo;
	}
}
