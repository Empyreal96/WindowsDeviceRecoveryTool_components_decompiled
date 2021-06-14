using System;

namespace ComponentAce.Compression.Interfaces
{
	// Token: 0x02000021 RID: 33
	public interface IItemsArray : ICloneable
	{
		// Token: 0x17000028 RID: 40
		IItem this[int index]
		{
			get;
			set;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000151 RID: 337
		int Count { get; }

		// Token: 0x06000152 RID: 338
		void AddItem(IItem item);

		// Token: 0x06000153 RID: 339
		bool FileExists(string fileName, ref int itemNo);

		// Token: 0x06000154 RID: 340
		void DeleteItem(int index);

		// Token: 0x06000155 RID: 341
		void ClearTags();

		// Token: 0x06000156 RID: 342
		void Clear();
	}
}
