using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Interfaces;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers
{
	// Token: 0x0200000B RID: 11
	internal class IOHelper
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00002A4C File Offset: 0x00000C4C
		public void CreateDir(string directory)
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002A70 File Offset: 0x00000C70
		public void CreateDirForPath(string fullPath)
		{
			string directoryName = Path.GetDirectoryName(fullPath);
			if (directoryName != null)
			{
				this.CreateDir(directoryName);
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002A98 File Offset: 0x00000C98
		public void SerializeReport(string reportPath, IReport report)
		{
			this.CreateDirForPath(reportPath);
			using (FileStream fileStream = new FileStream(reportPath, FileMode.Create))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				report.LocalPath = reportPath;
				binaryFormatter.Serialize(fileStream, report);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public void SaveReport(string reportPath, MsrReport report)
		{
			if (reportPath != null)
			{
				this.CreateDirForPath(reportPath);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(MsrReport));
				using (StreamWriter streamWriter = new StreamWriter(reportPath))
				{
					xmlSerializer.Serialize(streamWriter, report);
					streamWriter.Close();
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002B60 File Offset: 0x00000D60
		public void SaveReportAsCsv(string reportPath, IReport report)
		{
			if (reportPath != null)
			{
				this.CreateDirForPath(reportPath);
				string reportAsCsv = report.GetReportAsCsv();
				using (StreamWriter streamWriter = new StreamWriter(reportPath))
				{
					streamWriter.WriteLine(reportAsCsv);
					streamWriter.Close();
				}
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002BC4 File Offset: 0x00000DC4
		public void SaveReportAsCsv(string reportPath, SurveyReport report)
		{
			if (reportPath != null)
			{
				this.CreateDirForPath(reportPath);
				string reportAsCsv = report.GetReportAsCsv();
				using (StreamWriter streamWriter = new StreamWriter(reportPath))
				{
					streamWriter.WriteLine(reportAsCsv);
					streamWriter.Close();
				}
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002C28 File Offset: 0x00000E28
		public void SaveReport(string reportPath, ReportUpdateStatus4Parameters parameters)
		{
			if (reportPath != null)
			{
				this.CreateDirForPath(reportPath);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ReportUpdateStatus4Parameters));
				reportPath = reportPath.Replace(": ", string.Empty);
				using (StreamWriter streamWriter = new StreamWriter(reportPath))
				{
					xmlSerializer.Serialize(streamWriter, parameters);
					streamWriter.Close();
				}
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public IReport DeserializeReport(string reportPath)
		{
			IReport result;
			using (FileStream fileStream = new FileStream(reportPath, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				IReport report = (IReport)binaryFormatter.Deserialize(fileStream);
				report.LocalPath = reportPath;
				result = report;
			}
			return result;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002D04 File Offset: 0x00000F04
		public Report DeserializeFireReport(string reportPath)
		{
			Report result;
			using (FileStream fileStream = new FileStream(reportPath, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				Report report = (Report)binaryFormatter.Deserialize(fileStream);
				report.LocalPath = reportPath;
				result = report;
			}
			return result;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002D60 File Offset: 0x00000F60
		public void DeleteFile(string fileName)
		{
			try
			{
				File.Delete(fileName);
			}
			catch (Exception ex)
			{
				Tracer<IOHelper>.WriteInformation("Delete fire report failed. " + ex.Message);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public string[] GetFiles(string dir)
		{
			string[] files = Directory.GetFiles(dir);
			return (from x in files
			where !x.Contains("msr_")
			select x).ToArray<string>();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002E0C File Offset: 0x0000100C
		public string[] GetMsrFiles(string dir)
		{
			return Directory.GetFiles(dir, "msr_*.*");
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002E2C File Offset: 0x0000102C
		public string GetReportFileExtension(ReportFileType reportFileType)
		{
			string result;
			switch (reportFileType)
			{
			case ReportFileType.Xml:
				result = "xml";
				break;
			case ReportFileType.Binary:
				result = "dat";
				break;
			case ReportFileType.Csv:
				result = "csv";
				break;
			default:
				result = "xml";
				break;
			}
			return result;
		}
	}
}
