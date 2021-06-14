using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using MS.Win32;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Window" /> types to UI Automation.</summary>
	// Token: 0x020002F6 RID: 758
	public class WindowAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.WindowAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Window" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.WindowAutomationPeer" />.</param>
		// Token: 0x060028A2 RID: 10402 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public WindowAutomationPeer(Window owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Window" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.WindowAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains the word "Window".</returns>
		// Token: 0x060028A3 RID: 10403 RVA: 0x000BCF6D File Offset: 0x000BB16D
		protected override string GetClassNameCore()
		{
			return "Window";
		}

		/// <summary>Gets the text label of the <see cref="T:System.Windows.Window" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetName" />.</summary>
		/// <returns>A string that contains the <see cref="P:System.Windows.Automation.AutomationProperties.Name" /> or  <see cref="P:System.Windows.FrameworkElement.Name" /> of the <see cref="T:System.Windows.Window" />, or <see cref="F:System.String.Empty" /> if the name is <see langword="null" />.</returns>
		// Token: 0x060028A4 RID: 10404 RVA: 0x000BCF74 File Offset: 0x000BB174
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			if (text == string.Empty)
			{
				Window window = (Window)base.Owner;
				if (!window.IsSourceWindowNull)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					UnsafeNativeMethods.GetWindowText(new HandleRef(null, window.CriticalHandle), stringBuilder, stringBuilder.Capacity);
					text = stringBuilder.ToString();
					if (text == null)
					{
						text = string.Empty;
					}
				}
			}
			return text;
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Window" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.WindowAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Window" /> enumeration value.</returns>
		// Token: 0x060028A5 RID: 10405 RVA: 0x00096AE4 File Offset: 0x00094CE4
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Window;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Rect" /> representing the bounding rectangle of the <see cref="T:System.Windows.Window" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.WindowAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetBoundingRectangle" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Rect" /> that represents the screen coordinates of the <see cref="T:System.Windows.Window" />.</returns>
		// Token: 0x060028A6 RID: 10406 RVA: 0x000BCFE0 File Offset: 0x000BB1E0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override Rect GetBoundingRectangleCore()
		{
			Window window = (Window)base.Owner;
			Rect result = new Rect(0.0, 0.0, 0.0, 0.0);
			if (!window.IsSourceWindowNull)
			{
				NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, 0, 0);
				IntPtr criticalHandle = window.CriticalHandle;
				if (criticalHandle != IntPtr.Zero)
				{
					try
					{
						SafeNativeMethods.GetWindowRect(new HandleRef(null, criticalHandle), ref rect);
					}
					catch (Win32Exception)
					{
					}
				}
				result = new Rect((double)rect.left, (double)rect.top, (double)(rect.right - rect.left), (double)(rect.bottom - rect.top));
			}
			return result;
		}
	}
}
