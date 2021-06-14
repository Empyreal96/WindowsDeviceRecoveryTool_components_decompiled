using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.GridViewColumnHeader" /> types to UI Automation.</summary>
	// Token: 0x020002B9 RID: 697
	public class GridViewColumnHeaderAutomationPeer : FrameworkElementAutomationPeer, IInvokeProvider, ITransformProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.GridViewColumnHeaderAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.GridViewColumnHeader" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewColumnHeaderAutomationPeer" />.</param>
		// Token: 0x060026B8 RID: 9912 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public GridViewColumnHeaderAutomationPeer(GridViewColumnHeader owner) : base(owner)
		{
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.GridViewColumnHeader" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewColumnHeaderAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.HeaderItem" /> enumeration value.</returns>
		// Token: 0x060026B9 RID: 9913 RVA: 0x00094F9F File Offset: 0x0009319F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.HeaderItem;
		}

		/// <summary>Gets a value that indicates whether the element that is associated with this automation peer contains data that is presented to the user. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsContentElement" />.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x060026BA RID: 9914 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool IsContentElementCore()
		{
			return false;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.GridViewColumnHeader" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewColumnHeaderAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "GridViewColumnHeader".</returns>
		// Token: 0x060026BB RID: 9915 RVA: 0x000B7FEE File Offset: 0x000B61EE
		protected override string GetClassNameCore()
		{
			return "GridViewColumnHeader";
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.GridViewColumnHeader" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewColumnHeaderAutomationPeer" />. </summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Transform" /> or <see cref="F:System.Windows.Automation.Peers.PatternInterface.Invoke" />, this method returns a <see langword="this" /> pointer; otherwise this method returns <see langword="null" />.</returns>
		// Token: 0x060026BC RID: 9916 RVA: 0x000B7FF5 File Offset: 0x000B61F5
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Invoke || patternInterface == PatternInterface.Transform)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x060026BD RID: 9917 RVA: 0x000B8008 File Offset: 0x000B6208
		void IInvokeProvider.Invoke()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			GridViewColumnHeader gridViewColumnHeader = (GridViewColumnHeader)base.Owner;
			gridViewColumnHeader.AutomationClick();
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the element can be moved; otherwise <see langword="false" />.</returns>
		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x060026BE RID: 9918 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ITransformProvider.CanMove
		{
			get
			{
				return false;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the element can be resized; otherwise <see langword="false" />.</returns>
		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x060026BF RID: 9919 RVA: 0x00016748 File Offset: 0x00014948
		bool ITransformProvider.CanResize
		{
			get
			{
				return true;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the element can rotate; otherwise <see langword="false" />.</returns>
		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x060026C0 RID: 9920 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ITransformProvider.CanRotate
		{
			get
			{
				return false;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="x"> Absolute screen coordinates of the left side of the control.</param>
		/// <param name="y"> Absolute screen coordinates of the top of the control.</param>
		// Token: 0x060026C1 RID: 9921 RVA: 0x000B7B04 File Offset: 0x000B5D04
		void ITransformProvider.Move(double x, double y)
		{
			throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="width"> The new width of the window, in pixels.</param>
		/// <param name="height"> The new height of the window, in pixels.</param>
		// Token: 0x060026C2 RID: 9922 RVA: 0x000B8038 File Offset: 0x000B6238
		void ITransformProvider.Resize(double width, double height)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			if (width < 0.0)
			{
				throw new ArgumentOutOfRangeException("width");
			}
			if (height < 0.0)
			{
				throw new ArgumentOutOfRangeException("height");
			}
			GridViewColumnHeader gridViewColumnHeader = base.Owner as GridViewColumnHeader;
			if (gridViewColumnHeader != null)
			{
				if (gridViewColumnHeader.Column != null)
				{
					gridViewColumnHeader.Column.Width = width;
				}
				gridViewColumnHeader.Height = height;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="degrees"> The number of degrees to rotate the control.</param>
		// Token: 0x060026C3 RID: 9923 RVA: 0x000B7B04 File Offset: 0x000B5D04
		void ITransformProvider.Rotate(double degrees)
		{
			throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
		}
	}
}
