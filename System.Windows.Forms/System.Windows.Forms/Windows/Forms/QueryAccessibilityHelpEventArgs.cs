using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.QueryAccessibilityHelp" /> event.</summary>
	// Token: 0x02000326 RID: 806
	[ComVisible(true)]
	public class QueryAccessibilityHelpEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.QueryAccessibilityHelpEventArgs" /> class.</summary>
		// Token: 0x06003204 RID: 12804 RVA: 0x00088683 File Offset: 0x00086883
		public QueryAccessibilityHelpEventArgs()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.QueryAccessibilityHelpEventArgs" /> class.</summary>
		/// <param name="helpNamespace">The file containing Help for the <see cref="T:System.Windows.Forms.AccessibleObject" />. </param>
		/// <param name="helpString">The string defining what Help to get for the <see cref="T:System.Windows.Forms.AccessibleObject" />. </param>
		/// <param name="helpKeyword">The keyword to associate with the Help request for the <see cref="T:System.Windows.Forms.AccessibleObject" />. </param>
		// Token: 0x06003205 RID: 12805 RVA: 0x000E9F17 File Offset: 0x000E8117
		public QueryAccessibilityHelpEventArgs(string helpNamespace, string helpString, string helpKeyword)
		{
			this.helpNamespace = helpNamespace;
			this.helpString = helpString;
			this.helpKeyword = helpKeyword;
		}

		/// <summary>Gets or sets a value specifying the name of the Help file.</summary>
		/// <returns>The name of the Help file. This name can be in the form C:\path\sample.chm or /folder/file.htm.</returns>
		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06003206 RID: 12806 RVA: 0x000E9F34 File Offset: 0x000E8134
		// (set) Token: 0x06003207 RID: 12807 RVA: 0x000E9F3C File Offset: 0x000E813C
		public string HelpNamespace
		{
			get
			{
				return this.helpNamespace;
			}
			set
			{
				this.helpNamespace = value;
			}
		}

		/// <summary>Gets or sets the string defining what Help to get for the <see cref="T:System.Windows.Forms.AccessibleObject" />.</summary>
		/// <returns>The Help to retrieve for the accessible object.</returns>
		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06003208 RID: 12808 RVA: 0x000E9F45 File Offset: 0x000E8145
		// (set) Token: 0x06003209 RID: 12809 RVA: 0x000E9F4D File Offset: 0x000E814D
		public string HelpString
		{
			get
			{
				return this.helpString;
			}
			set
			{
				this.helpString = value;
			}
		}

		/// <summary>Gets or sets the Help keyword for the specified control.</summary>
		/// <returns>The Help topic associated with the <see cref="T:System.Windows.Forms.AccessibleObject" /> that was queried.</returns>
		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x0600320A RID: 12810 RVA: 0x000E9F56 File Offset: 0x000E8156
		// (set) Token: 0x0600320B RID: 12811 RVA: 0x000E9F5E File Offset: 0x000E815E
		public string HelpKeyword
		{
			get
			{
				return this.helpKeyword;
			}
			set
			{
				this.helpKeyword = value;
			}
		}

		// Token: 0x04001E2F RID: 7727
		private string helpNamespace;

		// Token: 0x04001E30 RID: 7728
		private string helpString;

		// Token: 0x04001E31 RID: 7729
		private string helpKeyword;
	}
}
