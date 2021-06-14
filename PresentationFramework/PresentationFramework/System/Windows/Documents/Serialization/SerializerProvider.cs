using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security;
using System.Windows.Xps.Serialization;
using Microsoft.Win32;
using MS.Internal.PresentationFramework;

namespace System.Windows.Documents.Serialization
{
	/// <summary>Manages serialization plug-ins created, using <see cref="T:System.Windows.Documents.Serialization.ISerializerFactory" /> and <see cref="T:System.Windows.Documents.Serialization.SerializerDescriptor" />, by manufacturers who have their own proprietary serialization formats.</summary>
	// Token: 0x0200043C RID: 1084
	public sealed class SerializerProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Serialization.SerializerProvider" /> class. </summary>
		// Token: 0x06003F85 RID: 16261 RVA: 0x0012514C File Offset: 0x0012334C
		[SecuritySafeCritical]
		public SerializerProvider()
		{
			SecurityHelper.DemandPlugInSerializerPermissions();
			List<SerializerDescriptor> list = new List<SerializerDescriptor>();
			SerializerDescriptor serializerDescriptor = this.CreateSystemSerializerDescriptor();
			if (serializerDescriptor != null)
			{
				list.Add(serializerDescriptor);
			}
			RegistryKey registryKey = SerializerProvider._rootKey.CreateSubKey("SOFTWARE\\Microsoft\\WinFX Serializers");
			if (registryKey != null)
			{
				foreach (string keyName in registryKey.GetSubKeyNames())
				{
					serializerDescriptor = SerializerDescriptor.CreateFromRegistry(registryKey, keyName);
					if (serializerDescriptor != null)
					{
						list.Add(serializerDescriptor);
					}
				}
				registryKey.Close();
			}
			this._installedSerializers = list.AsReadOnly();
		}

		/// <summary>Registers a serializer plug-in. </summary>
		/// <param name="serializerDescriptor">The <see cref="T:System.Windows.Documents.Serialization.SerializerDescriptor" /> for the plug-in.</param>
		/// <param name="overwrite">
		///       <see langword="true" /> to overwrite an existing registration for the same plug-in; otherwise, <see langword="false" />. See Remarks.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="serializerDescriptor" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="overwrite" /> is <see langword="false" /> and the plug-in is already registered.</exception>
		// Token: 0x06003F86 RID: 16262 RVA: 0x001251D4 File Offset: 0x001233D4
		[SecuritySafeCritical]
		public static void RegisterSerializer(SerializerDescriptor serializerDescriptor, bool overwrite)
		{
			SecurityHelper.DemandPlugInSerializerPermissions();
			if (serializerDescriptor == null)
			{
				throw new ArgumentNullException("serializerDescriptor");
			}
			RegistryKey registryKey = SerializerProvider._rootKey.CreateSubKey("SOFTWARE\\Microsoft\\WinFX Serializers");
			string text = string.Concat(new object[]
			{
				serializerDescriptor.DisplayName,
				"/",
				serializerDescriptor.AssemblyName,
				"/",
				serializerDescriptor.AssemblyVersion,
				"/",
				serializerDescriptor.WinFXVersion
			});
			if (!overwrite && registryKey.OpenSubKey(text) != null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderAlreadyRegistered"), text);
			}
			RegistryKey registryKey2 = registryKey.CreateSubKey(text);
			serializerDescriptor.WriteToRegistryKey(registryKey2);
			registryKey2.Close();
		}

		/// <summary>Deletes a serializer plug-in from the registry.</summary>
		/// <param name="serializerDescriptor">The <see cref="T:System.Windows.Documents.Serialization.SerializerDescriptor" /> for the plug-in.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="serializerDescriptor" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The plug-in is not registered. See Remarks.</exception>
		// Token: 0x06003F87 RID: 16263 RVA: 0x00125280 File Offset: 0x00123480
		[SecuritySafeCritical]
		public static void UnregisterSerializer(SerializerDescriptor serializerDescriptor)
		{
			SecurityHelper.DemandPlugInSerializerPermissions();
			if (serializerDescriptor == null)
			{
				throw new ArgumentNullException("serializerDescriptor");
			}
			RegistryKey registryKey = SerializerProvider._rootKey.CreateSubKey("SOFTWARE\\Microsoft\\WinFX Serializers");
			string text = string.Concat(new object[]
			{
				serializerDescriptor.DisplayName,
				"/",
				serializerDescriptor.AssemblyName,
				"/",
				serializerDescriptor.AssemblyVersion,
				"/",
				serializerDescriptor.WinFXVersion
			});
			if (registryKey.OpenSubKey(text) == null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderNotRegistered"), text);
			}
			registryKey.DeleteSubKeyTree(text);
		}

