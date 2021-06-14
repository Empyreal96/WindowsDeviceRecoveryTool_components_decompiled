using System;
using System.Collections;
using System.Windows;

namespace MS.Internal.Controls
{
	// Token: 0x0200075F RID: 1887
	internal abstract class ModelTreeEnumerator : IEnumerator
	{
		// Token: 0x06007830 RID: 30768 RVA: 0x00224137 File Offset: 0x00222337
		internal ModelTreeEnumerator(object content)
		{
			this._content = content;
		}

		// Token: 0x17001C79 RID: 7289
		// (get) Token: 0x06007831 RID: 30769 RVA: 0x0022414D File Offset: 0x0022234D
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x06007832 RID: 30770 RVA: 0x00224155 File Offset: 0x00222355
		bool IEnumerator.MoveNext()
		{
			return this.MoveNext();
		}

		// Token: 0x06007833 RID: 30771 RVA: 0x0022415D File Offset: 0x0022235D
		void IEnumerator.Reset()
		{
			this.Reset();
		}

		// Token: 0x17001C7A RID: 7290
		// (get) Token: 0x06007834 RID: 30772 RVA: 0x00224165 File Offset: 0x00222365
		protected object Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x17001C7B RID: 7291
		// (get) Token: 0x06007835 RID: 30773 RVA: 0x0022416D File Offset: 0x0022236D
		// (set) Token: 0x06007836 RID: 30774 RVA: 0x00224175 File Offset: 0x00222375
		protected int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x17001C7C RID: 7292
		// (get) Token: 0x06007837 RID: 30775 RVA: 0x0022417E File Offset: 0x0022237E
		protected virtual object Current
		{
			get
			{
				if (this._index == 0)
				{
					return this._content;
				}
				throw new InvalidOperationException(SR.Get("EnumeratorInvalidOperation"));
			}
		}

		// Token: 0x06007838 RID: 30776 RVA: 0x0022419E File Offset: 0x0022239E
		protected virtual bool MoveNext()
		{
			if (this._index < 1)
			{
				this._index++;
				if (this._index == 0)
				{
					this.VerifyUnchanged();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007839 RID: 30777 RVA: 0x002241C8 File Offset: 0x002223C8
		protected virtual void Reset()
		{
			this.VerifyUnchanged();
			this._index = -1;
		}

		// Token: 0x17001C7D RID: 7293
		// (get) Token: 0x0600783A RID: 30778
		protected abstract bool IsUnchanged { get; }

		// Token: 0x0600783B RID: 30779 RVA: 0x002241D7 File Offset: 0x002223D7
		protected void VerifyUnchanged()
		{
			if (!this.IsUnchanged)
			{
				throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
			}
		}

		// Token: 0x040038E9 RID: 14569
		private int _index = -1;

		// Token: 0x040038EA RID: 14570
		private object _content;
	}
}
