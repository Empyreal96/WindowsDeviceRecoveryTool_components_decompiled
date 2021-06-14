using System;
using System.Globalization;

namespace Microsoft.Data.Edm.Validation
{
	// Token: 0x020001ED RID: 493
	public class EdmError
	{
		// Token: 0x06000BD9 RID: 3033 RVA: 0x00022847 File Offset: 0x00020A47
		public EdmError(EdmLocation errorLocation, EdmErrorCode errorCode, string errorMessage)
		{
			this.ErrorLocation = errorLocation;
			this.ErrorCode = errorCode;
			this.ErrorMessage = errorMessage;
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x00022864 File Offset: 0x00020A64
		// (set) Token: 0x06000BDB RID: 3035 RVA: 0x0002286C File Offset: 0x00020A6C
		public EdmLocation ErrorLocation { get; private set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x00022875 File Offset: 0x00020A75
		// (set) Token: 0x06000BDD RID: 3037 RVA: 0x0002287D File Offset: 0x00020A7D
		public EdmErrorCode ErrorCode { get; private set; }

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00022886 File Offset: 0x00020A86
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x0002288E File Offset: 0x00020A8E
		public string ErrorMessage { get; private set; }

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00022898 File Offset: 0x00020A98
		public override string ToString()
		{
			if (this.ErrorLocation != null && !(this.ErrorLocation is ObjectLocation))
			{
				return string.Concat(new string[]
				{
					Convert.ToString(this.ErrorCode, CultureInfo.InvariantCulture),
					" : ",
					this.ErrorMessage,
					" : ",
					this.ErrorLocation.ToString()
				});
			}
			return Convert.ToString(this.ErrorCode, CultureInfo.InvariantCulture) + " : " + this.ErrorMessage;
		}
	}
}
