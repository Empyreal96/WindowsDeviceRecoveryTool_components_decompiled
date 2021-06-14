using System;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> types to UI Automation.</summary>
	// Token: 0x020002D8 RID: 728
	public class RangeBaseAutomationPeer : FrameworkElementAutomationPeer, IRangeValueProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.RangeBaseAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RangeBaseAutomationPeer" />.</param>
		// Token: 0x060027B6 RID: 10166 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public RangeBaseAutomationPeer(RangeBase owner) : base(owner)
		{
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RangeBaseAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.RangeValue" />, this method returns a <see langword="this" /> pointer; otherwise, this method returns <see langword="null" />.</returns>
		// Token: 0x060027B7 RID: 10167 RVA: 0x000BA96A File Offset: 0x000B8B6A
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.RangeValue)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000BA979 File Offset: 0x000B8B79
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseMinimumPropertyChangedEvent(double oldValue, double newValue)
		{
			base.RaisePropertyChangedEvent(RangeValuePatternIdentifiers.MinimumProperty, oldValue, newValue);
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000BA992 File Offset: 0x000B8B92
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseMaximumPropertyChangedEvent(double oldValue, double newValue)
		{
			base.RaisePropertyChangedEvent(RangeValuePatternIdentifiers.MaximumProperty, oldValue, newValue);
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000BA9AB File Offset: 0x000B8BAB
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseValuePropertyChangedEvent(double oldValue, double newValue)
		{
			base.RaisePropertyChangedEvent(RangeValuePatternIdentifiers.ValueProperty, oldValue, newValue);
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x000BA9C4 File Offset: 0x000B8BC4
		internal virtual void SetValueCore(double val)
		{
			RangeBase rangeBase = (RangeBase)base.Owner;
			if (val < rangeBase.Minimum || val > rangeBase.Maximum)
			{
				throw new ArgumentOutOfRangeException("val");
			}
			rangeBase.Value = val;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="val"> The value to set.</param>
		// Token: 0x060027BC RID: 10172 RVA: 0x000BAA02 File Offset: 0x000B8C02
		void IRangeValueProvider.SetValue(double val)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			this.SetValueCore(val);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The value of the control.</returns>
		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060027BD RID: 10173 RVA: 0x000BAA19 File Offset: 0x000B8C19
		double IRangeValueProvider.Value
		{
			get
			{
				return ((RangeBase)base.Owner).Value;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the range value is read-only; otherwise <see langword="false" />. </returns>
		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x060027BE RID: 10174 RVA: 0x000BAA2B File Offset: 0x000B8C2B
		bool IRangeValueProvider.IsReadOnly
		{
			get
			{
				return !base.IsEnabled();
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The maximum range value supported by the control.</returns>
		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x060027BF RID: 10175 RVA: 0x000BAA36 File Offset: 0x000B8C36
		double IRangeValueProvider.Maximum
		{
			get
			{
				return ((RangeBase)base.Owner).Maximum;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The minimum range value supported by the control.</returns>
		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x060027C0 RID: 10176 RVA: 0x000BAA48 File Offset: 0x000B8C48
		double IRangeValueProvider.Minimum
		{
			get
			{
				return ((RangeBase)base.Owner).Minimum;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The large-change value.</returns>
		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x060027C1 RID: 10177 RVA: 0x000BAA5A File Offset: 0x000B8C5A
		double IRangeValueProvider.LargeChange
		{
			get
			{
				return ((RangeBase)base.Owner).LargeChange;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The small-change value.</returns>
		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x060027C2 RID: 10178 RVA: 0x000BAA6C File Offset: 0x000B8C6C
		double IRangeValueProvider.SmallChange
		{
			get
			{
				return ((RangeBase)base.Owner).SmallChange;
			}
		}
	}
}
