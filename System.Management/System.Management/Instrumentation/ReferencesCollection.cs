using System;
using System.Collections.Specialized;

namespace System.Management.Instrumentation
{
	// Token: 0x020000B2 RID: 178
	internal class ReferencesCollection
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x00022705 File Offset: 0x00020905
		public StringCollection Namespaces
		{
			get
			{
				return this.namespaces;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x0002270D File Offset: 0x0002090D
		public StringCollection Assemblies
		{
			get
			{
				return this.assemblies;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00022715 File Offset: 0x00020915
		public CodeWriter UsingCode
		{
			get
			{
				return this.usingCode;
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00022720 File Offset: 0x00020920
		public void Add(Type type)
		{
			if (!this.namespaces.Contains(type.Namespace))
			{
				this.namespaces.Add(type.Namespace);
				this.usingCode.Line(string.Format("using {0};", type.Namespace));
			}
			if (!this.assemblies.Contains(type.Assembly.Location))
			{
				this.assemblies.Add(type.Assembly.Location);
			}
		}

		// Token: 0x040004EC RID: 1260
		private StringCollection namespaces = new StringCollection();

		// Token: 0x040004ED RID: 1261
		private StringCollection assemblies = new StringCollection();

		// Token: 0x040004EE RID: 1262
		private CodeWriter usingCode = new CodeWriter();
	}
}
