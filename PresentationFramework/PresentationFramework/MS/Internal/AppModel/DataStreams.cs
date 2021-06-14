using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;

namespace MS.Internal.AppModel
{
	// Token: 0x020007C1 RID: 1985
	[Serializable]
	internal class DataStreams
	{
		// Token: 0x06007B5E RID: 31582 RVA: 0x0022B256 File Offset: 0x00229456
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal DataStreams()
		{
		}

		// Token: 0x17001CCB RID: 7371
		// (get) Token: 0x06007B5F RID: 31583 RVA: 0x0022B26A File Offset: 0x0022946A
		internal bool HasAnyData
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				return (this._subStreams != null && this._subStreams.Count > 0) || (this._customJournaledObjects != null && this._customJournaledObjects.Count > 0);
			}
		}

		// Token: 0x06007B60 RID: 31584 RVA: 0x0022B29C File Offset: 0x0022949C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool HasSubStreams(object key)
		{
			return this._subStreams != null && this._subStreams.Contains(key);
		}

		// Token: 0x06007B61 RID: 31585 RVA: 0x0022B2B4 File Offset: 0x002294B4
		[SecurityCritical]
		private ArrayList GetSubStreams(object key)
		{
			ArrayList arrayList = (ArrayList)this._subStreams[key];
			if (arrayList == null)
			{
				arrayList = new ArrayList(3);
				this._subStreams[key] = arrayList;
			}
			return arrayList;
		}

		// Token: 0x06007B62 RID: 31586 RVA: 0x0022B2EC File Offset: 0x002294EC
		[SecurityCritical]
		private ArrayList SaveSubStreams(UIElement element)
		{
			ArrayList arrayList = null;
			if (element != null && element.PersistId != 0)
			{
				LocalValueEnumerator localValueEnumerator = element.GetLocalValueEnumerator();
				while (localValueEnumerator.MoveNext())
				{
					LocalValueEntry localValueEntry = localValueEnumerator.Current;
					FrameworkPropertyMetadata frameworkPropertyMetadata = localValueEntry.Property.GetMetadata(element.DependencyObjectType) as FrameworkPropertyMetadata;
					if (frameworkPropertyMetadata != null && frameworkPropertyMetadata.Journal && !(localValueEntry.Value is Expression) && localValueEntry.Property != Frame.SourceProperty)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList(3);
						}
						object value = element.GetValue(localValueEntry.Property);
						byte[] dataBytes = null;
						if (value != null && !(value is Uri))
						{
							MemoryStream memoryStream = new MemoryStream();
							new SecurityPermission(SecurityPermissionFlag.SerializationFormatter).Assert();
							try
							{
								this.Formatter.Serialize(memoryStream, value);
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
							dataBytes = memoryStream.ToArray();
							((IDisposable)memoryStream).Dispose();
						}
						arrayList.Add(new SubStream(localValueEntry.Property.Name, dataBytes));
					}
				}
			}
			return arrayList;
		}

		// Token: 0x06007B63 RID: 31587 RVA: 0x0022B410 File Offset: 0x00229610
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void SaveState(object node)
		{
			UIElement uielement = node as UIElement;
			if (uielement == null)
			{
				return;
			}
			int persistId = uielement.PersistId;
			if (persistId != 0)
			{
				ArrayList arrayList = this.SaveSubStreams(uielement);
				if (arrayList != null && !this._subStreams.Contains(persistId))
				{
					this._subStreams[persistId] = arrayList;
				}
				IJournalState journalState = node as IJournalState;
				if (journalState != null)
				{
					object journalState2 = journalState.GetJournalState(JournalReason.NewContentNavigation);
					if (journalState2 != null)
					{
						if (this._customJournaledObjects == null)
						{
							this._customJournaledObjects = new HybridDictionary(2);
						}
						if (!this._customJournaledObjects.Contains(persistId))
						{
							this._customJournaledObjects[persistId] = journalState2;
						}
					}
				}
			}
		}

		// Token: 0x06007B64 RID: 31588 RVA: 0x0022B4B8 File Offset: 0x002296B8
		internal void PrepareForSerialization()
		{
			if (this._customJournaledObjects != null)
			{
				foreach (object obj in this._customJournaledObjects)
				{
					CustomJournalStateInternal customJournalStateInternal = (CustomJournalStateInternal)((DictionaryEntry)obj).Value;
					customJournalStateInternal.PrepareForSerialization();
				}
			}
		}

		// Token: 0x06007B65 RID: 31589 RVA: 0x0022B528 File Offset: 0x00229728
		[SecuritySafeCritical]
		private void LoadSubStreams(UIElement element, ArrayList subStreams)
		{
			for (int i = 0; i < subStreams.Count; i++)
			{
				SubStream subStream = (SubStream)subStreams[i];
				DependencyProperty dependencyProperty = DependencyProperty.FromName(subStream._propertyName, element.GetType());
				if (dependencyProperty != null)
				{
					object value = null;
					if (subStream._data != null)
					{
						try
						{
							new SecurityPermission(SecurityPermissionFlag.SerializationFormatter).Demand();
							value = this.Formatter.Deserialize(new MemoryStream(subStream._data));
						}
						catch (SecurityException)
						{
							value = DependencyProperty.UnsetValue;
						}
					}
					element.SetValue(dependencyProperty, value);
				}
			}
		}

		// Token: 0x06007B66 RID: 31590 RVA: 0x0022B5BC File Offset: 0x002297BC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void LoadState(object node)
		{
			UIElement uielement = node as UIElement;
			if (uielement == null)
			{
				return;
			}
			int persistId = uielement.PersistId;
			if (persistId != 0)
			{
				if (this.HasSubStreams(persistId))
				{
					ArrayList subStreams = this.GetSubStreams(persistId);
					this.LoadSubStreams(uielement, subStreams);
				}
				if (this._customJournaledObjects != null && this._customJournaledObjects.Contains(persistId))
				{
					CustomJournalStateInternal state = (CustomJournalStateInternal)this._customJournaledObjects[persistId];
					IJournalState journalState = node as IJournalState;
					if (journalState != null)
					{
						journalState.RestoreJournalState(state);
					}
				}
			}
		}

		// Token: 0x06007B67 RID: 31591 RVA: 0x0022B648 File Offset: 0x00229848
		private void WalkLogicalTree(object node, DataStreams.NodeOperation operation)
		{
			if (node != null)
			{
				operation(node);
			}
			DependencyObject dependencyObject = node as DependencyObject;
			if (dependencyObject == null)
			{
				return;
			}
			IEnumerator enumerator = LogicalTreeHelper.GetChildren(dependencyObject).GetEnumerator();
			if (enumerator == null)
			{
				return;
			}
			while (enumerator.MoveNext())
			{
				object node2 = enumerator.Current;
				this.WalkLogicalTree(node2, operation);
			}
		}

		// Token: 0x06007B68 RID: 31592 RVA: 0x0022B68F File Offset: 0x0022988F
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void Save(object root)
		{
			if (this._subStreams == null)
			{
				this._subStreams = new HybridDictionary(3);
			}
			else
			{
				this._subStreams.Clear();
			}
			this.WalkLogicalTree(root, new DataStreams.NodeOperation(this.SaveState));
		}

		// Token: 0x06007B69 RID: 31593 RVA: 0x0022B6C5 File Offset: 0x002298C5
		internal void Load(object root)
		{
			if (this.HasAnyData)
			{
				this.WalkLogicalTree(root, new DataStreams.NodeOperation(this.LoadState));
			}
		}

		// Token: 0x06007B6A RID: 31594 RVA: 0x0022B6E2 File Offset: 0x002298E2
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void Clear()
		{
			this._subStreams = null;
			this._customJournaledObjects = null;
		}

		// Token: 0x17001CCC RID: 7372
		// (get) Token: 0x06007B6B RID: 31595 RVA: 0x0022B6F2 File Offset: 0x002298F2
		private BinaryFormatter Formatter
		{
			get
			{
				if (DataStreams._formatter == null)
				{
					DataStreams._formatter = new BinaryFormatter();
				}
				return DataStreams._formatter;
			}
		}

		// Token: 0x04003A17 RID: 14871
		[ThreadStatic]
		private static BinaryFormatter _formatter;

		// Token: 0x04003A18 RID: 14872
		[SecurityCritical]
		private HybridDictionary _subStreams = new HybridDictionary(3);

		// Token: 0x04003A19 RID: 14873
		private HybridDictionary _customJournaledObjects;

		// Token: 0x02000B79 RID: 2937
		// (Invoke) Token: 0x06008E3B RID: 36411
		private delegate void NodeOperation(object node);
	}
}
