using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200003E RID: 62
	internal sealed class EnumDefinitionIdentity : IEnumerator
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00006E3E File Offset: 0x0000503E
		internal EnumDefinitionIdentity(IEnumDefinitionIdentity e)
		{
			if (e == null)
			{
				throw new ArgumentNullException();
			}
			this._enum = e;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006E62 File Offset: 0x00005062
		private DefinitionIdentity GetCurrent()
		{
			if (this._current == null)
			{
				throw new InvalidOperationException();
			}
			return new DefinitionIdentity(this._current);
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00006E7D File Offset: 0x0000507D
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00006E7D File Offset: 0x0000507D
		public DefinitionIdentity Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000069BD File Offset: 0x00004BBD
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006E85 File Offset: 0x00005085
		public bool MoveNext()
		{
			if (this._enum.Next(1U, this._fetchList) == 1U)
			{
				this._current = this._fetchList[0];
				return true;
			}
			this._current = null;
			return false;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006EB4 File Offset: 0x000050B4
		public void Reset()
		{
			this._current = null;
			this._enum.Reset();
		}

		// Token: 0x0400012D RID: 301
		private IEnumDefinitionIdentity _enum;

		// Token: 0x0400012E RID: 302
		private IDefinitionIdentity _current;

		// Token: 0x0400012F RID: 303
		private IDefinitionIdentity[] _fetchList = new IDefinitionIdentity[1];
	}
}
