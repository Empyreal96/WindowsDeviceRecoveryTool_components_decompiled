using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000754 RID: 1876
	internal class ConnectionPointCookie
	{
		// Token: 0x06007797 RID: 30615 RVA: 0x00222168 File Offset: 0x00220368
		[SecurityCritical]
		internal ConnectionPointCookie(object source, object sink, Type eventInterface)
		{
			Exception ex = null;
			if (source is UnsafeNativeMethods.IConnectionPointContainer)
			{
				UnsafeNativeMethods.IConnectionPointContainer connectionPointContainer = (UnsafeNativeMethods.IConnectionPointContainer)source;
				try
				{
					Guid guid = eventInterface.GUID;
					if (connectionPointContainer.FindConnectionPoint(ref guid, out this.connectionPoint) != 0)
					{
						this.connectionPoint = null;
					}
				}
				catch (Exception ex2)
				{
					if (CriticalExceptions.IsCriticalException(ex2))
					{
						throw;
					}
					this.connectionPoint = null;
				}
				if (this.connectionPoint == null)
				{
					ex = new ArgumentException(SR.Get("AxNoEventInterface", new object[]
					{
						eventInterface.Name
					}));
				}
				else if (sink == null || (!eventInterface.IsInstanceOfType(sink) && !Marshal.IsComObject(sink)))
				{
					ex = new InvalidCastException(SR.Get("AxNoSinkImplementation", new object[]
					{
						eventInterface.Name
					}));
				}
				else
				{
					int num = this.connectionPoint.Advise(sink, ref this.cookie);
					if (num != 0)
					{
						this.cookie = 0;
						Marshal.FinalReleaseComObject(this.connectionPoint);
						this.connectionPoint = null;
						ex = new InvalidOperationException(SR.Get("AxNoSinkAdvise", new object[]
						{
							eventInterface.Name,
							num
						}));
					}
				}
			}
			else
			{
				ex = new InvalidCastException(SR.Get("AxNoConnectionPointContainer"));
			}
			if (this.connectionPoint != null && this.cookie != 0)
			{
				return;
			}
			if (this.connectionPoint != null)
			{
				Marshal.FinalReleaseComObject(this.connectionPoint);
			}
			if (ex == null)
			{
				throw new ArgumentException(SR.Get("AxNoConnectionPoint", new object[]
				{
					eventInterface.Name
				}));
			}
			throw ex;
		}

		// Token: 0x06007798 RID: 30616 RVA: 0x002222EC File Offset: 0x002204EC
		[SecurityCritical]
		internal void Disconnect()
		{
			if (this.connectionPoint != null && this.cookie != 0)
			{
				try
				{
					this.connectionPoint.Unadvise(this.cookie);
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.cookie = 0;
				}
				try
				{
					Marshal.FinalReleaseComObject(this.connectionPoint);
				}
				catch (Exception ex2)
				{
					if (CriticalExceptions.IsCriticalException(ex2))
					{
						throw;
					}
				}
				finally
				{
					this.connectionPoint = null;
				}
			}
		}

		// Token: 0x06007799 RID: 30617 RVA: 0x0022238C File Offset: 0x0022058C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		~ConnectionPointCookie()
		{
			this.Disconnect();
		}

		// Token: 0x040038C6 RID: 14534
		[SecurityCritical]
		private UnsafeNativeMethods.IConnectionPoint connectionPoint;

		// Token: 0x040038C7 RID: 14535
		private int cookie;
	}
}
