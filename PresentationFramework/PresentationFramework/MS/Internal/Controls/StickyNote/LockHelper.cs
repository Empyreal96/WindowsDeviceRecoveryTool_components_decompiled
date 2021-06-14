using System;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x02000768 RID: 1896
	internal class LockHelper
	{
		// Token: 0x06007869 RID: 30825 RVA: 0x00225179 File Offset: 0x00223379
		public bool IsLocked(LockHelper.LockFlag flag)
		{
			return (this._backingStore & flag) > (LockHelper.LockFlag)0;
		}

		// Token: 0x0600786A RID: 30826 RVA: 0x00225186 File Offset: 0x00223386
		private void Lock(LockHelper.LockFlag flag)
		{
			this._backingStore |= flag;
		}

		// Token: 0x0600786B RID: 30827 RVA: 0x00225196 File Offset: 0x00223396
		private void Unlock(LockHelper.LockFlag flag)
		{
			this._backingStore &= ~flag;
		}

		// Token: 0x04003909 RID: 14601
		private LockHelper.LockFlag _backingStore;

		// Token: 0x02000B6B RID: 2923
		[Flags]
		public enum LockFlag
		{
			// Token: 0x04004B48 RID: 19272
			AnnotationChanged = 1,
			// Token: 0x04004B49 RID: 19273
			DataChanged = 2
		}

		// Token: 0x02000B6C RID: 2924
		public class AutoLocker : IDisposable
		{
			// Token: 0x06008E18 RID: 36376 RVA: 0x0025B420 File Offset: 0x00259620
			public AutoLocker(LockHelper helper, LockHelper.LockFlag flag)
			{
				if (helper == null)
				{
					throw new ArgumentNullException("helper");
				}
				this._helper = helper;
				this._flag = flag;
				this._helper.Lock(this._flag);
			}

			// Token: 0x06008E19 RID: 36377 RVA: 0x0025B455 File Offset: 0x00259655
			public void Dispose()
			{
				this._helper.Unlock(this._flag);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06008E1A RID: 36378 RVA: 0x0000326D File Offset: 0x0000146D
			private AutoLocker()
			{
			}

			// Token: 0x04004B4A RID: 19274
			private LockHelper _helper;

			// Token: 0x04004B4B RID: 19275
			private LockHelper.LockFlag _flag;
		}
	}
}
