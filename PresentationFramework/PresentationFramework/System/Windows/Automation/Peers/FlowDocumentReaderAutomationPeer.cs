using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Documents;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.FlowDocumentReader" /> types to UI Automation.</summary>
	// Token: 0x020002B2 RID: 690
	public class FlowDocumentReaderAutomationPeer : FrameworkElementAutomationPeer, IMultipleViewProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.FlowDocumentReaderAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.FlowDocumentReader" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentReaderAutomationPeer" />.</param>
		// Token: 0x06002681 RID: 9857 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public FlowDocumentReaderAutomationPeer(FlowDocumentReader owner) : base(owner)
		{
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentReaderAutomationPeer" />. </summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.MultipleView" />, this method returns a <see langword="this" /> pointer. Otherwise, this method returns <see langword="null" />.</returns>
		// Token: 0x06002682 RID: 9858 RVA: 0x000B745C File Offset: 0x000B565C
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result;
			if (patternInterface == PatternInterface.MultipleView)
			{
				result = this;
			}
			else
			{
				result = base.GetPattern(patternInterface);
			}
			return result;
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentReaderAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x06002683 RID: 9859 RVA: 0x000B7480 File Offset: 0x000B5680
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			FlowDocument document = ((FlowDocumentReader)base.Owner).Document;
			if (document != null)
			{
				AutomationPeer automationPeer = ContentElementAutomationPeer.CreatePeerForElement(document);
				if (this._documentPeer != automationPeer)
				{
					if (this._documentPeer != null)
					{
						this._documentPeer.OnDisconnected();
					}
					this._documentPeer = (automationPeer as DocumentAutomationPeer);
				}
				if (automationPeer != null)
				{
					if (list == null)
					{
						list = new List<AutomationPeer>();
					}
					list.Add(automationPeer);
				}
			}
			return list;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentReaderAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>The name of the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentReaderAutomationPeer" />.</returns>
		// Token: 0x06002684 RID: 9860 RVA: 0x000B74EB File Offset: 0x000B56EB
		protected override string GetClassNameCore()
		{
			return "FlowDocumentReader";
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000B74F2 File Offset: 0x000B56F2
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseCurrentViewChangedEvent(FlowDocumentReaderViewingMode newMode, FlowDocumentReaderViewingMode oldMode)
		{
			if (newMode != oldMode)
			{
				base.RaisePropertyChangedEvent(MultipleViewPatternIdentifiers.CurrentViewProperty, this.ConvertModeToViewId(newMode), this.ConvertModeToViewId(oldMode));
			}
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x000B751C File Offset: 0x000B571C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseSupportedViewsChangedEvent(DependencyPropertyChangedEventArgs e)
		{
			bool flag;
			bool flag2;
			bool flag4;
			bool flag3;
			bool flag6;
			bool flag5;
			if (e.Property == FlowDocumentReader.IsPageViewEnabledProperty)
			{
				flag = (bool)e.NewValue;
				flag2 = (bool)e.OldValue;
				flag3 = (flag4 = this.FlowDocumentReader.IsTwoPageViewEnabled);
				flag5 = (flag6 = this.FlowDocumentReader.IsScrollViewEnabled);
			}
			else if (e.Property == FlowDocumentReader.IsTwoPageViewEnabledProperty)
			{
				flag2 = (flag = this.FlowDocumentReader.IsPageViewEnabled);
				flag4 = (bool)e.NewValue;
				flag3 = (bool)e.OldValue;
				flag5 = (flag6 = this.FlowDocumentReader.IsScrollViewEnabled);
			}
			else
			{
				flag2 = (flag = this.FlowDocumentReader.IsPageViewEnabled);
				flag3 = (flag4 = this.FlowDocumentReader.IsTwoPageViewEnabled);
				flag6 = (bool)e.NewValue;
				flag5 = (bool)e.OldValue;
			}
			if (flag != flag2 || flag4 != flag3 || flag6 != flag5)
			{
				int[] supportedViews = this.GetSupportedViews(flag, flag4, flag6);
				int[] supportedViews2 = this.GetSupportedViews(flag2, flag3, flag5);
				base.RaisePropertyChangedEvent(MultipleViewPatternIdentifiers.SupportedViewsProperty, supportedViews, supportedViews2);
			}
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x000B762C File Offset: 0x000B582C
		private int[] GetSupportedViews(bool single, bool facing, bool scroll)
		{
			int num = 0;
			if (single)
			{
				num++;
			}
			if (facing)
			{
				num++;
			}
			if (scroll)
			{
				num++;
			}
			int[] array = (num > 0) ? new int[num] : null;
			num = 0;
			if (single)
			{
				array[num++] = this.ConvertModeToViewId(FlowDocumentReaderViewingMode.Page);
			}
			if (facing)
			{
				array[num++] = this.ConvertModeToViewId(FlowDocumentReaderViewingMode.TwoPage);
			}
			if (scroll)
			{
				array[num++] = this.ConvertModeToViewId(FlowDocumentReaderViewingMode.Scroll);
			}
			return array;
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x00012630 File Offset: 0x00010830
		private int ConvertModeToViewId(FlowDocumentReaderViewingMode mode)
		{
			return (int)mode;
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x000B7694 File Offset: 0x000B5894
		private FlowDocumentReaderViewingMode ConvertViewIdToMode(int viewId)
		{
			Invariant.Assert(viewId >= 0 && viewId <= 2);
			return (FlowDocumentReaderViewingMode)viewId;
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x0600268A RID: 9866 RVA: 0x000B76AA File Offset: 0x000B58AA
		private FlowDocumentReader FlowDocumentReader
		{
			get
			{
				return (FlowDocumentReader)base.Owner;
			}
		}

		/// <summary>Returns the name of a control-specific view.</summary>
		/// <param name="viewId">The ID of a view. </param>
		/// <returns>The name of a control-specific view.</returns>
		// Token: 0x0600268B RID: 9867 RVA: 0x000B76B8 File Offset: 0x000B58B8
		string IMultipleViewProvider.GetViewName(int viewId)
		{
			string result = string.Empty;
			if (viewId >= 0 && viewId <= 2)
			{
				FlowDocumentReaderViewingMode flowDocumentReaderViewingMode = this.ConvertViewIdToMode(viewId);
				if (flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.Page)
				{
					result = SR.Get("FlowDocumentReader_MultipleViewProvider_PageViewName");
				}
				else if (flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.TwoPage)
				{
					result = SR.Get("FlowDocumentReader_MultipleViewProvider_TwoPageViewName");
				}
				else if (flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.Scroll)
				{
					result = SR.Get("FlowDocumentReader_MultipleViewProvider_ScrollViewName");
				}
			}
			return result;
		}

		/// <summary>Sets the current control-specific view. </summary>
		/// <param name="viewId">The ID of a view. </param>
		// Token: 0x0600268C RID: 9868 RVA: 0x000B770C File Offset: 0x000B590C
		void IMultipleViewProvider.SetCurrentView(int viewId)
		{
			if (viewId >= 0 && viewId <= 2)
			{
				this.FlowDocumentReader.ViewingMode = this.ConvertViewIdToMode(viewId);
			}
		}

		/// <summary>Gets the current control-specific view.</summary>
		/// <returns>The value for the current view of the UI Automation element.</returns>
		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x0600268D RID: 9869 RVA: 0x000B7728 File Offset: 0x000B5928
		int IMultipleViewProvider.CurrentView
		{
			get
			{
				return this.ConvertModeToViewId(this.FlowDocumentReader.ViewingMode);
			}
		}

		/// <summary>Returns a collection of control-specific view identifiers.</summary>
		/// <returns>A collection of values that identifies the views available for a UI Automation element.</returns>
		// Token: 0x0600268E RID: 9870 RVA: 0x000B773B File Offset: 0x000B593B
		int[] IMultipleViewProvider.GetSupportedViews()
		{
			return this.GetSupportedViews(this.FlowDocumentReader.IsPageViewEnabled, this.FlowDocumentReader.IsTwoPageViewEnabled, this.FlowDocumentReader.IsScrollViewEnabled);
		}

		// Token: 0x04001B76 RID: 7030
		private DocumentAutomationPeer _documentPeer;
	}
}
