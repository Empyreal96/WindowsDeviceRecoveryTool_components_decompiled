using System;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Windows.Baml2006;
using System.Xaml;
using System.Xaml.Permissions;

namespace System.Windows
{
	/// <summary>Represents deferrable content that is held within BAML as a stream.</summary>
	// Token: 0x020000AC RID: 172
	[TypeConverter(typeof(DeferrableContentConverter))]
	public class DeferrableContent
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0000A5ED File Offset: 0x000087ED
		// (set) Token: 0x0600039D RID: 925 RVA: 0x0000A5F5 File Offset: 0x000087F5
		internal XamlLoadPermission LoadPermission { [SecurityCritical] get; [SecurityCritical] private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000A5FE File Offset: 0x000087FE
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0000A606 File Offset: 0x00008806
		internal Stream Stream { [SecurityCritical] [SecurityTreatAsSafe] get; [SecurityCritical] private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000A60F File Offset: 0x0000880F
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0000A617 File Offset: 0x00008817
		internal Baml2006SchemaContext SchemaContext { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000A620 File Offset: 0x00008820
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x0000A628 File Offset: 0x00008828
		internal IXamlObjectWriterFactory ObjectWriterFactory { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000A631 File Offset: 0x00008831
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0000A639 File Offset: 0x00008839
		internal XamlObjectWriterSettings ObjectWriterParentSettings { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0000A642 File Offset: 0x00008842
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x0000A64A File Offset: 0x0000884A
		internal object RootObject { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000A653 File Offset: 0x00008853
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x0000A65B File Offset: 0x0000885B
		internal IServiceProvider ServiceProvider { get; private set; }

		// Token: 0x060003AA RID: 938 RVA: 0x0000A664 File Offset: 0x00008864
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal DeferrableContent(Stream stream, Baml2006SchemaContext schemaContext, IXamlObjectWriterFactory objectWriterFactory, IServiceProvider serviceProvider, object rootObject)
		{
			this.ObjectWriterParentSettings = objectWriterFactory.GetParentSettings();
			if (this.ObjectWriterParentSettings.AccessLevel != null)
			{
				XamlLoadPermission xamlLoadPermission = new XamlLoadPermission(this.ObjectWriterParentSettings.AccessLevel);
				xamlLoadPermission.Demand();
				this.LoadPermission = xamlLoadPermission;
			}
			bool flag = false;
			if (schemaContext.LocalAssembly != null)
			{
				flag = schemaContext.LocalAssembly.ImageRuntimeVersion.StartsWith("v2", StringComparison.Ordinal);
			}
			if (flag)
			{
				this.ObjectWriterParentSettings.SkipProvideValueOnRoot = true;
			}
			this.Stream = stream;
			this.SchemaContext = schemaContext;
			this.ObjectWriterFactory = objectWriterFactory;
			this.ServiceProvider = serviceProvider;
			this.RootObject = rootObject;
		}
	}
}
