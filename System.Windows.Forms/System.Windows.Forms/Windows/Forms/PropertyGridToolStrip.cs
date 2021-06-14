using System;

namespace System.Windows.Forms
{
	// Token: 0x0200031D RID: 797
	internal class PropertyGridToolStrip : ToolStrip
	{
		// Token: 0x060031BC RID: 12732 RVA: 0x000E8EAA File Offset: 0x000E70AA
		public PropertyGridToolStrip(PropertyGrid parentPropertyGrid)
		{
			this._parentPropertyGrid = parentPropertyGrid;
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x060031BD RID: 12733 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool SupportsUiaProviders
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x000E8EB9 File Offset: 0x000E70B9
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new PropertyGridToolStripAccessibleObject(this, this._parentPropertyGrid);
		}

		// Token: 0x04001E1D RID: 7709
		private PropertyGrid _parentPropertyGrid;
	}
}
