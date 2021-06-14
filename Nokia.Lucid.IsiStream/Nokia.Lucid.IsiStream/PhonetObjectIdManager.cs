using System;
using System.Collections.Generic;
using System.Linq;

namespace Nokia.Lucid.IsiStream
{
	// Token: 0x02000007 RID: 7
	internal static class PhonetObjectIdManager
	{
		// Token: 0x06000049 RID: 73 RVA: 0x000027B8 File Offset: 0x000009B8
		public static byte LeaseObjectId()
		{
			byte result;
			try
			{
				int num = Enumerable.Range(1, 239).Except(PhonetObjectIdManager.ObjectIds).First<int>();
				if (num != 0)
				{
					PhonetObjectIdManager.ObjectIds.Add(num);
				}
				result = (byte)num;
			}
			catch (Exception)
			{
				throw new IndexOutOfRangeException("Max amount of streams already created, all ObjectIds already in use");
			}
			return result;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002810 File Offset: 0x00000A10
		public static void ReleaseObjectId(byte objectId)
		{
			PhonetObjectIdManager.ObjectIds.Remove((int)objectId);
		}

		// Token: 0x0400001A RID: 26
		private const byte MinObjectIdValue = 1;

		// Token: 0x0400001B RID: 27
		private const byte MaxObjectIdValue = 239;

		// Token: 0x0400001C RID: 28
		private static readonly List<int> ObjectIds = new List<int>();
	}
}