		/// <summary>Initializes an object derived from the abstract <see cref="T:System.Windows.Documents.Serialization.SerializerWriter" /> class for the specified <see cref="T:System.IO.Stream" /> that will use the specified descriptor.</summary>
		/// <param name="serializerDescriptor">A <see cref="T:System.Windows.Documents.Serialization.SerializerDescriptor" /> that contains serialization information for the <see cref="T:System.Windows.Documents.Serialization.SerializerWriter" />.</param>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> to which the returned object writes.</param>
		/// <returns>An object of a class derived from <see cref="T:System.Windows.Documents.Serialization.SerializerWriter" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">One of the parameters is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="serializerDescriptor" /> is specifying the wrong version.-or-It is not registered.-or-The assembly file cannot be found.-or-The assembly cannot be loaded.</exception>
		// Token: 0x06003F88 RID: 16264 RVA: 0x0012531C File Offset: 0x0012351C
		[SecuritySafeCritical]
		public SerializerWriter CreateSerializerWriter(SerializerDescriptor serializerDescriptor, Stream stream)
		{
			SecurityHelper.DemandPlugInSerializerPermissions();
			SerializerWriter result = null;
			if (serializerDescriptor == null)
			{
				throw new ArgumentNullException("serializerDescriptor");
			}
			string paramName = string.Concat(new object[]
			{
				serializerDescriptor.DisplayName,
				"/",
				serializerDescriptor.AssemblyName,
				"/",
				serializerDescriptor.AssemblyVersion,
				"/",
				serializerDescriptor.WinFXVersion
			});
			if (!serializerDescriptor.IsLoadable)
			{
				throw new ArgumentException(SR.Get("SerializerProviderWrongVersion"), paramName);
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			bool flag = false;
			foreach (SerializerDescriptor serializerDescriptor2 in this.InstalledSerializers)
			{
				if (serializerDescriptor2.Equals(serializerDescriptor))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				throw new ArgumentException(SR.Get("SerializerProviderUnknownSerializer"), paramName);
			}
			try
			{
				ISerializerFactory serializerFactory = serializerDescriptor.CreateSerializerFactory();
				result = serializerFactory.CreateSerializerWriter(stream);
			}
			catch (FileNotFoundException)
			{
				throw new ArgumentException(SR.Get("SerializerProviderCannotLoad"), serializerDescriptor.DisplayName);
			}
			catch (FileLoadException)
			{
				throw new ArgumentException(SR.Get("SerializerProviderCannotLoad"), serializerDescriptor.DisplayName);
			}
			catch (BadImageFormatException)
			{
				throw new ArgumentException(SR.Get("SerializerProviderCannotLoad"), serializerDescriptor.DisplayName);
			}
			catch (MissingMethodException)
			{
				throw new ArgumentException(SR.Get("SerializerProviderCannotLoad"), serializerDescriptor.DisplayName);
			}
			return result;
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x001254AC File Offset: 0x001236AC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private SerializerDescriptor CreateSystemSerializerDescriptor()
		{
			SecurityHelper.DemandPlugInSerializerPermissions();
			return SerializerDescriptor.CreateFromFactoryInstance(new XpsSerializerFactory());
		}

		/// <summary>Gets a collection of the installed plug-in serializers.</summary>
		/// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of the <see cref="T:System.Windows.Documents.Serialization.SerializerDescriptor" /> objects already registered. </returns>
		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x06003F8A RID: 16266 RVA: 0x001254CC File Offset: 0x001236CC
		public ReadOnlyCollection<SerializerDescriptor> InstalledSerializers
		{
			get
			{
				return this._installedSerializers;
			}
		}

		// Token: 0x04002740 RID: 10048
		private const string _registryPath = "SOFTWARE\\Microsoft\\WinFX Serializers";

		// Token: 0x04002741 RID: 10049
		private static readonly RegistryKey _rootKey = Registry.LocalMachine;

		// Token: 0x04002742 RID: 10050
		private ReadOnlyCollection<SerializerDescriptor> _installedSerializers;
	}
}
