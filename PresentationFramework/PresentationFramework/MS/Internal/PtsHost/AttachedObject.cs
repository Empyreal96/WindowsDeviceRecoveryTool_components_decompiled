using System;
using System.Windows;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000619 RID: 1561
	internal class AttachedObject : EmbeddedObject
	{
		// Token: 0x060067C3 RID: 26563 RVA: 0x001D1769 File Offset: 0x001CF969
		internal AttachedObject(int dcp, BaseParagraph para) : base(dcp)
		{
			this.Para = para;
		}

		// Token: 0x060067C4 RID: 26564 RVA: 0x001D1779 File Offset: 0x001CF979
		internal override void Dispose()
		{
			this.Para.Dispose();
			this.Para = null;
			base.Dispose();
		}

		// Token: 0x060067C5 RID: 26565 RVA: 0x001D1794 File Offset: 0x001CF994
		internal override void Update(EmbeddedObject newObject)
		{
			AttachedObject attachedObject = newObject as AttachedObject;
			ErrorHandler.Assert(attachedObject != null, ErrorHandler.EmbeddedObjectTypeMismatch);
			ErrorHandler.Assert(attachedObject.Element.Equals(this.Element), ErrorHandler.EmbeddedObjectOwnerMismatch);
			this.Dcp = attachedObject.Dcp;
			this.Para.SetUpdateInfo(PTS.FSKCHANGE.fskchInside, false);
		}

		// Token: 0x17001919 RID: 6425
		// (get) Token: 0x060067C6 RID: 26566 RVA: 0x001D17EA File Offset: 0x001CF9EA
		internal override DependencyObject Element
		{
			get
			{
				return this.Para.Element;
			}
		}

		// Token: 0x04003388 RID: 13192
		internal BaseParagraph Para;
	}
}
