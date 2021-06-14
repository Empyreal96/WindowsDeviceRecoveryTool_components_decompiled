using System;
using System.IO;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x02000068 RID: 104
	internal class TarWriter : LegacyTarWriter
	{
		// Token: 0x06000479 RID: 1145 RVA: 0x000203DE File Offset: 0x0001F3DE
		public TarWriter(Stream writeStream, int codepage, DoOnStreamOperationFailureDelegate writeToStreamFailureDelegate, DoOnStreamOperationFailureDelegate readFromStreamFailureDelegate) : base(writeStream, codepage, writeToStreamFailureDelegate, readFromStreamFailureDelegate)
		{
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000203EC File Offset: 0x0001F3EC
		internal override void WriteHeader(string name, DateTime lastModificationTime, long count, int userId, int groupId, int mode, char typeFlag)
		{
			UnixStandartHeader unixStandartHeader = new UnixStandartHeader(this._codePage);
			unixStandartHeader.FileName = name;
			unixStandartHeader.LastModification = lastModificationTime;
			unixStandartHeader.SizeInBytes = count;
			unixStandartHeader.UserId = userId;
			unixStandartHeader.UserName = Convert.ToString(userId, 8);
			unixStandartHeader.GroupId = groupId;
			unixStandartHeader.GroupName = Convert.ToString(groupId, 8);
			unixStandartHeader.Mode = mode;
			unixStandartHeader.TypeFlag = typeFlag;
			if (!ReadWriteHelper.WriteToStream(unixStandartHeader.GetHeaderValue(), 0, unixStandartHeader.HeaderSize, this.OutStream, this._writeToStreamFailureDelegate))
			{
				throw ExceptionBuilder.Exception(ErrorCode.WriteToTheStreamFailed);
			}
		}
	}
}
