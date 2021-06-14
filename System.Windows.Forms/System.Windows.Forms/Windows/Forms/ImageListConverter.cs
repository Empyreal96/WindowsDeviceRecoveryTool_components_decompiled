using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000285 RID: 645
	internal class ImageListConverter : ComponentConverter
	{
		// Token: 0x060026A1 RID: 9889 RVA: 0x000B6C67 File Offset: 0x000B4E67
		public ImageListConverter() : base(typeof(ImageList))
		{
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
