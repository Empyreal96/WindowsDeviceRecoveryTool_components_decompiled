using System;
using System.Collections;

namespace System.Management
{
	/// <summary>Manages asynchronous operations and handles management information and events received asynchronously.          </summary>
	// Token: 0x02000025 RID: 37
	public class ManagementOperationObserver
	{
		/// <summary>Occurs when a new object is available.</summary>
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600011E RID: 286 RVA: 0x0000790C File Offset: 0x00005B0C
		// (remove) Token: 0x0600011F RID: 287 RVA: 0x00007944 File Offset: 0x00005B44
		public event ObjectReadyEventHandler ObjectReady;

		/// <summary>Occurs when an operation has completed.</summary>
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000120 RID: 288 RVA: 0x0000797C File Offset: 0x00005B7C
		// (remove) Token: 0x06000121 RID: 289 RVA: 0x000079B4 File Offset: 0x00005BB4
		public event CompletedEventHandler Completed;

		/// <summary>Occurs to indicate the progress of an ongoing operation.</summary>
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000122 RID: 290 RVA: 0x000079EC File Offset: 0x00005BEC
		// (remove) Token: 0x06000123 RID: 291 RVA: 0x00007A24 File Offset: 0x00005C24
		public event ProgressEventHandler Progress;

		/// <summary>Occurs when an object has been successfully committed.</summary>
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000124 RID: 292 RVA: 0x00007A5C File Offset: 0x00005C5C
		// (remove) Token: 0x06000125 RID: 293 RVA: 0x00007A94 File Offset: 0x00005C94
		public event ObjectPutEventHandler ObjectPut;

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementOperationObserver" /> class. This is the default constructor.          </summary>
		// Token: 0x06000126 RID: 294 RVA: 0x00007AC9 File Offset: 0x00005CC9
		public ManagementOperationObserver()
		{
			this.m_sinkCollection = new Hashtable();
			this.delegateInvoker = new WmiDelegateInvoker(this);
		}

		/// <summary>Cancels all outstanding operations.          </summary>
		// Token: 0x06000127 RID: 295 RVA: 0x00007AE8 File Offset: 0x00005CE8
		public void Cancel()
		{
			Hashtable hashtable = new Hashtable();
			Hashtable sinkCollection = this.m_sinkCollection;
			lock (sinkCollection)
			{
				IDictionaryEnumerator enumerator = this.m_sinkCollection.GetEnumerator();
				try
				{
					enumerator.Reset();
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
					}
				}
				catch
				{
				}
			}
			try
			{
				IDictionaryEnumerator enumerator2 = hashtable.GetEnumerator();
				enumerator2.Reset();
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					WmiEventSink wmiEventSink = (WmiEventSink)((DictionaryEntry)obj2).Value;
					try
					{
						wmiEventSink.Cancel();
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007BD4 File Offset: 0x00005DD4
		internal WmiEventSink GetNewSink(ManagementScope scope, object context)
		{
			WmiEventSink result;
			try
			{
				WmiEventSink wmiEventSink = WmiEventSink.GetWmiEventSink(this, context, scope, null, null);
				Hashtable sinkCollection = this.m_sinkCollection;
				lock (sinkCollection)
				{
					this.m_sinkCollection.Add(wmiEventSink.GetHashCode(), wmiEventSink);
				}
				result = wmiEventSink;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00007C48 File Offset: 0x00005E48
		internal bool HaveListenersForProgress
		{
			get
			{
				bool result = false;
				try
				{
					if (this.Progress != null)
					{
						result = (this.Progress.GetInvocationList().Length != 0);
					}
				}
				catch
				{
				}
				return result;
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00007C88 File Offset: 0x00005E88
		internal WmiEventSink GetNewPutSink(ManagementScope scope, object context, string path, string className)
		{
			WmiEventSink result;
			try
			{
				WmiEventSink wmiEventSink = WmiEventSink.GetWmiEventSink(this, context, scope, path, className);
				Hashtable sinkCollection = this.m_sinkCollection;
				lock (sinkCollection)
				{
					this.m_sinkCollection.Add(wmiEventSink.GetHashCode(), wmiEventSink);
				}
				result = wmiEventSink;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00007CFC File Offset: 0x00005EFC
		internal WmiGetEventSink GetNewGetSink(ManagementScope scope, object context, ManagementObject managementObject)
		{
			WmiGetEventSink result;
			try
			{
				WmiGetEventSink wmiGetEventSink = WmiGetEventSink.GetWmiGetEventSink(this, context, scope, managementObject);
				Hashtable sinkCollection = this.m_sinkCollection;
				lock (sinkCollection)
				{
					this.m_sinkCollection.Add(wmiGetEventSink.GetHashCode(), wmiGetEventSink);
				}
				result = wmiGetEventSink;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00007D70 File Offset: 0x00005F70
		internal void RemoveSink(WmiEventSink eventSink)
		{
			try
			{
				Hashtable sinkCollection = this.m_sinkCollection;
				lock (sinkCollection)
				{
					this.m_sinkCollection.Remove(eventSink.GetHashCode());
				}
				eventSink.ReleaseStub();
			}
			catch
			{
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00007DD8 File Offset: 0x00005FD8
		internal void FireObjectReady(ObjectReadyEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.ObjectReady, args);
			}
			catch
			{
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007E0C File Offset: 0x0000600C
		internal void FireCompleted(CompletedEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.Completed, args);
			}
			catch
			{
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00007E40 File Offset: 0x00006040
		internal void FireProgress(ProgressEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.Progress, args);
			}
			catch
			{
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007E74 File Offset: 0x00006074
		internal void FireObjectPut(ObjectPutEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.ObjectPut, args);
			}
			catch
			{
			}
		}

		// Token: 0x04000117 RID: 279
		private Hashtable m_sinkCollection;

		// Token: 0x04000118 RID: 280
		private WmiDelegateInvoker delegateInvoker;
	}
}
