using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x02000121 RID: 289
	public abstract class EdmNamedElement : EdmElement, IEdmNamedElement, IEdmElement
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x0000DB38 File Offset: 0x0000BD38
		protected EdmNamedElement(string name)
		{
			EdmUtil.CheckArgumentNull<string>(name, "name");
			this.name = name;
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x0000DB53 File Offset: 0x0000BD53
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x04000204 RID: 516
		private readonly string name;
	}
}
