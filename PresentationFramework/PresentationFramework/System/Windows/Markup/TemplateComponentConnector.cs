using System;

namespace System.Windows.Markup
{
	// Token: 0x0200022F RID: 559
	internal class TemplateComponentConnector : IComponentConnector
	{
		// Token: 0x06002257 RID: 8791 RVA: 0x000AADA6 File Offset: 0x000A8FA6
		internal TemplateComponentConnector(IComponentConnector componentConnector, IStyleConnector styleConnector)
		{
			this._styleConnector = styleConnector;
			this._componentConnector = componentConnector;
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000AADBC File Offset: 0x000A8FBC
		public void InitializeComponent()
		{
			this._componentConnector.InitializeComponent();
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x000AADC9 File Offset: 0x000A8FC9
		public void Connect(int connectionId, object target)
		{
			if (this._styleConnector != null)
			{
				this._styleConnector.Connect(connectionId, target);
				return;
			}
			this._componentConnector.Connect(connectionId, target);
		}

		// Token: 0x040019EC RID: 6636
		private IStyleConnector _styleConnector;

		// Token: 0x040019ED RID: 6637
		private IComponentConnector _componentConnector;
	}
}
