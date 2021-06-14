using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Internal
{
	// Token: 0x02000118 RID: 280
	internal abstract class VersioningList<TElement> : IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600054C RID: 1356
		public abstract int Count { get; }

		// Token: 0x17000241 RID: 577
		public TElement this[int index]
		{
			get
			{
				if ((ulong)index >= (ulong)((long)this.Count))
				{
					throw new IndexOutOfRangeException();
				}
				return this.IndexedElement(index);
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
		public static VersioningList<TElement> Create()
		{
			return new VersioningList<TElement>.EmptyVersioningList();
		}

		// Token: 0x0600054F RID: 1359
		public abstract VersioningList<TElement> Add(TElement value);

		// Token: 0x06000550 RID: 1360 RVA: 0x0000D2B7 File Offset: 0x0000B4B7
		public VersioningList<TElement> RemoveAt(int index)
		{
			if ((ulong)index >= (ulong)((long)this.Count))
			{
				throw new IndexOutOfRangeException();
			}
			return this.RemoveIndexedElement(index);
		}

		// Token: 0x06000551 RID: 1361
		public abstract IEnumerator<TElement> GetEnumerator();

		// Token: 0x06000552 RID: 1362 RVA: 0x0000D2D1 File Offset: 0x0000B4D1
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000553 RID: 1363
		protected abstract TElement IndexedElement(int index);

		// Token: 0x06000554 RID: 1364
		protected abstract VersioningList<TElement> RemoveIndexedElement(int index);

		// Token: 0x02000119 RID: 281
		internal sealed class EmptyVersioningList : VersioningList<TElement>
		{
			// Token: 0x17000242 RID: 578
			// (get) Token: 0x06000556 RID: 1366 RVA: 0x0000D2E1 File Offset: 0x0000B4E1
			public override int Count
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x06000557 RID: 1367 RVA: 0x0000D2E4 File Offset: 0x0000B4E4
			public override VersioningList<TElement> Add(TElement value)
			{
				return new VersioningList<TElement>.LinkedVersioningList(this, value);
			}

			// Token: 0x06000558 RID: 1368 RVA: 0x0000D2ED File Offset: 0x0000B4ED
			public override IEnumerator<TElement> GetEnumerator()
			{
				return new VersioningList<TElement>.EmptyListEnumerator();
			}

			// Token: 0x06000559 RID: 1369 RVA: 0x0000D2F4 File Offset: 0x0000B4F4
			protected override TElement IndexedElement(int index)
			{
				throw new IndexOutOfRangeException();
			}

			// Token: 0x0600055A RID: 1370 RVA: 0x0000D2FB File Offset: 0x0000B4FB
			protected override VersioningList<TElement> RemoveIndexedElement(int index)
			{
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x0200011A RID: 282
		internal sealed class EmptyListEnumerator : IEnumerator<TElement>, IDisposable, IEnumerator
		{
			// Token: 0x17000243 RID: 579
			// (get) Token: 0x0600055C RID: 1372 RVA: 0x0000D30C File Offset: 0x0000B50C
			public TElement Current
			{
				get
				{
					return default(TElement);
				}
			}

			// Token: 0x17000244 RID: 580
			// (get) Token: 0x0600055D RID: 1373 RVA: 0x0000D322 File Offset: 0x0000B522
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0600055E RID: 1374 RVA: 0x0000D32F File Offset: 0x0000B52F
			public void Dispose()
			{
			}

			// Token: 0x0600055F RID: 1375 RVA: 0x0000D331 File Offset: 0x0000B531
			public bool MoveNext()
			{
				return false;
			}

			// Token: 0x06000560 RID: 1376 RVA: 0x0000D334 File Offset: 0x0000B534
			public void Reset()
			{
			}
		}

		// Token: 0x0200011B RID: 283
		internal sealed class LinkedVersioningList : VersioningList<TElement>
		{
			// Token: 0x06000562 RID: 1378 RVA: 0x0000D33E File Offset: 0x0000B53E
			public LinkedVersioningList(VersioningList<TElement> preceding, TElement last)
			{
				this.preceding = preceding;
				this.last = last;
			}

			// Token: 0x17000245 RID: 581
			// (get) Token: 0x06000563 RID: 1379 RVA: 0x0000D354 File Offset: 0x0000B554
			public override int Count
			{
				get
				{
					return this.preceding.Count + 1;
				}
			}

			// Token: 0x17000246 RID: 582
			// (get) Token: 0x06000564 RID: 1380 RVA: 0x0000D363 File Offset: 0x0000B563
			public VersioningList<TElement> Preceding
			{
				get
				{
					return this.preceding;
				}
			}

			// Token: 0x17000247 RID: 583
			// (get) Token: 0x06000565 RID: 1381 RVA: 0x0000D36B File Offset: 0x0000B56B
			public TElement Last
			{
				get
				{
					return this.last;
				}
			}

			// Token: 0x17000248 RID: 584
			// (get) Token: 0x06000566 RID: 1382 RVA: 0x0000D374 File Offset: 0x0000B574
			private int Depth
			{
				get
				{
					int num = 0;
					for (VersioningList<TElement>.LinkedVersioningList linkedVersioningList = this; linkedVersioningList != null; linkedVersioningList = (linkedVersioningList.Preceding as VersioningList<TElement>.LinkedVersioningList))
					{
						num++;
					}
					return num;
				}
			}

			// Token: 0x06000567 RID: 1383 RVA: 0x0000D39B File Offset: 0x0000B59B
			public override VersioningList<TElement> Add(TElement value)
			{
				if (this.Depth < 5)
				{
					return new VersioningList<TElement>.LinkedVersioningList(this, value);
				}
				return new VersioningList<TElement>.ArrayVersioningList(this, value);
			}

			// Token: 0x06000568 RID: 1384 RVA: 0x0000D3B5 File Offset: 0x0000B5B5
			public override IEnumerator<TElement> GetEnumerator()
			{
				return new VersioningList<TElement>.LinkedListEnumerator(this);
			}

			// Token: 0x06000569 RID: 1385 RVA: 0x0000D3BD File Offset: 0x0000B5BD
			protected override TElement IndexedElement(int index)
			{
				if (index == this.Count - 1)
				{
					return this.last;
				}
				return this.preceding.IndexedElement(index);
			}

			// Token: 0x0600056A RID: 1386 RVA: 0x0000D3DD File Offset: 0x0000B5DD
			protected override VersioningList<TElement> RemoveIndexedElement(int index)
			{
				if (index == this.Count - 1)
				{
					return this.preceding;
				}
				return new VersioningList<TElement>.LinkedVersioningList(this.preceding.RemoveIndexedElement(index), this.last);
			}

			// Token: 0x040001F3 RID: 499
			private readonly VersioningList<TElement> preceding;

			// Token: 0x040001F4 RID: 500
			private readonly TElement last;
		}

		// Token: 0x0200011C RID: 284
		internal sealed class LinkedListEnumerator : IEnumerator<TElement>, IDisposable, IEnumerator
		{
			// Token: 0x0600056B RID: 1387 RVA: 0x0000D408 File Offset: 0x0000B608
			public LinkedListEnumerator(VersioningList<TElement>.LinkedVersioningList list)
			{
				this.list = list;
				this.preceding = list.Preceding.GetEnumerator();
			}

			// Token: 0x17000249 RID: 585
			// (get) Token: 0x0600056C RID: 1388 RVA: 0x0000D428 File Offset: 0x0000B628
			public TElement Current
			{
				get
				{
					if (this.complete)
					{
						return this.list.Last;
					}
					return this.preceding.Current;
				}
			}

			// Token: 0x1700024A RID: 586
			// (get) Token: 0x0600056D RID: 1389 RVA: 0x0000D449 File Offset: 0x0000B649
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0600056E RID: 1390 RVA: 0x0000D456 File Offset: 0x0000B656
			public void Dispose()
			{
			}

			// Token: 0x0600056F RID: 1391 RVA: 0x0000D458 File Offset: 0x0000B658
			public bool MoveNext()
			{
				if (this.complete)
				{
					return false;
				}
				if (!this.preceding.MoveNext())
				{
					this.complete = true;
				}
				return true;
			}

			// Token: 0x06000570 RID: 1392 RVA: 0x0000D479 File Offset: 0x0000B679
			public void Reset()
			{
				this.preceding.Reset();
				this.complete = false;
			}

			// Token: 0x040001F5 RID: 501
			private readonly VersioningList<TElement>.LinkedVersioningList list;

			// Token: 0x040001F6 RID: 502
			private IEnumerator<TElement> preceding;

			// Token: 0x040001F7 RID: 503
			private bool complete;
		}

		// Token: 0x0200011D RID: 285
		internal sealed class ArrayVersioningList : VersioningList<TElement>
		{
			// Token: 0x06000571 RID: 1393 RVA: 0x0000D490 File Offset: 0x0000B690
			public ArrayVersioningList(VersioningList<TElement> preceding, TElement last)
			{
				this.elements = new TElement[preceding.Count + 1];
				int num = 0;
				foreach (TElement telement in preceding)
				{
					this.elements[num++] = telement;
				}
				this.elements[num] = last;
			}

			// Token: 0x06000572 RID: 1394 RVA: 0x0000D50C File Offset: 0x0000B70C
			private ArrayVersioningList(TElement[] elements)
			{
				this.elements = elements;
			}

			// Token: 0x1700024B RID: 587
			// (get) Token: 0x06000573 RID: 1395 RVA: 0x0000D51B File Offset: 0x0000B71B
			public override int Count
			{
				get
				{
					return this.elements.Length;
				}
			}

			// Token: 0x06000574 RID: 1396 RVA: 0x0000D525 File Offset: 0x0000B725
			public TElement ElementAt(int index)
			{
				return this.elements[index];
			}

			// Token: 0x06000575 RID: 1397 RVA: 0x0000D533 File Offset: 0x0000B733
			public override VersioningList<TElement> Add(TElement value)
			{
				return new VersioningList<TElement>.LinkedVersioningList(this, value);
			}

			// Token: 0x06000576 RID: 1398 RVA: 0x0000D53C File Offset: 0x0000B73C
			public override IEnumerator<TElement> GetEnumerator()
			{
				return new VersioningList<TElement>.ArrayListEnumerator(this);
			}

			// Token: 0x06000577 RID: 1399 RVA: 0x0000D544 File Offset: 0x0000B744
			protected override TElement IndexedElement(int index)
			{
				return this.elements[index];
			}

			// Token: 0x06000578 RID: 1400 RVA: 0x0000D554 File Offset: 0x0000B754
			protected override VersioningList<TElement> RemoveIndexedElement(int index)
			{
				if (this.elements.Length == 1)
				{
					return new VersioningList<TElement>.EmptyVersioningList();
				}
				int num = 0;
				TElement[] array = new TElement[this.elements.Length - 1];
				for (int i = 0; i < this.elements.Length; i++)
				{
					if (i != index)
					{
						array[num++] = this.elements[i];
					}
				}
				return new VersioningList<TElement>.ArrayVersioningList(array);
			}

			// Token: 0x040001F8 RID: 504
			private readonly TElement[] elements;
		}

		// Token: 0x0200011E RID: 286
		internal sealed class ArrayListEnumerator : IEnumerator<TElement>, IDisposable, IEnumerator
		{
			// Token: 0x06000579 RID: 1401 RVA: 0x0000D5B8 File Offset: 0x0000B7B8
			public ArrayListEnumerator(VersioningList<TElement>.ArrayVersioningList array)
			{
				this.array = array;
			}

			// Token: 0x1700024C RID: 588
			// (get) Token: 0x0600057A RID: 1402 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
			public TElement Current
			{
				get
				{
					if (this.index <= this.array.Count)
					{
						return this.array.ElementAt(this.index - 1);
					}
					return default(TElement);
				}
			}

			// Token: 0x1700024D RID: 589
			// (get) Token: 0x0600057B RID: 1403 RVA: 0x0000D605 File Offset: 0x0000B805
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0600057C RID: 1404 RVA: 0x0000D612 File Offset: 0x0000B812
			public void Dispose()
			{
			}

			// Token: 0x0600057D RID: 1405 RVA: 0x0000D614 File Offset: 0x0000B814
			public bool MoveNext()
			{
				int count = this.array.Count;
				if (this.index <= count)
				{
					this.index++;
				}
				return this.index <= count;
			}

			// Token: 0x0600057E RID: 1406 RVA: 0x0000D650 File Offset: 0x0000B850
			public void Reset()
			{
				this.index = 0;
			}

			// Token: 0x040001F9 RID: 505
			private readonly VersioningList<TElement>.ArrayVersioningList array;

			// Token: 0x040001FA RID: 506
			private int index;
		}
	}
}
