using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Management.Instrumentation
{
	/// <summary>Provides helper functions for exposing events and data for management. There is a single instance of this class per application domain.          Note: the WMI .NET libraries are now considered in final state, and no further development, enhancements, or updates will be available for non-security related issues affecting these libraries. The MI APIs should be used for all new development.</summary>
	// Token: 0x020000B7 RID: 183
	public class Instrumentation
	{
		// Token: 0x060004E6 RID: 1254
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern int GetCurrentProcessId();

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x000236FC File Offset: 0x000218FC
		internal static string ProcessIdentity
		{
			get
			{
				Type typeFromHandle = typeof(Instrumentation);
				lock (typeFromHandle)
				{
					if (Instrumentation.processIdentity == null)
					{
						Instrumentation.processIdentity = Guid.NewGuid().ToString().ToLower(CultureInfo.InvariantCulture);
					}
				}
				return Instrumentation.processIdentity;
			}
		}

		/// <summary>Registers the management instance or event classes in the specified assembly with WMI. This ensures that the instrumentation schema is accessible to <see cref="N:System.Management" /> client applications.          </summary>
		/// <param name="assemblyToRegister">The <see cref="T:System.Reflection.Assembly" /> containing instrumentation instance or event types.</param>
		// Token: 0x060004E8 RID: 1256 RVA: 0x00023768 File Offset: 0x00021968
		public static void RegisterAssembly(Assembly assemblyToRegister)
		{
			if (null == assemblyToRegister)
			{
				throw new ArgumentNullException("assemblyToRegister");
			}
			Instrumentation.GetInstrumentedAssembly(assemblyToRegister);
		}

		/// <summary>Determines if the instrumentation schema of the specified assembly has already been correctly registered with WMI.          </summary>
		/// <param name="assemblyToRegister">The <see cref="T:System.Reflection.Assembly" /> containing instrumentation instance or event types.</param>
		/// <returns>
		///     <see langword="true" /> if the instrumentation schema in the specified assembly is registered with WMI; otherwise, <see langword="false" />.</returns>
		// Token: 0x060004E9 RID: 1257 RVA: 0x00023788 File Offset: 0x00021988
		public static bool IsAssemblyRegistered(Assembly assemblyToRegister)
		{
			if (null == assemblyToRegister)
			{
				throw new ArgumentNullException("assemblyToRegister");
			}
			Hashtable obj = Instrumentation.instrumentedAssemblies;
			lock (obj)
			{
				if (Instrumentation.instrumentedAssemblies.ContainsKey(assemblyToRegister))
				{
					return true;
				}
			}
			SchemaNaming schemaNaming = SchemaNaming.GetSchemaNaming(assemblyToRegister);
			return schemaNaming != null && schemaNaming.IsAssemblyRegistered();
		}

		/// <summary>Raises a management event.          </summary>
		/// <param name="eventData">The object that determines the class, properties, and values of the event.</param>
		// Token: 0x060004EA RID: 1258 RVA: 0x000237FC File Offset: 0x000219FC
		public static void Fire(object eventData)
		{
			IEvent @event = eventData as IEvent;
			if (@event != null)
			{
				@event.Fire();
				return;
			}
			Instrumentation.GetFireFunction(eventData.GetType())(eventData);
		}

		/// <summary>Makes an instance visible through management instrumentation.          </summary>
		/// <param name="instanceData">The object that is to be visible through management instrumentation.</param>
		// Token: 0x060004EB RID: 1259 RVA: 0x0002382C File Offset: 0x00021A2C
		public static void Publish(object instanceData)
		{
			Type type = instanceData as Type;
			Assembly assembly = instanceData as Assembly;
			IInstance instance = instanceData as IInstance;
			if (type != null)
			{
				Instrumentation.GetInstrumentedAssembly(type.Assembly);
				return;
			}
			if (assembly != null)
			{
				Instrumentation.GetInstrumentedAssembly(assembly);
				return;
			}
			if (instance != null)
			{
				instance.Published = true;
				return;
			}
			Instrumentation.GetPublishFunction(instanceData.GetType())(instanceData);
		}

		/// <summary>Makes an instance that was previously published through the <see cref="M:System.Management.Instrumentation.Instrumentation.Publish(System.Object)" />             method no longer visible through management instrumentation.          </summary>
		/// <param name="instanceData">The object to remove from visibility for management instrumentation.</param>
		// Token: 0x060004EC RID: 1260 RVA: 0x00023894 File Offset: 0x00021A94
		public static void Revoke(object instanceData)
		{
			IInstance instance = instanceData as IInstance;
			if (instance != null)
			{
				instance.Published = false;
				return;
			}
			Instrumentation.GetRevokeFunction(instanceData.GetType())(instanceData);
		}

		/// <summary>Specifies the maximum number of objects of the specified type to be provided at a time.                       </summary>
		/// <param name="instrumentationClass">The class for which the batch size is being set.</param>
		/// <param name="batchSize">The maximum number of objects to be provided at a time.</param>
		// Token: 0x060004ED RID: 1261 RVA: 0x000238C4 File Offset: 0x00021AC4
		public static void SetBatchSize(Type instrumentationClass, int batchSize)
		{
			Instrumentation.GetInstrumentedAssembly(instrumentationClass.Assembly).SetBatchSize(instrumentationClass, batchSize);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000238D8 File Offset: 0x00021AD8
		internal static ProvisionFunction GetFireFunction(Type type)
		{
			return new ProvisionFunction(Instrumentation.GetInstrumentedAssembly(type.Assembly).Fire);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x000238F0 File Offset: 0x00021AF0
		internal static ProvisionFunction GetPublishFunction(Type type)
		{
			return new ProvisionFunction(Instrumentation.GetInstrumentedAssembly(type.Assembly).Publish);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00023908 File Offset: 0x00021B08
		internal static ProvisionFunction GetRevokeFunction(Type type)
		{
			return new ProvisionFunction(Instrumentation.GetInstrumentedAssembly(type.Assembly).Revoke);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00023920 File Offset: 0x00021B20
		private static void Initialize(Assembly assembly)
		{
			Hashtable obj = Instrumentation.instrumentedAssemblies;
			lock (obj)
			{
				if (!Instrumentation.instrumentedAssemblies.ContainsKey(assembly))
				{
					SchemaNaming schemaNaming = SchemaNaming.GetSchemaNaming(assembly);
					if (schemaNaming != null)
					{
						if (!schemaNaming.IsAssemblyRegistered())
						{
							if (!WMICapabilities.IsUserAdmin())
							{
								throw new Exception(RC.GetString("ASSEMBLY_NOT_REGISTERED"));
							}
							schemaNaming.DecoupledProviderInstanceName = AssemblyNameUtility.UniqueToAssemblyFullVersion(assembly);
							schemaNaming.RegisterNonAssemblySpecificSchema(null);
							schemaNaming.RegisterAssemblySpecificSchema();
						}
						InstrumentedAssembly value = new InstrumentedAssembly(assembly, schemaNaming);
						Instrumentation.instrumentedAssemblies.Add(assembly, value);
					}
				}
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000239C0 File Offset: 0x00021BC0
		private static InstrumentedAssembly GetInstrumentedAssembly(Assembly assembly)
		{
			Hashtable obj = Instrumentation.instrumentedAssemblies;
			InstrumentedAssembly result;
			lock (obj)
			{
				if (!Instrumentation.instrumentedAssemblies.ContainsKey(assembly))
				{
					Instrumentation.Initialize(assembly);
				}
				result = (InstrumentedAssembly)Instrumentation.instrumentedAssemblies[assembly];
			}
			return result;
		}

		// Token: 0x04000503 RID: 1283
		private static string processIdentity = null;

		// Token: 0x04000504 RID: 1284
		private static Hashtable instrumentedAssemblies = new Hashtable();
	}
}
