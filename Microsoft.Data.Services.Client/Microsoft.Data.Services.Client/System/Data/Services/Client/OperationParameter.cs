using System;

namespace System.Data.Services.Client
{
	// Token: 0x0200002B RID: 43
	public abstract class OperationParameter
	{
		// Token: 0x0600014A RID: 330 RVA: 0x0000801A File Offset: 0x0000621A
		protected OperationParameter(string name, object value)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(Strings.Context_MissingOperationParameterName);
			}
			this.parameterName = name;
			this.parameterValue = value;
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00008043 File Offset: 0x00006243
		public string Name
		{
			get
			{
				return this.parameterName;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000804B File Offset: 0x0000624B
		public object Value
		{
			get
			{
				return this.parameterValue;
			}
		}

		// Token: 0x040001E0 RID: 480
		private string parameterName;

		// Token: 0x040001E1 RID: 481
		private object parameterValue;
	}
}
