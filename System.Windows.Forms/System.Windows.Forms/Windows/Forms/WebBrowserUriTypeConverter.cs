using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000433 RID: 1075
	internal class WebBrowserUriTypeConverter : UriTypeConverter
	{
		// Token: 0x06004AF8 RID: 19192 RVA: 0x00135C50 File Offset: 0x00133E50
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			Uri uri = base.ConvertFrom(context, culture, value) as Uri;
			if (uri != null && !string.IsNullOrEmpty(uri.OriginalString) && !uri.IsAbsoluteUri)
			{
				try
				{
					uri = new Uri("http://" + uri.OriginalString.Trim());
				}
				catch (UriFormatException)
				{
				}
			}
			return uri;
		}
	}
}
