using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the context menu event. </summary>
	// Token: 0x0200048D RID: 1165
	public sealed class ContextMenuEventArgs : RoutedEventArgs
	{
		// Token: 0x0600447F RID: 17535 RVA: 0x00137AC4 File Offset: 0x00135CC4
		internal ContextMenuEventArgs(object source, bool opening) : this(source, opening, -1.0, -1.0)
		{
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x00137AE0 File Offset: 0x00135CE0
		internal ContextMenuEventArgs(object source, bool opening, double left, double top)
		{
			this._left = left;
			this._top = top;
			base.RoutedEvent = (opening ? ContextMenuService.ContextMenuOpeningEvent : ContextMenuService.ContextMenuClosingEvent);
			base.Source = source;
		}

		/// <summary> Gets the horizontal position of the mouse.  </summary>
		/// <returns>The horizontal position of the mouse.</returns>
		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x06004481 RID: 17537 RVA: 0x00137B13 File Offset: 0x00135D13
		public double CursorLeft
		{
			get
			{
				return this._left;
			}
		}

		/// <summary>Gets the vertical position of the mouse.  </summary>
		/// <returns>The vertical position of the mouse. </returns>
		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x06004482 RID: 17538 RVA: 0x00137B1B File Offset: 0x00135D1B
		public double CursorTop
		{
			get
			{
				return this._top;
			}
		}

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x06004483 RID: 17539 RVA: 0x00137B23 File Offset: 0x00135D23
		// (set) Token: 0x06004484 RID: 17540 RVA: 0x00137B2B File Offset: 0x00135D2B
		internal DependencyObject TargetElement
		{
			get
			{
				return this._targetElement;
			}
			set
			{
				this._targetElement = value;
			}
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x00137B34 File Offset: 0x00135D34
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			ContextMenuEventHandler contextMenuEventHandler = (ContextMenuEventHandler)genericHandler;
			contextMenuEventHandler(genericTarget, this);
		}

		// Token: 0x0400289B RID: 10395
		private double _left;

		// Token: 0x0400289C RID: 10396
		private double _top;

		// Token: 0x0400289D RID: 10397
		private DependencyObject _targetElement;
	}
}
