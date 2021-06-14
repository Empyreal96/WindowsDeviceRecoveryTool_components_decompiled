using System;

namespace System.Management.Instrumentation
{
	/// <summary>Specifies that a class provides event or instance instrumentation.                       Note: the WMI .NET libraries are now considered in final state, and no further development, enhancements, or updates will be available for non-security related issues affecting these libraries. The MI APIs should be used for all new development.</summary>
	// Token: 0x020000AE RID: 174
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class InstrumentationClassAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Management.Instrumentation.InstrumentationClassAttribute" /> class that is used if this type is derived from another type that has the <see cref="T:System.Management.Instrumentation.InstrumentationClassAttribute" /> attribute, or if this is a top-level instrumentation class (for example, an instance or abstract class                without a base class, or an event derived from __ExtrinsicEvent).          </summary>
		/// <param name="instrumentationType">The type of instrumentation provided by this class.</param>
		// Token: 0x06000495 RID: 1173 RVA: 0x000223CC File Offset: 0x000205CC
		public InstrumentationClassAttribute(InstrumentationType instrumentationType)
		{
			this.instrumentationType = instrumentationType;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.Instrumentation.InstrumentationClassAttribute" /> class that has schema for an existing base class. The class must contain proper member definitions for the properties of the existing WMI base class.          </summary>
		/// <param name="instrumentationType">The type of instrumentation provided by this class.</param>
		/// <param name="managedBaseClassName">The name of the base class.</param>
		// Token: 0x06000496 RID: 1174 RVA: 0x000223DB File Offset: 0x000205DB
		public InstrumentationClassAttribute(InstrumentationType instrumentationType, string managedBaseClassName)
		{
			this.instrumentationType = instrumentationType;
			this.managedBaseClassName = managedBaseClassName;
		}

		/// <summary>Gets or sets the type of instrumentation provided by this class.          </summary>
		/// <returns>Returns an <see cref="T:System.Management.Instrumentation.InstrumentationType" /> enumeration value containing the type of instrumentation provided by this class.</returns>
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x000223F1 File Offset: 0x000205F1
		public InstrumentationType InstrumentationType
		{
			get
			{
				return this.instrumentationType;
			}
		}

		/// <summary>Gets or sets the name of the base class of this instrumentation class.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the name of the base class of this instrumentation class.</returns>
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x000223F9 File Offset: 0x000205F9
		public string ManagedBaseClassName
		{
			get
			{
				if (this.managedBaseClassName == null || this.managedBaseClassName.Length == 0)
				{
					return null;
				}
				return this.managedBaseClassName;
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00022418 File Offset: 0x00020618
		internal static InstrumentationClassAttribute GetAttribute(Type type)
		{
			if (type == typeof(BaseEvent) || type == typeof(Instance))
			{
				return null;
			}
			object[] customAttributes = type.GetCustomAttributes(typeof(InstrumentationClassAttribute), true);
			if (customAttributes.Length != 0)
			{
				return (InstrumentationClassAttribute)customAttributes[0];
			}
			return null;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0002246B File Offset: 0x0002066B
		internal static Type GetBaseInstrumentationType(Type type)
		{
			if (InstrumentationClassAttribute.GetAttribute(type.BaseType) != null)
			{
				return type.BaseType;
			}
			return null;
		}

		// Token: 0x040004E7 RID: 1255
		private InstrumentationType instrumentationType;

		// Token: 0x040004E8 RID: 1256
		private string managedBaseClassName;
	}
}
