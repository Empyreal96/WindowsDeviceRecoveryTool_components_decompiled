using System;
using System.Configuration;

namespace System.Windows.Forms
{
	/// <summary>Defines a new <see cref="T:System.Configuration.ConfigurationSection" /> for parsing application settings. This class cannot be inherited. </summary>
	// Token: 0x02000435 RID: 1077
	public sealed class WindowsFormsSection : ConfigurationSection
	{
		// Token: 0x06004AFC RID: 19196 RVA: 0x00135D00 File Offset: 0x00133F00
		internal static WindowsFormsSection GetSection()
		{
			WindowsFormsSection result = null;
			try
			{
				result = (WindowsFormsSection)PrivilegedConfigurationManager.GetSection("system.windows.forms");
			}
			catch
			{
				result = new WindowsFormsSection();
			}
			return result;
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x00135D3C File Offset: 0x00133F3C
		private static ConfigurationPropertyCollection EnsureStaticPropertyBag()
		{
			if (WindowsFormsSection.s_properties == null)
			{
				WindowsFormsSection.s_propJitDebugging = new ConfigurationProperty("jitDebugging", typeof(bool), false, ConfigurationPropertyOptions.None);
				WindowsFormsSection.s_properties = new ConfigurationPropertyCollection
				{
					WindowsFormsSection.s_propJitDebugging
				};
			}
			return WindowsFormsSection.s_properties;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.WindowsFormsSection" /> class. </summary>
		// Token: 0x06004AFE RID: 19198 RVA: 0x00135D8C File Offset: 0x00133F8C
		public WindowsFormsSection()
		{
			WindowsFormsSection.EnsureStaticPropertyBag();
		}

		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x06004AFF RID: 19199 RVA: 0x00135D9A File Offset: 0x00133F9A
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return WindowsFormsSection.EnsureStaticPropertyBag();
			}
		}

		/// <summary>Gets or sets a value indicating whether just-in-time (JIT) debugging is used.</summary>
		/// <returns>
		///     <see langword="true" /> if JIT debugging is used; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x06004B00 RID: 19200 RVA: 0x00135DA1 File Offset: 0x00133FA1
		// (set) Token: 0x06004B01 RID: 19201 RVA: 0x00135DB3 File Offset: 0x00133FB3
		[ConfigurationProperty("jitDebugging", DefaultValue = false)]
		public bool JitDebugging
		{
			get
			{
				return (bool)base[WindowsFormsSection.s_propJitDebugging];
			}
			set
			{
				base[WindowsFormsSection.s_propJitDebugging] = value;
			}
		}

		// Token: 0x04002753 RID: 10067
		internal const bool JitDebuggingDefault = false;

		// Token: 0x04002754 RID: 10068
		private static ConfigurationPropertyCollection s_properties;

		// Token: 0x04002755 RID: 10069
		private static ConfigurationProperty s_propJitDebugging;
	}
}
