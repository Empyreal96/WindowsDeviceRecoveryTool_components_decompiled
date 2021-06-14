using System;
using System.ComponentModel;

namespace System.Windows.Media.Animation
{
	/// <summary>A trigger action that provides functionality for seeking (skipping) to a specified time within the active period of a <see cref="T:System.Windows.Media.Animation.Storyboard" />.</summary>
	// Token: 0x02000185 RID: 389
	public sealed class SeekStoryboard : ControllableStoryboardAction
	{
		/// <summary>Gets or sets the amount by which the storyboard should move forward or backward from the seek origin <see cref="P:System.Windows.Media.Animation.SeekStoryboard.Origin" />. .</summary>
		/// <returns>A positive or negative value that specifies the amount by which the storyboard should move forward or backward from the seek origin <see cref="P:System.Windows.Media.Animation.SeekStoryboard.Origin" />. The default value is 0. </returns>
		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x0600169E RID: 5790 RVA: 0x0007083E File Offset: 0x0006EA3E
		// (set) Token: 0x0600169F RID: 5791 RVA: 0x00070846 File Offset: 0x0006EA46
		public TimeSpan Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"SeekStoryboard"
					}));
				}
				this._offset = value;
			}
		}

		/// <summary>Returns a value that indicates whether the <see cref="P:System.Windows.Media.Animation.SeekStoryboard.Offset" /> property of this <see cref="T:System.Windows.Media.Animation.SeekStoryboard" /> should be serialized.</summary>
		/// <returns>
		///     true if the <see cref="P:System.Windows.Media.Animation.SeekStoryboard.Offset" /> property of this <see cref="T:System.Windows.Media.Animation.SeekStoryboard" /> should be serialized; otherwise, false.</returns>
		// Token: 0x060016A0 RID: 5792 RVA: 0x00070878 File Offset: 0x0006EA78
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeOffset()
		{
			return !TimeSpan.Zero.Equals(this._offset);
		}

		/// <summary>Gets or sets the position from which this seek operation's <see cref="P:System.Windows.Media.Animation.SeekStoryboard.Offset" /> is applied. </summary>
		/// <returns>The position from which this seek operation's <see cref="P:System.Windows.Media.Animation.SeekStoryboard.Offset" /> is applied. The default value is <see cref="F:System.Windows.Media.Animation.TimeSeekOrigin.BeginTime" />.</returns>
		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x060016A1 RID: 5793 RVA: 0x0007089B File Offset: 0x0006EA9B
		// (set) Token: 0x060016A2 RID: 5794 RVA: 0x000708A4 File Offset: 0x0006EAA4
		[DefaultValue(TimeSeekOrigin.BeginTime)]
		public TimeSeekOrigin Origin
		{
			get
			{
				return this._origin;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"SeekStoryboard"
					}));
				}
				if (value == TimeSeekOrigin.BeginTime || value == TimeSeekOrigin.Duration)
				{
					this._origin = value;
					return;
				}
				throw new ArgumentException(SR.Get("Storyboard_UnrecognizedTimeSeekOrigin"));
			}
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x000708F5 File Offset: 0x0006EAF5
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.Seek(containingFE, this.Offset, this.Origin);
				return;
			}
			storyboard.Seek(containingFCE, this.Offset, this.Origin);
		}

		// Token: 0x040012A7 RID: 4775
		private TimeSpan _offset = TimeSpan.Zero;

		// Token: 0x040012A8 RID: 4776
		private TimeSeekOrigin _origin;
	}
}
