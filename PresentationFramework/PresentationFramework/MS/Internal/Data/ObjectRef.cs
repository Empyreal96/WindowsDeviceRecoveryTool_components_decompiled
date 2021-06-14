using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000733 RID: 1843
	internal abstract class ObjectRef
	{
		// Token: 0x060075F8 RID: 30200 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual object GetObject(DependencyObject d, ObjectRefArgs args)
		{
			return null;
		}

		// Token: 0x060075F9 RID: 30201 RVA: 0x0021A60D File Offset: 0x0021880D
		internal virtual object GetDataObject(DependencyObject d, ObjectRefArgs args)
		{
			return this.GetObject(d, args);
		}

		// Token: 0x060075FA RID: 30202 RVA: 0x0021A617 File Offset: 0x00218817
		internal bool TreeContextIsRequired(DependencyObject target)
		{
			return this.ProtectedTreeContextIsRequired(target);
		}

		// Token: 0x060075FB RID: 30203 RVA: 0x0000B02A File Offset: 0x0000922A
		protected virtual bool ProtectedTreeContextIsRequired(DependencyObject target)
		{
			return false;
		}

		// Token: 0x17001C19 RID: 7193
		// (get) Token: 0x060075FC RID: 30204 RVA: 0x0021A620 File Offset: 0x00218820
		internal bool UsesMentor
		{
			get
			{
				return this.ProtectedUsesMentor;
			}
		}

		// Token: 0x17001C1A RID: 7194
		// (get) Token: 0x060075FD RID: 30205 RVA: 0x00016748 File Offset: 0x00014948
		protected virtual bool ProtectedUsesMentor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060075FE RID: 30206
		internal abstract string Identify();
	}
}
