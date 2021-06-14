using System;
using System.IO.Packaging;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Utility;

namespace System.Windows.Navigation
{
	/// <summary>Represents an entry in either back or forward navigation history.</summary>
	// Token: 0x02000304 RID: 772
	[Serializable]
	public class JournalEntry : DependencyObject, ISerializable
	{
		// Token: 0x060028F4 RID: 10484 RVA: 0x000BDB38 File Offset: 0x000BBD38
		internal JournalEntry(JournalEntryGroupState jeGroupState, Uri uri)
		{
			this._jeGroupState = jeGroupState;
			if (jeGroupState != null)
			{
				jeGroupState.GroupExitEntry = this;
			}
			this.Source = uri;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Navigation.JournalEntry" /> class. </summary>
		/// <param name="info">The serialization information.</param>
		/// <param name="context">The streaming context.</param>
		// Token: 0x060028F5 RID: 10485 RVA: 0x000BDB58 File Offset: 0x000BBD58
		protected JournalEntry(SerializationInfo info, StreamingContext context)
		{
			this._id = info.GetInt32("_id");
			this._source = (Uri)info.GetValue("_source", typeof(Uri));
			this._entryType = (JournalEntryType)info.GetValue("_entryType", typeof(JournalEntryType));
			this._jeGroupState = (JournalEntryGroupState)info.GetValue("_jeGroupState", typeof(JournalEntryGroupState));
			this._customContentState = (CustomContentState)info.GetValue("_customContentState", typeof(CustomContentState));
			this._rootViewerState = (CustomJournalStateInternal)info.GetValue("_rootViewerState", typeof(CustomJournalStateInternal));
			this.Name = info.GetString("Name");
		}

		/// <summary>Called when this object is serialized.</summary>
		/// <param name="info">The data that is required to serialize the target object.</param>
		/// <param name="context">The streaming context.</param>
		// Token: 0x060028F6 RID: 10486 RVA: 0x000BDC30 File Offset: 0x000BBE30
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("_id", this._id);
			info.AddValue("_source", this._source);
			info.AddValue("_entryType", this._entryType);
			info.AddValue("_jeGroupState", this._jeGroupState);
			info.AddValue("_customContentState", this._customContentState);
			info.AddValue("_rootViewerState", this._rootViewerState);
			info.AddValue("Name", this.Name);
		}

		/// <summary>Gets the <see cref="P:System.Windows.Navigation.JournalEntry.Name" /> attached property of the journal entry for the specified element. </summary>
		/// <param name="dependencyObject">The element from which to get the attached property value.</param>
		/// <returns>The <see cref="P:System.Windows.Navigation.JournalEntry.Name" /> attached property of the journal entry for the specified element. </returns>
		// Token: 0x060028F7 RID: 10487 RVA: 0x000BDCC7 File Offset: 0x000BBEC7
		public static string GetName(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				return null;
			}
			return (string)dependencyObject.GetValue(JournalEntry.NameProperty);
		}

