using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Security;
using System.Xml;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services.DataPackageRules
{
	// Token: 0x02000037 RID: 55
	public sealed class LocalDataPackageAccess
	{
		// Token: 0x060002EA RID: 746 RVA: 0x0000C1F6 File Offset: 0x0000A3F6
		public LocalDataPackageAccess() : this(new FileHelper())
		{
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000C206 File Offset: 0x0000A406
		internal LocalDataPackageAccess(FileHelper fileHelper)
		{
			this.fileHelper = fileHelper;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000C218 File Offset: 0x0000A418
		public ReadOnlyCollection<string> GetVplPathList(string productType, string productCode, string searchPath)
		{
			ReadOnlyCollection<string> result;
			try
			{
				object[] args = new object[]
				{
					string.IsNullOrEmpty(productCode) ? "*" : productCode
				};
				string directory = Path.Combine(searchPath, productType);
				string searchPattern = string.Format(CultureInfo.CurrentCulture, "*_{0}_*.vpl", args);
				string[] filesFromDirectory = this.fileHelper.GetFilesFromDirectory(directory, searchPattern);
				result = Array.AsReadOnly<string>(filesFromDirectory);
			}
			catch (ArgumentNullException error)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error);
				result = Array.AsReadOnly<string>(new string[0]);
			}
			catch (UnauthorizedAccessException error2)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error2);
				result = Array.AsReadOnly<string>(new string[0]);
			}
			catch (IOException error3)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error3);
				result = Array.AsReadOnly<string>(new string[0]);
			}
			return result;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000C2F4 File Offset: 0x0000A4F4
		public ReadOnlyCollection<string> GetVplPathList(string productType, string productCode, IEnumerable<string> searchPaths)
		{
			List<string> list = new List<string>();
			foreach (string searchPath in searchPaths)
			{
				list.AddRange(this.GetVplPathList(productType, productCode, searchPath));
			}
			return list.AsReadOnly();
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000C368 File Offset: 0x0000A568
		public ReadOnlyCollection<string> GetVplPathList(string productType, string searchPath)
		{
			return this.GetVplPathList(productType, null, searchPath);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000C384 File Offset: 0x0000A584
		public ReadOnlyCollection<string> GetVplPathList(string productType, IEnumerable<string> searchPaths)
		{
			return this.GetVplPathList(productType, null, searchPaths);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000C3A0 File Offset: 0x0000A5A0
		public string GetProductCodeFromVpl(string vplFilePath)
		{
			string empty;
			try
			{
				using (Stream fileStream = this.GetFileStream(vplFilePath))
				{
					using (XmlReader xmlReader = XmlReader.Create(fileStream))
					{
						while (xmlReader.Read())
						{
							if (xmlReader.LocalName == "ProductCode")
							{
								return xmlReader.ReadInnerXml();
							}
						}
						empty = string.Empty;
					}
				}
			}
			catch (ArgumentNullException error)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error);
				empty = string.Empty;
			}
			catch (SecurityException error2)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error2);
				empty = string.Empty;
			}
			return empty;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000C47C File Offset: 0x0000A67C
		public string GetSoftwareVersionFromVpl(string vplFilePath)
		{
			string empty;
			try
			{
				using (Stream fileStream = this.GetFileStream(vplFilePath))
				{
					using (XmlReader xmlReader = XmlReader.Create(fileStream))
					{
						while (xmlReader.Read())
						{
							if (xmlReader.LocalName == "SwVersion")
							{
								return xmlReader.ReadInnerXml();
							}
						}
						empty = string.Empty;
					}
				}
			}
			catch (ArgumentNullException error)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error);
				empty = string.Empty;
			}
			catch (SecurityException error2)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error2);
				empty = string.Empty;
			}
			return empty;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000C558 File Offset: 0x0000A758
		public string GetVariantVersionFromVpl(string vplFilePath)
		{
			string empty;
			try
			{
				using (Stream fileStream = this.GetFileStream(vplFilePath))
				{
					using (XmlReader xmlReader = XmlReader.Create(fileStream))
					{
						while (xmlReader.Read())
						{
							if (xmlReader.LocalName == "VariantVersion")
							{
								return xmlReader.ReadInnerXml();
							}
						}
						empty = string.Empty;
					}
				}
			}
			catch (ArgumentNullException error)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error);
				empty = string.Empty;
			}
			catch (SecurityException error2)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error2);
				empty = string.Empty;
			}
			return empty;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000C634 File Offset: 0x0000A834
		public string GetVariantDescriptionFromVpl(string vplFilePath)
		{
			string result;
			try
			{
				using (Stream fileStream = this.GetFileStream(vplFilePath))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(fileStream))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(xmlTextReader);
						result = xmlDocument.GetElementsByTagName("Description")[0].InnerText;
					}
				}
			}
			catch (NullReferenceException error)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error);
				result = string.Empty;
			}
			catch (ArgumentNullException error2)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error2);
				result = string.Empty;
			}
			catch (SecurityException error3)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error3);
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000C720 File Offset: 0x0000A920
		private Stream GetFileStream(string vplFilePath)
		{
			Stream result;
			try
			{
				result = this.fileHelper.GetFileStream(vplFilePath);
			}
			catch (ArgumentException error)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error);
				result = null;
			}
			catch (NotSupportedException error2)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error2);
				result = null;
			}
			catch (IOException error3)
			{
				Tracer<LocalDataPackageAccess>.WriteError(error3);
				result = null;
			}
			return result;
		}

		// Token: 0x04000174 RID: 372
		private readonly FileHelper fileHelper;
	}
}
