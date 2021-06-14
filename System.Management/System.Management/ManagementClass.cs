using System;
using System.CodeDom;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Management
{
	/// <summary>Represents a Common Information Model (CIM) management class. A management class is a WMI class such as Win32_LogicalDisk, which can represent a disk drive, and Win32_Process, which represents a process such as Notepad.exe. The members of this class enable you to access WMI data using a specific WMI class path. For more information, see "Win32 Classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.</summary>
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class ManagementClass : ManagementObject
	{
		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data necessary to deserialize the field represented by this instance.          </summary>
		/// <param name="info">The object to be populated with serialization information.</param>
		/// <param name="context">The location where serialized data will be stored and retrieved.</param>
		// Token: 0x06000020 RID: 32 RVA: 0x0000284C File Offset: 0x00000A4C
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002858 File Offset: 0x00000A58
		internal static ManagementClass GetManagementClass(IWbemClassObjectFreeThreaded wbemObject, ManagementClass mgObj)
		{
			ManagementClass managementClass = new ManagementClass();
			managementClass.wbemObject = wbemObject;
			if (mgObj != null)
			{
				managementClass.scope = ManagementScope._Clone(mgObj.scope);
				ManagementPath path = mgObj.Path;
				if (path != null)
				{
					managementClass.path = ManagementPath._Clone(path);
				}
				object obj = null;
				int num = 0;
				int num2 = wbemObject.Get_("__CLASS", 0, ref obj, ref num, ref num);
				if (num2 < 0)
				{
					if (((long)num2 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				if (obj != DBNull.Value)
				{
					managementClass.path.internalClassName = (string)obj;
				}
				ObjectGetOptions options = mgObj.Options;
				if (options != null)
				{
					managementClass.options = ObjectGetOptions._Clone(options);
				}
			}
			return managementClass;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000291C File Offset: 0x00000B1C
		internal static ManagementClass GetManagementClass(IWbemClassObjectFreeThreaded wbemObject, ManagementScope scope)
		{
			ManagementClass managementClass = new ManagementClass();
			managementClass.path = new ManagementPath(ManagementPath.GetManagementPath(wbemObject));
			if (scope != null)
			{
				managementClass.scope = ManagementScope._Clone(scope);
			}
			managementClass.wbemObject = wbemObject;
			return managementClass;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementClass" /> class. This is the default constructor.</summary>
		// Token: 0x06000023 RID: 35 RVA: 0x00002957 File Offset: 0x00000B57
		public ManagementClass() : this(null, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementClass" /> class. The class represents a Common Information Model (CIM) management class from WMI such as Win32_LogicalDisk, which can represent a disk drive, and Win32_Process, which represents a process such as Notepad.exe. For more information, see "Win32 Classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.</summary>
		/// <param name="path">A <see cref="T:System.Management.ManagementPath" /> specifying the WMI class to which to bind. The parameter must specify a WMI class path. The class represents a CIM management class from WMI. CIM classes represent management information including hardware, software, processes, and so on. For more information about the CIM classes available in Windows, see "Win32 classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library. </param>
		// Token: 0x06000024 RID: 36 RVA: 0x00002962 File Offset: 0x00000B62
		public ManagementClass(ManagementPath path) : this(null, path, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementClass" /> class initialized to the given path. The class represents a Common Information Model (CIM) management class from WMI such as Win32_LogicalDisk, which can represent a disk drive, and Win32_Process, which represents a process such as Notepad.exe. For more information, see "Win32 Classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.</summary>
		/// <param name="path">The path to the WMI class. The class represents a CIM management class from WMI. CIM classes represent management information including hardware, software, processes, and so on. For more information about the CIM classes available in Windows, see "Win32 classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.</param>
		// Token: 0x06000025 RID: 37 RVA: 0x0000296D File Offset: 0x00000B6D
		public ManagementClass(string path) : this(null, new ManagementPath(path), null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementClass" /> class initialized to the given WMI class path using the specified options. The class represents a Common Information Model (CIM) management class from WMI such as Win32_LogicalDisk, which can represent a disk drive, and Win32_Process, which represents a process such as Notepad.exe. For more information, see "Win32 Classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.</summary>
		/// <param name="path">A <see cref="T:System.Management.ManagementPath" /> instance representing the WMI class path. The class represents a CIM management class from WMI. CIM classes represent management information including hardware, software, processes, and so on. For more information about the CIM classes available in Windows, see "Win32 classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.</param>
		/// <param name="options">An <see cref="T:System.Management.ObjectGetOptions" /> representing the options to use when retrieving this class. </param>
		// Token: 0x06000026 RID: 38 RVA: 0x0000297D File Offset: 0x00000B7D
		public ManagementClass(ManagementPath path, ObjectGetOptions options) : this(null, path, options)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementClass" /> class initialized to the given WMI class path using the specified options. The class represents a Common Information Model (CIM) management class from WMI such as Win32_LogicalDisk, which can represent a disk drive, and Win32_Process, which represents a process such as Notepad.exe. For more information, see "Win32 Classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.</summary>
		/// <param name="path">The path to the WMI class. The class represents a CIM management class from WMI. CIM classes represent management information including hardware, software, processes, and so on. For more information about the CIM classes available in Windows, see "Win32 classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library. </param>
		/// <param name="options">An <see cref="T:System.Management.ObjectGetOptions" /> representing the options to use when retrieving the WMI class. </param>
		// Token: 0x06000027 RID: 39 RVA: 0x00002988 File Offset: 0x00000B88
		public ManagementClass(string path, ObjectGetOptions options) : this(null, new ManagementPath(path), options)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementClass" /> class for the specified WMI class in the specified scope and with the specified options. The class represents a Common Information Model (CIM) management class from WMI such as Win32_LogicalDisk, which can represent a disk drive, and Win32_Process, which represents a process such as Notepad.exe. For more information, see "Win32 Classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.</summary>
		/// <param name="scope">A <see cref="T:System.Management.ManagementScope" /> that specifies the scope (server and namespace) where the WMI class resides. </param>
		/// <param name="path">A <see cref="T:System.Management.ManagementPath" /> that represents the path to the WMI class in the specified scope. The class represents a CIM management class from WMI. CIM classes represent management information including hardware, software, processes, and so on. For more information about the CIM classes available in Windows, see "Win32 classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.  </param>
		/// <param name="options">An <see cref="T:System.Management.ObjectGetOptions" /> that specifies the options to use when retrieving the WMI class. </param>
		// Token: 0x06000028 RID: 40 RVA: 0x00002998 File Offset: 0x00000B98
		public ManagementClass(ManagementScope scope, ManagementPath path, ObjectGetOptions options) : base(scope, path, options)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementClass" /> class for the specified WMI class, in the specified scope, and with the specified options. The class represents a Common Information Model (CIM) management class from WMI such as Win32_LogicalDisk, which can represent a disk drive, and Win32_Process, which represents a process such as Notepad.exe. For more information, see "Win32 Classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.</summary>
		/// <param name="scope">The scope in which the WMI class resides. </param>
		/// <param name="path">The path to the WMI class within the specified scope. The class represents a CIM management class from WMI. CIM classes represent management information including hardware, software, processes, and so on. For more information about the CIM classes available in Windows, see "Win32 classes" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library. </param>
		/// <param name="options">An <see cref="T:System.Management.ObjectGetOptions" /> that specifies the options to use when retrieving the WMI class. </param>
		// Token: 0x06000029 RID: 41 RVA: 0x000029A3 File Offset: 0x00000BA3
		public ManagementClass(string scope, string path, ObjectGetOptions options) : base(new ManagementScope(scope), new ManagementPath(path), options)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementClass" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="info">An instance of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> class containing the information required to serialize the new <see cref="T:System.Management.ManagementClass" />.</param>
		/// <param name="context">An instance of the <see cref="T:System.Runtime.Serialization.StreamingContext" /> class containing the source of the serialized stream associated with the new <see cref="T:System.Management.ManagementClass" />.</param>
		// Token: 0x0600002A RID: 42 RVA: 0x000029B8 File Offset: 0x00000BB8
		protected ManagementClass(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Gets or sets the path of the WMI class to which the <see cref="T:System.Management.ManagementClass" /> object is bound.</summary>
		/// <returns>The path of the object's class.</returns>
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000029C2 File Offset: 0x00000BC2
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000029CA File Offset: 0x00000BCA
		public override ManagementPath Path
		{
			get
			{
				return base.Path;
			}
			set
			{
				if (value == null || value.IsClass || value.IsEmpty)
				{
					base.Path = value;
					return;
				}
				throw new ArgumentOutOfRangeException("value");
			}
		}

		/// <summary>Gets an array containing all WMI classes in the inheritance hierarchy from this class to the top of the hierarchy.</summary>
		/// <returns>A string collection containing the names of all WMI classes in the inheritance hierarchy of this class.</returns>
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000029F4 File Offset: 0x00000BF4
		public StringCollection Derivation
		{
			get
			{
				StringCollection stringCollection = new StringCollection();
				int num = 0;
				int num2 = 0;
				object obj = null;
				int num3 = base.wbemObject.Get_("__DERIVATION", 0, ref obj, ref num, ref num2);
				if (num3 < 0)
				{
					if (((long)num3 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num3);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num3, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				if (obj != null)
				{
					stringCollection.AddRange((string[])obj);
				}
				return stringCollection;
			}
		}

		/// <summary>Gets or sets a collection of <see cref="T:System.Management.MethodData" /> objects that represent the methods defined in the WMI class.</summary>
		/// <returns>A <see cref="T:System.Management.MethodDataCollection" /> representing the methods defined in the WMI class.</returns>
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002A67 File Offset: 0x00000C67
		public MethodDataCollection Methods
		{
			get
			{
				this.Initialize(true);
				if (this.methods == null)
				{
					this.methods = new MethodDataCollection(this);
				}
				return this.methods;
			}
		}

		/// <summary>Returns the collection of all instances of the class.</summary>
		/// <returns>A collection of the <see cref="T:System.Management.ManagementObject" /> objects representing the instances of the class.</returns>
		// Token: 0x0600002F RID: 47 RVA: 0x00002A8A File Offset: 0x00000C8A
		public ManagementObjectCollection GetInstances()
		{
			return this.GetInstances(null);
		}

		/// <summary>Returns the collection of all instances of the class using the specified options.</summary>
		/// <param name="options">The additional operation options. </param>
		/// <returns>A collection of the <see cref="T:System.Management.ManagementObject" /> objects representing the instances of the class, according to the specified options.</returns>
		// Token: 0x06000030 RID: 48 RVA: 0x00002A94 File Offset: 0x00000C94
		public ManagementObjectCollection GetInstances(EnumerationOptions options)
		{
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			IEnumWbemClassObject enumWbem = null;
			EnumerationOptions enumerationOptions = (options == null) ? new EnumerationOptions() : ((EnumerationOptions)options.Clone());
			enumerationOptions.EnsureLocatable = false;
			enumerationOptions.PrototypeOnly = false;
			SecurityHandler securityHandler = null;
			int num = 0;
			try
			{
				securityHandler = base.Scope.GetSecurityHandler();
				num = this.scope.GetSecuredIWbemServicesHandler(base.Scope.GetIWbemServices()).CreateInstanceEnum_(base.ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbem);
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
				}
			}
			if (num < 0)
			{
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return new ManagementObjectCollection(base.Scope, enumerationOptions, enumWbem);
		}

		/// <summary>Returns the collection of all instances of the class, asynchronously.</summary>
		/// <param name="watcher">The object to handle the asynchronous operation's progress. </param>
		// Token: 0x06000031 RID: 49 RVA: 0x00002B94 File Offset: 0x00000D94
		public void GetInstances(ManagementOperationObserver watcher)
		{
			this.GetInstances(watcher, null);
		}

		/// <summary>Returns the collection of all instances of the class, asynchronously, using the specified options.</summary>
		/// <param name="watcher">The object to handle the asynchronous operation's progress. </param>
		/// <param name="options">The specified additional options for getting the instances. </param>
		// Token: 0x06000032 RID: 50 RVA: 0x00002BA0 File Offset: 0x00000DA0
		public void GetInstances(ManagementOperationObserver watcher, EnumerationOptions options)
		{
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			EnumerationOptions enumerationOptions = (options == null) ? new EnumerationOptions() : ((EnumerationOptions)options.Clone());
			enumerationOptions.EnsureLocatable = false;
			enumerationOptions.PrototypeOnly = false;
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(base.Scope, enumerationOptions.Context);
			SecurityHandler securityHandler = base.Scope.GetSecurityHandler();
			int num = this.scope.GetSecuredIWbemServicesHandler(base.Scope.GetIWbemServices()).CreateInstanceEnumAsync_(base.ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
			if (securityHandler != null)
			{
				securityHandler.Reset();
			}
			if (num < 0)
			{
				watcher.RemoveSink(newSink);
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		/// <summary>Returns the collection of all subclasses for the class.</summary>
		/// <returns>A collection of the <see cref="T:System.Management.ManagementObject" /> objects that represent the subclasses of the WMI class.</returns>
		// Token: 0x06000033 RID: 51 RVA: 0x00002CBC File Offset: 0x00000EBC
		public ManagementObjectCollection GetSubclasses()
		{
			return this.GetSubclasses(null);
		}

		/// <summary>Retrieves the subclasses of the class using the specified options.</summary>
		/// <param name="options">The specified additional options for retrieving subclasses of the class. </param>
		/// <returns>A collection of the <see cref="T:System.Management.ManagementObject" /> objects representing the subclasses of the WMI class, according to the specified options.</returns>
		// Token: 0x06000034 RID: 52 RVA: 0x00002CC8 File Offset: 0x00000EC8
		public ManagementObjectCollection GetSubclasses(EnumerationOptions options)
		{
			if (this.Path == null)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			IEnumWbemClassObject enumWbem = null;
			EnumerationOptions enumerationOptions = (options == null) ? new EnumerationOptions() : ((EnumerationOptions)options.Clone());
			enumerationOptions.EnsureLocatable = false;
			enumerationOptions.PrototypeOnly = false;
			SecurityHandler securityHandler = null;
			int num = 0;
			try
			{
				securityHandler = base.Scope.GetSecurityHandler();
				num = this.scope.GetSecuredIWbemServicesHandler(base.Scope.GetIWbemServices()).CreateClassEnum_(base.ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbem);
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
				}
			}
			if (num < 0)
			{
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return new ManagementObjectCollection(base.Scope, enumerationOptions, enumWbem);
		}

		/// <summary>Returns the collection of all classes derived from this class, asynchronously.</summary>
		/// <param name="watcher">The object to handle the asynchronous operation's progress. </param>
		// Token: 0x06000035 RID: 53 RVA: 0x00002DA8 File Offset: 0x00000FA8
		public void GetSubclasses(ManagementOperationObserver watcher)
		{
			this.GetSubclasses(watcher, null);
		}

		/// <summary>Retrieves all classes derived from this class, asynchronously, using the specified options.</summary>
		/// <param name="watcher">The object to handle the asynchronous operation's progress. </param>
		/// <param name="options">The specified additional options to use in the derived class retrieval. </param>
		// Token: 0x06000036 RID: 54 RVA: 0x00002DB4 File Offset: 0x00000FB4
		public void GetSubclasses(ManagementOperationObserver watcher, EnumerationOptions options)
		{
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			if (this.Path == null)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			EnumerationOptions enumerationOptions = (options == null) ? new EnumerationOptions() : ((EnumerationOptions)options.Clone());
			enumerationOptions.EnsureLocatable = false;
			enumerationOptions.PrototypeOnly = false;
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(base.Scope, enumerationOptions.Context);
			SecurityHandler securityHandler = base.Scope.GetSecurityHandler();
			int num = this.scope.GetSecuredIWbemServicesHandler(base.Scope.GetIWbemServices()).CreateClassEnumAsync_(base.ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
			if (securityHandler != null)
			{
				securityHandler.Reset();
			}
			if (num < 0)
			{
				watcher.RemoveSink(newSink);
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		/// <summary>Derives a new class from this class.</summary>
		/// <param name="newClassName">The name of the new class to be derived. </param>
		/// <returns>A new <see cref="T:System.Management.ManagementClass" /> that represents a new WMI class derived from the original class.</returns>
		// Token: 0x06000037 RID: 55 RVA: 0x00002EB4 File Offset: 0x000010B4
		public ManagementClass Derive(string newClassName)
		{
			ManagementClass result = null;
			if (newClassName == null)
			{
				throw new ArgumentNullException("newClassName");
			}
			ManagementPath managementPath = new ManagementPath();
			try
			{
				managementPath.ClassName = newClassName;
			}
			catch
			{
				throw new ArgumentOutOfRangeException("newClassName");
			}
			if (!managementPath.IsClass)
			{
				throw new ArgumentOutOfRangeException("newClassName");
			}
			if (base.PutButNotGot)
			{
				base.Get();
				base.PutButNotGot = false;
			}
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
			int num = base.wbemObject.SpawnDerivedClass_(0, out wbemClassObjectFreeThreaded);
			if (num >= 0)
			{
				object obj = newClassName;
				num = wbemClassObjectFreeThreaded.Put_("__CLASS", 0, ref obj, 0);
				if (num >= 0)
				{
					result = ManagementClass.GetManagementClass(wbemClassObjectFreeThreaded, this);
				}
			}
			if (num < 0)
			{
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return result;
		}

		/// <summary>Initializes a new instance of the WMI class.</summary>
		/// <returns>A <see cref="T:System.Management.ManagementObject" /> that represents a new instance of the WMI class.</returns>
		// Token: 0x06000038 RID: 56 RVA: 0x00002F88 File Offset: 0x00001188
		public ManagementObject CreateInstance()
		{
			ManagementObject result = null;
			if (base.PutButNotGot)
			{
				base.Get();
				base.PutButNotGot = false;
			}
			IWbemClassObjectFreeThreaded wbemObject = null;
			int num = base.wbemObject.SpawnInstance_(0, out wbemObject);
			if (num >= 0)
			{
				result = ManagementObject.GetManagementObject(wbemObject, base.Scope);
			}
			else if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
			}
			else
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
			return result;
		}

		/// <summary>Returns a copy of the object.</summary>
		/// <returns>The cloned object.</returns>
		// Token: 0x06000039 RID: 57 RVA: 0x00002FFC File Offset: 0x000011FC
		public override object Clone()
		{
			IWbemClassObjectFreeThreaded wbemObject = null;
			int num = base.wbemObject.Clone_(out wbemObject);
			if (num < 0)
			{
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return ManagementClass.GetManagementClass(wbemObject, this);
		}

		/// <summary>Retrieves classes related to the WMI class.</summary>
		/// <returns>A collection of the <see cref="T:System.Management.ManagementClass" /> or <see cref="T:System.Management.ManagementObject" /> objects that represents WMI classes or instances related to the WMI class.</returns>
		// Token: 0x0600003A RID: 58 RVA: 0x0000304D File Offset: 0x0000124D
		public ManagementObjectCollection GetRelatedClasses()
		{
			return this.GetRelatedClasses(null);
		}

		/// <summary>Retrieves classes related to the WMI class.</summary>
		/// <param name="relatedClass">The class from which resulting classes have to be derived. </param>
		/// <returns>A collection of classes related to this class.</returns>
		// Token: 0x0600003B RID: 59 RVA: 0x00003056 File Offset: 0x00001256
		public ManagementObjectCollection GetRelatedClasses(string relatedClass)
		{
			return this.GetRelatedClasses(relatedClass, null, null, null, null, null, null);
		}

		/// <summary>Retrieves classes related to the WMI class based on the specified options.</summary>
		/// <param name="relatedClass">The class from which resulting classes have to be derived. </param>
		/// <param name="relationshipClass">The relationship type which resulting classes must have with the source class. </param>
		/// <param name="relationshipQualifier">This qualifier must be present on the relationship. </param>
		/// <param name="relatedQualifier">This qualifier must be present on the resulting classes. </param>
		/// <param name="relatedRole">The resulting classes must have this role in the relationship. </param>
		/// <param name="thisRole">The source class must have this role in the relationship. </param>
		/// <param name="options">The options for retrieving the resulting classes. </param>
		/// <returns>A collection of classes related to this class.</returns>
		// Token: 0x0600003C RID: 60 RVA: 0x00003068 File Offset: 0x00001268
		public ManagementObjectCollection GetRelatedClasses(string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, EnumerationOptions options)
		{
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			IEnumWbemClassObject enumWbem = null;
			EnumerationOptions enumerationOptions = (options != null) ? ((EnumerationOptions)options.Clone()) : new EnumerationOptions();
			enumerationOptions.EnumerateDeep = true;
			RelatedObjectQuery relatedObjectQuery = new RelatedObjectQuery(true, this.Path.Path, relatedClass, relationshipClass, relatedQualifier, relationshipQualifier, relatedRole, thisRole);
			SecurityHandler securityHandler = null;
			int num = 0;
			try
			{
				securityHandler = base.Scope.GetSecurityHandler();
				num = this.scope.GetSecuredIWbemServicesHandler(base.Scope.GetIWbemServices()).ExecQuery_(relatedObjectQuery.QueryLanguage, relatedObjectQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbem);
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
				}
			}
			if (num < 0)
			{
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return new ManagementObjectCollection(base.Scope, enumerationOptions, enumWbem);
		}

		/// <summary>Retrieves classes related to the WMI class, asynchronously.</summary>
		/// <param name="watcher">The object to handle the asynchronous operation's progress. </param>
		// Token: 0x0600003D RID: 61 RVA: 0x00003188 File Offset: 0x00001388
		public void GetRelatedClasses(ManagementOperationObserver watcher)
		{
			this.GetRelatedClasses(watcher, null);
		}

		/// <summary>Retrieves classes related to the WMI class, asynchronously, given the related class name.</summary>
		/// <param name="watcher">The object to handle the asynchronous operation's progress. </param>
		/// <param name="relatedClass">The name of the related class. </param>
		// Token: 0x0600003E RID: 62 RVA: 0x00003194 File Offset: 0x00001394
		public void GetRelatedClasses(ManagementOperationObserver watcher, string relatedClass)
		{
			this.GetRelatedClasses(watcher, relatedClass, null, null, null, null, null, null);
		}

		/// <summary>Retrieves classes related to the WMI class, asynchronously, using the specified options.</summary>
		/// <param name="watcher">Handler for progress and results of the asynchronous operation. </param>
		/// <param name="relatedClass">The class from which resulting classes have to be derived. </param>
		/// <param name="relationshipClass">The relationship type which resulting classes must have with the source class. </param>
		/// <param name="relationshipQualifier">This qualifier must be present on the relationship. </param>
		/// <param name="relatedQualifier">This qualifier must be present on the resulting classes. </param>
		/// <param name="relatedRole">The resulting classes must have this role in the relationship. </param>
		/// <param name="thisRole">The source class must have this role in the relationship. </param>
		/// <param name="options">The options for retrieving the resulting classes. </param>
		// Token: 0x0600003F RID: 63 RVA: 0x000031B0 File Offset: 0x000013B0
		public void GetRelatedClasses(ManagementOperationObserver watcher, string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, EnumerationOptions options)
		{
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(true);
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			EnumerationOptions enumerationOptions = (options != null) ? ((EnumerationOptions)options.Clone()) : new EnumerationOptions();
			enumerationOptions.EnumerateDeep = true;
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(base.Scope, enumerationOptions.Context);
			RelatedObjectQuery relatedObjectQuery = new RelatedObjectQuery(true, this.Path.Path, relatedClass, relationshipClass, relatedQualifier, relationshipQualifier, relatedRole, thisRole);
			SecurityHandler securityHandler = base.Scope.GetSecurityHandler();
			int num = this.scope.GetSecuredIWbemServicesHandler(base.Scope.GetIWbemServices()).ExecQueryAsync_(relatedObjectQuery.QueryLanguage, relatedObjectQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
			if (securityHandler != null)
			{
				securityHandler.Reset();
			}
			if (num < 0)
			{
				watcher.RemoveSink(newSink);
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		/// <summary>Retrieves relationship classes that relate the class to others.</summary>
		/// <returns>A collection of association classes that relate the class to any other class.</returns>
		// Token: 0x06000040 RID: 64 RVA: 0x000032EF File Offset: 0x000014EF
		public ManagementObjectCollection GetRelationshipClasses()
		{
			return this.GetRelationshipClasses(null);
		}

		/// <summary>Retrieves relationship classes that relate the class to others, where the endpoint class is the specified class.</summary>
		/// <param name="relationshipClass">The endpoint class for all relationship classes returned. </param>
		/// <returns>A collection of association classes that relate the class to the specified class. For more information about relationship classes, see "ASSOCIATORS OF Statement" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.</returns>
		// Token: 0x06000041 RID: 65 RVA: 0x000032F8 File Offset: 0x000014F8
		public ManagementObjectCollection GetRelationshipClasses(string relationshipClass)
		{
			return this.GetRelationshipClasses(relationshipClass, null, null, null);
		}

		/// <summary>Retrieves relationship classes that relate this class to others, according to specified options.</summary>
		/// <param name="relationshipClass">All resulting relationship classes must derive from this class. </param>
		/// <param name="relationshipQualifier">Resulting relationship classes must have this qualifier. </param>
		/// <param name="thisRole">The source class must have this role in the resulting relationship classes. </param>
		/// <param name="options">Specifies options for retrieving the results. </param>
		/// <returns>A collection of association classes that relate this class to others, according to the specified options. For more information about relationship classes, see "ASSOCIATORS OF Statement" in the Windows Management Instrumentation documentation in the MSDN Library at http://msdn.microsoft.com/library.</returns>
		// Token: 0x06000042 RID: 66 RVA: 0x00003304 File Offset: 0x00001504
		public ManagementObjectCollection GetRelationshipClasses(string relationshipClass, string relationshipQualifier, string thisRole, EnumerationOptions options)
		{
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			this.Initialize(false);
			IEnumWbemClassObject enumWbem = null;
			EnumerationOptions enumerationOptions = (options != null) ? options : new EnumerationOptions();
			enumerationOptions.EnumerateDeep = true;
			RelationshipQuery relationshipQuery = new RelationshipQuery(true, this.Path.Path, relationshipClass, relationshipQualifier, thisRole);
			SecurityHandler securityHandler = null;
			int num = 0;
			try
			{
				securityHandler = base.Scope.GetSecurityHandler();
				num = this.scope.GetSecuredIWbemServicesHandler(base.Scope.GetIWbemServices()).ExecQuery_(relationshipQuery.QueryLanguage, relationshipQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbem);
			}
			finally
			{
				if (securityHandler != null)
				{
					securityHandler.Reset();
				}
			}
			if (num < 0)
			{
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return new ManagementObjectCollection(base.Scope, enumerationOptions, enumWbem);
		}

		/// <summary>Retrieves relationship classes that relate the class to others, asynchronously.</summary>
		/// <param name="watcher">The object to handle the asynchronous operation's progress. </param>
		// Token: 0x06000043 RID: 67 RVA: 0x00003414 File Offset: 0x00001614
		public void GetRelationshipClasses(ManagementOperationObserver watcher)
		{
			this.GetRelationshipClasses(watcher, null);
		}

		/// <summary>Retrieves relationship classes that relate the class to the specified WMI class, asynchronously.</summary>
		/// <param name="watcher">The object to handle the asynchronous operation's progress. </param>
		/// <param name="relationshipClass">The WMI class to which all returned relationships should point. </param>
		// Token: 0x06000044 RID: 68 RVA: 0x0000341E File Offset: 0x0000161E
		public void GetRelationshipClasses(ManagementOperationObserver watcher, string relationshipClass)
		{
			this.GetRelationshipClasses(watcher, relationshipClass, null, null, null);
		}

		/// <summary>Retrieves relationship classes that relate the class according to the specified options, asynchronously.</summary>
		/// <param name="watcher">The handler for progress and results of the asynchronous operation. </param>
		/// <param name="relationshipClass">The class from which all resulting relationship classes must derive. </param>
		/// <param name="relationshipQualifier">The qualifier which the resulting relationship classes must have. </param>
		/// <param name="thisRole">The role which the source class must have in the resulting relationship classes. </param>
		/// <param name="options">The options for retrieving the results. </param>
		// Token: 0x06000045 RID: 69 RVA: 0x0000342C File Offset: 0x0000162C
		public void GetRelationshipClasses(ManagementOperationObserver watcher, string relationshipClass, string relationshipQualifier, string thisRole, EnumerationOptions options)
		{
			if (this.Path == null || this.Path.Path == null || this.Path.Path.Length == 0)
			{
				throw new InvalidOperationException();
			}
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			this.Initialize(true);
			EnumerationOptions enumerationOptions = (options != null) ? ((EnumerationOptions)options.Clone()) : new EnumerationOptions();
			enumerationOptions.EnumerateDeep = true;
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(base.Scope, enumerationOptions.Context);
			RelationshipQuery relationshipQuery = new RelationshipQuery(true, this.Path.Path, relationshipClass, relationshipQualifier, thisRole);
			SecurityHandler securityHandler = base.Scope.GetSecurityHandler();
			int num = this.scope.GetSecuredIWbemServicesHandler(base.Scope.GetIWbemServices()).ExecQueryAsync_(relationshipQuery.QueryLanguage, relationshipQuery.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
			if (securityHandler != null)
			{
				securityHandler.Reset();
			}
			if (num < 0)
			{
				watcher.RemoveSink(newSink);
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		/// <summary>Generates a strongly-typed class for a given WMI class.</summary>
		/// <param name="includeSystemClassInClassDef">
		///       <see langword="true" /> to include the class for managing system properties; otherwise, <see langword="false" />. </param>
		/// <param name="systemPropertyClass">
		///       <see langword="true" /> to have the generated class manage system properties; otherwise, <see langword="false" />. </param>
		/// <returns>A <see cref="T:System.CodeDom.CodeTypeDeclaration" /> representing the declaration for the strongly-typed class.</returns>
		// Token: 0x06000046 RID: 70 RVA: 0x00003568 File Offset: 0x00001768
		public CodeTypeDeclaration GetStronglyTypedClassCode(bool includeSystemClassInClassDef, bool systemPropertyClass)
		{
			base.Get();
			ManagementClassGenerator managementClassGenerator = new ManagementClassGenerator(this);
			return managementClassGenerator.GenerateCode(includeSystemClassInClassDef, systemPropertyClass);
		}

		/// <summary>Generates a strongly-typed class for a given WMI class. This function generates code for Visual Basic, C#, JScript, J#, or C++ depending on the input parameters.</summary>
		/// <param name="lang">The language of the code to be generated. This code language comes from the <see cref="T:System.Management.CodeLanguage" /> enumeration.</param>
		/// <param name="filePath">The path of the file where the code is to be written. </param>
		/// <param name="classNamespace">The.NET namespace into which the class should be generated. If this is empty, the namespace will be generated from the WMI namespace. </param>
		/// <returns>
		///     <see langword="true" />, if the method succeeded; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000047 RID: 71 RVA: 0x0000358C File Offset: 0x0000178C
		public bool GetStronglyTypedClassCode(CodeLanguage lang, string filePath, string classNamespace)
		{
			base.Get();
			ManagementClassGenerator managementClassGenerator = new ManagementClassGenerator(this);
			return managementClassGenerator.GenerateCode(lang, filePath, classNamespace);
		}

		// Token: 0x04000077 RID: 119
		private MethodDataCollection methods;
	}
}
