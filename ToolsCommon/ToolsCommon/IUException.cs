using System;
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200000D RID: 13
	public class IUException : Exception
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00005664 File Offset: 0x00003864
		public IUException()
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000566C File Offset: 0x0000386C
		public IUException(string message) : base(message)
		{
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005675 File Offset: 0x00003875
		public IUException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000567F File Offset: 0x0000387F
		public IUException(string message, params object[] args) : this(string.Format(message, args))
		{
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000568E File Offset: 0x0000388E
		public IUException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005698 File Offset: 0x00003898
		public IUException(Exception innerException, string message) : base(message, innerException)
		{
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000056A2 File Offset: 0x000038A2
		public IUException(Exception innerException, string message, params object[] args) : this(innerException, string.Format(message, args))
		{
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000056B4 File Offset: 0x000038B4
		public string MessageTrace
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (Exception ex = this; ex != null; ex = ex.InnerException)
				{
					if (!string.IsNullOrEmpty(ex.Message))
					{
						stringBuilder.AppendLine(ex.Message);
					}
				}
				return stringBuilder.ToString();
			}
		}
	}
}
