using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see langword="Scroll" /> event.</summary>
	// Token: 0x02000349 RID: 841
	[ComVisible(true)]
	public class ScrollEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ScrollEventArgs" /> class using the given values for the <see cref="P:System.Windows.Forms.ScrollEventArgs.Type" /> and <see cref="P:System.Windows.Forms.ScrollEventArgs.NewValue" /> properties.</summary>
		/// <param name="type">One of the <see cref="T:System.Windows.Forms.ScrollEventType" /> values. </param>
		/// <param name="newValue">The new value for the scroll bar. </param>
		// Token: 0x060034E7 RID: 13543 RVA: 0x000F1B56 File Offset: 0x000EFD56
		public ScrollEventArgs(ScrollEventType type, int newValue)
		{
			this.type = type;
			this.newValue = newValue;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ScrollEventArgs" /> class using the given values for the <see cref="P:System.Windows.Forms.ScrollEventArgs.Type" />, <see cref="P:System.Windows.Forms.ScrollEventArgs.NewValue" />, and <see cref="P:System.Windows.Forms.ScrollEventArgs.ScrollOrientation" /> properties.</summary>
		/// <param name="type">One of the <see cref="T:System.Windows.Forms.ScrollEventType" /> values. </param>
		/// <param name="newValue">The new value for the scroll bar. </param>
		/// <param name="scroll">One of the <see cref="T:System.Windows.Forms.ScrollOrientation" /> values. </param>
		// Token: 0x060034E8 RID: 13544 RVA: 0x000F1B73 File Offset: 0x000EFD73
		public ScrollEventArgs(ScrollEventType type, int newValue, ScrollOrientation scroll)
		{
			this.type = type;
			this.newValue = newValue;
			this.scrollOrientation = scroll;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ScrollEventArgs" /> class using the given values for the <see cref="P:System.Windows.Forms.ScrollEventArgs.Type" />, <see cref="P:System.Windows.Forms.ScrollEventArgs.OldValue" />, and <see cref="P:System.Windows.Forms.ScrollEventArgs.NewValue" /> properties.</summary>
		/// <param name="type">One of the <see cref="T:System.Windows.Forms.ScrollEventType" /> values. </param>
		/// <param name="oldValue">The old value for the scroll bar. </param>
		/// <param name="newValue">The new value for the scroll bar. </param>
		// Token: 0x060034E9 RID: 13545 RVA: 0x000F1B97 File Offset: 0x000EFD97
		public ScrollEventArgs(ScrollEventType type, int oldValue, int newValue)
		{
			this.type = type;
			this.newValue = newValue;
			this.oldValue = oldValue;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ScrollEventArgs" /> class using the given values for the <see cref="P:System.Windows.Forms.ScrollEventArgs.Type" />, <see cref="P:System.Windows.Forms.ScrollEventArgs.OldValue" />, <see cref="P:System.Windows.Forms.ScrollEventArgs.NewValue" />, and <see cref="P:System.Windows.Forms.ScrollEventArgs.ScrollOrientation" /> properties.</summary>
		/// <param name="type">One of the <see cref="T:System.Windows.Forms.ScrollEventType" /> values. </param>
		/// <param name="oldValue">The old value for the scroll bar. </param>
		/// <param name="newValue">The new value for the scroll bar. </param>
		/// <param name="scroll">One of the <see cref="T:System.Windows.Forms.ScrollOrientation" /> values. </param>
		// Token: 0x060034EA RID: 13546 RVA: 0x000F1BBB File Offset: 0x000EFDBB
		public ScrollEventArgs(ScrollEventType type, int oldValue, int newValue, ScrollOrientation scroll)
		{
			this.type = type;
			this.newValue = newValue;
			this.scrollOrientation = scroll;
			this.oldValue = oldValue;
		}

		/// <summary>Gets the scroll bar orientation that raised the <see langword="Scroll" /> event.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ScrollOrientation" /> values.</returns>
		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x060034EB RID: 13547 RVA: 0x000F1BE7 File Offset: 0x000EFDE7
		public ScrollOrientation ScrollOrientation
		{
			get
			{
				return this.scrollOrientation;
			}
		}

		/// <summary>Gets the type of scroll event that occurred.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ScrollEventType" /> values.</returns>
		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x060034EC RID: 13548 RVA: 0x000F1BEF File Offset: 0x000EFDEF
		public ScrollEventType Type
		{
			get
			{
				return this.type;
			}
		}

		/// <summary>Gets or sets the new <see cref="P:System.Windows.Forms.ScrollBar.Value" /> of the scroll bar.</summary>
		/// <returns>The numeric value that the <see cref="P:System.Windows.Forms.ScrollBar.Value" /> property will be changed to.</returns>
		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x060034ED RID: 13549 RVA: 0x000F1BF7 File Offset: 0x000EFDF7
		// (set) Token: 0x060034EE RID: 13550 RVA: 0x000F1BFF File Offset: 0x000EFDFF
		public int NewValue
		{
			get
			{
				return this.newValue;
			}
			set
			{
				this.newValue = value;
			}
		}

		/// <summary>Gets the old <see cref="P:System.Windows.Forms.ScrollBar.Value" /> of the scroll bar.</summary>
		/// <returns>The numeric value that the <see cref="P:System.Windows.Forms.ScrollBar.Value" /> property contained before it changed.</returns>
		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x060034EF RID: 13551 RVA: 0x000F1C08 File Offset: 0x000EFE08
		public int OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		// Token: 0x04002076 RID: 8310
		private readonly ScrollEventType type;

		// Token: 0x04002077 RID: 8311
		private int newValue;

		// Token: 0x04002078 RID: 8312
		private ScrollOrientation scrollOrientation;

		// Token: 0x04002079 RID: 8313
		private int oldValue = -1;
	}
}
