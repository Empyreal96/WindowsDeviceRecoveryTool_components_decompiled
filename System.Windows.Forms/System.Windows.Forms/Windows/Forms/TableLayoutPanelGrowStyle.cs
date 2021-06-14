using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how a <see cref="T:System.Windows.Forms.TableLayoutPanel" /> will gain additional rows or columns after its existing cells are full.</summary>
	// Token: 0x02000381 RID: 897
	public enum TableLayoutPanelGrowStyle
	{
		/// <summary>The <see cref="T:System.Windows.Forms.TableLayoutPanel" /> does not allow additional rows or columns after it is full.</summary>
		// Token: 0x0400226A RID: 8810
		FixedSize,
		/// <summary>The <see cref="T:System.Windows.Forms.TableLayoutPanel" /> gains additional rows after it is full.</summary>
		// Token: 0x0400226B RID: 8811
		AddRows,
		/// <summary>The <see cref="T:System.Windows.Forms.TableLayoutPanel" /> gains additional columns after it is full.</summary>
		// Token: 0x0400226C RID: 8812
		AddColumns
	}
}
