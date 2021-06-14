using System;
using System.Globalization;
using System.Text;

namespace MS.Internal.Xaml.Context
{
	// Token: 0x02000804 RID: 2052
	internal class XamlContextStack<T> where T : XamlFrame
	{
		// Token: 0x06007DF1 RID: 32241 RVA: 0x00234F18 File Offset: 0x00233118
		public XamlContextStack(Func<T> creationDelegate)
		{
			this._creationDelegate = creationDelegate;
			this.Grow();
			this._depth = 0;
		}

		// Token: 0x06007DF2 RID: 32242 RVA: 0x00234F3C File Offset: 0x0023313C
		public XamlContextStack(XamlContextStack<T> source, bool copy)
		{
			this._creationDelegate = source._creationDelegate;
			this._depth = source.Depth;
			if (!copy)
			{
				this._currentFrame = source.CurrentFrame;
				return;
			}
			T t = source.CurrentFrame;
			T t2 = default(T);
			while (t != null)
			{
				T t3 = (T)((object)t.Clone());
				if (this._currentFrame == null)
				{
					this._currentFrame = t3;
				}
				if (t2 != null)
				{
					t2.Previous = t3;
				}
				t2 = t3;
				t = (T)((object)t.Previous);
			}
		}

		// Token: 0x06007DF3 RID: 32243 RVA: 0x00234FE8 File Offset: 0x002331E8
		private void Grow()
		{
			T currentFrame = this._currentFrame;
			this._currentFrame = this._creationDelegate();
			this._currentFrame.Previous = currentFrame;
		}

		// Token: 0x17001D42 RID: 7490
		// (get) Token: 0x06007DF4 RID: 32244 RVA: 0x00235023 File Offset: 0x00233223
		public T CurrentFrame
		{
			get
			{
				return this._currentFrame;
			}
		}

		// Token: 0x17001D43 RID: 7491
		// (get) Token: 0x06007DF5 RID: 32245 RVA: 0x0023502B File Offset: 0x0023322B
		public T PreviousFrame
		{
			get
			{
				return (T)((object)this._currentFrame.Previous);
			}
		}

		// Token: 0x17001D44 RID: 7492
		// (get) Token: 0x06007DF6 RID: 32246 RVA: 0x00235042 File Offset: 0x00233242
		public T PreviousPreviousFrame
		{
			get
			{
				return (T)((object)this._currentFrame.Previous.Previous);
			}
		}

		// Token: 0x06007DF7 RID: 32247 RVA: 0x00235060 File Offset: 0x00233260
		public T GetFrame(int depth)
		{
			T t = this._currentFrame;
			while (t.Depth > depth)
			{
				t = (T)((object)t.Previous);
			}
			return t;
		}

		// Token: 0x06007DF8 RID: 32248 RVA: 0x00235098 File Offset: 0x00233298
		public void PushScope()
		{
			if (this._recycledFrame == null)
			{
				this.Grow();
			}
			else
			{
				T currentFrame = this._currentFrame;
				this._currentFrame = this._recycledFrame;
				this._recycledFrame = (T)((object)this._recycledFrame.Previous);
				this._currentFrame.Previous = currentFrame;
			}
			this._depth++;
		}

		// Token: 0x06007DF9 RID: 32249 RVA: 0x0023510C File Offset: 0x0023330C
		public void PopScope()
		{
			this._depth--;
			T currentFrame = this._currentFrame;
			this._currentFrame = (T)((object)this._currentFrame.Previous);
			currentFrame.Previous = this._recycledFrame;
			this._recycledFrame = currentFrame;
			currentFrame.Reset();
		}

		// Token: 0x17001D45 RID: 7493
		// (get) Token: 0x06007DFA RID: 32250 RVA: 0x00235171 File Offset: 0x00233371
		// (set) Token: 0x06007DFB RID: 32251 RVA: 0x00235179 File Offset: 0x00233379
		public int Depth
		{
			get
			{
				return this._depth;
			}
			set
			{
				this._depth = value;
			}
		}

		// Token: 0x06007DFC RID: 32252 RVA: 0x00235182 File Offset: 0x00233382
		public void Trim()
		{
			this._recycledFrame = default(T);
		}

		// Token: 0x17001D46 RID: 7494
		// (get) Token: 0x06007DFD RID: 32253 RVA: 0x00235190 File Offset: 0x00233390
		public string Frames
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				T currentFrame = this._currentFrame;
				stringBuilder.AppendLine("Stack: " + ((this._currentFrame == null) ? -1 : (this._currentFrame.Depth + 1)).ToString(CultureInfo.InvariantCulture) + " frames");
				this.ShowFrame(stringBuilder, this._currentFrame);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06007DFE RID: 32254 RVA: 0x00235204 File Offset: 0x00233404
		private void ShowFrame(StringBuilder sb, T iteratorFrame)
		{
			if (iteratorFrame == null)
			{
				return;
			}
			if (iteratorFrame.Previous != null)
			{
				this.ShowFrame(sb, (T)((object)iteratorFrame.Previous));
			}
			sb.AppendLine(string.Concat(new object[]
			{
				"  ",
				iteratorFrame.Depth,
				" ",
				iteratorFrame.ToString()
			}));
		}

		// Token: 0x04003B76 RID: 15222
		private int _depth = -1;

		// Token: 0x04003B77 RID: 15223
		private T _currentFrame;

		// Token: 0x04003B78 RID: 15224
		private T _recycledFrame;

		// Token: 0x04003B79 RID: 15225
		private Func<T> _creationDelegate;
	}
}
