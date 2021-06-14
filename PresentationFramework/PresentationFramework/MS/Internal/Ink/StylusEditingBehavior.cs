using System;
using System.Security;
using System.Windows.Controls;
using System.Windows.Input;

namespace MS.Internal.Ink
{
	// Token: 0x02000694 RID: 1684
	internal abstract class StylusEditingBehavior : EditingBehavior, IStylusEditing
	{
		// Token: 0x06006DF6 RID: 28150 RVA: 0x001F9B67 File Offset: 0x001F7D67
		internal StylusEditingBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06006DF7 RID: 28151 RVA: 0x001FA528 File Offset: 0x001F8728
		internal void SwitchToMode(InkCanvasEditingMode mode)
		{
			this._disableInput = true;
			try
			{
				this.OnSwitchToMode(mode);
			}
			finally
			{
				this._disableInput = false;
			}
		}

		// Token: 0x06006DF8 RID: 28152 RVA: 0x001FA560 File Offset: 0x001F8760
		[SecurityCritical]
		void IStylusEditing.AddStylusPoints(StylusPointCollection stylusPoints, bool userInitiated)
		{
			if (this._disableInput)
			{
				return;
			}
			if (!base.EditingCoordinator.UserIsEditing)
			{
				base.EditingCoordinator.UserIsEditing = true;
				this.StylusInputBegin(stylusPoints, userInitiated);
				return;
			}
			this.StylusInputContinue(stylusPoints, userInitiated);
		}

		// Token: 0x06006DF9 RID: 28153
		protected abstract void OnSwitchToMode(InkCanvasEditingMode mode);

		// Token: 0x06006DFA RID: 28154 RVA: 0x00002137 File Offset: 0x00000337
		protected override void OnActivate()
		{
		}

		// Token: 0x06006DFB RID: 28155 RVA: 0x00002137 File Offset: 0x00000337
		protected override void OnDeactivate()
		{
		}

		// Token: 0x06006DFC RID: 28156 RVA: 0x001FA595 File Offset: 0x001F8795
		protected sealed override void OnCommit(bool commit)
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				base.EditingCoordinator.UserIsEditing = false;
				this.StylusInputEnd(commit);
				return;
			}
			this.OnCommitWithoutStylusInput(commit);
		}

		// Token: 0x06006DFD RID: 28157 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		protected virtual void StylusInputBegin(StylusPointCollection stylusPoints, bool userInitiated)
		{
		}

		// Token: 0x06006DFE RID: 28158 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		protected virtual void StylusInputContinue(StylusPointCollection stylusPoints, bool userInitiated)
		{
		}

		// Token: 0x06006DFF RID: 28159 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void StylusInputEnd(bool commit)
		{
		}

		// Token: 0x06006E00 RID: 28160 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnCommitWithoutStylusInput(bool commit)
		{
		}

		// Token: 0x0400361F RID: 13855
		private bool _disableInput;
	}
}
