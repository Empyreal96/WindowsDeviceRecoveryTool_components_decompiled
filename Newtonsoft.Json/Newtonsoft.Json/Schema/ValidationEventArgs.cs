using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200009A RID: 154
	public class ValidationEventArgs : EventArgs
	{
		// Token: 0x060007EE RID: 2030 RVA: 0x0001E57B File Offset: 0x0001C77B
		internal ValidationEventArgs(JsonSchemaException ex)
		{
			ValidationUtils.ArgumentNotNull(ex, "ex");
			this._ex = ex;
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0001E595 File Offset: 0x0001C795
		public JsonSchemaException Exception
		{
			get
			{
				return this._ex;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x0001E59D File Offset: 0x0001C79D
		public string Path
		{
			get
			{
				return this._ex.Path;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0001E5AA File Offset: 0x0001C7AA
		public string Message
		{
			get
			{
				return this._ex.Message;
			}
		}

		// Token: 0x040002AE RID: 686
		private readonly JsonSchemaException _ex;
	}
}
