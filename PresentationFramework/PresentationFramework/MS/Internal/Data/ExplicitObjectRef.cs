using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000736 RID: 1846
	internal sealed class ExplicitObjectRef : ObjectRef
	{
		// Token: 0x0600760F RID: 30223 RVA: 0x0021AD16 File Offset: 0x00218F16
		internal ExplicitObjectRef(object o)
		{
			if (o is DependencyObject)
			{
				this._element = new WeakReference(o);
				return;
			}
			this._object = o;
		}

		// Token: 0x06007610 RID: 30224 RVA: 0x0021AD3A File Offset: 0x00218F3A
		internal override object GetObject(DependencyObject d, ObjectRefArgs args)
		{
			if (this._element == null)
			{
				return this._object;
			}
			return this._element.Target;
		}

		// Token: 0x17001C1D RID: 7197
		// (get) Token: 0x06007611 RID: 30225 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool ProtectedUsesMentor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007612 RID: 30226 RVA: 0x0021AD56 File Offset: 0x00218F56
		internal override string Identify()
		{
			return "Source";
		}

		// Token: 0x04003855 RID: 14421
		private object _object;

		// Token: 0x04003856 RID: 14422
		private WeakReference _element;
	}
}
