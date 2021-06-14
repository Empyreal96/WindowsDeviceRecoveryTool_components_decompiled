using System;

namespace System.Windows
{
	/// <summary>Provides data for the <see cref="E:System.Windows.VisualStateGroup.CurrentStateChanging" /> and <see cref="E:System.Windows.VisualStateGroup.CurrentStateChanged" /> events. </summary>
	// Token: 0x02000137 RID: 311
	public sealed class VisualStateChangedEventArgs : EventArgs
	{
		// Token: 0x06000CDC RID: 3292 RVA: 0x0002FC74 File Offset: 0x0002DE74
		internal VisualStateChangedEventArgs(VisualState oldState, VisualState newState, FrameworkElement control, FrameworkElement stateGroupsRoot)
		{
			this._oldState = oldState;
			this._newState = newState;
			this._control = control;
			this._stateGroupsRoot = stateGroupsRoot;
		}

		/// <summary>Gets the state that the element is transitioning to or has transitioned from.</summary>
		/// <returns>The state that the element is transitioning to or has transitioned from.</returns>
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x0002FC99 File Offset: 0x0002DE99
		public VisualState OldState
		{
			get
			{
				return this._oldState;
			}
		}

		/// <summary>Gets the state that the element is transitioning to or has transitioned to.</summary>
		/// <returns>The state that the element is transitioning to or has transitioned to.</returns>
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x0002FCA1 File Offset: 0x0002DEA1
		public VisualState NewState
		{
			get
			{
				return this._newState;
			}
		}

		/// <summary>Gets the element that is transitioning states.</summary>
		/// <returns>The element that is transitioning states if the <see cref="T:System.Windows.VisualStateGroup" /> is in a <see cref="T:System.Windows.Controls.ControlTemplate" />; otherwise, <see langword="null" />.</returns>
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x0002FCA9 File Offset: 0x0002DEA9
		public FrameworkElement Control
		{
			get
			{
				return this._control;
			}
		}

		/// <summary>Gets the root element that contains the <see cref="T:System.Windows.VisualStateManager" />.</summary>
		/// <returns>The root element that contains the <see cref="T:System.Windows.VisualStateManager" />.</returns>
		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x0002FCB1 File Offset: 0x0002DEB1
		public FrameworkElement StateGroupsRoot
		{
			get
			{
				return this._stateGroupsRoot;
			}
		}

		// Token: 0x04000B26 RID: 2854
		private VisualState _oldState;

		// Token: 0x04000B27 RID: 2855
		private VisualState _newState;

		// Token: 0x04000B28 RID: 2856
		private FrameworkElement _control;

		// Token: 0x04000B29 RID: 2857
		private FrameworkElement _stateGroupsRoot;
	}
}
