using System;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Provides a base class for collecting layout scheme characteristics.</summary>
	// Token: 0x0200043A RID: 1082
	public abstract class LayoutSettings
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LayoutSettings" /> class. </summary>
		// Token: 0x06004B4C RID: 19276 RVA: 0x000027DB File Offset: 0x000009DB
		protected LayoutSettings()
		{
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x00136949 File Offset: 0x00134B49
		internal LayoutSettings(IArrangedElement owner)
		{
			this._owner = owner;
		}

		/// <summary>Gets the current table layout engine.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> currently being used.</returns>
		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06004B4E RID: 19278 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public virtual LayoutEngine LayoutEngine
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06004B4F RID: 19279 RVA: 0x00136958 File Offset: 0x00134B58
		internal IArrangedElement Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x04002785 RID: 10117
		private IArrangedElement _owner;
	}
}
