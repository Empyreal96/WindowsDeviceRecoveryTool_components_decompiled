using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for events that need a <see langword="true" /> or <see langword="false" /> answer to a question.</summary>
	// Token: 0x0200032A RID: 810
	public class QuestionEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.QuestionEventArgs" /> class using a default <see cref="P:System.Windows.Forms.QuestionEventArgs.Response" /> property value of <see langword="false" />.</summary>
		// Token: 0x06003219 RID: 12825 RVA: 0x000E9FA5 File Offset: 0x000E81A5
		public QuestionEventArgs()
		{
			this.response = false;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.QuestionEventArgs" /> class using the specified default value for the <see cref="P:System.Windows.Forms.QuestionEventArgs.Response" /> property.</summary>
		/// <param name="response">The default value of the <see cref="P:System.Windows.Forms.QuestionEventArgs.Response" /> property.</param>
		// Token: 0x0600321A RID: 12826 RVA: 0x000E9FB4 File Offset: 0x000E81B4
		public QuestionEventArgs(bool response)
		{
			this.response = response;
		}

		/// <summary>Gets or sets a value indicating the response to a question represented by the event.</summary>
		/// <returns>
		///     <see langword="true" /> for an affirmative response; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x0600321B RID: 12827 RVA: 0x000E9FC3 File Offset: 0x000E81C3
		// (set) Token: 0x0600321C RID: 12828 RVA: 0x000E9FCB File Offset: 0x000E81CB
		public bool Response
		{
			get
			{
				return this.response;
			}
			set
			{
				this.response = value;
			}
		}

		// Token: 0x04001E35 RID: 7733
		private bool response;
	}
}
