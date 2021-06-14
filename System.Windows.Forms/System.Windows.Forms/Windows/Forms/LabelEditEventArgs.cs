using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.BeforeLabelEdit" /> and <see cref="E:System.Windows.Forms.ListView.AfterLabelEdit" /> events.</summary>
	// Token: 0x020002AB RID: 683
	public class LabelEditEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LabelEditEventArgs" /> class with the specified index to the <see cref="T:System.Windows.Forms.ListViewItem" /> to edit.</summary>
		/// <param name="item">The zero-based index of the <see cref="T:System.Windows.Forms.ListViewItem" />, containing the label to edit. </param>
		// Token: 0x060027C6 RID: 10182 RVA: 0x000B9DBF File Offset: 0x000B7FBF
		public LabelEditEventArgs(int item)
		{
			this.item = item;
			this.label = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LabelEditEventArgs" /> class with the specified index to the <see cref="T:System.Windows.Forms.ListViewItem" /> being edited and the new text for the label of the <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		/// <param name="item">The zero-based index of the <see cref="T:System.Windows.Forms.ListViewItem" />, containing the label to edit. </param>
		/// <param name="label">The new text assigned to the label of the <see cref="T:System.Windows.Forms.ListViewItem" />. </param>
		// Token: 0x060027C7 RID: 10183 RVA: 0x000B9DD5 File Offset: 0x000B7FD5
		public LabelEditEventArgs(int item, string label)
		{
			this.item = item;
			this.label = label;
		}

		/// <summary>Gets the new text assigned to the label of the <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		/// <returns>The new text to associate with the <see cref="T:System.Windows.Forms.ListViewItem" /> or <see langword="null" /> if the text is unchanged. </returns>
		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x060027C8 RID: 10184 RVA: 0x000B9DEB File Offset: 0x000B7FEB
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		/// <summary>Gets the zero-based index of the <see cref="T:System.Windows.Forms.ListViewItem" /> containing the label to edit.</summary>
		/// <returns>The zero-based index of the <see cref="T:System.Windows.Forms.ListViewItem" />.</returns>
		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060027C9 RID: 10185 RVA: 0x000B9DF3 File Offset: 0x000B7FF3
		public int Item
		{
			get
			{
				return this.item;
			}
		}

		/// <summary>Gets or sets a value indicating whether changes made to the label of the <see cref="T:System.Windows.Forms.ListViewItem" /> should be canceled.</summary>
		/// <returns>
		///     <see langword="true" /> if the edit operation of the label for the <see cref="T:System.Windows.Forms.ListViewItem" /> should be canceled; otherwise <see langword="false" />.</returns>
		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060027CA RID: 10186 RVA: 0x000B9DFB File Offset: 0x000B7FFB
		// (set) Token: 0x060027CB RID: 10187 RVA: 0x000B9E03 File Offset: 0x000B8003
		public bool CancelEdit
		{
			get
			{
				return this.cancelEdit;
			}
			set
			{
				this.cancelEdit = value;
			}
		}

		// Token: 0x0400116B RID: 4459
		private readonly string label;

		// Token: 0x0400116C RID: 4460
		private readonly int item;

		// Token: 0x0400116D RID: 4461
		private bool cancelEdit;
	}
}
