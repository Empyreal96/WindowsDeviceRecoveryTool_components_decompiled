using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace System.Management.Instrumentation
{
	// Token: 0x020000B4 RID: 180
	internal sealed class EventSource : IWbemProviderInit, IWbemEventProvider, IWbemEventProviderQuerySink, IWbemEventProviderSecurity, IWbemServices_Old
	{
		// Token: 0x060004B7 RID: 1207 RVA: 0x0002286C File Offset: 0x00020A6C
		public EventSource(string namespaceName, string appName, InstrumentedAssembly instrumentedAssembly)
		{
			ArrayList obj = EventSource.eventSources;
			lock (obj)
			{
				if (EventSource.shutdownInProgress == 0)
				{
					this.instrumentedAssembly = instrumentedAssembly;
					int num = this.registrar.Register_(0, null, null, null, namespaceName, appName, this);
					if (num != 0)
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
					EventSource.eventSources.Add(this);
				}
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00022934 File Offset: 0x00020B34
		~EventSource()
		{
			this.UnRegister();
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00022960 File Offset: 0x00020B60
		private void UnRegister()
		{
			lock (this)
			{
				if (this.registrar != null)
				{
					if (this.workerThreadInitialized)
					{
						this.alive = false;
						this.doIndicate.Set();
						GC.KeepAlive(this);
						this.workerThreadInitialized = false;
					}
					this.registrar.UnRegister_();
					this.registrar = null;
				}
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000229DC File Offset: 0x00020BDC
		private static void ProcessExit(object o, EventArgs args)
		{
			if (EventSource.shutdownInProgress != 0)
			{
				return;
			}
			Interlocked.Increment(ref EventSource.shutdownInProgress);
			try
			{
				EventSource.preventShutdownLock.AcquireWriterLock(-1);
				ArrayList obj = EventSource.eventSources;
				lock (obj)
				{
					foreach (object obj2 in EventSource.eventSources)
					{
						EventSource eventSource = (EventSource)obj2;
						eventSource.UnRegister();
					}
				}
			}
			finally
			{
				EventSource.preventShutdownLock.ReleaseWriterLock();
				Thread.Sleep(50);
				EventSource.preventShutdownLock.AcquireWriterLock(-1);
				EventSource.preventShutdownLock.ReleaseWriterLock();
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00022AB0 File Offset: 0x00020CB0
		static EventSource()
		{
			AppDomain.CurrentDomain.ProcessExit += EventSource.ProcessExit;
			AppDomain.CurrentDomain.DomainUnload += EventSource.ProcessExit;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00022B04 File Offset: 0x00020D04
		public void MTAWorkerThread2()
		{
			for (;;)
			{
				this.doIndicate.WaitOne();
				if (!this.alive)
				{
					break;
				}
				for (;;)
				{
					EventSource.MTARequest mtarequest = null;
					object obj = this.critSec;
					lock (obj)
					{
						if (this.reqList.Count <= 0)
						{
							break;
						}
						mtarequest = (EventSource.MTARequest)this.reqList[0];
						this.reqList.RemoveAt(0);
					}
					try
					{
						if (this.pSinkMTA != null)
						{
							int num = this.pSinkMTA.Indicate_(mtarequest.lengthFromSTA, mtarequest.objectsFromSTA);
							if (num < 0)
							{
								if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
								{
									ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
								}
								else
								{
									Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
								}
							}
						}
						continue;
					}
					catch (Exception exception)
					{
						mtarequest.exception = exception;
						continue;
					}
					finally
					{
						mtarequest.doneIndicate.Set();
						GC.KeepAlive(this);
					}
					return;
				}
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00022C14 File Offset: 0x00020E14
		public void IndicateEvents(int length, IntPtr[] objects)
		{
			if (this.pSinkMTA == null)
			{
				return;
			}
			if (MTAHelper.IsNoContextMTA())
			{
				int num = this.pSinkMTA.Indicate_(length, objects);
				if (num < 0)
				{
					if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			else
			{
				EventSource.MTARequest mtarequest = new EventSource.MTARequest(length, objects);
				object obj = this.critSec;
				lock (obj)
				{
					if (!this.workerThreadInitialized)
					{
						Thread thread = new Thread(new ThreadStart(this.MTAWorkerThread2));
						thread.IsBackground = true;
						thread.SetApartmentState(ApartmentState.MTA);
						thread.Start();
						this.workerThreadInitialized = true;
					}
					int index = this.reqList.Add(mtarequest);
					if (!this.doIndicate.Set())
					{
						this.reqList.RemoveAt(index);
						throw new ManagementException(RC.GetString("WORKER_THREAD_WAKEUP_FAILED"));
					}
				}
				mtarequest.doneIndicate.WaitOne();
				if (mtarequest.exception != null)
				{
					throw mtarequest.exception;
				}
			}
			GC.KeepAlive(this);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00022D40 File Offset: 0x00020F40
		private void RelocateSinkRCWToMTA()
		{
			new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethodWithParam(this.RelocateSinkRCWToMTA_ThreadFuncion))
			{
				Parameter = this
			}.Start();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00022D6C File Offset: 0x00020F6C
		private void RelocateSinkRCWToMTA_ThreadFuncion(object param)
		{
			EventSource eventSource = (EventSource)param;
			eventSource.pSinkMTA = (IWbemObjectSink)EventSource.RelocateRCWToCurrentApartment(eventSource.pSinkNA);
			eventSource.pSinkNA = null;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00022DA0 File Offset: 0x00020FA0
		private void RelocateNamespaceRCWToMTA()
		{
			new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethodWithParam(this.RelocateNamespaceRCWToMTA_ThreadFuncion))
			{
				Parameter = this
			}.Start();
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00022DCC File Offset: 0x00020FCC
		private void RelocateNamespaceRCWToMTA_ThreadFuncion(object param)
		{
			EventSource eventSource = (EventSource)param;
			eventSource.pNamespaceMTA = (IWbemServices)EventSource.RelocateRCWToCurrentApartment(eventSource.pNamespaceNA);
			eventSource.pNamespaceNA = null;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00022E00 File Offset: 0x00021000
		private static object RelocateRCWToCurrentApartment(object comObject)
		{
			if (comObject == null)
			{
				return null;
			}
			IntPtr iunknownForObject = Marshal.GetIUnknownForObject(comObject);
			int num = Marshal.ReleaseComObject(comObject);
			if (num != 0)
			{
				throw new Exception();
			}
			comObject = Marshal.GetObjectForIUnknown(iunknownForObject);
			Marshal.Release(iunknownForObject);
			return comObject;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00022E39 File Offset: 0x00021039
		public bool Any()
		{
			return this.pSinkMTA == null || this.mapQueryIdToQuery.Count == 0;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00022E54 File Offset: 0x00021054
		int IWbemProviderInit.Initialize_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszUser, [In] int lFlags, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszNamespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszLocale, [MarshalAs(UnmanagedType.Interface)] [In] IWbemServices pNamespace, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemProviderInitSink pInitSink)
		{
			this.pNamespaceNA = pNamespace;
			this.RelocateNamespaceRCWToMTA();
			this.pSinkNA = null;
			this.pSinkMTA = null;
			Hashtable obj = this.mapQueryIdToQuery;
			lock (obj)
			{
				this.mapQueryIdToQuery.Clear();
			}
			pInitSink.SetStatus_(0, 0);
			Marshal.ReleaseComObject(pInitSink);
			return 0;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00022EC8 File Offset: 0x000210C8
		int IWbemEventProvider.ProvideEvents_([MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pSink, [In] int lFlags)
		{
			this.pSinkNA = pSink;
			this.RelocateSinkRCWToMTA();
			return 0;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00022ED8 File Offset: 0x000210D8
		int IWbemEventProviderQuerySink.NewQuery_([In] uint dwId, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQueryLanguage, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQuery)
		{
			Hashtable obj = this.mapQueryIdToQuery;
			lock (obj)
			{
				if (this.mapQueryIdToQuery.ContainsKey(dwId))
				{
					this.mapQueryIdToQuery.Remove(dwId);
				}
				this.mapQueryIdToQuery.Add(dwId, wszQuery);
			}
			return 0;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00022F4C File Offset: 0x0002114C
		int IWbemEventProviderQuerySink.CancelQuery_([In] uint dwId)
		{
			Hashtable obj = this.mapQueryIdToQuery;
			lock (obj)
			{
				this.mapQueryIdToQuery.Remove(dwId);
			}
			return 0;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000F034 File Offset: 0x0000D234
		int IWbemEventProviderSecurity.AccessCheck_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszQueryLanguage, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQuery, [In] int lSidLength, [In] ref byte pSid)
		{
			return 0;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.OpenNamespace_([MarshalAs(UnmanagedType.BStr)] [In] string strNamespace, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemServices ppWorkingNamespace, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.CancelAsyncCall_([MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pSink)
		{
			return -2147217396;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00022F9F File Offset: 0x0002119F
		int IWbemServices_Old.QueryObjectSink_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemObjectSink ppResponseHandler)
		{
			ppResponseHandler = null;
			return -2147217396;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.GetObject_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppObject, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00022FAC File Offset: 0x000211AC
		int IWbemServices_Old.GetObjectAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			Match match = Regex.Match(strObjectPath.ToLower(CultureInfo.InvariantCulture), "(.*?)\\.instanceid=\"(.*?)\",processid=\"(.*?)\"");
			if (!match.Success)
			{
				pResponseHandler.SetStatus_(0, -2147217406, null, IntPtr.Zero);
				Marshal.ReleaseComObject(pResponseHandler);
				return -2147217406;
			}
			string value = match.Groups[1].Value;
			string value2 = match.Groups[2].Value;
			string value3 = match.Groups[3].Value;
			if (Instrumentation.ProcessIdentity != value3)
			{
				pResponseHandler.SetStatus_(0, -2147217406, null, IntPtr.Zero);
				Marshal.ReleaseComObject(pResponseHandler);
				return -2147217406;
			}
			int num = ((IConvertible)value2).ToInt32((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int)));
			object obj = null;
			try
			{
				InstrumentedAssembly.readerWriterLock.AcquireReaderLock(-1);
				obj = InstrumentedAssembly.mapIDToPublishedObject[num.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int)))];
			}
			finally
			{
				InstrumentedAssembly.readerWriterLock.ReleaseReaderLock();
			}
			if (obj != null)
			{
				Type type = (Type)this.instrumentedAssembly.mapTypeToConverter[obj.GetType()];
				if (type != null)
				{
					object obj2 = Activator.CreateInstance(type);
					ConvertToWMI convertToWMI = (ConvertToWMI)Delegate.CreateDelegate(typeof(ConvertToWMI), obj2, "ToWMI");
					object obj3 = obj;
					lock (obj3)
					{
						convertToWMI(obj);
					}
					IntPtr[] array = new IntPtr[]
					{
						(IntPtr)obj2.GetType().GetField("instWbemObjectAccessIP").GetValue(obj2)
					};
					Marshal.AddRef(array[0]);
					IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = new IWbemClassObjectFreeThreaded(array[0]);
					object obj4 = num;
					wbemClassObjectFreeThreaded.Put_("InstanceId", 0, ref obj4, 0);
					obj4 = Instrumentation.ProcessIdentity;
					wbemClassObjectFreeThreaded.Put_("ProcessId", 0, ref obj4, 0);
					pResponseHandler.Indicate_(1, array);
					pResponseHandler.SetStatus_(0, 0, null, IntPtr.Zero);
					Marshal.ReleaseComObject(pResponseHandler);
					return 0;
				}
			}
			pResponseHandler.SetStatus_(0, -2147217406, null, IntPtr.Zero);
			Marshal.ReleaseComObject(pResponseHandler);
			return -2147217406;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.PutClass_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObject, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.PutClassAsync_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObject, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.DeleteClass_([MarshalAs(UnmanagedType.BStr)] [In] string strClass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.DeleteClassAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strClass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00023218 File Offset: 0x00021418
		int IWbemServices_Old.CreateClassEnum_([MarshalAs(UnmanagedType.BStr)] [In] string strSuperclass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum)
		{
			ppEnum = null;
			return -2147217396;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.CreateClassEnumAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strSuperclass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.PutInstance_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInst, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.PutInstanceAsync_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInst, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.DeleteInstance_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.DeleteInstanceAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00023218 File Offset: 0x00021418
		int IWbemServices_Old.CreateInstanceEnum_([MarshalAs(UnmanagedType.BStr)] [In] string strFilter, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum)
		{
			ppEnum = null;
			return -2147217396;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00023224 File Offset: 0x00021424
		int IWbemServices_Old.CreateInstanceEnumAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strFilter, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			try
			{
				EventSource.preventShutdownLock.AcquireReaderLock(-1);
				if (EventSource.shutdownInProgress != 0)
				{
					return 0;
				}
				uint num = (uint)(Environment.TickCount + 100);
				Type type = null;
				foreach (object obj in this.instrumentedAssembly.mapTypeToConverter.Keys)
				{
					Type type2 = (Type)obj;
					if (string.Compare(ManagedNameAttribute.GetMemberName(type2), strFilter, StringComparison.Ordinal) == 0)
					{
						type = type2;
						break;
					}
				}
				if (null == type)
				{
					return 0;
				}
				int num2 = 64;
				IntPtr[] array = new IntPtr[num2];
				IntPtr[] array2 = new IntPtr[num2];
				ConvertToWMI[] array3 = new ConvertToWMI[num2];
				IWbemClassObjectFreeThreaded[] array4 = new IWbemClassObjectFreeThreaded[num2];
				int lHandle = 0;
				int num3 = 0;
				object processIdentity = Instrumentation.ProcessIdentity;
				try
				{
					InstrumentedAssembly.readerWriterLock.AcquireReaderLock(-1);
					foreach (object obj2 in InstrumentedAssembly.mapIDToPublishedObject)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
						if (EventSource.shutdownInProgress != 0)
						{
							return 0;
						}
						if (!(type != dictionaryEntry.Value.GetType()))
						{
							if (array3[num3] == null)
							{
								object obj3 = Activator.CreateInstance((Type)this.instrumentedAssembly.mapTypeToConverter[type]);
								array3[num3] = (ConvertToWMI)Delegate.CreateDelegate(typeof(ConvertToWMI), obj3, "ToWMI");
								object value = dictionaryEntry.Value;
								lock (value)
								{
									array3[num3](dictionaryEntry.Value);
								}
								array[num3] = (IntPtr)obj3.GetType().GetField("instWbemObjectAccessIP").GetValue(obj3);
								Marshal.AddRef(array[num3]);
								array4[num3] = new IWbemClassObjectFreeThreaded(array[num3]);
								array4[num3].Put_("ProcessId", 0, ref processIdentity, 0);
								if (num3 == 0)
								{
									int num4;
									WmiNetUtilsHelper.GetPropertyHandle_f27(27, array4[num3], "InstanceId", out num4, out lHandle);
								}
							}
							else
							{
								object value2 = dictionaryEntry.Value;
								lock (value2)
								{
									array3[num3](dictionaryEntry.Value);
								}
								array[num3] = (IntPtr)array3[num3].Target.GetType().GetField("instWbemObjectAccessIP").GetValue(array3[num3].Target);
								Marshal.AddRef(array[num3]);
								array4[num3] = new IWbemClassObjectFreeThreaded(array[num3]);
								array4[num3].Put_("ProcessId", 0, ref processIdentity, 0);
								if (num3 == 0)
								{
									int num5;
									WmiNetUtilsHelper.GetPropertyHandle_f27(27, array4[num3], "InstanceId", out num5, out lHandle);
								}
							}
							string text = (string)dictionaryEntry.Key;
							WmiNetUtilsHelper.WritePropertyValue_f28(28, array4[num3], lHandle, (text.Length + 1) * 2, text);
							num3++;
							if (num3 == num2 || Environment.TickCount >= (int)num)
							{
								for (int i = 0; i < num3; i++)
								{
									WmiNetUtilsHelper.Clone_f(12, array[i], out array2[i]);
								}
								int num6 = pResponseHandler.Indicate_(num3, array2);
								for (int j = 0; j < num3; j++)
								{
									Marshal.Release(array2[j]);
								}
								if (num6 != 0)
								{
									return 0;
								}
								num3 = 0;
								num = (uint)(Environment.TickCount + 100);
							}
						}
					}
				}
				finally
				{
					InstrumentedAssembly.readerWriterLock.ReleaseReaderLock();
				}
				if (num3 > 0)
				{
					for (int k = 0; k < num3; k++)
					{
						WmiNetUtilsHelper.Clone_f(12, array[k], out array2[k]);
					}
					pResponseHandler.Indicate_(num3, array2);
					for (int l = 0; l < num3; l++)
					{
						Marshal.Release(array2[l]);
					}
				}
			}
			finally
			{
				pResponseHandler.SetStatus_(0, 0, null, IntPtr.Zero);
				Marshal.ReleaseComObject(pResponseHandler);
				EventSource.preventShutdownLock.ReleaseReaderLock();
			}
			return 0;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x000236E4 File Offset: 0x000218E4
		int IWbemServices_Old.ExecQuery_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum)
		{
			ppEnum = null;
			return -2147217396;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.ExecQueryAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000236E4 File Offset: 0x000218E4
		int IWbemServices_Old.ExecNotificationQuery_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum)
		{
			ppEnum = null;
			return -2147217396;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.ExecNotificationQueryAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.ExecMethod_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [MarshalAs(UnmanagedType.BStr)] [In] string strMethodName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInParams, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppOutParams, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00022F98 File Offset: 0x00021198
		int IWbemServices_Old.ExecMethodAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [MarshalAs(UnmanagedType.BStr)] [In] string strMethodName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInParams, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x040004F3 RID: 1267
		private IWbemDecoupledRegistrar registrar = (IWbemDecoupledRegistrar)new WbemDecoupledRegistrar();

		// Token: 0x040004F4 RID: 1268
		private static ArrayList eventSources = new ArrayList();

		// Token: 0x040004F5 RID: 1269
		private InstrumentedAssembly instrumentedAssembly;

		// Token: 0x040004F6 RID: 1270
		private static int shutdownInProgress = 0;

		// Token: 0x040004F7 RID: 1271
		private static ReaderWriterLock preventShutdownLock = new ReaderWriterLock();

		// Token: 0x040004F8 RID: 1272
		private IWbemServices pNamespaceNA;

		// Token: 0x040004F9 RID: 1273
		private IWbemObjectSink pSinkNA;

		// Token: 0x040004FA RID: 1274
		private IWbemServices pNamespaceMTA;

		// Token: 0x040004FB RID: 1275
		private IWbemObjectSink pSinkMTA;

		// Token: 0x040004FC RID: 1276
		private ArrayList reqList = new ArrayList(3);

		// Token: 0x040004FD RID: 1277
		private object critSec = new object();

		// Token: 0x040004FE RID: 1278
		private AutoResetEvent doIndicate = new AutoResetEvent(false);

		// Token: 0x040004FF RID: 1279
		private bool workerThreadInitialized;

		// Token: 0x04000500 RID: 1280
		private bool alive = true;

		// Token: 0x04000501 RID: 1281
		private Hashtable mapQueryIdToQuery = new Hashtable();

		// Token: 0x0200010A RID: 266
		private class MTARequest
		{
			// Token: 0x06000675 RID: 1653 RVA: 0x000274DE File Offset: 0x000256DE
			public MTARequest(int length, IntPtr[] objects)
			{
				this.lengthFromSTA = length;
				this.objectsFromSTA = objects;
			}

			// Token: 0x0400056E RID: 1390
			public AutoResetEvent doneIndicate = new AutoResetEvent(false);

			// Token: 0x0400056F RID: 1391
			public Exception exception;

			// Token: 0x04000570 RID: 1392
			public int lengthFromSTA = -1;

			// Token: 0x04000571 RID: 1393
			public IntPtr[] objectsFromSTA;
		}
	}
}
