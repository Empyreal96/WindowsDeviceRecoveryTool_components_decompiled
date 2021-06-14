using System;
using System.Windows.Controls;
using MS.Internal.AppModel;

namespace System.Windows.Navigation
{
	/// <summary>An abstract base class that is the parent of all page function classes.</summary>
	// Token: 0x0200031F RID: 799
	public abstract class PageFunctionBase : Page
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Navigation.PageFunctionBase" /> type.</summary>
		// Token: 0x06002A59 RID: 10841 RVA: 0x000C2669 File Offset: 0x000C0869
		protected PageFunctionBase()
		{
			this.PageFunctionId = Guid.NewGuid();
			this.ParentPageFunctionId = Guid.Empty;
		}

		/// <summary>Gets or sets a value that indicates whether the page function should not be added to navigation history.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> that indicates whether a page function should not be added to navigation history. The default value is <see langword="false" />.</returns>
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06002A5A RID: 10842 RVA: 0x000C268E File Offset: 0x000C088E
		// (set) Token: 0x06002A5B RID: 10843 RVA: 0x000C2696 File Offset: 0x000C0896
		public bool RemoveFromJournal
		{
			get
			{
				return this._fRemoveFromJournal;
			}
			set
			{
				this._fRemoveFromJournal = value;
			}
		}

		/// <summary>Override this method to initialize a <see cref="T:System.Windows.Navigation.PageFunction`1" /> when it is navigated to for the first time.</summary>
		// Token: 0x06002A5C RID: 10844 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void Start()
		{
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000C269F File Offset: 0x000C089F
		internal void CallStart()
		{
			this.Start();
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000C26A7 File Offset: 0x000C08A7
		internal void _OnReturnUnTyped(object o)
		{
			if (this._finish != null)
			{
				this._finish(this, o);
			}
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000C26C0 File Offset: 0x000C08C0
		internal void _AddEventHandler(Delegate d)
		{
			PageFunctionBase pageFunctionBase = d.Target as PageFunctionBase;
			if (pageFunctionBase != null)
			{
				this.ParentPageFunctionId = pageFunctionBase.PageFunctionId;
			}
			this._returnHandler = Delegate.Combine(this._returnHandler, d);
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000C26FA File Offset: 0x000C08FA
		internal void _RemoveEventHandler(Delegate d)
		{
			this._returnHandler = Delegate.Remove(this._returnHandler, d);
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000C270E File Offset: 0x000C090E
		internal void _DetachEvents()
		{
			this._returnHandler = null;
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000C2718 File Offset: 0x000C0918
		internal void _OnFinish(object returnEventArgs)
		{
			RaiseTypedEventArgs args = new RaiseTypedEventArgs(this._returnHandler, returnEventArgs);
			this.RaiseTypedEvent(this, args);
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06002A63 RID: 10851 RVA: 0x000C273F File Offset: 0x000C093F
		// (set) Token: 0x06002A64 RID: 10852 RVA: 0x000C2747 File Offset: 0x000C0947
		internal Guid PageFunctionId
		{
			get
			{
				return this._pageFunctionId;
			}
			set
			{
				this._pageFunctionId = value;
			}
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06002A65 RID: 10853 RVA: 0x000C2750 File Offset: 0x000C0950
		// (set) Token: 0x06002A66 RID: 10854 RVA: 0x000C2758 File Offset: 0x000C0958
		internal Guid ParentPageFunctionId
		{
			get
			{
				return this._parentPageFunctionId;
			}
			set
			{
				this._parentPageFunctionId = value;
			}
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06002A67 RID: 10855 RVA: 0x000C2761 File Offset: 0x000C0961
		internal Delegate _Return
		{
			get
			{
				return this._returnHandler;
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x000C2769 File Offset: 0x000C0969
		// (set) Token: 0x06002A69 RID: 10857 RVA: 0x000C2771 File Offset: 0x000C0971
		internal bool _Resume
		{
			get
			{
				return this._resume;
			}
			set
			{
				this._resume = value;
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06002A6A RID: 10858 RVA: 0x000C277A File Offset: 0x000C097A
		// (set) Token: 0x06002A6B RID: 10859 RVA: 0x000C2782 File Offset: 0x000C0982
		internal ReturnEventSaver _Saver
		{
			get
			{
				return this._saverInfo;
			}
			set
			{
				this._saverInfo = value;
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06002A6C RID: 10860 RVA: 0x000C278B File Offset: 0x000C098B
		// (set) Token: 0x06002A6D RID: 10861 RVA: 0x000C2793 File Offset: 0x000C0993
		internal FinishEventHandler FinishHandler
		{
			get
			{
				return this._finish;
			}
			set
			{
				this._finish = value;
			}
		}

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06002A6E RID: 10862 RVA: 0x000C279C File Offset: 0x000C099C
		// (remove) Token: 0x06002A6F RID: 10863 RVA: 0x000C27D4 File Offset: 0x000C09D4
		internal event EventToRaiseTypedEvent RaiseTypedEvent;

		// Token: 0x04001C39 RID: 7225
		private Guid _pageFunctionId;

		// Token: 0x04001C3A RID: 7226
		private Guid _parentPageFunctionId;

		// Token: 0x04001C3B RID: 7227
		private bool _fRemoveFromJournal = true;

		// Token: 0x04001C3C RID: 7228
		private bool _resume;

		// Token: 0x04001C3D RID: 7229
		private ReturnEventSaver _saverInfo;

		// Token: 0x04001C3E RID: 7230
		private FinishEventHandler _finish;

		// Token: 0x04001C3F RID: 7231
		private Delegate _returnHandler;
	}
}
