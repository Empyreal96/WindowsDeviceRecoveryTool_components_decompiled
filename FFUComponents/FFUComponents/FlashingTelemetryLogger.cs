using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Diagnostics.Telemetry;
using Microsoft.Diagnostics.Tracing;

namespace FFUComponents
{
	// Token: 0x0200005F RID: 95
	public sealed class FlashingTelemetryLogger
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000A3CA File Offset: 0x000085CA
		private FlashingTelemetryLogger()
		{
			this.logger = new TelemetryEventSource("Microsoft-Windows-Deployment-Flashing");
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000A3ED File Offset: 0x000085ED
		public static FlashingTelemetryLogger Instance
		{
			get
			{
				if (FlashingTelemetryLogger.instance == null)
				{
					FlashingTelemetryLogger.instance = new FlashingTelemetryLogger();
				}
				return FlashingTelemetryLogger.instance;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000A408 File Offset: 0x00008608
		public void LogFlashingInitialized(Guid sessionId, IFFUDevice device, bool optimizeHint, string ffuFile)
		{
			try
			{
				this.LogString("FlashingInitialized", sessionId, new string[]
				{
					optimizeHint.ToString()
				});
				this.LogString("DeviceInfo", sessionId, new string[]
				{
					device.GetType().Name,
					device.DeviceFriendlyName
				});
				string location = base.GetType().Assembly.Location;
				FileInfo fileInfo = new FileInfo(location);
				this.LogString("DllInfo", sessionId, new string[]
				{
					location,
					FileVersionInfo.GetVersionInfo(location).ProductVersion,
					fileInfo.CreationTime.ToString("yyMMdd-HHmm", CultureInfo.InvariantCulture),
					fileInfo.LastWriteTime.ToString("yyMMdd-HHmm", CultureInfo.InvariantCulture)
				});
				string location2 = Assembly.GetEntryAssembly().Location;
				FileInfo fileInfo2 = new FileInfo(location2);
				string fileName = Path.GetFileName(location2);
				this.LogString("ExeInfo", sessionId, new string[]
				{
					fileName,
					location2,
					FileVersionInfo.GetVersionInfo(location2).ProductVersion,
					fileInfo2.CreationTime.ToString("yyMMdd-HHmm", CultureInfo.InvariantCulture),
					fileInfo2.LastWriteTime.ToString("yyMMdd-HHmm", CultureInfo.InvariantCulture)
				});
				this.LogFFUReadSpeed(sessionId, ffuFile);
			}
			catch
			{
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000A590 File Offset: 0x00008790
		public void LogFlashingStarted(Guid sessionId)
		{
			this.LogString("FlashingStarted", sessionId, new string[0]);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000A5A4 File Offset: 0x000087A4
		public void LogFlashingEnded(Guid sessionId, Stopwatch flashingStopwatch, string ffuFilePath, IFFUDevice device)
		{
			if ((float)flashingStopwatch.ElapsedMilliseconds == 0f)
			{
				this.LogFlashingFailed(sessionId, "Flashing took less than 1 second, which is impossible.");
				return;
			}
			FileInfo fileInfo = new FileInfo(ffuFilePath);
			float num = (float)fileInfo.Length / 1024f / 1024f;
			float flashingSpeed = num / ((float)flashingStopwatch.ElapsedMilliseconds / 1000f);
			this.LogFlashingCompleted(sessionId, flashingStopwatch.Elapsed.TotalSeconds, flashingSpeed, device);
			this.LogString("FFUInfo", sessionId, new string[]
			{
				ffuFilePath,
				num.ToString()
			});
			this.LogFFULocation(sessionId, ffuFilePath);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000A640 File Offset: 0x00008840
		public void LogFlashingException(Guid sessionId, Exception e)
		{
			this.LogFlashingFailed(sessionId, e.Message);
			if (e.InnerException != null)
			{
				this.LogString("FlashingException", sessionId, new string[]
				{
					e.InnerException.ToString()
				});
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000A684 File Offset: 0x00008884
		public void LogThorDeviceUSBConnectionType(Guid sessionId, ConnectionType connectionType)
		{
			this.LogString("ThorDeviceUSBConnectionType", sessionId, new string[]
			{
				connectionType.ToString()
			});
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000B2E0 File Offset: 0x000094E0
		private void LogString(string _eventName, Guid sessionId, params string[] values)
		{
			switch (values.Length)
			{
			case 0:
				this.logger.Write("Flashing", this.telemetryOptionMeasures, new
				{
					EventName = _eventName,
					Value1 = sessionId
				});
				return;
			case 1:
				this.logger.Write("Flashing", this.telemetryOptionMeasures, new
				{
					EventName = _eventName,
					Value1 = sessionId,
					Value2 = values[0]
				});
				return;
			case 2:
				this.logger.Write("Flashing", this.telemetryOptionMeasures, new
				{
					EventName = _eventName,
					Value1 = sessionId,
					Value2 = values[0],
					Value3 = values[1]
				});
				return;
			case 3:
				this.logger.Write("Flashing", this.telemetryOptionMeasures, new
				{
					EventName = _eventName,
					Value1 = sessionId,
					Value2 = values[0],
					Value3 = values[1],
					Value4 = values[2]
				});
				return;
			case 4:
				this.logger.Write("Flashing", this.telemetryOptionMeasures, new
				{
					EventName = _eventName,
					Value1 = sessionId,
					Value2 = values[0],
					Value3 = values[1],
					Value4 = values[2],
					Value5 = values[3]
				});
				return;
			case 5:
				this.logger.Write("Flashing", this.telemetryOptionMeasures, new
				{
					EventName = _eventName,
					Value1 = sessionId,
					Value2 = values[0],
					Value3 = values[1],
					Value4 = values[2],
					Value5 = values[3],
					Value6 = values[4]
				});
				return;
			default:
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string value in values)
				{
					stringBuilder.Append(value);
					stringBuilder.Append(";");
				}
				this.logger.Write("Flashing", this.telemetryOptionMeasures, new
				{
					EventName = _eventName,
					Value1 = sessionId,
					Value2 = "Values count exceeded supported count.",
					Value3 = stringBuilder.ToString()
				});
				return;
			}
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000B450 File Offset: 0x00009650
		private void LogFFULocation(Guid sessionId, string ffuFilePath)
		{
			try
			{
				Uri uri = new Uri(ffuFilePath);
				if (uri.IsUnc)
				{
					if (uri.IsLoopback)
					{
						DriveType localPathDriveType = this.GetLocalPathDriveType(uri.LocalPath);
						if (localPathDriveType == DriveType.Unknown)
						{
							this.LogFileLocation(sessionId, ffuFilePath, DriveType.Network, "The location was inferred based on best guess and can be inaccurate.");
						}
						else
						{
							this.LogFileLocation(sessionId, ffuFilePath, localPathDriveType, null);
						}
					}
					else
					{
						this.LogFileLocation(sessionId, ffuFilePath, DriveType.Network, "The location was inferred based on best guess and can be inaccurate.");
					}
				}
				else
				{
					this.LogFileLocation(sessionId, ffuFilePath, this.GetLocalPathDriveType(ffuFilePath), null);
				}
			}
			catch (Exception ex)
			{
				this.LogString("GetFFUFileLocationFailed", sessionId, new string[]
				{
					ffuFilePath,
					ex.Message
				});
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000B4F8 File Offset: 0x000096F8
		private void LogFlashingCompleted(Guid sessionId, double elapsedTimeSeconds, float flashingSpeed, IFFUDevice device)
		{
			this.LogString("FlashingCompleted", sessionId, new string[]
			{
				elapsedTimeSeconds.ToString(),
				flashingSpeed.ToString(),
				device.GetType().Name
			});
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000B53C File Offset: 0x0000973C
		private void LogFlashingFailed(Guid sessionId, string message)
		{
			this.LogString("FlashingFailed", sessionId, new string[]
			{
				message
			});
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000B564 File Offset: 0x00009764
		private void LogFileLocation(Guid sessionId, string ffuFilePath, DriveType ffuDriveType, string warningMessage)
		{
			this.LogString("FFUFileLocation", sessionId, new string[]
			{
				ffuFilePath,
				ffuDriveType.ToString(),
				warningMessage
			});
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000B59C File Offset: 0x0000979C
		private DriveType GetLocalPathDriveType(string filePath)
		{
			foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
			{
				if (string.Equals(Path.GetPathRoot(filePath), driveInfo.Name, StringComparison.OrdinalIgnoreCase))
				{
					return driveInfo.DriveType;
				}
			}
			return DriveType.Unknown;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000B5E4 File Offset: 0x000097E4
		private void LogFFUReadSpeed(Guid sessionId, string ffuFilePath)
		{
			try
			{
				using (BinaryReader binaryReader = new BinaryReader(new FileStream(ffuFilePath, FileMode.Open, FileAccess.Read)))
				{
					int num = 52428800;
					byte[] buffer = new byte[num];
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					binaryReader.Read(buffer, 0, num);
					stopwatch.Stop();
					this.LogString("FFUReadSpeed", sessionId, new string[]
					{
						(50000f / (float)stopwatch.ElapsedMilliseconds).ToString(),
						stopwatch.ElapsedMilliseconds.ToString(),
						Stopwatch.IsHighResolution.ToString()
					});
				}
			}
			catch (Exception ex)
			{
				this.LogString("GetFFUReadSpeedFailed", sessionId, new string[]
				{
					ffuFilePath,
					ex.Message
				});
			}
		}

		// Token: 0x040001D4 RID: 468
		public const string ErrorFlashingTimeTooShort = "Flashing took less than 1 second, which is impossible.";

		// Token: 0x040001D5 RID: 469
		private const string generalEventName = "Flashing";

		// Token: 0x040001D6 RID: 470
		private const string fileLocationEventName = "FFUFileLocation";

		// Token: 0x040001D7 RID: 471
		private const string fileLocationNotReliableWarning = "The location was inferred based on best guess and can be inaccurate.";

		// Token: 0x040001D8 RID: 472
		private const int ffuReadSpeedTestLenghMB = 50;

		// Token: 0x040001D9 RID: 473
		private static FlashingTelemetryLogger instance;

		// Token: 0x040001DA RID: 474
		private TelemetryEventSource logger;

		// Token: 0x040001DB RID: 475
		private readonly EventSourceOptions telemetryOptionMeasures = TelemetryEventSource.CriticalDataOptions();
	}
}
