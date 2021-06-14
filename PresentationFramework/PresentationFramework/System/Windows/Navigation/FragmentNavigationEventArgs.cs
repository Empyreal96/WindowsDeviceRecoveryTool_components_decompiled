using System;

namespace System.Windows.Navigation
{
	/// <summary>Provides data for the FragmentNavigation event.</summary>
	// Token: 0x020002FF RID: 767
	public class FragmentNavigationEventArgs : EventArgs
	{
		// Token: 0x060028C1 RID: 10433 RVA: 0x000BD3EE File Offset: 0x000BB5EE
		internal FragmentNavigationEventArgs(string fragment, object Navigator)
		{
			this._fragment = fragment;
			this._navigator = Navigator;
		}

		/// <summary>Gets the uniform resource identifier (URI) fragment.</summary>
		/// <returns>The URI fragment. If you set the property to an empty string, the top of the content will be navigated to by default.</returns>
		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060028C2 RID: 10434 RVA: 0x000BD404 File Offset: 0x000BB604
		public string Fragment
		{
			get
			{
				return this._fragment;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the fragment navigation has been handled. </summary>
		/// <returns>
		///     <see langword="true" /> if the navigation has been handled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060028C3 RID: 10435 RVA: 0x000BD40C File Offset: 0x000BB60C
		// (set) Token: 0x060028C4 RID: 10436 RVA: 0x000BD414 File Offset: 0x000BB614
		public bool Handled
		{
			get
			{
				return this._handled;
			}
			set
			{
				this._handled = value;
			}
		}

		/// <summary>The navigator that raised the <see cref="E:System.Windows.Navigation.NavigationService.FragmentNavigation" /> event.</summary>
		/// <returns>A <see cref="T:System.Object" /> that refers to the navigator (Internet Explorer, <see cref="T:System.Windows.Navigation.NavigationWindow" />, <see cref="T:System.Windows.Controls.Frame" />.) that is navigating to the content fragment.</returns>
		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060028C5 RID: 10437 RVA: 0x000BD41D File Offset: 0x000BB61D
		public object Navigator
		{
			get
			{
				return this._navigator;
			}
		}

		// Token: 0x04001BB3 RID: 7091
		private string _fragment;

		// Token: 0x04001BB4 RID: 7092
		private bool _handled;

		// Token: 0x04001BB5 RID: 7093
		private object _navigator;
	}
}
