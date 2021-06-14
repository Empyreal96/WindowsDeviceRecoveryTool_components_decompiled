using System;
using System.Reflection;

namespace System.Management.Instrumentation
{
	/// <summary>Allows an instrumented class, or member of an instrumented class, to present an alternate name through management instrumentation.          Note: the WMI .NET libraries are now considered in final state, and no further development, enhancements, or updates will be available for non-security related issues affecting these libraries. The MI APIs should be used for all new development.</summary>
	// Token: 0x020000AF RID: 175
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public class ManagedNameAttribute : Attribute
	{
		/// <summary>Gets the name of the managed entity.          </summary>
		/// <returns>Returns a T:System.String value containing the name of the managed entity.</returns>
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x00022482 File Offset: 0x00020682
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.Instrumentation.ManagedNameAttribute" /> class that allows the alternate name to be specified for the type, field, property, method, or parameter to which this attribute is applied.          </summary>
		/// <param name="name">The alternate name for the type, field, property, method, or parameter to which this attribute is applied.</param>
		// Token: 0x0600049C RID: 1180 RVA: 0x0002248A File Offset: 0x0002068A
		public ManagedNameAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0002249C File Offset: 0x0002069C
		internal static string GetMemberName(MemberInfo member)
		{
			object[] customAttributes = member.GetCustomAttributes(typeof(ManagedNameAttribute), false);
			if (customAttributes.Length != 0)
			{
				ManagedNameAttribute managedNameAttribute = (ManagedNameAttribute)customAttributes[0];
				if (managedNameAttribute.name != null && managedNameAttribute.name.Length != 0)
				{
					return managedNameAttribute.name;
				}
			}
			return member.Name;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000224EC File Offset: 0x000206EC
		internal static string GetBaseClassName(Type type)
		{
			InstrumentationClassAttribute attribute = InstrumentationClassAttribute.GetAttribute(type);
			string managedBaseClassName = attribute.ManagedBaseClassName;
			if (managedBaseClassName != null)
			{
				return managedBaseClassName;
			}
			if (InstrumentationClassAttribute.GetAttribute(type.BaseType) == null)
			{
				switch (attribute.InstrumentationType)
				{
				case InstrumentationType.Instance:
					return null;
				case InstrumentationType.Event:
					return "__ExtrinsicEvent";
				case InstrumentationType.Abstract:
					return null;
				}
			}
			return ManagedNameAttribute.GetMemberName(type.BaseType);
		}

		// Token: 0x040004E9 RID: 1257
		private string name;
	}
}
