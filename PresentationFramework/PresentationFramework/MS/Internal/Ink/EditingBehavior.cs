using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MS.Internal.Ink
{
	// Token: 0x02000684 RID: 1668
	internal abstract class EditingBehavior
	{
		// Token: 0x06006D1D RID: 27933 RVA: 0x001F5AA4 File Offset: 0x001F3CA4
		internal EditingBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas)
		{
			if (inkCanvas == null)
			{
				throw new ArgumentNullException("inkCanvas");
			}
			if (editingCoordinator == null)
			{
				throw new ArgumentNullException("editingCoordinator");
			}
			this._inkCanvas = inkCanvas;
			this._editingCoordinator = editingCoordinator;
		}

		// Token: 0x06006D1E RID: 27934 RVA: 0x001F5AD6 File Offset: 0x001F3CD6
		public void Activate()
		{
			this.OnActivate();
		}

		// Token: 0x06006D1F RID: 27935 RVA: 0x001F5ADE File Offset: 0x001F3CDE
		public void Deactivate()
		{
			this.OnDeactivate();
		}

		// Token: 0x06006D20 RID: 27936 RVA: 0x001F5AE6 File Offset: 0x001F3CE6
		public void Commit(bool commit)
		{
			this.OnCommit(commit);
		}

		// Token: 0x06006D21 RID: 27937 RVA: 0x001F5AEF File Offset: 0x001F3CEF
		public void UpdateTransform()
		{
			if (!this.EditingCoordinator.IsTransformValid(this))
			{
				this.OnTransformChanged();
			}
		}

		// Token: 0x17001A08 RID: 6664
		// (get) Token: 0x06006D22 RID: 27938 RVA: 0x001F5B05 File Offset: 0x001F3D05
		public Cursor Cursor
		{
			get
			{
				if (this._cachedCursor == null || !this.EditingCoordinator.IsCursorValid(this))
				{
					this._cachedCursor = this.GetCurrentCursor();
				}
				return this._cachedCursor;
			}
		}

		// Token: 0x06006D23 RID: 27939
		protected abstract void OnActivate();

		// Token: 0x06006D24 RID: 27940
		protected abstract void OnDeactivate();

		// Token: 0x06006D25 RID: 27941
		protected abstract void OnCommit(bool commit);

		// Token: 0x06006D26 RID: 27942
		protected abstract Cursor GetCurrentCursor();

		// Token: 0x06006D27 RID: 27943 RVA: 0x001F5B2F File Offset: 0x001F3D2F
		protected void SelfDeactivate()
		{
			this.EditingCoordinator.DeactivateDynamicBehavior();
		}

		// Token: 0x06006D28 RID: 27944 RVA: 0x001F5B3C File Offset: 0x001F3D3C
		protected Matrix GetElementTransformMatrix()
		{
			Transform layoutTransform = this.InkCanvas.LayoutTransform;
			Transform renderTransform = this.InkCanvas.RenderTransform;
			Matrix value = layoutTransform.Value;
			return value * renderTransform.Value;
		}

		// Token: 0x06006D29 RID: 27945 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnTransformChanged()
		{
		}

		// Token: 0x17001A09 RID: 6665
		// (get) Token: 0x06006D2A RID: 27946 RVA: 0x001F5B76 File Offset: 0x001F3D76
		protected InkCanvas InkCanvas
		{
			get
			{
				return this._inkCanvas;
			}
		}

		// Token: 0x17001A0A RID: 6666
		// (get) Token: 0x06006D2B RID: 27947 RVA: 0x001F5B7E File Offset: 0x001F3D7E
		protected EditingCoordinator EditingCoordinator
		{
			get
			{
				return this._editingCoordinator;
			}
		}

		// Token: 0x040035D3 RID: 13779
		private InkCanvas _inkCanvas;

		// Token: 0x040035D4 RID: 13780
		private EditingCoordinator _editingCoordinator;

		// Token: 0x040035D5 RID: 13781
		private Cursor _cachedCursor;
	}
}
