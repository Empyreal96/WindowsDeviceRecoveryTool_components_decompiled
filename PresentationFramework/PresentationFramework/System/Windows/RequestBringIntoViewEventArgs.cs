using System;

namespace System.Windows
{
	/// <summary>Provides data for the <see cref="E:System.Windows.FrameworkElement.RequestBringIntoView" /> routed event.</summary>
	// Token: 0x020000E4 RID: 228
	public class RequestBringIntoViewEventArgs : RoutedEventArgs
	{
		// Token: 0x060007CC RID: 1996 RVA: 0x00018EDC File Offset: 0x000170DC
		internal RequestBringIntoViewEventArgs(DependencyObject target, Rect targetRect)
		{
			this._target = target;
			this._rcTarget = targetRect;
		}

		/// <summary>Gets the object that should be made visible in response to the event.</summary>
		/// <returns>The object that called <see cref="M:System.Windows.FrameworkElement.BringIntoView" />.</returns>
		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00018EF2 File Offset: 0x000170F2
		public DependencyObject TargetObject
		{
			get
			{
				return this._target;
			}
		}

		/// <summary>Gets the rectangular region in the object's coordinate space which should be made visible.</summary>
		/// <returns>The requested rectangular space.</returns>
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x00018EFA File Offset: 0x000170FA
		public Rect TargetRect
		{
			get
			{
				return this._rcTarget;
			}
		}

		/// <summary>Invokes event handlers in a type-specific way, which can increase event system efficiency.</summary>
		/// <param name="genericHandler">The generic handler to call in a type-specific way.</param>
		/// <param name="genericTarget">The target to call the handler on.</param>
		// Token: 0x060007CF RID: 1999 RVA: 0x00018F04 File Offset: 0x00017104
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			RequestBringIntoViewEventHandler requestBringIntoViewEventHandler = (RequestBringIntoViewEventHandler)genericHandler;
			requestBringIntoViewEventHandler(genericTarget, this);
		}

		// Token: 0x04000774 RID: 1908
		private DependencyObject _target;

		// Token: 0x04000775 RID: 1909
		private Rect _rcTarget;
	}
}
