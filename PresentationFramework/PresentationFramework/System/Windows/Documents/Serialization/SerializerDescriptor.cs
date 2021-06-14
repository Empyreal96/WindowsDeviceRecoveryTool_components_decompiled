using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Windows.Controls;
using Microsoft.Win32;
using MS.Internal.PresentationFramework;

namespace System.Windows.Documents.Serialization
{
	/// <summary>Provides information about installed plug-in serializers. </summary>
	// Token: 0x0200043D RID: 1085
	public sealed class SerializerDescriptor
	{
		// Token: 0x06003F8C RID: 16268 RVA: 0x0000326D File Offset: 0x0000146D
		private SerializerDescriptor()
		{
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x001254E0 File Offset: 0x001236E0
		private static string GetNonEmptyRegistryString(RegistryKey key, string value)
		{
			string text = key.GetValue(value) as string;
			if (text == null)
			{
				throw new KeyNotFoundException();
			}
			return text;
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Documents.Serialization.SerializerDescriptor" /> through a given <see cref="T:System.Windows.Documents.Serialization.ISerializerFactory" /> implementation. </summary>
		/// <param name="factoryInstance">The source of data for the new <see cref="T:System.Windows.Documents.Serialization.SerializerDescriptor" />.</param>
		/// <returns>A new <see cref="T:System.Windows.Documents.Serialization.SerializerDescriptor" /> with its properties initialized with values from the given <see cref="T:System.Windows.Documents.Serialization.ISerializerFactory" /> implementation. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="factoryInstance" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">One or more of the following properties of the <paramref name="factoryInstance" /> is null: <see cref="P:System.Windows.Documents.Serialization.SerializerDescriptor.DisplayName" />, <see cref="P:System.Windows.Documents.Serialization.SerializerDescriptor.ManufacturerName" />, <see cref="P:System.Windows.Documents.Serialization.SerializerDescriptor.ManufacturerWebsite" />, and <see cref="P:System.Windows.Documents.Serialization.SerializerDescriptor.DefaultFileExtension" /></exception>
		// Token: 0x06003F8E RID: 16270 RVA: 0x00125504 File Offset: 0x00123704
		[SecuritySafeCritical]
		public static SerializerDescriptor CreateFromFactoryInstance(ISerializerFactory factoryInstance)
		{
			SecurityHelper.DemandPlugInSerializerPermissions();
			if (factoryInstance == null)
			{
				throw new ArgumentNullException("factoryInstance");
			}
			if (factoryInstance.DisplayName == null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderDisplayNameNull"));
			}
			if (factoryInstance.ManufacturerName == null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderManufacturerNameNull"));
			}
			if (factoryInstance.ManufacturerWebsite == null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderManufacturerWebsiteNull"));
			}
			if (factoryInstance.DefaultFileExtension == null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderDefaultFileExtensionNull"));
			}
			SerializerDescriptor serializerDescriptor = new SerializerDescriptor();
			serializerDescriptor._displayName = factoryInstance.DisplayName;
			serializerDescriptor._manufacturerName = factoryInstance.ManufacturerName;
			serializerDescriptor._manufacturerWebsite = factoryInstance.ManufacturerWebsite;
			serializerDescriptor._defaultFileExtension = factoryInstance.DefaultFileExtension;
			serializerDescriptor._isLoadable = true;
			Type type = factoryInstance.GetType();
			serializerDescriptor._assemblyName = type.Assembly.FullName;
			serializerDescriptor._assemblyPath = type.Assembly.Location;
			serializerDescriptor._assemblyVersion = type.Assembly.GetName().Version;
			serializerDescriptor._factoryInterfaceName = type.FullName;
			serializerDescriptor._winFXVersion = typeof(Button).Assembly.GetName().Version;
			return serializerDescriptor;
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x00125634 File Offset: 0x00123834
		[SecuritySafeCritical]
		internal ISerializerFactory CreateSerializerFactory()
		{
			SecurityHelper.DemandPlugInSerializerPermissions();
			string assemblyPath = this.AssemblyPath;
			Assembly assembly = Assembly.LoadFrom(assemblyPath);
			return assembly.CreateInstance(this.FactoryInterfaceName) as ISerializerFactory;
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x00125668 File Offset: 0x00123868
		internal void WriteToRegistryKey(RegistryKey key)
		{
			key.SetValue("uiLanguage", CultureInfo.CurrentUICulture.Name);
			key.SetValue("displayName", this.DisplayName);
			key.SetValue("manufacturerName", this.ManufacturerName);
			key.SetValue("manufacturerWebsite", this.ManufacturerWebsite);
			key.SetValue("defaultFileExtension", this.DefaultFileExtension);
			key.SetValue("assemblyName", this.AssemblyName);
			key.SetValue("assemblyPath", this.AssemblyPath);
			key.SetValue("factoryInterfaceName", this.FactoryInterfaceName);
			key.SetValue("assemblyVersion", this.AssemblyVersion.ToString());
			key.SetValue("winFXVersion", this.WinFXVersion.ToString());
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x00125730 File Offset: 0x00123930
		[SecuritySafeCritical]
		internal static SerializerDescriptor CreateFromRegistry(RegistryKey plugIns, string keyName)
		{
			SecurityHelper.DemandPlugInSerializerPermissions();
			SerializerDescriptor serializerDescriptor = new SerializerDescriptor();
			try
			{
				RegistryKey registryKey = plugIns.OpenSubKey(keyName);
				serializerDescriptor._displayName = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "displayName");
				serializerDescriptor._manufacturerName = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "manufacturerName");
				serializerDescriptor._manufacturerWebsite = new Uri(SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "manufacturerWebsite"));
				serializerDescriptor._defaultFileExtension = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "defaultFileExtension");
				serializerDescriptor._assemblyName = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "assemblyName");
				serializerDescriptor._assemblyPath = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "assemblyPath");
				serializerDescriptor._factoryInterfaceName = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "factoryInterfaceName");
				serializerDescriptor._assemblyVersion = new Version(SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "assemblyVersion"));
				serializerDescriptor._winFXVersion = new Version(SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "winFXVersion"));
				string nonEmptyRegistryString = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "uiLanguage");
				registryKey.Close();
				if (!nonEmptyRegistryString.Equals(CultureInfo.CurrentUICulture.Name))
				{
					ISerializerFactory serializerFactory = serializerDescriptor.CreateSerializerFactory();
					serializerDescriptor._displayName = serializerFactory.DisplayName;
					serializerDescriptor._manufacturerName = serializerFactory.ManufacturerName;
					serializerDescriptor._manufacturerWebsite = serializerFactory.ManufacturerWebsite;
					serializerDescriptor._defaultFileExtension = serializerFactory.DefaultFileExtension;
					registryKey = plugIns.CreateSubKey(keyName);
					serializerDescriptor.WriteToRegistryKey(registryKey);
					registryKey.Close();
				}
			}
			catch (KeyNotFoundException)
			{
				serializerDescriptor = null;
			}
			if (serializerDescriptor != null)
			{
				Assembly assembly = Assembly.ReflectionOnlyLoadFrom(serializerDescriptor._assemblyPath);
				if (typeof(Button).Assembly.GetName().Version == serializerDescriptor._winFXVersion && assembly != null && assembly.GetName().Version == serializerDescriptor._assemblyVersion)
				{
					serializerDescriptor._isLoadable = true;
				}
			}
			return serializerDescriptor;
		}

		/// <summary>Gets the public display name of the serializer. </summary>
		/// <returns>The public display name of the serializer. </returns>
		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x06003F92 RID: 16274 RVA: 0x001258EC File Offset: 0x00123AEC
		public string DisplayName
		{
			get
			{
				return this._displayName;
			}
		}

		/// <summary>Gets the name of the company that developed the serializer. </summary>
		/// <returns>The name of the company that developed the plug-in serializer. </returns>
		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x06003F93 RID: 16275 RVA: 0x001258F4 File Offset: 0x00123AF4
		public string ManufacturerName
		{
			get
			{
				return this._manufacturerName;
			}
		}

		/// <summary>Gets the web address of the company that developed the serializer. </summary>
		/// <returns>The web address of the company that developed the serializer. </returns>
		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06003F94 RID: 16276 RVA: 0x001258FC File Offset: 0x00123AFC
		public Uri ManufacturerWebsite
		{
			get
			{
				return this._manufacturerWebsite;
			}
		}

		/// <summary>Gets the default extension associated with files that the serializer outputs. </summary>
		/// <returns>The default extension associated with files that the serializer outputs. </returns>
		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x06003F95 RID: 16277 RVA: 0x00125904 File Offset: 0x00123B04
		public string DefaultFileExtension
		{
			get
			{
				return this._defaultFileExtension;
			}
		}

		/// <summary>Gets the name of the assembly that contains the serializer. </summary>
		/// <returns>The name of the assembly (usually a DLL) that contains the plug-in serializer. </returns>
		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06003F96 RID: 16278 RVA: 0x0012590C File Offset: 0x00123B0C
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		/// <summary>Gets the path to the assembly file that contains the serializer. </summary>
		/// <returns>The path to the assembly file that contains the plug-in serializer. </returns>
		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06003F97 RID: 16279 RVA: 0x00125914 File Offset: 0x00123B14
		public string AssemblyPath
		{
			get
			{
				return this._assemblyPath;
			}
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Documents.Serialization.ISerializerFactory" /> derived class that implements the serializer. </summary>
		/// <returns>The name of the <see cref="T:System.Windows.Documents.Serialization.ISerializerFactory" /> derived class that implements the serializer. </returns>
		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06003F98 RID: 16280 RVA: 0x0012591C File Offset: 0x00123B1C
		public string FactoryInterfaceName
		{
			get
			{
				return this._factoryInterfaceName;
			}
		}

		/// <summary>Gets the version of the assembly that contains the serializer. </summary>
		/// <returns>The version of the assembly that contains the plug-in serializer. </returns>
		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x06003F99 RID: 16281 RVA: 0x00125924 File Offset: 0x00123B24
		public Version AssemblyVersion
		{
			get
			{
				return this._assemblyVersion;
			}
		}

		/// <summary>Gets the version of Microsoft .NET Framework required by the serializer.</summary>
		/// <returns>The version of Microsoft .NET Framework required by the plug-in serializer. </returns>
		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x06003F9A RID: 16282 RVA: 0x0012592C File Offset: 0x00123B2C
		public Version WinFXVersion
		{
			get
			{
				return this._winFXVersion;
			}
		}

		/// <summary>Gets a value indicating whether the serializer can be loaded with the currently installed version of Microsoft .NET Framework.</summary>
		/// <returns>
		///     <see langword="true" /> if the serializer assembly can be loaded; otherwise, <see langword="false" />.  The default is <see langword="false" />.</returns>
		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x06003F9B RID: 16283 RVA: 0x00125934 File Offset: 0x00123B34
		public bool IsLoadable
		{
			get
			{
				return this._isLoadable;
			}
		}

		/// <summary>Tests two <see cref="T:System.Windows.Documents.Serialization.SerializerDescriptor" /> objects for equality.</summary>
		/// <param name="obj">The object to be compared with this <see cref="T:System.Windows.Documents.Serialization.SerializerDescriptor" />.</param>
		/// <returns>
		///     <see langword="true" /> if both are equal; otherwise, <see langword="false" />. </returns>
		// Token: 0x06003F9C RID: 16284 RVA: 0x0012593C File Offset: 0x00123B3C
		public override bool Equals(object obj)
		{
			SerializerDescriptor serializerDescriptor = obj as SerializerDescriptor;
			return serializerDescriptor != null && (serializerDescriptor._displayName == this._displayName && serializerDescriptor._assemblyName == this._assemblyName && serializerDescriptor._assemblyPath == this._assemblyPath && serializerDescriptor._factoryInterfaceName == this._factoryInterfaceName && serializerDescriptor._defaultFileExtension == this._defaultFileExtension && serializerDescriptor._assemblyVersion == this._assemblyVersion) && serializerDescriptor._winFXVersion == this._winFXVersion;
		}

		/// <summary>Gets the unique hash code value of the serializer. </summary>
		/// <returns>The unique hash code value of the serializer. </returns>
		// Token: 0x06003F9D RID: 16285 RVA: 0x001259E0 File Offset: 0x00123BE0
		public override int GetHashCode()
		{
			string text = string.Concat(new object[]
			{
				this._displayName,
				"/",
				this._assemblyName,
				"/",
				this._assemblyPath,
				"/",
				this._factoryInterfaceName,
				"/",
				this._assemblyVersion,
				"/",
				this._winFXVersion
			});
			return text.GetHashCode();
		}

		// Token: 0x04002743 RID: 10051
		private string _displayName;

		// Token: 0x04002744 RID: 10052
		private string _manufacturerName;

		// Token: 0x04002745 RID: 10053
		private Uri _manufacturerWebsite;

		// Token: 0x04002746 RID: 10054
		private string _defaultFileExtension;

		// Token: 0x04002747 RID: 10055
		private string _assemblyName;

		// Token: 0x04002748 RID: 10056
		private string _assemblyPath;

		// Token: 0x04002749 RID: 10057
		private string _factoryInterfaceName;

		// Token: 0x0400274A RID: 10058
		private Version _assemblyVersion;

		// Token: 0x0400274B RID: 10059
		private Version _winFXVersion;

		// Token: 0x0400274C RID: 10060
		private bool _isLoadable;
	}
}
