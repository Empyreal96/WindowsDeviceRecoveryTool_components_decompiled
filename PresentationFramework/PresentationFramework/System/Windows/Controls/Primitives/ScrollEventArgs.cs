using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides data for a <see cref="E:System.Windows.Controls.Primitives.ScrollBar.Scroll" /> event that occurs when the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> moves. </summary>
	// Token: 0x020005A6 RID: 1446
	public class ScrollEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes an instance of the <see cref="T:System.Windows.Controls.Primitives.ScrollEventArgs" /> class by using the specified <see cref="T:System.Windows.Controls.Primitives.ScrollEventType" /> enumeration value and the new location of the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control in the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />. </summary>
		/// <param name="scrollEventType">A <see cref="T:System.Windows.Controls.Primitives.ScrollEventType" /> enumeration value that describes the type of <see cref="T:System.Windows.Controls.Primitives.Thumb" /> movement that caused the event.</param>
		/// <param name="newValue">The value that corresponds to the new location of the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> in the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />.</param>
		// Token: 0x06005FB7 RID: 24503 RVA: 0x001AD45F File Offset: 0x001AB65F
		public ScrollEventArgs(ScrollEventType scrollEventType, double newValue)
		{
			this._scrollEventType = scrollEventType;
			this._newValue = newValue;
			base.RoutedEvent = ScrollBar.ScrollEvent;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.Primitives.ScrollEventType" /> enumeration value that describes the change in the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> position that caused this event.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.Primitives.ScrollEventType" /> enumeration value that describes the type of <see cref="T:System.Windows.Controls.Primitives.Thumb" /> movement that caused the <see cref="E:System.Windows.Controls.Primitives.ScrollBar.Scroll" /> event.</returns>
		// Token: 0x1700170D RID: 5901
		// (get) Token: 0x06005FB8 RID: 24504 RVA: 0x001AD480 File Offset: 0x001AB680
		public ScrollEventType ScrollEventType
		{
			get
			{
				return this._scrollEventType;
			}
		}

		/// <summary>Gets a value that represents the new location of the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> in the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />. </summary>
		/// <returns>The value that corresponds to the new position of the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> in the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />.</returns>
		// Token: 0x1700170E RID: 5902
		// (get) Token: 0x06005FB9 RID: 24505 RVA: 0x001AD488 File Offset: 0x001AB688
		public double NewValue
		{
			get
			{
				return this._newValue;
			}
		}

		/// <summary>Performs the appropriate type casting to call the type-safe <see cref="T:System.Windows.Controls.Primitives.ScrollEventHandler" /> delegate for the <see cref="E:System.Windows.Controls.Primitives.ScrollBar.Scroll" /> event. </summary>
		/// <param name="genericHandler">The event handler to call.</param>
		/// <param name="genericTarget">The current object along the event's route.</param>
		// Token: 0x06005FBA RID: 24506 RVA: 0x001AD490 File Offset: 0x001AB690
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			ScrollEventHandler scrollEventHandler = (ScrollEventHandler)genericHandler;
			scrollEventHandler(genericTarget, this);
		}

		// Token: 0x040030D2 RID: 12498
		private ScrollEventType _scrollEventType;

		// Token: 0x040030D3 RID: 12499
		private double _newValue;
	}
}
