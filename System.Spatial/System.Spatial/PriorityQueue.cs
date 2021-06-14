using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x02000041 RID: 65
	internal class PriorityQueue<TValue>
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000503C File Offset: 0x0000323C
		public int Count
		{
			get
			{
				return this.data.Count;
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000504C File Offset: 0x0000324C
		public TValue Peek()
		{
			if (this.data.Count == 0)
			{
				throw new InvalidOperationException(Strings.PriorityQueueOperationNotValidOnEmptyQueue);
			}
			return (TValue)((object)this.data[0].Value);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000050B0 File Offset: 0x000032B0
		public void Enqueue(double priority, TValue value)
		{
			this.data.Add(new KeyValuePair<double, object>(priority, value));
			this.data.Sort((KeyValuePair<double, object> lhs, KeyValuePair<double, object> rhs) => -lhs.Key.CompareTo(rhs.Key));
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00005118 File Offset: 0x00003318
		public bool Contains(double priority)
		{
			return this.data.Any((KeyValuePair<double, object> v) => v.Key == priority);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000514C File Offset: 0x0000334C
		public TValue DequeueByPriority(double priority)
		{
			foreach (KeyValuePair<double, object> item in this.data)
			{
				if (item.Key == priority)
				{
					this.data.Remove(item);
					return (TValue)((object)item.Value);
				}
			}
			throw new InvalidOperationException(Strings.PriorityQueueDoesNotContainItem(priority));
		}

		// Token: 0x0400003E RID: 62
		private readonly List<KeyValuePair<double, object>> data = new List<KeyValuePair<double, object>>();
	}
}
