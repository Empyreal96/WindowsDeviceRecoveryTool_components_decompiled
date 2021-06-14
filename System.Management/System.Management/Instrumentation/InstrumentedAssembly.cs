using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Microsoft.CSharp;

namespace System.Management.Instrumentation
{
	// Token: 0x020000B9 RID: 185
	internal class InstrumentedAssembly
	{
		// Token: 0x060004F9 RID: 1273 RVA: 0x00023A34 File Offset: 0x00021C34
		private void InitEventSource(object param)
		{
			InstrumentedAssembly instrumentedAssembly = (InstrumentedAssembly)param;
			instrumentedAssembly.source = new EventSource(instrumentedAssembly.naming.NamespaceName, instrumentedAssembly.naming.DecoupledProviderInstanceName, this);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00023A6C File Offset: 0x00021C6C
		public void FindReferences(Type type, CompilerParameters parameters)
		{
			if (!parameters.ReferencedAssemblies.Contains(type.Assembly.Location))
			{
				parameters.ReferencedAssemblies.Add(type.Assembly.Location);
			}
			if (type.BaseType != null)
			{
				this.FindReferences(type.BaseType, parameters);
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				if (type2.Assembly != type.Assembly)
				{
					this.FindReferences(type2, parameters);
				}
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00023AF8 File Offset: 0x00021CF8
		public bool IsInstrumentedType(Type type)
		{
			if (null != type.GetInterface("System.Management.Instrumentation.IEvent", false) || null != type.GetInterface("System.Management.Instrumentation.IInstance", false))
			{
				return true;
			}
			object[] customAttributes = type.GetCustomAttributes(typeof(InstrumentationClassAttribute), true);
			return customAttributes != null && customAttributes.Length != 0;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00023B4C File Offset: 0x00021D4C
		public InstrumentedAssembly(Assembly assembly, SchemaNaming naming)
		{
			SecurityHelper.UnmanagedCode.Demand();
			this.naming = naming;
			Assembly assembly2 = naming.PrecompiledAssembly;
			if (null == assembly2)
			{
				CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider();
				CompilerParameters compilerParameters = new CompilerParameters();
				compilerParameters.GenerateInMemory = true;
				compilerParameters.ReferencedAssemblies.Add(assembly.Location);
				compilerParameters.ReferencedAssemblies.Add(typeof(BaseEvent).Assembly.Location);
				compilerParameters.ReferencedAssemblies.Add(typeof(Component).Assembly.Location);
				foreach (Type type in assembly.GetTypes())
				{
					if (this.IsInstrumentedType(type))
					{
						this.FindReferences(type, compilerParameters);
					}
				}
				CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, new string[]
				{
					naming.Code
				});
				foreach (object obj in compilerResults.Errors)
				{
					CompilerError compilerError = (CompilerError)obj;
					Console.WriteLine(compilerError.ToString());
				}
				if (compilerResults.Errors.HasErrors)
				{
					throw new Exception(RC.GetString("FAILED_TO_BUILD_GENERATED_ASSEMBLY"));
				}
				assembly2 = compilerResults.CompiledAssembly;
			}
			Type type2 = assembly2.GetType("WMINET_Converter");
			this.mapTypeToConverter = (Hashtable)type2.GetField("mapTypeToConverter").GetValue(null);
			if (!MTAHelper.IsNoContextMTA())
			{
				new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethodWithParam(this.InitEventSource))
				{
					Parameter = this
				}.Start();
				return;
			}
			this.InitEventSource(this);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00023D1C File Offset: 0x00021F1C
		public void Fire(object o)
		{
			SecurityHelper.UnmanagedCode.Demand();
			this.Fire(o.GetType(), o);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00023D38 File Offset: 0x00021F38
		public void Publish(object o)
		{
			SecurityHelper.UnmanagedCode.Demand();
			try
			{
				InstrumentedAssembly.readerWriterLock.AcquireWriterLock(-1);
				if (!InstrumentedAssembly.mapPublishedObjectToID.ContainsKey(o))
				{
					InstrumentedAssembly.mapIDToPublishedObject.Add(InstrumentedAssembly.upcountId.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int))), o);
					InstrumentedAssembly.mapPublishedObjectToID.Add(o, InstrumentedAssembly.upcountId);
					InstrumentedAssembly.upcountId++;
				}
			}
			finally
			{
				InstrumentedAssembly.readerWriterLock.ReleaseWriterLock();
			}
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00023DD8 File Offset: 0x00021FD8
		public void Revoke(object o)
		{
			SecurityHelper.UnmanagedCode.Demand();
			try
			{
				InstrumentedAssembly.readerWriterLock.AcquireWriterLock(-1);
				object obj = InstrumentedAssembly.mapPublishedObjectToID[o];
				if (obj != null)
				{
					int num = (int)obj;
					InstrumentedAssembly.mapPublishedObjectToID.Remove(o);
					InstrumentedAssembly.mapIDToPublishedObject.Remove(num.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int))));
				}
			}
			finally
			{
				InstrumentedAssembly.readerWriterLock.ReleaseWriterLock();
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00023E64 File Offset: 0x00022064
		public void SetBatchSize(Type t, int batchSize)
		{
			this.GetTypeInfo(t).SetBatchSize(batchSize);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00023E74 File Offset: 0x00022074
		private InstrumentedAssembly.TypeInfo GetTypeInfo(Type t)
		{
			Hashtable obj = this.mapTypeToTypeInfo;
			InstrumentedAssembly.TypeInfo result;
			lock (obj)
			{
				if (this.lastType == t)
				{
					result = this.lastTypeInfo;
				}
				else
				{
					this.lastType = t;
					InstrumentedAssembly.TypeInfo typeInfo = (InstrumentedAssembly.TypeInfo)this.mapTypeToTypeInfo[t];
					if (typeInfo == null)
					{
						typeInfo = new InstrumentedAssembly.TypeInfo(this.source, this.naming, (Type)this.mapTypeToConverter[t]);
						this.mapTypeToTypeInfo.Add(t, typeInfo);
					}
					this.lastTypeInfo = typeInfo;
					result = typeInfo;
				}
			}
			return result;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00023F1C File Offset: 0x0002211C
		public void Fire(Type t, object o)
		{
			InstrumentedAssembly.TypeInfo typeInfo = this.GetTypeInfo(t);
			typeInfo.Fire(o);
		}

		// Token: 0x04000505 RID: 1285
		private SchemaNaming naming;

		// Token: 0x04000506 RID: 1286
		public EventSource source;

		// Token: 0x04000507 RID: 1287
		public Hashtable mapTypeToConverter;

		// Token: 0x04000508 RID: 1288
		public static ReaderWriterLock readerWriterLock = new ReaderWriterLock();

		// Token: 0x04000509 RID: 1289
		public static Hashtable mapIDToPublishedObject = new Hashtable();

		// Token: 0x0400050A RID: 1290
		private static Hashtable mapPublishedObjectToID = new Hashtable();

		// Token: 0x0400050B RID: 1291
		private static int upcountId = 3839;

		// Token: 0x0400050C RID: 1292
		private Hashtable mapTypeToTypeInfo = new Hashtable();

		// Token: 0x0400050D RID: 1293
		private InstrumentedAssembly.TypeInfo lastTypeInfo;

		// Token: 0x0400050E RID: 1294
		private Type lastType;

		// Token: 0x0200010B RID: 267
		private class TypeInfo
		{
			// Token: 0x06000676 RID: 1654 RVA: 0x00027508 File Offset: 0x00025708
			public void Fire(object o)
			{
				if (this.source.Any())
				{
					return;
				}
				if (!this.batchEvents)
				{
					lock (this)
					{
						this.convertFunctionNoBatch(o);
						this.wbemObjects[0] = (IntPtr)this.fieldInfo.GetValue(this.convertFunctionNoBatch.Target);
						this.source.IndicateEvents(1, this.wbemObjects);
						return;
					}
				}
				lock (this)
				{
					ConvertToWMI[] array = this.convertFunctionsBatch;
					int num = this.currentIndex;
					this.currentIndex = num + 1;
					array[num](o);
					this.wbemObjects[this.currentIndex - 1] = (IntPtr)this.fieldInfo.GetValue(this.convertFunctionsBatch[this.currentIndex - 1].Target);
					if (this.cleanupThread == null)
					{
						int tickCount = Environment.TickCount;
						if (tickCount - this.lastFire < 1000)
						{
							this.lastFire = Environment.TickCount;
							this.cleanupThread = new Thread(new ThreadStart(this.Cleanup));
							this.cleanupThread.SetApartmentState(ApartmentState.MTA);
							this.cleanupThread.Start();
						}
						else
						{
							this.source.IndicateEvents(this.currentIndex, this.wbemObjects);
							this.currentIndex = 0;
							this.lastFire = tickCount;
						}
					}
					else if (this.currentIndex == this.batchSize)
					{
						this.source.IndicateEvents(this.currentIndex, this.wbemObjects);
						this.currentIndex = 0;
						this.lastFire = Environment.TickCount;
					}
				}
			}

			// Token: 0x06000677 RID: 1655 RVA: 0x000276E0 File Offset: 0x000258E0
			public void SetBatchSize(int batchSize)
			{
				if (batchSize <= 0)
				{
					throw new ArgumentOutOfRangeException("batchSize");
				}
				if (!WMICapabilities.MultiIndicateSupported)
				{
					batchSize = 1;
				}
				lock (this)
				{
					if (this.currentIndex > 0)
					{
						this.source.IndicateEvents(this.currentIndex, this.wbemObjects);
						this.currentIndex = 0;
						this.lastFire = Environment.TickCount;
					}
					this.wbemObjects = new IntPtr[batchSize];
					if (batchSize > 1)
					{
						this.batchEvents = true;
						this.batchSize = batchSize;
						this.convertFunctionsBatch = new ConvertToWMI[batchSize];
						for (int i = 0; i < batchSize; i++)
						{
							object obj = Activator.CreateInstance(this.converterType);
							this.convertFunctionsBatch[i] = (ConvertToWMI)Delegate.CreateDelegate(typeof(ConvertToWMI), obj, "ToWMI");
							this.wbemObjects[i] = this.ExtractIntPtr(obj);
						}
						this.fieldInfo = this.convertFunctionsBatch[0].Target.GetType().GetField("instWbemObjectAccessIP");
					}
					else
					{
						this.fieldInfo = this.convertFunctionNoBatch.Target.GetType().GetField("instWbemObjectAccessIP");
						this.wbemObjects[0] = this.ExtractIntPtr(this.convertFunctionNoBatch.Target);
						this.batchEvents = false;
					}
				}
			}

			// Token: 0x06000678 RID: 1656 RVA: 0x00027848 File Offset: 0x00025A48
			public IntPtr ExtractIntPtr(object o)
			{
				return (IntPtr)o.GetType().GetField("instWbemObjectAccessIP").GetValue(o);
			}

			// Token: 0x06000679 RID: 1657 RVA: 0x00027868 File Offset: 0x00025A68
			public void Cleanup()
			{
				int i = 0;
				while (i < 20)
				{
					Thread.Sleep(100);
					if (this.currentIndex == 0)
					{
						i++;
					}
					else
					{
						i = 0;
						if (Environment.TickCount - this.lastFire >= 100)
						{
							lock (this)
							{
								if (this.currentIndex > 0)
								{
									this.source.IndicateEvents(this.currentIndex, this.wbemObjects);
									this.currentIndex = 0;
									this.lastFire = Environment.TickCount;
								}
							}
						}
					}
				}
				this.cleanupThread = null;
			}

			// Token: 0x0600067A RID: 1658 RVA: 0x00027908 File Offset: 0x00025B08
			public TypeInfo(EventSource source, SchemaNaming naming, Type converterType)
			{
				this.converterType = converterType;
				this.source = source;
				object target = Activator.CreateInstance(converterType);
				this.convertFunctionNoBatch = (ConvertToWMI)Delegate.CreateDelegate(typeof(ConvertToWMI), target, "ToWMI");
				this.SetBatchSize(this.batchSize);
			}

			// Token: 0x04000572 RID: 1394
			private FieldInfo fieldInfo;

			// Token: 0x04000573 RID: 1395
			private int batchSize = 64;

			// Token: 0x04000574 RID: 1396
			private bool batchEvents = true;

			// Token: 0x04000575 RID: 1397
			private ConvertToWMI[] convertFunctionsBatch;

			// Token: 0x04000576 RID: 1398
			private ConvertToWMI convertFunctionNoBatch;

			// Token: 0x04000577 RID: 1399
			private IntPtr[] wbemObjects;

			// Token: 0x04000578 RID: 1400
			private Type converterType;

			// Token: 0x04000579 RID: 1401
			private int currentIndex;

			// Token: 0x0400057A RID: 1402
			public EventSource source;

			// Token: 0x0400057B RID: 1403
			public int lastFire;

			// Token: 0x0400057C RID: 1404
			public Thread cleanupThread;
		}
	}
}
