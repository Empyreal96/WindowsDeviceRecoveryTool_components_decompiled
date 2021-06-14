using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Management.Instrumentation
{
	// Token: 0x020000C1 RID: 193
	internal class SchemaNaming
	{
		// Token: 0x06000521 RID: 1313 RVA: 0x00025558 File Offset: 0x00023758
		public static SchemaNaming GetSchemaNaming(Assembly assembly)
		{
			InstrumentedAttribute attribute = InstrumentedAttribute.GetAttribute(assembly);
			if (attribute == null)
			{
				return null;
			}
			return new SchemaNaming(attribute.NamespaceName, attribute.SecurityDescriptor, assembly);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00025583 File Offset: 0x00023783
		private SchemaNaming(string namespaceName, string securityDescriptor, Assembly assembly)
		{
			this.assembly = assembly;
			this.assemblyInfo = new SchemaNaming.AssemblySpecificNaming(namespaceName, securityDescriptor, assembly);
			if (!SchemaNaming.DoesInstanceExist(this.RegistrationPath))
			{
				this.assemblyInfo.DecoupledProviderInstanceName = AssemblyNameUtility.UniqueToAssemblyMinorVersion(assembly);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x000255BE File Offset: 0x000237BE
		public string NamespaceName
		{
			get
			{
				return this.assemblyInfo.NamespaceName;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x000255CB File Offset: 0x000237CB
		public string SecurityDescriptor
		{
			get
			{
				return this.assemblyInfo.SecurityDescriptor;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x000255D8 File Offset: 0x000237D8
		// (set) Token: 0x06000526 RID: 1318 RVA: 0x000255E5 File Offset: 0x000237E5
		public string DecoupledProviderInstanceName
		{
			get
			{
				return this.assemblyInfo.DecoupledProviderInstanceName;
			}
			set
			{
				this.assemblyInfo.DecoupledProviderInstanceName = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x000255F3 File Offset: 0x000237F3
		private string AssemblyUniqueIdentifier
		{
			get
			{
				return this.assemblyInfo.AssemblyUniqueIdentifier;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00025600 File Offset: 0x00023800
		private string AssemblyName
		{
			get
			{
				return this.assemblyInfo.AssemblyName;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x0002560D File Offset: 0x0002380D
		private string AssemblyPath
		{
			get
			{
				return this.assemblyInfo.AssemblyPath;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x0002561A File Offset: 0x0002381A
		private string Win32ProviderClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "__Win32Provider");
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00025631 File Offset: 0x00023831
		private string DecoupledProviderClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "MSFT_DecoupledProvider");
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00025648 File Offset: 0x00023848
		private string InstrumentationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "WMINET_Instrumentation");
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x0002565F File Offset: 0x0002385F
		private string EventProviderRegistrationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "__EventProviderRegistration");
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00025676 File Offset: 0x00023876
		private string EventProviderRegistrationPath
		{
			get
			{
				return SchemaNaming.AppendProperty(this.EventProviderRegistrationClassPath, "provider", "\\\\\\\\.\\\\" + this.ProviderPath.Replace("\\", "\\\\").Replace("\"", "\\\""));
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x000256B6 File Offset: 0x000238B6
		private string InstanceProviderRegistrationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "__InstanceProviderRegistration");
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x000256CD File Offset: 0x000238CD
		private string InstanceProviderRegistrationPath
		{
			get
			{
				return SchemaNaming.AppendProperty(this.InstanceProviderRegistrationClassPath, "provider", "\\\\\\\\.\\\\" + this.ProviderPath.Replace("\\", "\\\\").Replace("\"", "\\\""));
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x0002570D File Offset: 0x0002390D
		private string ProviderClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "WMINET_ManagedAssemblyProvider");
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00025724 File Offset: 0x00023924
		private string ProviderPath
		{
			get
			{
				return SchemaNaming.AppendProperty(this.ProviderClassPath, "Name", this.assemblyInfo.DecoupledProviderInstanceName);
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00025741 File Offset: 0x00023941
		private string RegistrationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "WMINET_InstrumentedAssembly");
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00025758 File Offset: 0x00023958
		private string RegistrationPath
		{
			get
			{
				return SchemaNaming.AppendProperty(this.RegistrationClassPath, "Name", this.assemblyInfo.DecoupledProviderInstanceName);
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00025775 File Offset: 0x00023975
		private string GlobalRegistrationNamespace
		{
			get
			{
				return "root\\MicrosoftWmiNet";
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0002577C File Offset: 0x0002397C
		private string GlobalInstrumentationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath("root\\MicrosoftWmiNet", "WMINET_Instrumentation");
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0002578D File Offset: 0x0002398D
		private string GlobalRegistrationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath("root\\MicrosoftWmiNet", "WMINET_InstrumentedNamespaces");
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0002579E File Offset: 0x0002399E
		private string GlobalRegistrationPath
		{
			get
			{
				return SchemaNaming.AppendProperty(this.GlobalRegistrationClassPath, "NamespaceName", this.assemblyInfo.NamespaceName.Replace("\\", "\\\\"));
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x000257CA File Offset: 0x000239CA
		private string GlobalNamingClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath("root\\MicrosoftWmiNet", "WMINET_Naming");
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x000257DB File Offset: 0x000239DB
		private string DataDirectory
		{
			get
			{
				return Path.Combine(WMICapabilities.FrameworkDirectory, this.NamespaceName);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x000257ED File Offset: 0x000239ED
		private string MofPath
		{
			get
			{
				return Path.Combine(this.DataDirectory, this.DecoupledProviderInstanceName + ".mof");
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0002580A File Offset: 0x00023A0A
		private string CodePath
		{
			get
			{
				return Path.Combine(this.DataDirectory, this.DecoupledProviderInstanceName + ".cs");
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00025827 File Offset: 0x00023A27
		private string PrecompiledAssemblyPath
		{
			get
			{
				return Path.Combine(this.DataDirectory, this.DecoupledProviderInstanceName + ".dll");
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00025844 File Offset: 0x00023A44
		private static string MakeClassPath(string namespaceName, string className)
		{
			return namespaceName + ":" + className;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00025852 File Offset: 0x00023A52
		private static string AppendProperty(string classPath, string propertyName, string propertyValue)
		{
			return string.Concat(new string[]
			{
				classPath,
				".",
				propertyName,
				"=\"",
				propertyValue,
				"\""
			});
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00025884 File Offset: 0x00023A84
		public bool IsAssemblyRegistered()
		{
			if (SchemaNaming.DoesInstanceExist(this.RegistrationPath))
			{
				ManagementObject managementObject = new ManagementObject(this.RegistrationPath);
				return string.Compare(this.AssemblyUniqueIdentifier, managementObject["RegisteredBuild"].ToString(), StringComparison.OrdinalIgnoreCase) == 0;
			}
			return false;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x000258CC File Offset: 0x00023ACC
		private bool IsSchemaToBeCompared()
		{
			bool result = false;
			if (SchemaNaming.DoesInstanceExist(this.RegistrationPath))
			{
				ManagementObject managementObject = new ManagementObject(this.RegistrationPath);
				result = (string.Compare(this.AssemblyUniqueIdentifier, managementObject["RegisteredBuild"].ToString(), StringComparison.OrdinalIgnoreCase) != 0);
			}
			return result;
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00025915 File Offset: 0x00023B15
		private ManagementObject RegistrationInstance
		{
			get
			{
				if (this.registrationInstance == null)
				{
					this.registrationInstance = new ManagementObject(this.RegistrationPath);
				}
				return this.registrationInstance;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x00025938 File Offset: 0x00023B38
		public string Code
		{
			get
			{
				string result;
				using (StreamReader streamReader = new StreamReader(this.CodePath))
				{
					result = streamReader.ReadToEnd();
				}
				return result;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x00025978 File Offset: 0x00023B78
		public string Mof
		{
			get
			{
				string result;
				using (StreamReader streamReader = new StreamReader(this.MofPath))
				{
					result = streamReader.ReadToEnd();
				}
				return result;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x000259B8 File Offset: 0x00023BB8
		public Assembly PrecompiledAssembly
		{
			get
			{
				if (File.Exists(this.PrecompiledAssemblyPath))
				{
					return Assembly.LoadFrom(this.PrecompiledAssemblyPath);
				}
				return null;
			}
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x000259D4 File Offset: 0x00023BD4
		private bool IsClassAlreadyPresentInRepository(ManagementObject obj)
		{
			bool result = false;
			string text = SchemaNaming.MakeClassPath(this.NamespaceName, (string)obj.SystemProperties["__CLASS"].Value);
			if (SchemaNaming.DoesClassExist(text))
			{
				ManagementObject managementObject = new ManagementClass(text);
				result = managementObject.CompareTo(obj, ComparisonSettings.IgnoreObjectSource | ComparisonSettings.IgnoreCase);
			}
			return result;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00025A24 File Offset: 0x00023C24
		private string GenerateMof(string[] mofs)
		{
			return string.Concat(new string[]
			{
				"//**************************************************************************\r\n",
				string.Format("//* {0}\r\n", this.DecoupledProviderInstanceName),
				string.Format("//* {0}\r\n", this.AssemblyUniqueIdentifier),
				"//**************************************************************************\r\n",
				"#pragma autorecover\r\n",
				SchemaNaming.EnsureNamespaceInMof(this.GlobalRegistrationNamespace),
				SchemaNaming.EnsureNamespaceInMof(this.NamespaceName),
				SchemaNaming.PragmaNamespace(this.GlobalRegistrationNamespace),
				SchemaNaming.GetMofFormat(new ManagementClass(this.GlobalInstrumentationClassPath)),
				SchemaNaming.GetMofFormat(new ManagementClass(this.GlobalRegistrationClassPath)),
				SchemaNaming.GetMofFormat(new ManagementClass(this.GlobalNamingClassPath)),
				SchemaNaming.GetMofFormat(new ManagementObject(this.GlobalRegistrationPath)),
				SchemaNaming.PragmaNamespace(this.NamespaceName),
				SchemaNaming.GetMofFormat(new ManagementClass(this.InstrumentationClassPath)),
				SchemaNaming.GetMofFormat(new ManagementClass(this.RegistrationClassPath)),
				SchemaNaming.GetMofFormat(new ManagementClass(this.DecoupledProviderClassPath)),
				SchemaNaming.GetMofFormat(new ManagementClass(this.ProviderClassPath)),
				SchemaNaming.GetMofFormat(new ManagementObject(this.ProviderPath)),
				SchemaNaming.GetMofFormat(new ManagementObject(this.EventProviderRegistrationPath)),
				SchemaNaming.GetMofFormat(new ManagementObject(this.InstanceProviderRegistrationPath)),
				string.Concat(mofs),
				SchemaNaming.GetMofFormat(new ManagementObject(this.RegistrationPath))
			});
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00025BB0 File Offset: 0x00023DB0
		public void RegisterNonAssemblySpecificSchema(InstallContext installContext)
		{
			SecurityHelper.UnmanagedCode.Demand();
			WmiNetUtilsHelper.VerifyClientKey_f();
			SchemaNaming.InstallLogWrapper context = new SchemaNaming.InstallLogWrapper(installContext);
			SchemaNaming.EnsureNamespace(context, this.GlobalRegistrationNamespace);
			SchemaNaming.EnsureClassExists(context, this.GlobalInstrumentationClassPath, new SchemaNaming.ClassMaker(this.MakeGlobalInstrumentationClass));
			SchemaNaming.EnsureClassExists(context, this.GlobalRegistrationClassPath, new SchemaNaming.ClassMaker(this.MakeNamespaceRegistrationClass));
			SchemaNaming.EnsureClassExists(context, this.GlobalNamingClassPath, new SchemaNaming.ClassMaker(this.MakeNamingClass));
			SchemaNaming.EnsureNamespace(context, this.NamespaceName);
			SchemaNaming.EnsureClassExists(context, this.InstrumentationClassPath, new SchemaNaming.ClassMaker(this.MakeInstrumentationClass));
			SchemaNaming.EnsureClassExists(context, this.RegistrationClassPath, new SchemaNaming.ClassMaker(this.MakeRegistrationClass));
			try
			{
				ManagementClass managementClass = new ManagementClass(this.DecoupledProviderClassPath);
				if (managementClass["HostingModel"].ToString() != "Decoupled:Com")
				{
					managementClass.Delete();
				}
			}
			catch (ManagementException ex)
			{
				if (ex.ErrorCode != ManagementStatus.NotFound)
				{
					throw ex;
				}
			}
			SchemaNaming.EnsureClassExists(context, this.DecoupledProviderClassPath, new SchemaNaming.ClassMaker(this.MakeDecoupledProviderClass));
			SchemaNaming.EnsureClassExists(context, this.ProviderClassPath, new SchemaNaming.ClassMaker(this.MakeProviderClass));
			if (!SchemaNaming.DoesInstanceExist(this.GlobalRegistrationPath))
			{
				this.RegisterNamespaceAsInstrumented();
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00025D00 File Offset: 0x00023F00
		public void RegisterAssemblySpecificSchema()
		{
			SecurityHelper.UnmanagedCode.Demand();
			Type[] instrumentedTypes = InstrumentedAttribute.GetInstrumentedTypes(this.assembly);
			StringCollection stringCollection = new StringCollection();
			StringCollection stringCollection2 = new StringCollection();
			StringCollection stringCollection3 = new StringCollection();
			string[] array = new string[instrumentedTypes.Length];
			CodeWriter codeWriter = new CodeWriter();
			ReferencesCollection referencesCollection = new ReferencesCollection();
			codeWriter.AddChild(referencesCollection.UsingCode);
			referencesCollection.Add(typeof(object));
			referencesCollection.Add(typeof(ManagementClass));
			referencesCollection.Add(typeof(Marshal));
			referencesCollection.Add(typeof(SuppressUnmanagedCodeSecurityAttribute));
			referencesCollection.Add(typeof(FieldInfo));
			referencesCollection.Add(typeof(Hashtable));
			codeWriter.Line();
			CodeWriter codeWriter2 = codeWriter.AddChild("public class WMINET_Converter");
			codeWriter2.Line("public static Hashtable mapTypeToConverter = new Hashtable();");
			CodeWriter codeWriter3 = codeWriter2.AddChild("static WMINET_Converter()");
			Hashtable hashtable = new Hashtable();
			for (int i = 0; i < instrumentedTypes.Length; i++)
			{
				hashtable[instrumentedTypes[i]] = "ConvertClass_" + i;
			}
			bool flag = this.IsSchemaToBeCompared();
			bool flag2 = false;
			if (!flag)
			{
				flag2 = !this.IsAssemblyRegistered();
			}
			for (int j = 0; j < instrumentedTypes.Length; j++)
			{
				SchemaMapping schemaMapping = new SchemaMapping(instrumentedTypes[j], this, hashtable);
				codeWriter3.Line(string.Format("mapTypeToConverter[typeof({0})] = typeof({1});", schemaMapping.ClassType.FullName.Replace('+', '.'), schemaMapping.CodeClassName));
				if (flag && !this.IsClassAlreadyPresentInRepository(schemaMapping.NewClass))
				{
					flag2 = true;
				}
				SchemaNaming.ReplaceClassIfNecessary(schemaMapping.ClassPath, schemaMapping.NewClass);
				array[j] = SchemaNaming.GetMofFormat(schemaMapping.NewClass);
				codeWriter.AddChild(schemaMapping.Code);
				switch (schemaMapping.InstrumentationType)
				{
				case InstrumentationType.Instance:
					stringCollection2.Add(schemaMapping.ClassName);
					break;
				case InstrumentationType.Event:
					stringCollection.Add(schemaMapping.ClassName);
					break;
				case InstrumentationType.Abstract:
					stringCollection3.Add(schemaMapping.ClassName);
					break;
				}
			}
			this.RegisterAssemblySpecificDecoupledProviderInstance();
			this.RegisterProviderAsEventProvider(stringCollection);
			this.RegisterProviderAsInstanceProvider();
			this.RegisterAssemblyAsInstrumented();
			Directory.CreateDirectory(this.DataDirectory);
			using (StreamWriter streamWriter = new StreamWriter(this.CodePath, false, Encoding.Unicode))
			{
				streamWriter.WriteLine(codeWriter);
				streamWriter.WriteLine("class IWOA\r\n{\r\nprotected const string DllName = \"wminet_utils.dll\";\r\nprotected const string EntryPointName = \"UFunc\";\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"GetPropertyHandle\")] public static extern int GetPropertyHandle_f27(int vFunc, IntPtr pWbemClassObject, [In][MarshalAs(UnmanagedType.LPWStr)]  string   wszPropertyName, [Out] out Int32 pType, [Out] out Int32 plHandle);\r\n//[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Byte aData);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadPropertyValue\")] public static extern int ReadPropertyValue_f29(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lBufferSize, [Out] out Int32 plNumBytes, [Out] out Byte aData);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadDWORD\")] public static extern int ReadDWORD_f30(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out] out UInt32 pdw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDWORD\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] UInt32 dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadQWORD\")] public static extern int ReadQWORD_f32(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out] out UInt64 pqw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteQWORD\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] UInt64 pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"GetPropertyInfoByHandle\")] public static extern int GetPropertyInfoByHandle_f34(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out][MarshalAs(UnmanagedType.BStr)]  out string   pstrName, [Out] out Int32 pType);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Lock\")] public static extern int Lock_f35(int vFunc, IntPtr pWbemClassObject, [In] Int32 lFlags);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Unlock\")] public static extern int Unlock_f36(int vFunc, IntPtr pWbemClassObject, [In] Int32 lFlags);\r\n\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Put\")] public static extern int Put_f5(int vFunc, IntPtr pWbemClassObject, [In][MarshalAs(UnmanagedType.LPWStr)]  string   wszName, [In] Int32 lFlags, [In] ref object pVal, [In] Int32 Type);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In][MarshalAs(UnmanagedType.LPWStr)] string str);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Byte n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref SByte n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Int16 n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref UInt16 n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\", CharSet=CharSet.Unicode)] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Char c);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDWORD\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteSingle\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Single dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteQWORD\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int64 pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDouble\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Double pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Clone\")] public static extern int Clone_f(int vFunc, IntPtr pWbemClassObject, [Out] out IntPtr ppCopy);\r\n}\r\ninterface IWmiConverter\r\n{\r\n    void ToWMI(object obj);\r\n    ManagementObject GetInstance();\r\n}\r\nclass SafeAssign\r\n{\r\n    public static UInt16 boolTrue = 0xffff;\r\n    public static UInt16 boolFalse = 0;\r\n    static Hashtable validTypes = new Hashtable();\r\n    static SafeAssign()\r\n    {\r\n        validTypes.Add(typeof(SByte), null);\r\n        validTypes.Add(typeof(Byte), null);\r\n        validTypes.Add(typeof(Int16), null);\r\n        validTypes.Add(typeof(UInt16), null);\r\n        validTypes.Add(typeof(Int32), null);\r\n        validTypes.Add(typeof(UInt32), null);\r\n        validTypes.Add(typeof(Int64), null);\r\n        validTypes.Add(typeof(UInt64), null);\r\n        validTypes.Add(typeof(Single), null);\r\n        validTypes.Add(typeof(Double), null);\r\n        validTypes.Add(typeof(Boolean), null);\r\n        validTypes.Add(typeof(String), null);\r\n        validTypes.Add(typeof(Char), null);\r\n        validTypes.Add(typeof(DateTime), null);\r\n        validTypes.Add(typeof(TimeSpan), null);\r\n        validTypes.Add(typeof(ManagementObject), null);\r\n        nullClass.SystemProperties [\"__CLASS\"].Value = \"nullInstance\";\r\n    }\r\n    public static object GetInstance(object o)\r\n    {\r\n        if(o is ManagementObject)\r\n            return o;\r\n        return null;\r\n    }\r\n    static ManagementClass nullClass = new ManagementClass(new ManagementPath(@\"" + this.NamespaceName + "\"));\r\n    \r\n    public static ManagementObject GetManagementObject(object o)\r\n    {\r\n        if(o != null && o is ManagementObject)\r\n            return o as ManagementObject;\r\n        // Must return empty instance\r\n        return nullClass.CreateInstance();\r\n    }\r\n    public static object GetValue(object o)\r\n    {\r\n        Type t = o.GetType();\r\n        if(t.IsArray)\r\n            t = t.GetElementType();\r\n        if(validTypes.Contains(t))\r\n            return o;\r\n        return null;\r\n    }\r\n    public static string WMITimeToString(DateTime dt)\r\n    {\r\n        TimeSpan ts = dt.Subtract(dt.ToUniversalTime());\r\n        int diffUTC = (ts.Minutes + ts.Hours * 60);\r\n        if(diffUTC >= 0)\r\n            return String.Format(\"{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.{6:D3}000+{7:D3}\", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, diffUTC);\r\n        return String.Format(\"{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.{6:D3}000-{7:D3}\", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, -diffUTC);\r\n    }\r\n    public static string WMITimeToString(TimeSpan ts)\r\n    {\r\n        return String.Format(\"{0:D8}{1:D2}{2:D2}{3:D2}.{4:D3}000:000\", ts.Days, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);\r\n    }\r\n    public static string[] WMITimeArrayToStringArray(DateTime[] dates)\r\n    {\r\n        string[] strings = new string[dates.Length];\r\n        for(int i=0;i<dates.Length;i++)\r\n            strings[i] = WMITimeToString(dates[i]);\r\n        return strings;\r\n    }\r\n    public static string[] WMITimeArrayToStringArray(TimeSpan[] timeSpans)\r\n    {\r\n        string[] strings = new string[timeSpans.Length];\r\n        for(int i=0;i<timeSpans.Length;i++)\r\n            strings[i] = WMITimeToString(timeSpans[i]);\r\n        return strings;\r\n    }\r\n}\r\n");
			}
			using (StreamWriter streamWriter2 = new StreamWriter(this.MofPath, false, Encoding.Unicode))
			{
				streamWriter2.WriteLine(this.GenerateMof(array));
			}
			if (flag2)
			{
				SchemaNaming.RegisterSchemaUsingMofcomp(this.MofPath);
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00025FF0 File Offset: 0x000241F0
		private void RegisterNamespaceAsInstrumented()
		{
			ManagementClass managementClass = new ManagementClass(this.GlobalRegistrationClassPath);
			ManagementObject managementObject = managementClass.CreateInstance();
			managementObject["NamespaceName"] = this.NamespaceName;
			managementObject.Put();
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00026028 File Offset: 0x00024228
		private void RegisterAssemblyAsInstrumented()
		{
			ManagementClass managementClass = new ManagementClass(this.RegistrationClassPath);
			ManagementObject managementObject = managementClass.CreateInstance();
			managementObject["Name"] = this.DecoupledProviderInstanceName;
			managementObject["RegisteredBuild"] = this.AssemblyUniqueIdentifier;
			managementObject["FullName"] = this.AssemblyName;
			managementObject["PathToAssembly"] = this.AssemblyPath;
			managementObject["Code"] = "";
			managementObject["Mof"] = "";
			managementObject.Put();
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x000260B4 File Offset: 0x000242B4
		private void RegisterAssemblySpecificDecoupledProviderInstance()
		{
			ManagementClass managementClass = new ManagementClass(this.ProviderClassPath);
			ManagementObject managementObject = managementClass.CreateInstance();
			managementObject["Name"] = this.DecoupledProviderInstanceName;
			managementObject["HostingModel"] = "Decoupled:Com";
			if (this.SecurityDescriptor != null)
			{
				managementObject["SecurityDescriptor"] = this.SecurityDescriptor;
			}
			managementObject.Put();
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00026118 File Offset: 0x00024318
		private string RegisterProviderAsEventProvider(StringCollection events)
		{
			ManagementClass managementClass = new ManagementClass(this.EventProviderRegistrationClassPath);
			ManagementObject managementObject = managementClass.CreateInstance();
			managementObject["provider"] = "\\\\.\\" + this.ProviderPath;
			string[] array = new string[events.Count];
			int num = 0;
			foreach (string str in events)
			{
				array[num++] = "select * from " + str;
			}
			managementObject["EventQueryList"] = array;
			return managementObject.Put().Path;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x000261D0 File Offset: 0x000243D0
		private string RegisterProviderAsInstanceProvider()
		{
			ManagementClass managementClass = new ManagementClass(this.InstanceProviderRegistrationClassPath);
			ManagementObject managementObject = managementClass.CreateInstance();
			managementObject["provider"] = "\\\\.\\" + this.ProviderPath;
			managementObject["SupportsGet"] = true;
			managementObject["SupportsEnumeration"] = true;
			return managementObject.Put().Path;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00026238 File Offset: 0x00024438
		private ManagementClass MakeNamingClass()
		{
			ManagementClass managementClass = new ManagementClass(this.GlobalInstrumentationClassPath);
			ManagementClass managementClass2 = managementClass.Derive("WMINET_Naming");
			managementClass2.Qualifiers.Add("abstract", true);
			PropertyDataCollection properties = managementClass2.Properties;
			properties.Add("InstrumentedAssembliesClassName", "WMINET_InstrumentedAssembly", CimType.String);
			return managementClass2;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0002628C File Offset: 0x0002448C
		private ManagementClass MakeInstrumentationClass()
		{
			ManagementClass managementClass = new ManagementClass(this.NamespaceName, "", null);
			managementClass.SystemProperties["__CLASS"].Value = "WMINET_Instrumentation";
			managementClass.Qualifiers.Add("abstract", true);
			return managementClass;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000262DC File Offset: 0x000244DC
		private ManagementClass MakeGlobalInstrumentationClass()
		{
			ManagementClass managementClass = new ManagementClass("root\\MicrosoftWmiNet", "", null);
			managementClass.SystemProperties["__CLASS"].Value = "WMINET_Instrumentation";
			managementClass.Qualifiers.Add("abstract", true);
			return managementClass;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0002632C File Offset: 0x0002452C
		private ManagementClass MakeRegistrationClass()
		{
			ManagementClass managementClass = new ManagementClass(this.InstrumentationClassPath);
			ManagementClass managementClass2 = managementClass.Derive("WMINET_InstrumentedAssembly");
			PropertyDataCollection properties = managementClass2.Properties;
			properties.Add("Name", CimType.String, false);
			PropertyData propertyData = properties["Name"];
			propertyData.Qualifiers.Add("key", true);
			properties.Add("RegisteredBuild", CimType.String, false);
			properties.Add("FullName", CimType.String, false);
			properties.Add("PathToAssembly", CimType.String, false);
			properties.Add("Code", CimType.String, false);
			properties.Add("Mof", CimType.String, false);
			return managementClass2;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x000263CC File Offset: 0x000245CC
		private ManagementClass MakeNamespaceRegistrationClass()
		{
			ManagementClass managementClass = new ManagementClass(this.GlobalInstrumentationClassPath);
			ManagementClass managementClass2 = managementClass.Derive("WMINET_InstrumentedNamespaces");
			PropertyDataCollection properties = managementClass2.Properties;
			properties.Add("NamespaceName", CimType.String, false);
			PropertyData propertyData = properties["NamespaceName"];
			propertyData.Qualifiers.Add("key", true);
			return managementClass2;
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00026428 File Offset: 0x00024628
		private ManagementClass MakeDecoupledProviderClass()
		{
			ManagementClass managementClass = new ManagementClass(this.Win32ProviderClassPath);
			ManagementClass managementClass2 = managementClass.Derive("MSFT_DecoupledProvider");
			PropertyDataCollection properties = managementClass2.Properties;
			properties.Add("HostingModel", "Decoupled:Com", CimType.String);
			properties.Add("SecurityDescriptor", CimType.String, false);
			properties.Add("Version", 1, CimType.UInt32);
			properties["CLSID"].Value = "{54D8502C-527D-43f7-A506-A9DA075E229C}";
			return managementClass2;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0002649C File Offset: 0x0002469C
		private ManagementClass MakeProviderClass()
		{
			ManagementClass managementClass = new ManagementClass(this.DecoupledProviderClassPath);
			ManagementClass managementClass2 = managementClass.Derive("WMINET_ManagedAssemblyProvider");
			PropertyDataCollection properties = managementClass2.Properties;
			properties.Add("Assembly", CimType.String, false);
			return managementClass2;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x000264D8 File Offset: 0x000246D8
		private static void RegisterSchemaUsingMofcomp(string mofPath)
		{
			Process process = Process.Start(new ProcessStartInfo
			{
				Arguments = mofPath,
				FileName = WMICapabilities.InstallationDirectory + "\\mofcomp.exe",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			});
			process.WaitForExit();
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0002652C File Offset: 0x0002472C
		private static void EnsureNamespace(string baseNamespace, string childNamespaceName)
		{
			if (!SchemaNaming.DoesInstanceExist(baseNamespace + ":__NAMESPACE.Name=\"" + childNamespaceName + "\""))
			{
				ManagementClass managementClass = new ManagementClass(baseNamespace + ":__NAMESPACE");
				ManagementObject managementObject = managementClass.CreateInstance();
				managementObject["Name"] = childNamespaceName;
				managementObject.Put();
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0002657C File Offset: 0x0002477C
		private static void EnsureNamespace(SchemaNaming.InstallLogWrapper context, string namespaceName)
		{
			context.LogMessage(RC.GetString("NAMESPACE_ENSURE") + " " + namespaceName);
			string text = null;
			foreach (string text2 in namespaceName.Split(new char[]
			{
				'\\'
			}))
			{
				if (text == null)
				{
					text = text2;
				}
				else
				{
					SchemaNaming.EnsureNamespace(text, text2);
					text = text + "\\" + text2;
				}
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000265E8 File Offset: 0x000247E8
		private static void EnsureClassExists(SchemaNaming.InstallLogWrapper context, string classPath, SchemaNaming.ClassMaker classMakerFunction)
		{
			try
			{
				context.LogMessage(RC.GetString("CLASS_ENSURE") + " " + classPath);
				ManagementClass managementClass = new ManagementClass(classPath);
				managementClass.Get();
			}
			catch (ManagementException ex)
			{
				if (ex.ErrorCode != ManagementStatus.NotFound)
				{
					throw ex;
				}
				context.LogMessage(RC.GetString("CLASS_ENSURECREATE") + " " + classPath);
				ManagementClass managementClass2 = classMakerFunction();
				managementClass2.Put();
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0002666C File Offset: 0x0002486C
		private static bool DoesInstanceExist(string objectPath)
		{
			bool result = false;
			try
			{
				ManagementObject managementObject = new ManagementObject(objectPath);
				managementObject.Get();
				result = true;
			}
			catch (ManagementException ex)
			{
				if (ManagementStatus.InvalidNamespace != ex.ErrorCode && ManagementStatus.InvalidClass != ex.ErrorCode && ManagementStatus.NotFound != ex.ErrorCode)
				{
					throw ex;
				}
			}
			return result;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000266CC File Offset: 0x000248CC
		private static bool DoesClassExist(string objectPath)
		{
			bool result = false;
			try
			{
				ManagementObject managementObject = new ManagementClass(objectPath);
				managementObject.Get();
				result = true;
			}
			catch (ManagementException ex)
			{
				if (ManagementStatus.InvalidNamespace != ex.ErrorCode && ManagementStatus.InvalidClass != ex.ErrorCode && ManagementStatus.NotFound != ex.ErrorCode)
				{
					throw ex;
				}
			}
			return result;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0002672C File Offset: 0x0002492C
		private static ManagementClass SafeGetClass(string classPath)
		{
			ManagementClass result = null;
			try
			{
				ManagementClass managementClass = new ManagementClass(classPath);
				managementClass.Get();
				result = managementClass;
			}
			catch (ManagementException ex)
			{
				if (ex.ErrorCode != ManagementStatus.NotFound)
				{
					throw ex;
				}
			}
			return result;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00026770 File Offset: 0x00024970
		private static void ReplaceClassIfNecessary(string classPath, ManagementClass newClass)
		{
			try
			{
				ManagementClass managementClass = SchemaNaming.SafeGetClass(classPath);
				if (managementClass == null)
				{
					newClass.Put();
				}
				else if (newClass.GetText(TextFormat.Mof) != managementClass.GetText(TextFormat.Mof))
				{
					managementClass.Delete();
					newClass.Put();
				}
			}
			catch (ManagementException innerException)
			{
				string format = RC.GetString("CLASS_NOTREPLACED_EXCEPT") + "\r\n{0}\r\n{1}";
				throw new ArgumentException(string.Format(format, classPath, newClass.GetText(TextFormat.Mof)), innerException);
			}
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000267F0 File Offset: 0x000249F0
		private static string GetMofFormat(ManagementObject obj)
		{
			return obj.GetText(TextFormat.Mof).Replace("\n", "\r\n") + "\r\n";
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00026812 File Offset: 0x00024A12
		private static string PragmaNamespace(string namespaceName)
		{
			return string.Format("#pragma namespace(\"\\\\\\\\.\\\\{0}\")\r\n\r\n", namespaceName.Replace("\\", "\\\\"));
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0002682E File Offset: 0x00024A2E
		private static string EnsureNamespaceInMof(string baseNamespace, string childNamespaceName)
		{
			return string.Format("{0}instance of __Namespace\r\n{{\r\n  Name = \"{1}\";\r\n}};\r\n\r\n", SchemaNaming.PragmaNamespace(baseNamespace), childNamespaceName);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00026844 File Offset: 0x00024A44
		private static string EnsureNamespaceInMof(string namespaceName)
		{
			string text = "";
			string text2 = null;
			foreach (string text3 in namespaceName.Split(new char[]
			{
				'\\'
			}))
			{
				if (text2 == null)
				{
					text2 = text3;
				}
				else
				{
					text += SchemaNaming.EnsureNamespaceInMof(text2, text3);
					text2 = text2 + "\\" + text3;
				}
			}
			return text;
		}

		// Token: 0x0400051C RID: 1308
		private Assembly assembly;

		// Token: 0x0400051D RID: 1309
		private const string Win32ProviderClassName = "__Win32Provider";

		// Token: 0x0400051E RID: 1310
		private const string EventProviderRegistrationClassName = "__EventProviderRegistration";

		// Token: 0x0400051F RID: 1311
		private const string InstanceProviderRegistrationClassName = "__InstanceProviderRegistration";

		// Token: 0x04000520 RID: 1312
		private const string DecoupledProviderClassName = "MSFT_DecoupledProvider";

		// Token: 0x04000521 RID: 1313
		private const string ProviderClassName = "WMINET_ManagedAssemblyProvider";

		// Token: 0x04000522 RID: 1314
		private const string InstrumentationClassName = "WMINET_Instrumentation";

		// Token: 0x04000523 RID: 1315
		private const string InstrumentedAssembliesClassName = "WMINET_InstrumentedAssembly";

		// Token: 0x04000524 RID: 1316
		private const string DecoupledProviderCLSID = "{54D8502C-527D-43f7-A506-A9DA075E229C}";

		// Token: 0x04000525 RID: 1317
		private const string GlobalWmiNetNamespace = "root\\MicrosoftWmiNet";

		// Token: 0x04000526 RID: 1318
		private const string InstrumentedNamespacesClassName = "WMINET_InstrumentedNamespaces";

		// Token: 0x04000527 RID: 1319
		private const string NamingClassName = "WMINET_Naming";

		// Token: 0x04000528 RID: 1320
		private SchemaNaming.AssemblySpecificNaming assemblyInfo;

		// Token: 0x04000529 RID: 1321
		private ManagementObject registrationInstance;

		// Token: 0x0400052A RID: 1322
		private const string iwoaDef = "class IWOA\r\n{\r\nprotected const string DllName = \"wminet_utils.dll\";\r\nprotected const string EntryPointName = \"UFunc\";\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"GetPropertyHandle\")] public static extern int GetPropertyHandle_f27(int vFunc, IntPtr pWbemClassObject, [In][MarshalAs(UnmanagedType.LPWStr)]  string   wszPropertyName, [Out] out Int32 pType, [Out] out Int32 plHandle);\r\n//[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Byte aData);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadPropertyValue\")] public static extern int ReadPropertyValue_f29(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lBufferSize, [Out] out Int32 plNumBytes, [Out] out Byte aData);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadDWORD\")] public static extern int ReadDWORD_f30(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out] out UInt32 pdw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDWORD\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] UInt32 dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadQWORD\")] public static extern int ReadQWORD_f32(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out] out UInt64 pqw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteQWORD\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] UInt64 pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"GetPropertyInfoByHandle\")] public static extern int GetPropertyInfoByHandle_f34(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out][MarshalAs(UnmanagedType.BStr)]  out string   pstrName, [Out] out Int32 pType);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Lock\")] public static extern int Lock_f35(int vFunc, IntPtr pWbemClassObject, [In] Int32 lFlags);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Unlock\")] public static extern int Unlock_f36(int vFunc, IntPtr pWbemClassObject, [In] Int32 lFlags);\r\n\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Put\")] public static extern int Put_f5(int vFunc, IntPtr pWbemClassObject, [In][MarshalAs(UnmanagedType.LPWStr)]  string   wszName, [In] Int32 lFlags, [In] ref object pVal, [In] Int32 Type);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In][MarshalAs(UnmanagedType.LPWStr)] string str);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Byte n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref SByte n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Int16 n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref UInt16 n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\", CharSet=CharSet.Unicode)] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Char c);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDWORD\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteSingle\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Single dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteQWORD\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int64 pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDouble\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Double pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Clone\")] public static extern int Clone_f(int vFunc, IntPtr pWbemClassObject, [Out] out IntPtr ppCopy);\r\n}\r\ninterface IWmiConverter\r\n{\r\n    void ToWMI(object obj);\r\n    ManagementObject GetInstance();\r\n}\r\nclass SafeAssign\r\n{\r\n    public static UInt16 boolTrue = 0xffff;\r\n    public static UInt16 boolFalse = 0;\r\n    static Hashtable validTypes = new Hashtable();\r\n    static SafeAssign()\r\n    {\r\n        validTypes.Add(typeof(SByte), null);\r\n        validTypes.Add(typeof(Byte), null);\r\n        validTypes.Add(typeof(Int16), null);\r\n        validTypes.Add(typeof(UInt16), null);\r\n        validTypes.Add(typeof(Int32), null);\r\n        validTypes.Add(typeof(UInt32), null);\r\n        validTypes.Add(typeof(Int64), null);\r\n        validTypes.Add(typeof(UInt64), null);\r\n        validTypes.Add(typeof(Single), null);\r\n        validTypes.Add(typeof(Double), null);\r\n        validTypes.Add(typeof(Boolean), null);\r\n        validTypes.Add(typeof(String), null);\r\n        validTypes.Add(typeof(Char), null);\r\n        validTypes.Add(typeof(DateTime), null);\r\n        validTypes.Add(typeof(TimeSpan), null);\r\n        validTypes.Add(typeof(ManagementObject), null);\r\n        nullClass.SystemProperties [\"__CLASS\"].Value = \"nullInstance\";\r\n    }\r\n    public static object GetInstance(object o)\r\n    {\r\n        if(o is ManagementObject)\r\n            return o;\r\n        return null;\r\n    }\r\n    static ManagementClass nullClass = new ManagementClass(";

		// Token: 0x0400052B RID: 1323
		private const string iwoaDefEnd = ");\r\n    \r\n    public static ManagementObject GetManagementObject(object o)\r\n    {\r\n        if(o != null && o is ManagementObject)\r\n            return o as ManagementObject;\r\n        // Must return empty instance\r\n        return nullClass.CreateInstance();\r\n    }\r\n    public static object GetValue(object o)\r\n    {\r\n        Type t = o.GetType();\r\n        if(t.IsArray)\r\n            t = t.GetElementType();\r\n        if(validTypes.Contains(t))\r\n            return o;\r\n        return null;\r\n    }\r\n    public static string WMITimeToString(DateTime dt)\r\n    {\r\n        TimeSpan ts = dt.Subtract(dt.ToUniversalTime());\r\n        int diffUTC = (ts.Minutes + ts.Hours * 60);\r\n        if(diffUTC >= 0)\r\n            return String.Format(\"{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.{6:D3}000+{7:D3}\", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, diffUTC);\r\n        return String.Format(\"{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.{6:D3}000-{7:D3}\", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, -diffUTC);\r\n    }\r\n    public static string WMITimeToString(TimeSpan ts)\r\n    {\r\n        return String.Format(\"{0:D8}{1:D2}{2:D2}{3:D2}.{4:D3}000:000\", ts.Days, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);\r\n    }\r\n    public static string[] WMITimeArrayToStringArray(DateTime[] dates)\r\n    {\r\n        string[] strings = new string[dates.Length];\r\n        for(int i=0;i<dates.Length;i++)\r\n            strings[i] = WMITimeToString(dates[i]);\r\n        return strings;\r\n    }\r\n    public static string[] WMITimeArrayToStringArray(TimeSpan[] timeSpans)\r\n    {\r\n        string[] strings = new string[timeSpans.Length];\r\n        for(int i=0;i<timeSpans.Length;i++)\r\n            strings[i] = WMITimeToString(timeSpans[i]);\r\n        return strings;\r\n    }\r\n}\r\n";

		// Token: 0x0200010C RID: 268
		private class InstallLogWrapper
		{
			// Token: 0x0600067B RID: 1659 RVA: 0x0002796B File Offset: 0x00025B6B
			public InstallLogWrapper(InstallContext context)
			{
				this.context = context;
			}

			// Token: 0x0600067C RID: 1660 RVA: 0x0002797A File Offset: 0x00025B7A
			public void LogMessage(string str)
			{
				if (this.context != null)
				{
					this.context.LogMessage(str);
				}
			}

			// Token: 0x0400057D RID: 1405
			private InstallContext context;
		}

		// Token: 0x0200010D RID: 269
		private class AssemblySpecificNaming
		{
			// Token: 0x0600067D RID: 1661 RVA: 0x00027990 File Offset: 0x00025B90
			public AssemblySpecificNaming(string namespaceName, string securityDescriptor, Assembly assembly)
			{
				this.namespaceName = namespaceName;
				this.securityDescriptor = securityDescriptor;
				this.decoupledProviderInstanceName = AssemblyNameUtility.UniqueToAssemblyFullVersion(assembly);
				this.assemblyUniqueIdentifier = AssemblyNameUtility.UniqueToAssemblyBuild(assembly);
				this.assemblyName = assembly.FullName;
				this.assemblyPath = assembly.Location;
			}

			// Token: 0x170000E6 RID: 230
			// (get) Token: 0x0600067E RID: 1662 RVA: 0x000279E1 File Offset: 0x00025BE1
			public string NamespaceName
			{
				get
				{
					return this.namespaceName;
				}
			}

			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x0600067F RID: 1663 RVA: 0x000279E9 File Offset: 0x00025BE9
			public string SecurityDescriptor
			{
				get
				{
					return this.securityDescriptor;
				}
			}

			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x06000680 RID: 1664 RVA: 0x000279F1 File Offset: 0x00025BF1
			// (set) Token: 0x06000681 RID: 1665 RVA: 0x000279F9 File Offset: 0x00025BF9
			public string DecoupledProviderInstanceName
			{
				get
				{
					return this.decoupledProviderInstanceName;
				}
				set
				{
					this.decoupledProviderInstanceName = value;
				}
			}

			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x06000682 RID: 1666 RVA: 0x00027A02 File Offset: 0x00025C02
			public string AssemblyUniqueIdentifier
			{
				get
				{
					return this.assemblyUniqueIdentifier;
				}
			}

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x06000683 RID: 1667 RVA: 0x00027A0A File Offset: 0x00025C0A
			public string AssemblyName
			{
				get
				{
					return this.assemblyName;
				}
			}

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x06000684 RID: 1668 RVA: 0x00027A12 File Offset: 0x00025C12
			public string AssemblyPath
			{
				get
				{
					return this.assemblyPath;
				}
			}

			// Token: 0x0400057E RID: 1406
			private string namespaceName;

			// Token: 0x0400057F RID: 1407
			private string securityDescriptor;

			// Token: 0x04000580 RID: 1408
			private string decoupledProviderInstanceName;

			// Token: 0x04000581 RID: 1409
			private string assemblyUniqueIdentifier;

			// Token: 0x04000582 RID: 1410
			private string assemblyName;

			// Token: 0x04000583 RID: 1411
			private string assemblyPath;
		}

		// Token: 0x0200010E RID: 270
		// (Invoke) Token: 0x06000686 RID: 1670
		private delegate ManagementClass ClassMaker();
	}
}
