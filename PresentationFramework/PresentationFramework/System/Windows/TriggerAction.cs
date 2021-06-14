using System;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Describes an action to perform for a trigger.</summary>
	// Token: 0x02000132 RID: 306
	public abstract class TriggerAction : DependencyObject
	{
		// Token: 0x06000C87 RID: 3207 RVA: 0x0002F2D4 File Offset: 0x0002D4D4
		internal TriggerAction()
		{
		}

		// Token: 0x06000C88 RID: 3208
		internal abstract void Invoke(FrameworkElement fe, FrameworkContentElement fce, Style targetStyle, FrameworkTemplate targetTemplate, long layer);

		// Token: 0x06000C89 RID: 3209
		internal abstract void Invoke(FrameworkElement fe);

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x0002F2DC File Offset: 0x0002D4DC
		internal TriggerBase ContainingTrigger
		{
			get
			{
				return this._containingTrigger;
			}
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0002F2E4 File Offset: 0x0002D4E4
		internal void Seal(TriggerBase containingTrigger)
		{
			if (base.IsSealed && containingTrigger != this._containingTrigger)
			{
				throw new InvalidOperationException(SR.Get("TriggerActionMustBelongToASingleTrigger"));
			}
			this._containingTrigger = containingTrigger;
			this.Seal();
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0002F314 File Offset: 0x0002D514
		internal override void Seal()
		{
			if (base.IsSealed)
			{
				throw new InvalidOperationException(SR.Get("TriggerActionAlreadySealed"));
			}
			base.Seal();
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0002F334 File Offset: 0x0002D534
		internal void CheckSealed()
		{
			if (base.IsSealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"TriggerAction"
				}));
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0002F35C File Offset: 0x0002D55C
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return this._inheritanceContext;
			}
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0002F364 File Offset: 0x0002D564
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			InheritanceContextHelper.AddInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0002F379 File Offset: 0x0002D579
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			InheritanceContextHelper.RemoveInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x0002F38E File Offset: 0x0002D58E
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this._hasMultipleInheritanceContexts;
			}
		}

		// Token: 0x04000B13 RID: 2835
		private TriggerBase _containingTrigger;

		// Token: 0x04000B14 RID: 2836
		private DependencyObject _inheritanceContext;

		// Token: 0x04000B15 RID: 2837
		private bool _hasMultipleInheritanceContexts;
	}
}
