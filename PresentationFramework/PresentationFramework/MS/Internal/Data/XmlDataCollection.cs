using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Xml;

namespace MS.Internal.Data
{
	// Token: 0x0200074F RID: 1871
	internal class XmlDataCollection : ReadOnlyObservableCollection<XmlNode>
	{
		// Token: 0x06007746 RID: 30534 RVA: 0x002217F4 File Offset: 0x0021F9F4
		internal XmlDataCollection(XmlDataProvider xmlDataProvider) : base(new ObservableCollection<XmlNode>())
		{
			this._xds = xmlDataProvider;
		}

		// Token: 0x17001C56 RID: 7254
		// (get) Token: 0x06007747 RID: 30535 RVA: 0x00221808 File Offset: 0x0021FA08
		// (set) Token: 0x06007748 RID: 30536 RVA: 0x00221831 File Offset: 0x0021FA31
		internal XmlNamespaceManager XmlNamespaceManager
		{
			get
			{
				if (this._nsMgr == null && this._xds != null)
				{
					this._nsMgr = this._xds.XmlNamespaceManager;
				}
				return this._nsMgr;
			}
			set
			{
				this._nsMgr = value;
			}
		}

		// Token: 0x17001C57 RID: 7255
		// (get) Token: 0x06007749 RID: 30537 RVA: 0x0022183A File Offset: 0x0021FA3A
		internal XmlDataProvider ParentXmlDataProvider
		{
			get
			{
				return this._xds;
			}
		}

		// Token: 0x0600774A RID: 30538 RVA: 0x00221844 File Offset: 0x0021FA44
		internal bool CollectionHasChanged(XmlNodeList nodes)
		{
			int count = base.Count;
			if (count != nodes.Count)
			{
				return true;
			}
			for (int i = 0; i < count; i++)
			{
				if (base[i] != nodes[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600774B RID: 30539 RVA: 0x00221884 File Offset: 0x0021FA84
		internal void SynchronizeCollection(XmlNodeList nodes)
		{
			if (nodes == null)
			{
				base.Items.Clear();
				return;
			}
			int i = 0;
			while (i < base.Count)
			{
				if (i >= nodes.Count)
				{
					break;
				}
				if (base[i] != nodes[i])
				{
					int num = i + 1;
					while (num < nodes.Count && base[i] != nodes[num])
					{
						num++;
					}
					if (num < nodes.Count)
					{
						while (i < num)
						{
							base.Items.Insert(i, nodes[i]);
							i++;
						}
						i++;
					}
					else
					{
						base.Items.RemoveAt(i);
					}
				}
				else
				{
					i++;
				}
			}
			while (i < base.Count)
			{
				base.Items.RemoveAt(i);
			}
			while (i < nodes.Count)
			{
				base.Items.Insert(i, nodes[i]);
				i++;
			}
		}

		// Token: 0x040038BA RID: 14522
		private XmlDataProvider _xds;

		// Token: 0x040038BB RID: 14523
		private XmlNamespaceManager _nsMgr;
	}
}
