using System;

namespace System.Windows.Markup
{
	// Token: 0x02000228 RID: 552
	internal class ReaderContextStackData
	{
		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x000AA302 File Offset: 0x000A8502
		internal ReaderFlags ContextType
		{
			get
			{
				return this._contextFlags & ReaderFlags.ContextTypeMask;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x0600221D RID: 8733 RVA: 0x000AA310 File Offset: 0x000A8510
		// (set) Token: 0x0600221E RID: 8734 RVA: 0x000AA318 File Offset: 0x000A8518
		internal object ObjectData
		{
			get
			{
				return this._contextData;
			}
			set
			{
				this._contextData = value;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x0600221F RID: 8735 RVA: 0x000AA321 File Offset: 0x000A8521
		// (set) Token: 0x06002220 RID: 8736 RVA: 0x000AA329 File Offset: 0x000A8529
		internal object Key
		{
			get
			{
				return this._contextKey;
			}
			set
			{
				this._contextKey = value;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06002221 RID: 8737 RVA: 0x000AA332 File Offset: 0x000A8532
		// (set) Token: 0x06002222 RID: 8738 RVA: 0x000AA33A File Offset: 0x000A853A
		internal string Uid
		{
			get
			{
				return this._uid;
			}
			set
			{
				this._uid = value;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06002223 RID: 8739 RVA: 0x000AA343 File Offset: 0x000A8543
		// (set) Token: 0x06002224 RID: 8740 RVA: 0x000AA34B File Offset: 0x000A854B
		internal string ElementNameOrPropertyName
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06002225 RID: 8741 RVA: 0x000AA354 File Offset: 0x000A8554
		// (set) Token: 0x06002226 RID: 8742 RVA: 0x000AA35C File Offset: 0x000A855C
		internal object ContentProperty
		{
			get
			{
				return this._contentProperty;
			}
			set
			{
				this._contentProperty = value;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002227 RID: 8743 RVA: 0x000AA365 File Offset: 0x000A8565
		// (set) Token: 0x06002228 RID: 8744 RVA: 0x000AA36D File Offset: 0x000A856D
		internal Type ExpectedType
		{
			get
			{
				return this._expectedType;
			}
			set
			{
				this._expectedType = value;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002229 RID: 8745 RVA: 0x000AA376 File Offset: 0x000A8576
		// (set) Token: 0x0600222A RID: 8746 RVA: 0x000AA37E File Offset: 0x000A857E
		internal short ExpectedTypeId
		{
			get
			{
				return this._expectedTypeId;
			}
			set
			{
				this._expectedTypeId = value;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x0600222B RID: 8747 RVA: 0x000AA387 File Offset: 0x000A8587
		// (set) Token: 0x0600222C RID: 8748 RVA: 0x000AA38F File Offset: 0x000A858F
		internal bool CreateUsingTypeConverter
		{
			get
			{
				return this._createUsingTypeConverter;
			}
			set
			{
				this._createUsingTypeConverter = value;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x0600222D RID: 8749 RVA: 0x000AA398 File Offset: 0x000A8598
		// (set) Token: 0x0600222E RID: 8750 RVA: 0x000AA3A0 File Offset: 0x000A85A0
		internal ReaderFlags ContextFlags
		{
			get
			{
				return this._contextFlags;
			}
			set
			{
				this._contextFlags = value;
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x0600222F RID: 8751 RVA: 0x000AA3A9 File Offset: 0x000A85A9
		internal bool NeedToAddToTree
		{
			get
			{
				return this.CheckFlag(ReaderFlags.NeedToAddToTree);
			}
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x000AA3B2 File Offset: 0x000A85B2
		internal void MarkAddedToTree()
		{
			this.ContextFlags = ((this.ContextFlags | ReaderFlags.AddedToTree) & (ReaderFlags)65534);
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x000AA3C8 File Offset: 0x000A85C8
		internal bool CheckFlag(ReaderFlags flag)
		{
			return (this.ContextFlags & flag) == flag;
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x000AA3D5 File Offset: 0x000A85D5
		internal void SetFlag(ReaderFlags flag)
		{
			this.ContextFlags |= flag;
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000AA3E5 File Offset: 0x000A85E5
		internal void ClearFlag(ReaderFlags flag)
		{
			this.ContextFlags &= ~flag;
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002234 RID: 8756 RVA: 0x000AA3F7 File Offset: 0x000A85F7
		internal bool IsObjectElement
		{
			get
			{
				return this.ContextType == ReaderFlags.DependencyObject || this.ContextType == ReaderFlags.ClrObject;
			}
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000AA418 File Offset: 0x000A8618
		internal void ClearData()
		{
			this._contextFlags = ReaderFlags.Unknown;
			this._contextData = null;
			this._contextKey = null;
			this._contentProperty = null;
			this._expectedType = null;
			this._expectedTypeId = 0;
			this._createUsingTypeConverter = false;
			this._uid = null;
			this._name = null;
		}

		// Token: 0x040019CF RID: 6607
		private ReaderFlags _contextFlags;

		// Token: 0x040019D0 RID: 6608
		private object _contextData;

		// Token: 0x040019D1 RID: 6609
		private object _contextKey;

		// Token: 0x040019D2 RID: 6610
		private string _uid;

		// Token: 0x040019D3 RID: 6611
		private string _name;

		// Token: 0x040019D4 RID: 6612
		private object _contentProperty;

		// Token: 0x040019D5 RID: 6613
		private Type _expectedType;

		// Token: 0x040019D6 RID: 6614
		private short _expectedTypeId;

		// Token: 0x040019D7 RID: 6615
		private bool _createUsingTypeConverter;
	}
}
