using System;
using System.Text;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000006 RID: 6
	public class GptPartition
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002718 File Offset: 0x00000918
		private GptPartition(ulong firstLBA, ulong lastLBA, string name)
		{
			this.FirstLBA = firstLBA;
			this.LastLBA = lastLBA;
			this.Name = name;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002735 File Offset: 0x00000935
		// (set) Token: 0x06000019 RID: 25 RVA: 0x0000273D File Offset: 0x0000093D
		public ulong FirstLBA { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002746 File Offset: 0x00000946
		// (set) Token: 0x0600001B RID: 27 RVA: 0x0000274E File Offset: 0x0000094E
		public ulong LastLBA { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002757 File Offset: 0x00000957
		// (set) Token: 0x0600001D RID: 29 RVA: 0x0000275F File Offset: 0x0000095F
		public string Name { get; private set; }

		// Token: 0x0600001E RID: 30 RVA: 0x00002768 File Offset: 0x00000968
		public static bool ReadFrom(byte[] data, uint partitionCount, uint partitionEntrySize, out GptPartition[] partitions)
		{
			partitions = new GptPartition[partitionCount];
			for (uint num = 0U; num < partitionCount; num += 1U)
			{
				if (!GptPartition.ReadPartition(data, num, partitionEntrySize, out partitions[(int)((UIntPtr)num)]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000027A0 File Offset: 0x000009A0
		private static bool ReadPartition(byte[] data, uint index, uint partitionEntrySize, out GptPartition partition)
		{
			int num = (int)(index * partitionEntrySize);
			num += 32;
			ulong firstLBA = BitConverter.ToUInt64(data, num);
			num += 8;
			ulong lastLBA = BitConverter.ToUInt64(data, num);
			num += 8;
			num += 8;
			int count = (int)(partitionEntrySize - 56U);
			string @string = Encoding.Unicode.GetString(data, num, count);
			char[] trimChars = new char[1];
			string name = @string.TrimEnd(trimChars);
			partition = new GptPartition(firstLBA, lastLBA, name);
			return true;
		}
	}
}
