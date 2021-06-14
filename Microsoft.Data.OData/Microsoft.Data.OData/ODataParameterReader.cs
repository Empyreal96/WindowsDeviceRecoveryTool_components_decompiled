using System;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000150 RID: 336
	public abstract class ODataParameterReader
	{
		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000914 RID: 2324
		public abstract ODataParameterReaderState State { get; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000915 RID: 2325
		public abstract string Name { get; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000916 RID: 2326
		public abstract object Value { get; }

		// Token: 0x06000917 RID: 2327
		public abstract ODataCollectionReader CreateCollectionReader();

		// Token: 0x06000918 RID: 2328
		public abstract bool Read();

		// Token: 0x06000919 RID: 2329
		public abstract Task<bool> ReadAsync();
	}
}
