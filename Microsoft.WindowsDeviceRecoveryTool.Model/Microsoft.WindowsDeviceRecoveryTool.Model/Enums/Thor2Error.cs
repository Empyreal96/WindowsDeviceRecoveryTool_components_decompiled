using System;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Enums
{
	// Token: 0x0200002C RID: 44
	public class Thor2Error
	{
		// Token: 0x06000138 RID: 312 RVA: 0x0000453C File Offset: 0x0000273C
		public static Thor2ErrorType GetErrorType(Thor2ExitCode exitCode)
		{
			Thor2ErrorType result;
			if (exitCode == Thor2ExitCode.Thor2AllOk)
			{
				Tracer<Thor2Error>.WriteInformation("Flash process exited with no error");
				result = Thor2ErrorType.NoError;
			}
			else if (exitCode >= Thor2ExitCode.Thor2ErrorConnectionNotFound && exitCode < (Thor2ExitCode)85000U)
			{
				Tracer<Thor2Error>.WriteError("THOR2 error occured", new object[0]);
				result = Thor2ErrorType.Thor2Error;
			}
			else if (exitCode >= (Thor2ExitCode)131072U && exitCode < (Thor2ExitCode)196608U)
			{
				Tracer<Thor2Error>.WriteError("File error occured", new object[0]);
				result = Thor2ErrorType.FileError;
			}
			else if (exitCode >= (Thor2ExitCode)196608U && exitCode < (Thor2ExitCode)262144U)
			{
				Tracer<Thor2Error>.WriteError("Device error occured", new object[0]);
				result = Thor2ErrorType.DeviceError;
			}
			else if (exitCode >= (Thor2ExitCode)262144U && exitCode < (Thor2ExitCode)327680U)
			{
				Tracer<Thor2Error>.WriteError("Message error occured", new object[0]);
				result = Thor2ErrorType.MessageError;
			}
			else if (exitCode >= (Thor2ExitCode)327680U && exitCode < Thor2ExitCode.DevReportedErrorDuringSffuProgramming)
			{
				Tracer<Thor2Error>.WriteError("Messaging error occured", new object[0]);
				result = Thor2ErrorType.MessagingError;
			}
			else if (exitCode >= Thor2ExitCode.DevReportedErrorDuringSffuProgramming && exitCode < Thor2ExitCode.FfuParsingError)
			{
				Tracer<Thor2Error>.WriteError("Device reported ver 2 error during FFU programming", new object[0]);
				result = Thor2ErrorType.FfuProgrammingVer2Error;
			}
			else if (exitCode >= Thor2ExitCode.FfuParsingError && exitCode < (Thor2ExitCode)2293760U)
			{
				Tracer<Thor2Error>.WriteError("FFU parsing error occured", new object[0]);
				result = Thor2ErrorType.FfuParsingError;
			}
			else if (exitCode >= (Thor2ExitCode)4194304000U && exitCode < (Thor2ExitCode)4211081215U)
			{
				Tracer<Thor2Error>.WriteError("FlashApp error occured", new object[0]);
				result = Thor2ErrorType.FlashAppError;
			}
			else
			{
				Tracer<Thor2Error>.WriteError("Unhandled error occured", new object[0]);
				result = Thor2ErrorType.UnhandledError;
			}
			return result;
		}
	}
}
