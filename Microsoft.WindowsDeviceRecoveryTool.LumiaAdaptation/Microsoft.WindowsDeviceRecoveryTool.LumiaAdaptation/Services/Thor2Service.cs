using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;

namespace Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Services
{
	// Token: 0x0200000B RID: 11
	[Export]
	public class Thor2Service : IDisposable
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00006469 File Offset: 0x00004669
		[ImportingConstructor]
		public Thor2Service(ProcessManager processManager)
		{
			this.processManager = processManager;
			this.processManager.OnOutputDataReceived += this.Thor2ProcessOnOutputDataReceived;
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600008D RID: 141 RVA: 0x00006494 File Offset: 0x00004694
		// (remove) Token: 0x0600008E RID: 142 RVA: 0x000064D0 File Offset: 0x000046D0
		public event Action<ProgressChangedEventArgs> ProgressChanged;

		// Token: 0x0600008F RID: 143 RVA: 0x0000650C File Offset: 0x0000470C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00006520 File Offset: 0x00004720
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00006548 File Offset: 0x00004748
		public void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<Thor2Service>.LogEntry("FlashDevice");
			int num = 2;
			this.BootPhoneToUefiMode(phone, cancellationToken);
			Thor2ExitCode thor2ExitCode;
			for (;;)
			{
				this.RaiseProgressChangedEvent(0, "FlashingMessageCheckingAntiTheftStatus");
				thor2ExitCode = this.CheckResetProtectionStatus(phone, cancellationToken);
				if (!this.IsPhoneNotRespondingThor2ExitCode(thor2ExitCode))
				{
					goto IL_5B;
				}
				if (num <= 0)
				{
					this.RestartPhoneAndThrowResetProtectionException(phone, this.GetThor2ErrorDesciption(thor2ExitCode));
					goto IL_5B;
				}
				IL_72:
				if (!this.IsPhoneNotRespondingThor2ExitCode(thor2ExitCode) || num-- <= 0)
				{
					break;
				}
				continue;
				IL_5B:
				this.RaiseProgressChangedEvent(0, "FlashingMessageConnectingWithPhone");
				thor2ExitCode = this.RunFlashProcess(phone, cancellationToken);
				goto IL_72;
			}
			this.TryThrowThor2FlashException(thor2ExitCode);
			Tracer<Thor2Service>.LogExit("FlashDevice");
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000065F3 File Offset: 0x000047F3
		public void EmergencyFlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<Thor2Service>.LogEntry("EmergencyFlashDevice");
			this.RaiseProgressChangedEvent(0, "EmergencyFlashingMessage");
			this.RunEmergencyFlashProcess(phone, cancellationToken);
			Tracer<Thor2Service>.LogExit("EmergencyFlashDevice");
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00006624 File Offset: 0x00004824
		private Thor2ExitCode CheckResetProtectionStatus(Phone phone, CancellationToken cancellationToken)
		{
			VplPackageFileInfo vplPackageFileInfo = phone.PackageFileInfo as VplPackageFileInfo;
			if (vplPackageFileInfo == null || string.IsNullOrWhiteSpace(vplPackageFileInfo.FfuFilePath))
			{
				throw new CheckResetProtectionException("Can not initialize Reading Reset Protection Status");
			}
			this.antiTheftVerPhone = (this.antiTheftVerFfu = null);
			Thor2ExitCode thor2ExitCode = this.ReadAntiTheftVersionFromPhone(phone, cancellationToken);
			if (thor2ExitCode == (Thor2ExitCode)3221225477U)
			{
				if (this.lastRawMessageReceived != null)
				{
					long num = long.Parse(this.lastRawMessageReceived.Substring(14, 4), NumberStyles.HexNumber);
					num += (long)((ulong)-100663296);
					Thor2ExitCode thor2ExitCode2 = (Thor2ExitCode)num;
					if (Enum.IsDefined(typeof(Thor2ExitCode), thor2ExitCode2))
					{
						thor2ExitCode = thor2ExitCode2;
					}
				}
			}
			Thor2ExitCode result;
			if (thor2ExitCode == Thor2ExitCode.Thor2ErrorUefiInvalidParameter || thor2ExitCode == (Thor2ExitCode)4194304011U || this.IsPhoneNotRespondingThor2ExitCode(thor2ExitCode))
			{
				result = thor2ExitCode;
			}
			else
			{
				if (thor2ExitCode != Thor2ExitCode.Thor2AllOk)
				{
					this.RestartPhoneAndThrowResetProtectionException(phone, this.GetThor2ErrorDesciption(thor2ExitCode));
				}
				Thor2ExitCode thor2ExitCode3 = this.ReadAntiTheftVersionFromFfu(vplPackageFileInfo.FfuFilePath, cancellationToken);
				if (thor2ExitCode3 == Thor2ExitCode.Thor2NotResponding)
				{
					result = thor2ExitCode;
				}
				else
				{
					if (thor2ExitCode3 != Thor2ExitCode.Thor2AllOk)
					{
						this.RestartPhoneAndThrowResetProtectionException(phone, this.GetThor2ErrorDesciption(thor2ExitCode3));
					}
					if (string.IsNullOrWhiteSpace(this.antiTheftVerFfu) || string.IsNullOrWhiteSpace(this.antiTheftVerPhone))
					{
						this.RestartPhoneAndThrowResetProtectionException(phone, string.Format("Reading Reset Protection status Failed. phone: {0}, ffu: {1}", this.antiTheftVerPhone, this.antiTheftVerFfu));
					}
					if (VersionComparer.CompareVersions(this.antiTheftVerPhone, this.antiTheftVerFfu) > 0)
					{
						this.RestartPhoneAndThrowResetProtectionException(phone, string.Format("Reset Protection status Invalid. phone: {0}, ffu: {1}", this.antiTheftVerPhone, this.antiTheftVerFfu));
					}
					result = Thor2ExitCode.Thor2AllOk;
				}
			}
			return result;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00006800 File Offset: 0x00004A00
		private bool IsPhoneNotRespondingThor2ExitCode(Thor2ExitCode phoneExitCode)
		{
			return phoneExitCode == Thor2ExitCode.Thor2NotResponding || phoneExitCode == Thor2ExitCode.Thor2ErrorConnectionNotFound || phoneExitCode == Thor2ExitCode.Thor2ErrorNoDeviceWithinTimeout || phoneExitCode == Thor2ExitCode.Thor2ErrorBootToFlashAppFailed;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00006833 File Offset: 0x00004A33
		private void RestartPhoneAndThrowResetProtectionException(Phone phone, string exceptionMessage)
		{
			this.RestartPhoneToNormalMode(phone);
			throw new CheckResetProtectionException(exceptionMessage);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006844 File Offset: 0x00004A44
		private string GetThor2ErrorDesciption(Thor2ExitCode exitCode)
		{
			string arg = string.Format("0x{0:X8}", (int)exitCode);
			return string.Format("Reset protection status reading failed with error {0}", string.Format("{0} ({1})", arg, exitCode));
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006884 File Offset: 0x00004A84
		private void BootPhoneToUefiMode(Phone phone, CancellationToken cancellationToken)
		{
			this.RaiseProgressChangedEvent(0, "FlashingMessageChangeToFlashMode");
			string processArguments = string.Format("-mode rnd -bootflashapp -conn {0}", phone.PortId);
			int num = 2;
			Thor2ExitCode thor2ExitCode;
			do
			{
				thor2ExitCode = this.processManager.RunThor2ProcessWithArguments(processArguments, cancellationToken);
			}
			while (thor2ExitCode != Thor2ExitCode.Thor2AllOk && num-- > 0);
			if (thor2ExitCode != Thor2ExitCode.Thor2AllOk)
			{
				throw new FlashModeChangeException("Can not change device to flash mode.");
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000068EC File Offset: 0x00004AEC
		private void RestartPhoneToNormalMode(Phone phone)
		{
			Tracer<Thor2Service>.LogEntry("RestartPhoneToNormalMode");
			string processArguments = string.Format("-mode rnd -reboot -conn {0}", phone.PortId);
			this.processManager.RunThor2ProcessWithArguments(processArguments, CancellationToken.None);
			Tracer<Thor2Service>.LogExit("RestartPhoneToNormalMode");
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00006934 File Offset: 0x00004B34
		private Thor2ExitCode ReadAntiTheftVersionFromPhone(Phone phone, CancellationToken cancellationToken)
		{
			string processArguments = string.Format("-mode rnd -read_reset_protection_status -skip_gpt_check -skip_com_scan -disable_stdout_buffering -conn {0}", phone.PortId);
			return this.processManager.RunThor2ProcessWithArguments(processArguments, cancellationToken);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00006964 File Offset: 0x00004B64
		private Thor2ExitCode ReadAntiTheftVersionFromFfu(string ffuFilePath, CancellationToken cancellationToken)
		{
			string processArguments = string.Format("-mode ffureader -ffufile \"{0}\" -read_antitheft_version -disable_stdout_buffering", ffuFilePath);
			return this.processManager.RunThor2ProcessWithArguments(processArguments, cancellationToken);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00006990 File Offset: 0x00004B90
		private Thor2ExitCode RunFlashProcess(Phone phone, CancellationToken cancellationToken)
		{
			string processArguments = string.Format("-mode vpl -vplfile \"{0}\" -skip_exit_on_post_op_failure -disable_stdout_buffering -conn \"{1}\" -maxtransfersizekb 128 -reboot", phone.PackageFilePath, phone.PortId);
			return this.processManager.RunThor2ProcessWithArguments(processArguments, cancellationToken);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000069C8 File Offset: 0x00004BC8
		private void RunEmergencyFlashProcess(Phone phone, CancellationToken cancellationToken)
		{
			EmergencyPackageInfo emergencyPackageFileInfo = phone.EmergencyPackageFileInfo;
			if (emergencyPackageFileInfo == null)
			{
				throw new Exception("Can not initialize Emergency Flashing process");
			}
			bool flag = emergencyPackageFileInfo.IsAlphaCollins();
			VplPackageFileInfo vplPackageFileInfo = phone.PackageFileInfo as VplPackageFileInfo;
			if (flag && (vplPackageFileInfo == null || string.IsNullOrWhiteSpace(vplPackageFileInfo.FfuFilePath)))
			{
				throw new Exception("Can not initialize Emergency Flashing process");
			}
			if (flag)
			{
				this.RunEmergencyProcessForAlphaCollins(emergencyPackageFileInfo.HexFlasherFilePath, emergencyPackageFileInfo.MbnImageFilePath, vplPackageFileInfo.FfuFilePath, phone.PortId, cancellationToken);
			}
			else
			{
				this.RunEmergencyProcessForQuattro(phone.PortId, emergencyPackageFileInfo, cancellationToken);
			}
			Thread.Sleep(5000);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00006A7C File Offset: 0x00004C7C
		private void RunEmergencyProcessForQuattro(string portId, EmergencyPackageInfo emergencyPackageInfo, CancellationToken cancellationToken)
		{
			string processArguments = string.Format("-mode emergency -conn \"{0}\" -hexfile \"{1}\" -edfile \"{2}\" -skipffuflash -disable_stdout_buffering", portId, emergencyPackageInfo.HexFlasherFilePath, emergencyPackageInfo.EdpImageFilePath);
			Thor2ExitCode thor2ExitCode = this.processManager.RunThor2ProcessWithArguments(processArguments, cancellationToken);
			if (Enum.IsDefined(typeof(Thor2EmergencyV3ExitCodes), (uint)thor2ExitCode))
			{
				Thor2EmergencyV3ExitCodes thor2EmergencyV3ExitCodes = (Thor2EmergencyV3ExitCodes)thor2ExitCode;
				string arg = string.Format("0x{0:X8}", (int)thor2ExitCode);
				string message = string.Format("Flashing failed with error {0}", string.Format("{0} ({1})", arg, thor2EmergencyV3ExitCodes));
				throw new FlashException(message);
			}
			this.TryThrowThor2FlashException(thor2ExitCode);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00006B10 File Offset: 0x00004D10
		private void RunEmergencyProcessForAlphaCollins(string hexFlasherFilePath, string mbnImageFilePath, string ffuFilePath, string portId, CancellationToken cancellationToken)
		{
			string processArguments = string.Format("-mode emergency -hexfile \"{0}\" -mbnfile \"{1}\" -ffufile \"{2}\" -conn \"{3}\" -skipffuflash -disable_stdout_buffering", new object[]
			{
				hexFlasherFilePath,
				mbnImageFilePath,
				ffuFilePath,
				portId
			});
			Thor2ExitCode thor2ExitCode = this.processManager.RunThor2ProcessWithArguments(processArguments, cancellationToken);
			if (Enum.IsDefined(typeof(Thor2EmergencyV1ExitCodes), (uint)thor2ExitCode))
			{
				Thor2EmergencyV1ExitCodes thor2EmergencyV1ExitCodes = (Thor2EmergencyV1ExitCodes)thor2ExitCode;
				string arg = string.Format("0x{0:X8}", (int)thor2ExitCode);
				string message = string.Format("Flashing failed with error {0}", string.Format("{0} ({1})", arg, thor2EmergencyV1ExitCodes));
				throw new FlashException(message);
			}
			this.TryThrowThor2FlashException(thor2ExitCode);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00006BB8 File Offset: 0x00004DB8
		public void TryReadMissingInfoWithThor(Phone phone, CancellationToken token, bool raiseErrors = false)
		{
			Tracer<Thor2Service>.LogEntry("TryReadMissingInfoWithThor");
			this.deviceToFindInfo = phone;
			string processArguments = string.Format("-mode rnd -readphoneinfo -conn \"{0}\" -disable_stdout_buffering", phone.PortId);
			Thor2ExitCode thor2ExitCode = this.processManager.RunThor2ProcessWithArguments(processArguments, token);
			if (raiseErrors && thor2ExitCode != Thor2ExitCode.Thor2AllOk)
			{
				string arg = string.Format("0x{0:X8}", (int)thor2ExitCode);
				string message = string.Format("Reading phone info failed with error {0}", string.Format("{0} ({1})", arg, thor2ExitCode));
				throw new ReadPhoneInformationException(message);
			}
			this.deviceToFindInfo = null;
			Tracer<Thor2Service>.LogExit("TryReadMissingInfoWithThor");
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00006C50 File Offset: 0x00004E50
		public void RestartToNormalMode(Phone phone, CancellationToken token)
		{
			Tracer<Thor2Service>.LogEntry("RestartToNormalMode");
			string processArguments = string.Format("-mode rnd -bootnormalmode -conn \"{0}\" -disable_stdout_buffering", phone.PortId);
			this.processManager.RunThor2ProcessWithArguments(processArguments, token);
			Tracer<Thor2Service>.LogExit("RestartToNormalMode");
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00006CC0 File Offset: 0x00004EC0
		public bool IsDeviceConnected(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<Thor2Service>.LogEntry("IsDeviceConnected");
			this.deviceToFindInfo = phone;
			this.connectionEnumerationStarted = false;
			this.connectionList = new List<string>();
			this.processManager.RunThor2ProcessWithArguments("-mode list_connections -disable_stdout_buffering", cancellationToken);
			this.deviceToFindInfo = null;
			Tracer<Thor2Service>.LogExit("IsDeviceConnected");
			return this.connectionList.Any((string conn) => conn.Contains(phone.PortId));
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00006D44 File Offset: 0x00004F44
		private void TryThrowThor2FlashException(Thor2ExitCode flashProcessExitCode)
		{
			if (flashProcessExitCode == Thor2ExitCode.Thor2ErrorNoDeviceWithinTimeout || flashProcessExitCode == Thor2ExitCode.Thor2NotResponding || flashProcessExitCode == Thor2ExitCode.Thor2ErrorConnectionNotFound)
			{
				Tracer<Thor2Service>.WriteInformation("No Device connected");
				throw new NoDeviceException(string.Format("Flashing failed with error {0}", "No device connected within the Timeout"));
			}
			string text = string.Format("0x{0:X8}", (int)flashProcessExitCode);
			string text2 = string.Format("Flashing failed with error {0}", string.Format("{0} ({1})", text, flashProcessExitCode));
			if (text == "0xFA001106")
			{
				text2 = string.Format("Flashing failed with error {0}", string.Format("{0} ({1})", "0xFA001106", "SoftwareNotCorrectlySigned"));
				throw new SoftwareIsNotCorrectlySignedException(text2);
			}
			Thor2ErrorType errorType = Thor2Error.GetErrorType(flashProcessExitCode);
			if (errorType != Thor2ErrorType.NoError)
			{
				Tracer<Thor2Service>.WriteInformation(text2);
				throw new FlashException(text2);
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00006E18 File Offset: 0x00005018
		private void Thor2ProcessOnOutputDataReceived(DataReceivedEventArgs dataReceivedEventArgs)
		{
			try
			{
				string text = dataReceivedEventArgs.Data;
				if (!string.IsNullOrEmpty(text))
				{
					text = this.EscapeSpecialChars(text);
					bool flag = false;
					Tracer<Thor2Service>.WriteInformation(text);
					if (!string.IsNullOrEmpty(text))
					{
						if (text.Contains("Percents"))
						{
							this.lastProgressPercentage = int.Parse(text.Substring(9));
							flag = true;
						}
						else if (text.StartsWith("[THOR2_flash_state]"))
						{
							this.lastProgressMessage = "FlashingMessageInstallingSoftware";
							flag = true;
						}
						if (flag)
						{
							this.RaiseProgressChangedEvent();
						}
						if (text.ToLower().Contains("connection list start"))
						{
							this.connectionEnumerationStarted = true;
						}
						else if (text.ToLower().Contains("connection list end"))
						{
							this.connectionEnumerationStarted = false;
						}
						else if (this.connectionEnumerationStarted)
						{
							this.connectionList.Add(text);
						}
						if (text.Contains("TYPE:"))
						{
							this.deviceToFindInfo.HardwareModel = text.Remove(0, 5).Trim();
						}
						else if (text.Contains("CTR:"))
						{
							this.deviceToFindInfo.HardwareVariant = text.Remove(0, 4).Trim();
						}
						else if (text.Contains("IMEI:"))
						{
							this.deviceToFindInfo.Imei = text.Remove(0, 5).Trim();
						}
						if (text.Contains("Reset Protection version:"))
						{
							this.antiTheftVerPhone = text.Substring(25).Trim();
						}
						else if (text.Contains("Antitheft version:"))
						{
							this.antiTheftVerFfu = text.Substring(18).Trim();
						}
						if (text.ToLower().Contains("received raw message"))
						{
							this.lastRawMessageReceived = text.Substring(22).Trim();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Tracer<Thor2Service>.WriteWarning("Error parsing Thor2 output. Unable to parse string: {0}, exception {1}", new object[]
				{
					dataReceivedEventArgs.Data,
					ex
				});
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007074 File Offset: 0x00005274
		private string EscapeSpecialChars(string sourceString)
		{
			string result;
			if (string.IsNullOrWhiteSpace(sourceString))
			{
				result = sourceString;
			}
			else
			{
				result = sourceString.Replace("{", "{{").Replace("}", "}}");
			}
			return result;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000070B7 File Offset: 0x000052B7
		private void RaiseProgressChangedEvent(int progress, string progressMessage)
		{
			this.lastProgressPercentage = progress;
			this.lastProgressMessage = progressMessage;
			this.RaiseProgressChangedEvent();
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000070D0 File Offset: 0x000052D0
		private void RaiseProgressChangedEvent()
		{
			Action<ProgressChangedEventArgs> progressChanged = this.ProgressChanged;
			if (progressChanged != null)
			{
				progressChanged(new ProgressChangedEventArgs(this.lastProgressPercentage, this.lastProgressMessage));
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00007107 File Offset: 0x00005307
		internal void ReleaseManagedObjects()
		{
			this.processManager.ReleaseManagedObjects();
		}

		// Token: 0x0400003B RID: 59
		private const string FlashingErrorMessage = "Flashing failed with error {0}";

		// Token: 0x0400003C RID: 60
		private const string ResetProtectionErrorMessage = "Reset protection status reading failed with error {0}";

		// Token: 0x0400003D RID: 61
		private const string ReadingInfoErrorMessage = "Reading phone info failed with error {0}";

		// Token: 0x0400003E RID: 62
		private readonly ProcessManager processManager;

		// Token: 0x0400003F RID: 63
		private Phone deviceToFindInfo;

		// Token: 0x04000040 RID: 64
		private List<string> connectionList;

		// Token: 0x04000041 RID: 65
		private int lastProgressPercentage;

		// Token: 0x04000042 RID: 66
		private bool disposed;

		// Token: 0x04000043 RID: 67
		private bool connectionEnumerationStarted;

		// Token: 0x04000044 RID: 68
		private string lastProgressMessage;

		// Token: 0x04000045 RID: 69
		private string antiTheftVerPhone;

		// Token: 0x04000046 RID: 70
		private string antiTheftVerFfu;

		// Token: 0x04000047 RID: 71
		private string lastRawMessageReceived;
	}
}
