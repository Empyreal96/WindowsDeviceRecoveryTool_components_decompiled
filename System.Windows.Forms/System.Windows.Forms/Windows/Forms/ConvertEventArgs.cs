using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Binding.Format" /> and <see cref="E:System.Windows.Forms.Binding.Parse" /> events.</summary>
	// Token: 0x02000162 RID: 354
	public class ConvertEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ConvertEventArgs" /> class.</summary>
		/// <param name="value">An <see cref="T:System.Object" /> that contains the value of the current property. </param>
		/// <param name="desiredType">The <see cref="T:System.Type" /> of the value. </param>
		// Token: 0x0600102E RID: 4142 RVA: 0x000391F2 File Offset: 0x000373F2
		public ConvertEventArgs(object value, Type desiredType)
		{
			this.value = value;
			this.desiredType = desiredType;
		}

		/// <summary>Gets or sets the value of the <see cref="T:System.Windows.Forms.ConvertEventArgs" />.</summary>
		/// <returns>The value of the <see cref="T:System.Windows.Forms.ConvertEventArgs" />.</returns>
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x00039208 File Offset: 0x00037408
		// (set) Token: 0x06001030 RID: 4144 RVA: 0x00039210 File Offset: 0x00037410
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		/// <summary>Gets the data type of the desired value.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the desired value.</returns>
		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x00039219 File Offset: 0x00037419
		public Type DesiredType
		{
			get
			{
				return this.desiredType;
			}
		}

		// Token: 0x0400087E RID: 2174
		private object value;

		// Token: 0x0400087F RID: 2175
		private Type desiredType;
	}
}
