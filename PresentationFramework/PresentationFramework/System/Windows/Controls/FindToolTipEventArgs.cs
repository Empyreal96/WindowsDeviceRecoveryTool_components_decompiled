using System;

namespace System.Windows.Controls
{
	// Token: 0x0200054B RID: 1355
	internal sealed class FindToolTipEventArgs : RoutedEventArgs
	{
		// Token: 0x060058C7 RID: 22727 RVA: 0x00189100 File Offset: 0x00187300
		internal FindToolTipEventArgs(ToolTip.ToolTipTrigger triggerAction)
		{
			base.RoutedEvent = ToolTipService.FindToolTipEvent;
			this._triggerAction = triggerAction;
		}

		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x060058C8 RID: 22728 RVA: 0x0018911A File Offset: 0x0018731A
		// (set) Token: 0x060058C9 RID: 22729 RVA: 0x00189122 File Offset: 0x00187322
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

		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x060058CA RID: 22730 RVA: 0x0018912B File Offset: 0x0018732B
		// (set) Token: 0x060058CB RID: 22731 RVA: 0x00189133 File Offset: 0x00187333
		internal bool KeepCurrentActive
		{
			get
			{
				return this._keepCurrentActive;
			}
			set
			{
				this._keepCurrentActive = value;
			}
		}

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x060058CC RID: 22732 RVA: 0x0018913C File Offset: 0x0018733C
		internal ToolTip.ToolTipTrigger TriggerAction
		{
			get
			{
				return this._triggerAction;
			}
		}

		// Token: 0x060058CD RID: 22733 RVA: 0x00189144 File Offset: 0x00187344
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			FindToolTipEventHandler findToolTipEventHandler = (FindToolTipEventHandler)genericHandler;
			findToolTipEventHandler(genericTarget, this);
		}

		// Token: 0x04002EE4 RID: 12004
		private DependencyObject _targetElement;

		// Token: 0x04002EE5 RID: 12005
		private bool _keepCurrentActive;

		// Token: 0x04002EE6 RID: 12006
		private ToolTip.ToolTipTrigger _triggerAction;
	}
}
