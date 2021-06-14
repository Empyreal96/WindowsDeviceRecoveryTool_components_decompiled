using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class ImageCommonException : Exception
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00004859 File Offset: 0x00002A59
		public ImageCommonException()
		{
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004861 File Offset: 0x00002A61
		public ImageCommonException(string message) : base(message)
		{
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000486A File Offset: 0x00002A6A
		public ImageCommonException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004874 File Offset: 0x00002A74
		protected ImageCommonException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004880 File Offset: 0x00002A80
		public override string ToString()
		{
			string text = this.Message;
			if (base.InnerException != null)
			{
				text += base.InnerException.ToString();
			}
			return text;
		}
	}
}
