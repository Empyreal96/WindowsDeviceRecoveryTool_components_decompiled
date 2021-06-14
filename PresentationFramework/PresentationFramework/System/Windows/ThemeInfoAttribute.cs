using System;
using System.Reflection;

namespace System.Windows
{
	/// <summary>Specifies the location in which theme dictionaries are stored for an assembly. </summary>
	// Token: 0x0200012C RID: 300
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class ThemeInfoAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.ThemeInfoAttribute" /> class and creates an attribute that defines theme dictionary locations for types in an assembly.</summary>
		/// <param name="themeDictionaryLocation">The location of theme-specific resources.</param>
		/// <param name="genericDictionaryLocation">The location of generic, not theme-specific, resources.</param>
		// Token: 0x06000C36 RID: 3126 RVA: 0x0002D86F File Offset: 0x0002BA6F
		public ThemeInfoAttribute(ResourceDictionaryLocation themeDictionaryLocation, ResourceDictionaryLocation genericDictionaryLocation)
		{
			this._themeDictionaryLocation = themeDictionaryLocation;
			this._genericDictionaryLocation = genericDictionaryLocation;
		}

		/// <summary>The location of theme specific resources. </summary>
		/// <returns>The <see cref="T:System.Windows.ResourceDictionaryLocation" /> of the theme specific <see cref="T:System.Windows.ResourceDictionary" />.</returns>
		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0002D885 File Offset: 0x0002BA85
		public ResourceDictionaryLocation ThemeDictionaryLocation
		{
			get
			{
				return this._themeDictionaryLocation;
			}
		}

		/// <summary>The location of generic, not theme specific, resources. </summary>
		/// <returns>The <see cref="T:System.Windows.ResourceDictionaryLocation" /> of the generic <see cref="T:System.Windows.ResourceDictionary" />.</returns>
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x0002D88D File Offset: 0x0002BA8D
		public ResourceDictionaryLocation GenericDictionaryLocation
		{
			get
			{
				return this._genericDictionaryLocation;
			}
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0002D895 File Offset: 0x0002BA95
		internal static ThemeInfoAttribute FromAssembly(Assembly assembly)
		{
			return Attribute.GetCustomAttribute(assembly, typeof(ThemeInfoAttribute)) as ThemeInfoAttribute;
		}

		// Token: 0x04000AFB RID: 2811
		private ResourceDictionaryLocation _themeDictionaryLocation;

		// Token: 0x04000AFC RID: 2812
		private ResourceDictionaryLocation _genericDictionaryLocation;
	}
}
