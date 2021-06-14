using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Validation
{
	// Token: 0x02000238 RID: 568
	public sealed class ValidationContext
	{
		// Token: 0x06000C93 RID: 3219 RVA: 0x000252A8 File Offset: 0x000234A8
		internal ValidationContext(IEdmModel model, Func<object, bool> isBad)
		{
			this.model = model;
			this.isBad = isBad;
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x000252C9 File Offset: 0x000234C9
		public IEdmModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x000252D1 File Offset: 0x000234D1
		internal IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x000252D9 File Offset: 0x000234D9
		public bool IsBad(IEdmElement element)
		{
			return this.isBad(element);
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x000252E7 File Offset: 0x000234E7
		public void AddError(EdmLocation location, EdmErrorCode errorCode, string errorMessage)
		{
			this.AddError(new EdmError(location, errorCode, errorMessage));
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x000252F7 File Offset: 0x000234F7
		public void AddError(EdmError error)
		{
			this.errors.Add(error);
		}

		// Token: 0x04000599 RID: 1433
		private readonly List<EdmError> errors = new List<EdmError>();

		// Token: 0x0400059A RID: 1434
		private readonly IEdmModel model;

		// Token: 0x0400059B RID: 1435
		private readonly Func<object, bool> isBad;
	}
}
