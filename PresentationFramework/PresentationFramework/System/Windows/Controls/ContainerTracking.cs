using System;
using System.Diagnostics;

namespace System.Windows.Controls
{
	// Token: 0x02000487 RID: 1159
	internal class ContainerTracking<T>
	{
		// Token: 0x060043C9 RID: 17353 RVA: 0x00135C99 File Offset: 0x00133E99
		internal ContainerTracking(T container)
		{
			this._container = container;
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x060043CA RID: 17354 RVA: 0x00135CA8 File Offset: 0x00133EA8
		internal T Container
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x060043CB RID: 17355 RVA: 0x00135CB0 File Offset: 0x00133EB0
		internal ContainerTracking<T> Next
		{
			get
			{
				return this._next;
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x060043CC RID: 17356 RVA: 0x00135CB8 File Offset: 0x00133EB8
		internal ContainerTracking<T> Previous
		{
			get
			{
				return this._previous;
			}
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x00135CC0 File Offset: 0x00133EC0
		internal void StartTracking(ref ContainerTracking<T> root)
		{
			if (root != null)
			{
				root._previous = this;
			}
			this._next = root;
			root = this;
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x00135CDC File Offset: 0x00133EDC
		internal void StopTracking(ref ContainerTracking<T> root)
		{
			if (this._previous != null)
			{
				this._previous._next = this._next;
			}
			if (this._next != null)
			{
				this._next._previous = this._previous;
			}
			if (root == this)
			{
				root = this._next;
			}
			this._previous = null;
			this._next = null;
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		internal void Debug_AssertIsInList(ContainerTracking<T> root)
		{
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		internal void Debug_AssertNotInList(ContainerTracking<T> root)
		{
		}

		// Token: 0x04002863 RID: 10339
		private T _container;

		// Token: 0x04002864 RID: 10340
		private ContainerTracking<T> _next;

		// Token: 0x04002865 RID: 10341
		private ContainerTracking<T> _previous;
	}
}
