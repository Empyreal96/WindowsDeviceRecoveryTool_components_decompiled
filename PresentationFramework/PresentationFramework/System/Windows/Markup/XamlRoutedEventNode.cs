using System;

namespace System.Windows.Markup
{
	// Token: 0x02000251 RID: 593
	internal class XamlRoutedEventNode : XamlAttributeNode
	{
		// Token: 0x060022ED RID: 8941 RVA: 0x000AC5DE File Offset: 0x000AA7DE
		internal XamlRoutedEventNode(int lineNumber, int linePosition, int depth, RoutedEvent routedEvent, string assemblyName, string typeFullName, string routedEventName, string value) : base(XamlNodeType.RoutedEvent, lineNumber, linePosition, depth, value)
		{
			this._routedEvent = routedEvent;
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._routedEventName = routedEventName;
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x060022EE RID: 8942 RVA: 0x000AC60D File Offset: 0x000AA80D
		internal RoutedEvent Event
		{
			get
			{
				return this._routedEvent;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x060022EF RID: 8943 RVA: 0x000AC615 File Offset: 0x000AA815
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x060022F0 RID: 8944 RVA: 0x000AC61D File Offset: 0x000AA81D
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x060022F1 RID: 8945 RVA: 0x000AC625 File Offset: 0x000AA825
		internal string EventName
		{
			get
			{
				return this._routedEventName;
			}
		}

		// Token: 0x04001A55 RID: 6741
		private RoutedEvent _routedEvent;

		// Token: 0x04001A56 RID: 6742
		private string _assemblyName;

		// Token: 0x04001A57 RID: 6743
		private string _typeFullName;

		// Token: 0x04001A58 RID: 6744
		private string _routedEventName;
	}
}
