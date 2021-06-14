using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SoftwareRepository.Streaming
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	internal class DownloadMetadata
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002ABC File Offset: 0x00000CBC
		internal byte[] Serialize()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					new BinaryFormatter().Serialize(memoryStream, this);
				}
				catch
				{
					return null;
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002B14 File Offset: 0x00000D14
		internal static DownloadMetadata Deserialize(byte[] data)
		{
			DownloadMetadata downloadMetadata;
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				try
				{
					downloadMetadata = (new BinaryFormatter().Deserialize(memoryStream) as DownloadMetadata);
				}
				catch
				{
					return null;
				}
			}
			if (downloadMetadata == null || !downloadMetadata.IsValid())
			{
				return null;
			}
			return downloadMetadata;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002B78 File Offset: 0x00000D78
		private bool IsValid()
		{
			if (this.ChunkStates == null)
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < this.ChunkStates.Length; i++)
			{
				if (this.ChunkStates[i] == ChunkState.PartiallyDownloaded)
				{
					num++;
					if (this.PartialProgress == null || !this.PartialProgress.ContainsKey(i))
					{
						return false;
					}
				}
			}
			return this.PartialProgress == null || num == this.PartialProgress.Count;
		}

		// Token: 0x04000028 RID: 40
		internal ChunkState[] ChunkStates;

		// Token: 0x04000029 RID: 41
		internal Dictionary<int, long> PartialProgress;
	}
}
