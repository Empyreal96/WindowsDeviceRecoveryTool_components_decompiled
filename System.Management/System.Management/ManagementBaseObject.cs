using System;
using System.ComponentModel;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management
{
	/// <summary>Contains the basic elements of a management object. It serves as a base class to more specific management object classes.</summary>
	// Token: 0x02000009 RID: 9
	[ToolboxItem(false)]
	[Serializable]
	public class ManagementBaseObject : Component, ICloneable, ISerializable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002067 File Offset: 0x00000267
		internal IWbemClassObjectFreeThreaded wbemObject
		{
			get
			{
				if (this._wbemObject == null)
				{
					this.Initialize(true);
				}
				return this._wbemObject;
			}
			set
			{
				this._wbemObject = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementBaseObject" /> class that is serializable.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" /> ) for this serialization.</param>
		// Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
		protected ManagementBaseObject(SerializationInfo info, StreamingContext context)
		{
			this._wbemObject = (info.GetValue("wbemObject", typeof(IWbemClassObjectFreeThreaded)) as IWbemClassObjectFreeThreaded);
			if (this._wbemObject == null)
			{
				throw new SerializationException();
			}
			this.properties = null;
			this.systemProperties = null;
			this.qualifiers = null;
		}

		/// <summary>Releases the unmanaged resources used by the ManagementBaseObject.</summary>
		// Token: 0x06000004 RID: 4 RVA: 0x000020C6 File Offset: 0x000002C6
		public new void Dispose()
		{
			if (this._wbemObject != null)
			{
				this._wbemObject.Dispose();
				this._wbemObject = null;
			}
			base.Dispose();
			GC.SuppressFinalize(this);
		}

		/// <summary>Provides the internal WMI object represented by a <see cref="T:System.Management.ManagementObject" />.  </summary>
		/// <param name="managementObject">The <see cref="T:System.Management.ManagementBaseObject" /> that references the requested WMI object.</param>
		/// <returns>An <see cref="T:System.IntPtr" /> representing the internal WMI object.  </returns>
		// Token: 0x06000005 RID: 5 RVA: 0x000020EE File Offset: 0x000002EE
		public static explicit operator IntPtr(ManagementBaseObject managementObject)
		{
			if (managementObject == null)
			{
				return IntPtr.Zero;
			}
			return managementObject.wbemObject;
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Management.ManagementBaseObject" />.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> containing the information required to serialize the <see cref="T:System.Management.ManagementBaseObject" />.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> containing the source and destination of the serialized stream associated with the <see cref="T:System.Management.ManagementBaseObject" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="info" /> is <see langword="null" />.</exception>
		// Token: 0x06000006 RID: 6 RVA: 0x00002104 File Offset: 0x00000304
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("wbemObject", this.wbemObject, typeof(IWbemClassObjectFreeThreaded));
			info.AssemblyName = typeof(ManagementBaseObject).Assembly.FullName;
			info.FullTypeName = typeof(ManagementBaseObject).ToString();
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data necessary to deserialize the field represented by this instance.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" /> ) for this serialization.</param>
		// Token: 0x06000007 RID: 7 RVA: 0x0000215B File Offset: 0x0000035B
		protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			((ISerializable)this).GetObjectData(info, context);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002168 File Offset: 0x00000368
		internal static ManagementBaseObject GetBaseObject(IWbemClassObjectFreeThreaded wbemObject, ManagementScope scope)
		{
			ManagementBaseObject result;
			if (ManagementBaseObject._IsClass(wbemObject))
			{
				result = ManagementClass.GetManagementClass(wbemObject, scope);
			}
			else
			{
				result = ManagementObject.GetManagementObject(wbemObject, scope);
			}
			return result;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002192 File Offset: 0x00000392
		internal ManagementBaseObject(IWbemClassObjectFreeThreaded wbemObject)
		{
			this.wbemObject = wbemObject;
			this.properties = null;
			this.systemProperties = null;
			this.qualifiers = null;
		}

		/// <summary>Returns a copy of the object.</summary>
		/// <returns>The new cloned object.</returns>
		// Token: 0x0600000A RID: 10 RVA: 0x000021B8 File Offset: 0x000003B8
		public virtual object Clone()
		{
			IWbemClassObjectFreeThreaded wbemObject = null;
			int num = this.wbemObject.Clone_(out wbemObject);
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
			return new ManagementBaseObject(wbemObject);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002208 File Offset: 0x00000408
		internal virtual void Initialize(bool getObject)
		{
		}

		/// <summary>Gets a collection of <see cref="T:System.Management.PropertyData" /> objects describing the properties of the management object.</summary>
		/// <returns>Returns a <see cref="T:System.Management.PropertyDataCollection" /> that holds the properties for the management object.</returns>
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000220A File Offset: 0x0000040A
		public virtual PropertyDataCollection Properties
		{
			get
			{
				this.Initialize(true);
				if (this.properties == null)
				{
					this.properties = new PropertyDataCollection(this, false);
				}
				return this.properties;
			}
		}

		/// <summary>Gets  the collection of WMI system properties of the management object (for example, the class name, server, and namespace). WMI system property names begin with "__".</summary>
		/// <returns>Returns a <see cref="T:System.Management.PropertyDataCollection" /> that contains the system properties for a management object.</returns>
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000222E File Offset: 0x0000042E
		public virtual PropertyDataCollection SystemProperties
		{
			get
			{
				this.Initialize(false);
				if (this.systemProperties == null)
				{
					this.systemProperties = new PropertyDataCollection(this, true);
				}
				return this.systemProperties;
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Management.QualifierData" /> objects defined on the management object. Each element in the collection holds information such as the qualifier name, value, and flavor.</summary>
		/// <returns>Returns a <see cref="T:System.Management.QualifierDataCollection" /> that holds the qualifiers for the management object.</returns>
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002252 File Offset: 0x00000452
		public virtual QualifierDataCollection Qualifiers
		{
			get
			{
				this.Initialize(true);
				if (this.qualifiers == null)
				{
					this.qualifiers = new QualifierDataCollection(this);
				}
				return this.qualifiers;
			}
		}

		/// <summary>Gets the path to the management object's class.</summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementPath" /> that contains the class path to the management object's class.</returns>
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002278 File Offset: 0x00000478
		public virtual ManagementPath ClassPath
		{
			get
			{
				object obj = null;
				object obj2 = null;
				object obj3 = null;
				int num = 0;
				int num2 = 0;
				int num3 = this.wbemObject.Get_("__SERVER", 0, ref obj, ref num, ref num2);
				if (num3 == 0)
				{
					num3 = this.wbemObject.Get_("__NAMESPACE", 0, ref obj2, ref num, ref num2);
					if (num3 == 0)
					{
						num3 = this.wbemObject.Get_("__CLASS", 0, ref obj3, ref num, ref num2);
					}
				}
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
				ManagementPath managementPath = new ManagementPath();
				managementPath.Server = string.Empty;
				managementPath.NamespacePath = string.Empty;
				managementPath.ClassName = string.Empty;
				try
				{
					managementPath.Server = (string)((obj is DBNull) ? "" : obj);
					managementPath.NamespacePath = (string)((obj2 is DBNull) ? "" : obj2);
					managementPath.ClassName = (string)((obj3 is DBNull) ? "" : obj3);
				}
				catch
				{
				}
				return managementPath;
			}
		}

		/// <summary>Gets access to property values through [] notation. This property is the indexer for the <see cref="T:System.Management.ManagementBaseObject" /> class. You can use the default indexed properties defined by a type, but you cannot explicitly define your own. However, specifying the expando attribute on a class automatically provides a default indexed property whose type is Object and whose index type is String.</summary>
		/// <param name="propertyName">The name of the property of interest. </param>
		/// <returns>Returns an <see cref="T:System.Object" /> value that contains the management object for a specific class property.</returns>
		// Token: 0x17000006 RID: 6
		public object this[string propertyName]
		{
			get
			{
				return this.GetPropertyValue(propertyName);
			}
			set
			{
				this.Initialize(true);
				try
				{
					this.SetPropertyValue(propertyName, value);
				}
				catch (COMException e)
				{
					ManagementException.ThrowWithExtendedInfo(e);
				}
			}
		}

		/// <summary>Gets an equivalent accessor to a property's value.</summary>
		/// <param name="propertyName">The name of the property of interest. </param>
		/// <returns>The value of the specified property.</returns>
		// Token: 0x06000012 RID: 18 RVA: 0x000023F4 File Offset: 0x000005F4
		public object GetPropertyValue(string propertyName)
		{
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			if (propertyName.StartsWith("__", StringComparison.Ordinal))
			{
				return this.SystemProperties[propertyName].Value;
			}
			return this.Properties[propertyName].Value;
		}

		/// <summary>Gets the value of the specified qualifier.          </summary>
		/// <param name="qualifierName">The name of the qualifier of interest. </param>
		/// <returns>The value of the specified qualifier.</returns>
		// Token: 0x06000013 RID: 19 RVA: 0x00002440 File Offset: 0x00000640
		public object GetQualifierValue(string qualifierName)
		{
			return this.Qualifiers[qualifierName].Value;
		}

		/// <summary>Sets the value of the named qualifier.</summary>
		/// <param name="qualifierName">The name of the qualifier to set. This parameter cannot be null.</param>
		/// <param name="qualifierValue">The value to set.</param>
		// Token: 0x06000014 RID: 20 RVA: 0x00002453 File Offset: 0x00000653
		public void SetQualifierValue(string qualifierName, object qualifierValue)
		{
			this.Qualifiers[qualifierName].Value = qualifierValue;
		}

		/// <summary>Returns the value of the specified property qualifier.</summary>
		/// <param name="propertyName">The name of the property to which the qualifier belongs. </param>
		/// <param name="qualifierName">The name of the property qualifier of interest. </param>
		/// <returns>The value of the specified qualifier.</returns>
		// Token: 0x06000015 RID: 21 RVA: 0x00002467 File Offset: 0x00000667
		public object GetPropertyQualifierValue(string propertyName, string qualifierName)
		{
			return this.Properties[propertyName].Qualifiers[qualifierName].Value;
		}

		/// <summary>Sets the value of the specified property qualifier.</summary>
		/// <param name="propertyName">The name of the property to which the qualifier belongs.</param>
		/// <param name="qualifierName">The name of the property qualifier of interest.</param>
		/// <param name="qualifierValue">The new value for the qualifier.</param>
		// Token: 0x06000016 RID: 22 RVA: 0x00002485 File Offset: 0x00000685
		public void SetPropertyQualifierValue(string propertyName, string qualifierName, object qualifierValue)
		{
			this.Properties[propertyName].Qualifiers[qualifierName].Value = qualifierValue;
		}

		/// <summary>Returns a textual representation of the object in the specified format.          </summary>
		/// <param name="format">The requested textual format. </param>
		/// <returns>The textual representation of the object in the specified format.</returns>
		// Token: 0x06000017 RID: 23 RVA: 0x000024A4 File Offset: 0x000006A4
		public string GetText(TextFormat format)
		{
			string result = null;
			if (format == TextFormat.Mof)
			{
				int num = this.wbemObject.GetObjectText_(0, out result);
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
			if (format - TextFormat.CimDtd20 > 1)
			{
				return null;
			}
			IWbemObjectTextSrc wbemObjectTextSrc = (IWbemObjectTextSrc)new WbemObjectTextSrc();
			IWbemContext wbemContext = (IWbemContext)new WbemContext();
			object obj = true;
			wbemContext.SetValue_("IncludeQualifiers", 0, ref obj);
			wbemContext.SetValue_("IncludeClassOrigin", 0, ref obj);
			if (wbemObjectTextSrc != null)
			{
				int num = wbemObjectTextSrc.GetText_(0, (IWbemClassObject_DoNotMarshal)Marshal.GetObjectForIUnknown(this.wbemObject), (uint)format, wbemContext, out result);
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
			}
			return result;
		}

		/// <summary>Compares two management objects.</summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		///     <see langword="true" /> if this is an instance of <see cref="T:System.Management.ManagementBaseObject" /> and represents the same object as this instance; otherwise, <see langword="false" />.             </returns>
		// Token: 0x06000018 RID: 24 RVA: 0x00002594 File Offset: 0x00000794
		public override bool Equals(object obj)
		{
			bool result = false;
			try
			{
				if (!(obj is ManagementBaseObject))
				{
					return false;
				}
				result = this.CompareTo((ManagementBaseObject)obj, ComparisonSettings.IncludeAll);
			}
			catch (ManagementException ex)
			{
				if (ex.ErrorCode == ManagementStatus.NotFound && this is ManagementObject && obj is ManagementObject)
				{
					int num = string.Compare(((ManagementObject)this).Path.Path, ((ManagementObject)obj).Path.Path, StringComparison.OrdinalIgnoreCase);
					return num == 0;
				}
				return false;
			}
			catch
			{
				return false;
			}
			return result;
		}

		/// <summary>Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.</summary>
		/// <returns>A hash code for the current object.</returns>
		// Token: 0x06000019 RID: 25 RVA: 0x00002638 File Offset: 0x00000838
		public override int GetHashCode()
		{
			int result = 0;
			try
			{
				result = this.GetText(TextFormat.Mof).GetHashCode();
			}
			catch (ManagementException)
			{
				result = string.Empty.GetHashCode();
			}
			catch (COMException)
			{
				result = string.Empty.GetHashCode();
			}
			return result;
		}

		/// <summary>Compares this object to another, based on specified options.</summary>
		/// <param name="otherObject">The object to which to compare this object. </param>
		/// <param name="settings">Options on how to compare the objects. </param>
		/// <returns>
		///     <see langword="true" /> if the objects compared are equal according to the given options; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600001A RID: 26 RVA: 0x00002690 File Offset: 0x00000890
		public bool CompareTo(ManagementBaseObject otherObject, ComparisonSettings settings)
		{
			if (otherObject == null)
			{
				throw new ArgumentNullException("otherObject");
			}
			bool result = false;
			if (this.wbemObject != null)
			{
				int num = this.wbemObject.CompareTo_((int)settings, otherObject.wbemObject);
				if (262147 == num)
				{
					result = false;
				}
				else if (num == 0)
				{
					result = true;
				}
				else if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return result;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000270C File Offset: 0x0000090C
		internal string ClassName
		{
			get
			{
				object obj = null;
				int num = 0;
				int num2 = 0;
				int num3 = this.wbemObject.Get_("__CLASS", 0, ref obj, ref num, ref num2);
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
				if (obj is DBNull)
				{
					return string.Empty;
				}
				return (string)obj;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000277C File Offset: 0x0000097C
		private static bool _IsClass(IWbemClassObjectFreeThreaded wbemObject)
		{
			object obj = null;
			int num = 0;
			int num2 = 0;
			int num3 = wbemObject.Get_("__GENUS", 0, ref obj, ref num, ref num2);
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
			return (int)obj == 1;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000027D8 File Offset: 0x000009D8
		internal bool IsClass
		{
			get
			{
				return ManagementBaseObject._IsClass(this.wbemObject);
			}
		}

		/// <summary>Sets the value of the named property.</summary>
		/// <param name="propertyName">The name of the property to be changed.</param>
		/// <param name="propertyValue">The new value for this property.</param>
		// Token: 0x0600001E RID: 30 RVA: 0x000027E8 File Offset: 0x000009E8
		public void SetPropertyValue(string propertyName, object propertyValue)
		{
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			if (propertyName.StartsWith("__", StringComparison.Ordinal))
			{
				this.SystemProperties[propertyName].Value = propertyValue;
				return;
			}
			this.Properties[propertyName].Value = propertyValue;
		}

		// Token: 0x04000072 RID: 114
		private static WbemContext lockOnFastProx = WMICapabilities.IsWindowsXPOrHigher() ? null : new WbemContext();

		// Token: 0x04000073 RID: 115
		internal IWbemClassObjectFreeThreaded _wbemObject;

		// Token: 0x04000074 RID: 116
		private PropertyDataCollection properties;

		// Token: 0x04000075 RID: 117
		private PropertyDataCollection systemProperties;

		// Token: 0x04000076 RID: 118
		private QualifierDataCollection qualifiers;
	}
}