		/// <summary>Sets the <see cref="P:System.Windows.Navigation.JournalEntry.Name" /> attached property of the specified element.</summary>
		/// <param name="dependencyObject">The element on which to set the attached property value.</param>
		/// <param name="name">The name to be assigned to the attached property.</param>
		// Token: 0x060028F8 RID: 10488 RVA: 0x000BDCDE File Offset: 0x000BBEDE
		public static void SetName(DependencyObject dependencyObject, string name)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			dependencyObject.SetValue(JournalEntry.NameProperty, name);
		}

		/// <summary>Returns the <see cref="P:System.Windows.Navigation.JournalEntry.KeepAlive" /> attached property of the journal entry for the specified element. </summary>
		/// <param name="dependencyObject">The element from which to get the attached property value.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Navigation.JournalEntry.KeepAlive" /> attached property of the journal entry for the specified element. </returns>
		// Token: 0x060028F9 RID: 10489 RVA: 0x000BDCFA File Offset: 0x000BBEFA
		public static bool GetKeepAlive(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return (bool)dependencyObject.GetValue(JournalEntry.KeepAliveProperty);
		}

		/// <summary>Sets the <see cref="P:System.Windows.Navigation.JournalEntry.KeepAlive" /> attached property of the specified element.</summary>
		/// <param name="dependencyObject">The element on which to set the attached property value.</param>
		/// <param name="keepAlive">
		///       <see langword="true" /> to keep the journal entry in memory; otherwise, <see langword="false" />.</param>
		// Token: 0x060028FA RID: 10490 RVA: 0x000BDD1A File Offset: 0x000BBF1A
		public static void SetKeepAlive(DependencyObject dependencyObject, bool keepAlive)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			dependencyObject.SetValue(JournalEntry.KeepAliveProperty, keepAlive);
		}

		/// <summary>Gets or sets the URI of the content that was navigated to.</summary>
		/// <returns>The URI of the content that was navigated to, or <see langword="null" /> if no URI is associated with the entry.</returns>
		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x060028FB RID: 10491 RVA: 0x000BDD36 File Offset: 0x000BBF36
		// (set) Token: 0x060028FC RID: 10492 RVA: 0x000BDD3E File Offset: 0x000BBF3E
		public Uri Source
		{
			get
			{
				return this._source;
			}
			set
			{
				this._source = BindUriHelper.GetUriRelativeToPackAppBase(value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Navigation.CustomContentState" /> object that is associated with this journal entry.</summary>
		/// <returns>The <see cref="T:System.Windows.Navigation.CustomContentState" /> object that is associated with this journal entry. If one is not associated, <see langword="null" /> is returned.</returns>
		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x060028FD RID: 10493 RVA: 0x000BDD4C File Offset: 0x000BBF4C
		// (set) Token: 0x060028FE RID: 10494 RVA: 0x000BDD54 File Offset: 0x000BBF54
		public CustomContentState CustomContentState
		{
			get
			{
				return this._customContentState;
			}
			internal set
			{
				this._customContentState = value;
			}
		}

		/// <summary>Gets or sets the name of the journal entry.</summary>
		/// <returns>The name of the journal entry.</returns>
		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x060028FF RID: 10495 RVA: 0x000BDD5D File Offset: 0x000BBF5D
		// (set) Token: 0x06002900 RID: 10496 RVA: 0x000BDD6F File Offset: 0x000BBF6F
		public string Name
		{
			get
			{
				return (string)base.GetValue(JournalEntry.NameProperty);
			}
			set
			{
				base.SetValue(JournalEntry.NameProperty, value);
			}
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool IsPageFunction()
		{
			return false;
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool IsAlive()
		{
			return false;
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x000BDD80 File Offset: 0x000BBF80
		internal virtual void SaveState(object contentObject)
		{
			if (contentObject == null)
			{
				throw new ArgumentNullException("contentObject");
			}
			if (!this.IsAlive())
			{
				if (this._jeGroupState.JournalDataStreams == null)
				{
					this._jeGroupState.JournalDataStreams = new DataStreams();
				}
				this._jeGroupState.JournalDataStreams.Save(contentObject);
			}
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x000BDDD4 File Offset: 0x000BBFD4
		internal virtual void RestoreState(object contentObject)
		{
			if (contentObject == null)
			{
				throw new ArgumentNullException("contentObject");
			}
			if (!this.IsAlive())
			{
				DataStreams journalDataStreams = this._jeGroupState.JournalDataStreams;
				if (journalDataStreams != null)
				{
					journalDataStreams.Load(contentObject);
					journalDataStreams.Clear();
				}
			}
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x000BDE13 File Offset: 0x000BC013
		internal virtual bool Navigate(INavigator navigator, NavigationMode navMode)
		{
			if (this.Source != null)
			{
				return navigator.Navigate(this.Source, new NavigateInfo(this.Source, navMode, this));
			}
			Invariant.Assert(false, "Cannot navigate to a journal entry that does not have a Source.");
			return false;
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x000BDE4C File Offset: 0x000BC04C
		internal static string GetDisplayName(Uri uri, Uri siteOfOrigin)
		{
			if (!uri.IsAbsoluteUri)
			{
				return uri.ToString();
			}
			bool flag = string.Compare(uri.Scheme, PackUriHelper.UriSchemePack, StringComparison.OrdinalIgnoreCase) == 0;
			string text;
			if (flag)
			{
				Uri uri2 = BaseUriHelper.MakeRelativeToSiteOfOriginIfPossible(uri);
				if (!uri2.IsAbsoluteUri)
				{
					text = new Uri(siteOfOrigin, uri2).ToString();
				}
				else
				{
					string text2 = uri.AbsolutePath + uri.Query + uri.Fragment;
					string text3;
					string value;
					string text4;
					string text5;
					BaseUriHelper.GetAssemblyNameAndPart(new Uri(text2, UriKind.Relative), out text3, out value, out text4, out text5);
					if (!string.IsNullOrEmpty(value))
					{
						text = text3;
					}
					else
					{
						text = text2;
					}
				}
			}
			else
			{
				text = uri.ToString();
			}
			if (!string.IsNullOrEmpty(text) && text[0] == '/')
			{
				text = text.Substring(1);
			}
			return text;
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06002907 RID: 10503 RVA: 0x000BDEFF File Offset: 0x000BC0FF
		// (set) Token: 0x06002908 RID: 10504 RVA: 0x000BDF07 File Offset: 0x000BC107
		internal JournalEntryGroupState JEGroupState
		{
			get
			{
				return this._jeGroupState;
			}
			set
			{
				this._jeGroupState = value;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06002909 RID: 10505 RVA: 0x000BDF10 File Offset: 0x000BC110
		// (set) Token: 0x0600290A RID: 10506 RVA: 0x000BDF18 File Offset: 0x000BC118
		internal int Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x0600290B RID: 10507 RVA: 0x000BDF21 File Offset: 0x000BC121
		internal Guid NavigationServiceId
		{
			get
			{
				return this._jeGroupState.NavigationServiceId;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x0600290C RID: 10508 RVA: 0x000BDF2E File Offset: 0x000BC12E
		// (set) Token: 0x0600290D RID: 10509 RVA: 0x000BDF36 File Offset: 0x000BC136
		internal JournalEntryType EntryType
		{
			get
			{
				return this._entryType;
			}
			set
			{
				this._entryType = value;
			}
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x000BDF3F File Offset: 0x000BC13F
		internal bool IsNavigable()
		{
			return this._entryType == JournalEntryType.Navigable;
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x0600290F RID: 10511 RVA: 0x000BDF4A File Offset: 0x000BC14A
		internal uint ContentId
		{
			get
			{
				return this._jeGroupState.ContentId;
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06002910 RID: 10512 RVA: 0x000BDF57 File Offset: 0x000BC157
		// (set) Token: 0x06002911 RID: 10513 RVA: 0x000BDF5F File Offset: 0x000BC15F
		internal CustomJournalStateInternal RootViewerState
		{
			get
			{
				return this._rootViewerState;
			}
			set
			{
				this._rootViewerState = value;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Navigation.JournalEntry.Name" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Navigation.JournalEntry.Name" /> attached property.</returns>
		// Token: 0x04001BBF RID: 7103
		public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached("Name", typeof(string), typeof(JournalEntry), new PropertyMetadata(string.Empty));

		/// <summary>Identifies the <see cref="P:System.Windows.Navigation.JournalEntry.KeepAlive" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Navigation.JournalEntry.KeepAlive" /> attached property.</returns>
		// Token: 0x04001BC0 RID: 7104
		public static readonly DependencyProperty KeepAliveProperty = DependencyProperty.RegisterAttached("KeepAlive", typeof(bool), typeof(JournalEntry), new PropertyMetadata(false));

		// Token: 0x04001BC1 RID: 7105
		private int _id;

		// Token: 0x04001BC2 RID: 7106
		private JournalEntryGroupState _jeGroupState;

		// Token: 0x04001BC3 RID: 7107
		private Uri _source;

		// Token: 0x04001BC4 RID: 7108
		private JournalEntryType _entryType;

		// Token: 0x04001BC5 RID: 7109
		private CustomContentState _customContentState;

		// Token: 0x04001BC6 RID: 7110
		private CustomJournalStateInternal _rootViewerState;
	}
}
