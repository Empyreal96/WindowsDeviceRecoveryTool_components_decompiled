using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using MS.Win32;

namespace System.Windows
{
	/// <summary>Implements a markup extension that enables application authors to customize control styles based on the current system theme.</summary>
	// Token: 0x0200012B RID: 299
	[MarkupExtensionReturnType(typeof(Uri))]
	public class ThemeDictionaryExtension : MarkupExtension
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.ThemeDictionaryExtension" /> class.</summary>
		// Token: 0x06000C2C RID: 3116 RVA: 0x0000B03E File Offset: 0x0000923E
		public ThemeDictionaryExtension()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.ThemeDictionaryExtension" /> class, using the specified assembly name.</summary>
		/// <param name="assemblyName">The assembly name string.</param>
		// Token: 0x06000C2D RID: 3117 RVA: 0x0002D526 File Offset: 0x0002B726
		public ThemeDictionaryExtension(string assemblyName)
		{
			if (assemblyName != null)
			{
				this._assemblyName = assemblyName;
				return;
			}
			throw new ArgumentNullException("assemblyName");
		}

		/// <summary>Gets or sets a string setting a particular naming convention to identify which dictionary applies for a particular theme. </summary>
		/// <returns>The assembly name string.</returns>
		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0002D543 File Offset: 0x0002B743
		// (set) Token: 0x06000C2F RID: 3119 RVA: 0x0002D54B File Offset: 0x0002B74B
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
			set
			{
				this._assemblyName = value;
			}
		}

		/// <summary>Returns an object that should be set on the property where this extension is applied. For <see cref="T:System.Windows.ThemeDictionaryExtension" />, this is the URI value for a particular theme dictionary extension.</summary>
		/// <param name="serviceProvider">An object that can provide services for the markup extension. This service is expected to provide results for <see cref="T:System.Windows.Markup.IXamlTypeResolver" />.</param>
		/// <returns>The object value to set on the property where the extension is applied. </returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.ThemeDictionaryExtension.AssemblyName" /> property is <see langword="null" />. You must set this value during construction or before using the <see cref="M:System.Windows.ThemeDictionaryExtension.ProvideValue(System.IServiceProvider)" />  method.-or-
		///         <paramref name="serviceProvide" />r is <see langword="null" /> or does not provide a service for <see cref="T:System.Windows.Markup.IXamlTypeResolver" />.-or-
		///         <paramref name="serviceProvider" /> specifies a target type that does not match <see cref="P:System.Windows.ResourceDictionary.Source" />.</exception>
		// Token: 0x06000C30 RID: 3120 RVA: 0x0002D554 File Offset: 0x0002B754
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (string.IsNullOrEmpty(this.AssemblyName))
			{
				throw new InvalidOperationException(SR.Get("ThemeDictionaryExtension_Name"));
			}
			IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			if (provideValueTarget == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionNoContext", new object[]
				{
					base.GetType().Name,
					"IProvideValueTarget"
				}));
			}
			object targetObject = provideValueTarget.TargetObject;
			object targetProperty = provideValueTarget.TargetProperty;
			ResourceDictionary resourceDictionary = targetObject as ResourceDictionary;
			PropertyInfo left = targetProperty as PropertyInfo;
			if (resourceDictionary == null || (targetProperty != null && left != ThemeDictionaryExtension.SourceProperty))
			{
				throw new InvalidOperationException(SR.Get("ThemeDictionaryExtension_Source"));
			}
			ThemeDictionaryExtension.Register(resourceDictionary, this._assemblyName);
			resourceDictionary.IsSourcedFromThemeDictionary = true;
			return ThemeDictionaryExtension.GenerateUri(this._assemblyName, SystemResources.ResourceDictionaries.ThemedResourceName, UxThemeWrapper.ThemeName);
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x0002D62C File Offset: 0x0002B82C
		private static Uri GenerateUri(string assemblyName, string resourceName, string themeName)
		{
			StringBuilder stringBuilder = new StringBuilder(assemblyName.Length + 50);
			stringBuilder.Append("/");
			stringBuilder.Append(assemblyName);
			if (assemblyName.Equals("PresentationFramework", StringComparison.OrdinalIgnoreCase))
			{
				stringBuilder.Append('.');
				stringBuilder.Append(themeName);
			}
			stringBuilder.Append(";component/");
			stringBuilder.Append(resourceName);
			stringBuilder.Append(".xaml");
			return new Uri(stringBuilder.ToString(), UriKind.RelativeOrAbsolute);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x0002D6A8 File Offset: 0x0002B8A8
		internal static Uri GenerateFallbackUri(ResourceDictionary dictionary, string resourceName)
		{
			for (int i = 0; i < ThemeDictionaryExtension._themeDictionaryInfos.Count; i++)
			{
				ThemeDictionaryExtension.ThemeDictionaryInfo themeDictionaryInfo = ThemeDictionaryExtension._themeDictionaryInfos[i];
				if (!themeDictionaryInfo.DictionaryReference.IsAlive)
				{
					ThemeDictionaryExtension._themeDictionaryInfos.RemoveAt(i);
					i--;
				}
				else if ((ResourceDictionary)themeDictionaryInfo.DictionaryReference.Target == dictionary)
				{
					string themeName = resourceName.Split(new char[]
					{
						'/'
					})[1];
					return ThemeDictionaryExtension.GenerateUri(themeDictionaryInfo.AssemblyName, resourceName, themeName);
				}
			}
			return null;
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0002D72A File Offset: 0x0002B92A
		private static PropertyInfo SourceProperty
		{
			get
			{
				if (ThemeDictionaryExtension._sourceProperty == null)
				{
					ThemeDictionaryExtension._sourceProperty = typeof(ResourceDictionary).GetProperty("Source");
				}
				return ThemeDictionaryExtension._sourceProperty;
			}
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x0002D758 File Offset: 0x0002B958
		private static void Register(ResourceDictionary dictionary, string assemblyName)
		{
			if (ThemeDictionaryExtension._themeDictionaryInfos == null)
			{
				ThemeDictionaryExtension._themeDictionaryInfos = new List<ThemeDictionaryExtension.ThemeDictionaryInfo>();
			}
			ThemeDictionaryExtension.ThemeDictionaryInfo themeDictionaryInfo;
			for (int i = 0; i < ThemeDictionaryExtension._themeDictionaryInfos.Count; i++)
			{
				themeDictionaryInfo = ThemeDictionaryExtension._themeDictionaryInfos[i];
				if (!themeDictionaryInfo.DictionaryReference.IsAlive)
				{
					ThemeDictionaryExtension._themeDictionaryInfos.RemoveAt(i);
					i--;
				}
				else if (themeDictionaryInfo.DictionaryReference.Target == dictionary)
				{
					themeDictionaryInfo.AssemblyName = assemblyName;
					return;
				}
			}
			themeDictionaryInfo = new ThemeDictionaryExtension.ThemeDictionaryInfo();
			themeDictionaryInfo.DictionaryReference = new WeakReference(dictionary);
			themeDictionaryInfo.AssemblyName = assemblyName;
			ThemeDictionaryExtension._themeDictionaryInfos.Add(themeDictionaryInfo);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0002D7F0 File Offset: 0x0002B9F0
		internal static void OnThemeChanged()
		{
			if (ThemeDictionaryExtension._themeDictionaryInfos != null)
			{
				for (int i = 0; i < ThemeDictionaryExtension._themeDictionaryInfos.Count; i++)
				{
					ThemeDictionaryExtension.ThemeDictionaryInfo themeDictionaryInfo = ThemeDictionaryExtension._themeDictionaryInfos[i];
					if (!themeDictionaryInfo.DictionaryReference.IsAlive)
					{
						ThemeDictionaryExtension._themeDictionaryInfos.RemoveAt(i);
						i--;
					}
					else
					{
						ResourceDictionary resourceDictionary = (ResourceDictionary)themeDictionaryInfo.DictionaryReference.Target;
						resourceDictionary.Source = ThemeDictionaryExtension.GenerateUri(themeDictionaryInfo.AssemblyName, SystemResources.ResourceDictionaries.ThemedResourceName, UxThemeWrapper.ThemeName);
					}
				}
			}
		}

		// Token: 0x04000AF8 RID: 2808
		private string _assemblyName;

		// Token: 0x04000AF9 RID: 2809
		private static PropertyInfo _sourceProperty;

		// Token: 0x04000AFA RID: 2810
		[ThreadStatic]
		private static List<ThemeDictionaryExtension.ThemeDictionaryInfo> _themeDictionaryInfos;

		// Token: 0x02000831 RID: 2097
		private class ThemeDictionaryInfo
		{
			// Token: 0x04003CB5 RID: 15541
			public WeakReference DictionaryReference;

			// Token: 0x04003CB6 RID: 15542
			public string AssemblyName;
		}
	}
}
