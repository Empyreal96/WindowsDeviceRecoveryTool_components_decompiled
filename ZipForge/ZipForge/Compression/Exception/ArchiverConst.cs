using System;

namespace ComponentAce.Compression.Exception
{
	// Token: 0x02000002 RID: 2
	internal class ArchiverConst
	{
		// Token: 0x04000001 RID: 1
		internal const string SConfirmOverwrite = "Overwrite file \"{0}\" with \"{1}\"";

		// Token: 0x04000002 RID: 2
		internal const string SPasswordTitle = "Password for \"{0}\"";

		// Token: 0x04000003 RID: 3
		internal const string SPasswordPrompt = "Enter password: ";

		// Token: 0x04000004 RID: 4
		internal const string SOnRequestBlankDisk = "Please insert a blank disk #{0}";

		// Token: 0x04000005 RID: 5
		internal const string SOnRequestFirstDisk = "Please insert the first disk";

		// Token: 0x04000006 RID: 6
		internal const string SOnRequestLastDisk = "Please insert the last disk";

		// Token: 0x04000007 RID: 7
		internal const string SOnRequestMiddleDisk = "Please insert disk #{0}";

		// Token: 0x04000008 RID: 8
		internal const string SOnDiskFull = "Disk is full. Required free space: {0} bytes, but available only: {1} bytes. Clean the disk or find another blank disk";

		// Token: 0x04000009 RID: 9
		internal const string SOnProcessFileFailure = "{0}. File processing error, possibly disk is full";

		// Token: 0x0400000A RID: 10
		internal const string SWrongDiskRequestLastDisk = "File \"{0}\" not found on inserted disk. Please insert last disk with required file";

		// Token: 0x0400000B RID: 11
		internal const int MaxError = 65;

		// Token: 0x0400000C RID: 12
		internal static readonly string[] ErrorMessages = new string[]
		{
			"Unknown error",
			"Index out of bounds",
			"Invalid compression mode. Mode must be in [0-9] range",
			"Unexpected null pointer",
			"Invalid size or check sum of file or unsupported compression format",
			"File name is blank. Specify appropriate file name",
			"File \"{0}\" not found",
			"Archive is not open",
			"SFXStub property is not specified",
			"Cannot create file \"{0}\"",
			"Cannot create directory \"{0}\"",
			"Internal error. Update is not started",
			"Cannot open file \"{0}\"",
			"Cannot proceed when update is not ended. Preceding EndUpdate call is required.",
			"Cannot delete file \"{0}\"",
			"In-memory archive can be created only",
			"File is open in read-only mode",
			"Invalid compressed size, rfs.size = {0}, count = {1}",
			"Invalid archive file.",
			"Cannot create output file. Probably file is locked by another process. FileName = \"{0}\".",
			"Archive is open. You should close it before performing this operation.",
			"Unable to create directory",
			"Cannot find Zip64 directory end record",
			"Cannot compress file \"{0}\". Zip64 mode is not enabled",
			"Cannot open archive file. Opening aborted or invalid file",
			"Write to stream error",
			"Cannot place SFX stub on volume. Volume size limit is too small",
			"Archive is damaged. Open failed",
			"MakeSFX is not allowed for splitted or spanned archives. Set SFXStub before archive creation instead of MakeSFX",
			"Archive already has SFX stub",
			"Multi-volume archive is not allowed for custom stream",
			"Multi-spanned archive is not allowed for modification. Use BeginUpdate/EndUpdate methods to add files several times",
			"Incorrect password is specified for the encrypted archive",
			"Invalid volume name",
			"The unicode extra field signature is incorrect",
			"The extra field ID value is incorrect. Possibly the archive is damaged",
			"The OnRequestBlankVolume event handler doesn't exists. You should handle this event to work with spanning archives.",
			"The OnRequestFirstVolume event handler doesn't exists. You should handle this event to work with spanning archives.",
			"The OnRequestMiddleVolume event handler doesn't exists. You should handle this event to work with spanning archives.",
			"The OnRequestLastVolume event handler doesn't exists. You should handle this event to work with spanning archives.",
			"The disk is full",
			"Compression engine is not initialized",
			"Unknown compression method",
			"Unknown or not supported encryption algorithm",
			"An invalid file header object. You should invoke GetFileHeader method before trying to extract the file.",
			"The stream {0} does not support writing.",
			"The stream {0} does not support reading.",
			"File name {0} already exists in the archive. You can use OnFileRename event to modify the file name in runtime.",
			"Error occurred during trying to save stream to the archive.",
			"Error occurs during file compression. Not all data was compressed.",
			"Unexpected null pointer at the {0} variable.",
			"Expected local header or central directory signature but {0} value was found.",
			"Invalid read from stream bytes count. Expected {0}, actual {1}.",
			"Read from stream failed.",
			"Write to stream failed.",
			"You are trying to add more than one file to gzip archive. In that case you should set CreateSeparateArchivers to true.",
			"Can not write to the closed writer.",
			"You are trying to change file while not all the data from the previous one was read. If you do want to skip files use skipData parameter set to true.",
			"Expected to read 512 bytes of header, but actual {0} bytes was read.",
			"Tar archive is broken.",
			"Specified name '{0}' is too long. It must be less than {1} symbols.",
			"File name was not specified. Set the file name before trying to store file.",
			"File name you are trying to set is null or empty.",
			"File name can not be empty.",
			"Compressed stream should be specified by the CompressedStream property or it should be initialized in the constructor.",
			"OutputFilesDir property should be specified."
		};
	}
}
