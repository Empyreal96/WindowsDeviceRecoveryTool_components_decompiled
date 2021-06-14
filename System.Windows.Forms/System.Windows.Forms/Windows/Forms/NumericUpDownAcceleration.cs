using System;

namespace System.Windows.Forms
{
	/// <summary>Provides information specifying how acceleration should be performed on a spin box (also known as an up-down control) when the up or down button is pressed for specified time period.</summary>
	// Token: 0x020002FE RID: 766
	public class NumericUpDownAcceleration
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> class.</summary>
		/// <param name="seconds">The number of seconds the up or down button is pressed before the acceleration starts. </param>
		/// <param name="increment">The quantity the value displayed in the control should be incremented or decremented during acceleration.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="seconds" /> or <paramref name="increment" /> is less than 0.</exception>
		// Token: 0x06002E90 RID: 11920 RVA: 0x000D8944 File Offset: 0x000D6B44
		public NumericUpDownAcceleration(int seconds, decimal increment)
		{
			if (seconds < 0)
			{
				throw new ArgumentOutOfRangeException("seconds", seconds, SR.GetString("NumericUpDownLessThanZeroError"));
			}
			if (increment < 0m)
			{
				throw new ArgumentOutOfRangeException("increment", increment, SR.GetString("NumericUpDownLessThanZeroError"));
			}
			this.seconds = seconds;
			this.increment = increment;
		}

		/// <summary>Gets or sets the number of seconds the up or down button must be pressed before the acceleration starts.</summary>
		/// <returns>Gets or sets the number of seconds the up or down button must be pressed before the acceleration starts.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The set value is less than 0.</exception>
		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06002E91 RID: 11921 RVA: 0x000D89AC File Offset: 0x000D6BAC
		// (set) Token: 0x06002E92 RID: 11922 RVA: 0x000D89B4 File Offset: 0x000D6BB4
		public int Seconds
		{
			get
			{
				return this.seconds;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("seconds", value, SR.GetString("NumericUpDownLessThanZeroError"));
				}
				this.seconds = value;
			}
		}

		/// <summary>Gets or sets the quantity to increment or decrement the displayed value during acceleration.</summary>
		/// <returns>The quantity to increment or decrement the displayed value during acceleration.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The set value is less than 0.</exception>
		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06002E93 RID: 11923 RVA: 0x000D89DC File Offset: 0x000D6BDC
		// (set) Token: 0x06002E94 RID: 11924 RVA: 0x000D89E4 File Offset: 0x000D6BE4
		public decimal Increment
		{
			get
			{
				return this.increment;
			}
			set
			{
				if (value < 0m)
				{
					throw new ArgumentOutOfRangeException("increment", value, SR.GetString("NumericUpDownLessThanZeroError"));
				}
				this.increment = value;
			}
		}

		// Token: 0x04001D3A RID: 7482
		private int seconds;

		// Token: 0x04001D3B RID: 7483
		private decimal increment;
	}
}
