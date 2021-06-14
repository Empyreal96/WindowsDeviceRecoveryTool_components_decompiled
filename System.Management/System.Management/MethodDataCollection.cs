using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Management
{
	/// <summary>Represents the set of methods available in the collection.          </summary>
	// Token: 0x0200004D RID: 77
	public class MethodDataCollection : ICollection, IEnumerable
	{
		// Token: 0x060002CF RID: 719 RVA: 0x0000FED2 File Offset: 0x0000E0D2
		internal MethodDataCollection(ManagementObject parent)
		{
			this.parent = parent;
		}

		/// <summary>Gets the number of objects in the <see cref="T:System.Management.MethodDataCollection" /> collection.         </summary>
		/// <returns>Returns an <see cref="T:System.Int32" /> value representing the number of objects in the collection.</returns>
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000FEE4 File Offset: 0x0000E0E4
		public int Count
		{
			get
			{
				int num = 0;
				IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
				IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded2 = null;
				int num2 = -2147217407;
				Type typeFromHandle = typeof(MethodDataCollection.enumLock);
				lock (typeFromHandle)
				{
					try
					{
						num2 = this.parent.wbemObject.BeginMethodEnumeration_(0);
						if (num2 >= 0)
						{
							string text = "";
							while (text != null && num2 >= 0 && num2 != 262149)
							{
								text = null;
								wbemClassObjectFreeThreaded = null;
								wbemClassObjectFreeThreaded2 = null;
								num2 = this.parent.wbemObject.NextMethod_(0, out text, out wbemClassObjectFreeThreaded, out wbemClassObjectFreeThreaded2);
								if (num2 >= 0 && num2 != 262149)
								{
									num++;
								}
							}
							this.parent.wbemObject.EndMethodEnumeration_();
						}
					}
					catch (COMException e)
					{
						ManagementException.ThrowWithExtendedInfo(e);
					}
				}
				if (((long)num2 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
				}
				else if (((long)num2 & (long)((ulong)-2147483648)) != 0L)
				{
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
				return num;
			}
		}

		/// <summary>Gets a value that indicates whether the object is synchronized.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether the object is synchronized.</returns>
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000F034 File Offset: 0x0000D234
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the object to be used for synchronization.          </summary>
		/// <returns>Returns an <see cref="T:System.Object" /> value representing the object to be used for synchronization.</returns>
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000F037 File Offset: 0x0000D237
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Copies the <see cref="T:System.Management.MethodDataCollection" /> into an array.          </summary>
		/// <param name="array">The array to which to copy the collection. </param>
		/// <param name="index">The index from which to start. </param>
		// Token: 0x060002D3 RID: 723 RVA: 0x0000FFF8 File Offset: 0x0000E1F8
		public void CopyTo(Array array, int index)
		{
			foreach (MethodData value in this)
			{
				array.SetValue(value, index++);
			}
		}

		/// <summary>Copies the <see cref="T:System.Management.MethodDataCollection" /> to a specialized <see cref="T:System.Management.MethodData" /> array.          </summary>
		/// <param name="methodArray">The destination array to which to copy the <see cref="T:System.Management.MethodData" /> objects.</param>
		/// <param name="index">The index in the destination array from which to start the copy.</param>
		// Token: 0x060002D4 RID: 724 RVA: 0x00010050 File Offset: 0x0000E250
		public void CopyTo(MethodData[] methodArray, int index)
		{
			this.CopyTo(methodArray, index);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> that iterates through the <see cref="T:System.Management.MethodDataCollection" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Management.MethodDataCollection" />.</returns>
		// Token: 0x060002D5 RID: 725 RVA: 0x0001005A File Offset: 0x0000E25A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new MethodDataCollection.MethodDataEnumerator(this.parent);
		}

		/// <summary>Returns an enumerator for the <see cref="T:System.Management.MethodDataCollection" />.          </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> to enumerate through the collection.</returns>
		// Token: 0x060002D6 RID: 726 RVA: 0x0001005A File Offset: 0x0000E25A
		public MethodDataCollection.MethodDataEnumerator GetEnumerator()
		{
			return new MethodDataCollection.MethodDataEnumerator(this.parent);
		}

		/// <summary>Gets the specified <see cref="T:System.Management.MethodData" /> from the <see cref="T:System.Management.MethodDataCollection" />.          </summary>
		/// <param name="methodName">The name of the method requested.</param>
		/// <returns>Returns a <see cref="T:System.Management.MethodData" /> containing the method data for a specified method from the collection.</returns>
		// Token: 0x17000095 RID: 149
		public virtual MethodData this[string methodName]
		{
			get
			{
				if (methodName == null)
				{
					throw new ArgumentNullException("methodName");
				}
				return new MethodData(this.parent, methodName);
			}
		}

		/// <summary>Removes a <see cref="T:System.Management.MethodData" /> from the <see cref="T:System.Management.MethodDataCollection" />.          </summary>
		/// <param name="methodName">The name of the method to remove from the collection.</param>
		// Token: 0x060002D8 RID: 728 RVA: 0x00010084 File Offset: 0x0000E284
		public virtual void Remove(string methodName)
		{
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				throw new InvalidOperationException();
			}
			int num = -2147217407;
			try
			{
				num = this.parent.wbemObject.DeleteMethod_(methodName);
			}
			catch (COMException e)
			{
				ManagementException.ThrowWithExtendedInfo(e);
			}
			if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				return;
			}
			if (((long)num & (long)((ulong)-2147483648)) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		/// <summary>Adds a <see cref="T:System.Management.MethodData" /> to the <see cref="T:System.Management.MethodDataCollection" />. This overload will add a new method with no parameters to the collection.          </summary>
		/// <param name="methodName">The name of the method to add.</param>
		// Token: 0x060002D9 RID: 729 RVA: 0x0001011C File Offset: 0x0000E31C
		public virtual void Add(string methodName)
		{
			this.Add(methodName, null, null);
		}

		/// <summary>Adds a <see cref="T:System.Management.MethodData" /> to the <see cref="T:System.Management.MethodDataCollection" />. This overload will add a new method with the specified parameter objects to the collection.          </summary>
		/// <param name="methodName">The name of the method to add.</param>
		/// <param name="inParameters">The <see cref="T:System.Management.ManagementBaseObject" /> holding the input parameters to the method. </param>
		/// <param name="outParameters">The <see cref="T:System.Management.ManagementBaseObject" /> holding the output parameters to the method. </param>
		// Token: 0x060002DA RID: 730 RVA: 0x00010128 File Offset: 0x0000E328
		public virtual void Add(string methodName, ManagementBaseObject inParameters, ManagementBaseObject outParameters)
		{
			IWbemClassObjectFreeThreaded pInSignature = null;
			IWbemClassObjectFreeThreaded pOutSignature = null;
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				throw new InvalidOperationException();
			}
			if (inParameters != null)
			{
				pInSignature = inParameters.wbemObject;
			}
			if (outParameters != null)
			{
				pOutSignature = outParameters.wbemObject;
			}
			int num = -2147217407;
			try
			{
				num = this.parent.wbemObject.PutMethod_(methodName, 0, pInSignature, pOutSignature);
			}
			catch (COMException e)
			{
				ManagementException.ThrowWithExtendedInfo(e);
			}
			if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				return;
			}
			if (((long)num & (long)((ulong)-2147483648)) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x040001DD RID: 477
		private ManagementObject parent;

		// Token: 0x020000FD RID: 253
		private class enumLock
		{
		}

		/// <summary>Represents the enumerator for <see cref="T:System.Management.MethodData" /> objects in the <see cref="T:System.Management.MethodDataCollection" />. </summary>
		// Token: 0x020000FE RID: 254
		public class MethodDataEnumerator : IEnumerator
		{
			// Token: 0x0600065B RID: 1627 RVA: 0x00027360 File Offset: 0x00025560
			internal MethodDataEnumerator(ManagementObject parent)
			{
				this.parent = parent;
				this.methodNames = new ArrayList();
				IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
				IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded2 = null;
				int num = -2147217407;
				Type typeFromHandle = typeof(MethodDataCollection.enumLock);
				lock (typeFromHandle)
				{
					try
					{
						num = parent.wbemObject.BeginMethodEnumeration_(0);
						if (num >= 0)
						{
							string text = "";
							while (text != null && num >= 0 && num != 262149)
							{
								text = null;
								num = parent.wbemObject.NextMethod_(0, out text, out wbemClassObjectFreeThreaded, out wbemClassObjectFreeThreaded2);
								if (num >= 0 && num != 262149)
								{
									this.methodNames.Add(text);
								}
							}
							parent.wbemObject.EndMethodEnumeration_();
						}
					}
					catch (COMException e)
					{
						ManagementException.ThrowWithExtendedInfo(e);
					}
					this.en = this.methodNames.GetEnumerator();
				}
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				if (((long)num & (long)((ulong)-2147483648)) != 0L)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}

			/// <summary>Gets the current object in the collection.</summary>
			/// <returns>Returns the current element in the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x0600065C RID: 1628 RVA: 0x00027484 File Offset: 0x00025684
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			/// <summary>Returns the current <see cref="T:System.Management.MethodData" /> in the <see cref="T:System.Management.MethodDataCollection" /> enumeration.</summary>
			/// <returns>The current <see cref="T:System.Management.MethodData" /> item in the collection.</returns>
			// Token: 0x170000E5 RID: 229
			// (get) Token: 0x0600065D RID: 1629 RVA: 0x0002748C File Offset: 0x0002568C
			public MethodData Current
			{
				get
				{
					return new MethodData(this.parent, (string)this.en.Current);
				}
			}

			/// <summary>Moves to the next element in the <see cref="T:System.Management.MethodDataCollection" /> enumeration.</summary>
			/// <returns>
			///     <see langword="true" /> if the enumerator was successfully advanced to the next method; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
			// Token: 0x0600065E RID: 1630 RVA: 0x000274A9 File Offset: 0x000256A9
			public bool MoveNext()
			{
				return this.en.MoveNext();
			}

			/// <summary>Resets the enumerator to the beginning of the <see cref="T:System.Management.MethodDataCollection" /> enumeration.</summary>
			// Token: 0x0600065F RID: 1631 RVA: 0x000274B6 File Offset: 0x000256B6
			public void Reset()
			{
				this.en.Reset();
			}

			// Token: 0x04000551 RID: 1361
			private ManagementObject parent;

			// Token: 0x04000552 RID: 1362
			private ArrayList methodNames;

			// Token: 0x04000553 RID: 1363
			private IEnumerator en;
		}
	}
}
