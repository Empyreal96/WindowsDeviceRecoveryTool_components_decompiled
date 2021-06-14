using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Management
{
	/// <summary>Represents a collection of <see cref="T:System.Management.QualifierData" /> objects.          </summary>
	// Token: 0x0200004B RID: 75
	public class QualifierDataCollection : ICollection, IEnumerable
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x0000F9B6 File Offset: 0x0000DBB6
		internal QualifierDataCollection(ManagementBaseObject parent)
		{
			this.parent = parent;
			this.qualifierSetType = QualifierType.ObjectQualifier;
			this.propertyOrMethodName = null;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000F9D3 File Offset: 0x0000DBD3
		internal QualifierDataCollection(ManagementBaseObject parent, string propertyOrMethodName, QualifierType type)
		{
			this.parent = parent;
			this.propertyOrMethodName = propertyOrMethodName;
			this.qualifierSetType = type;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000F9F0 File Offset: 0x0000DBF0
		private IWbemQualifierSetFreeThreaded GetTypeQualifierSet()
		{
			return this.GetTypeQualifierSet(this.qualifierSetType);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000FA00 File Offset: 0x0000DC00
		private IWbemQualifierSetFreeThreaded GetTypeQualifierSet(QualifierType qualifierSetType)
		{
			IWbemQualifierSetFreeThreaded result = null;
			int num;
			switch (qualifierSetType)
			{
			case QualifierType.ObjectQualifier:
				num = this.parent.wbemObject.GetQualifierSet_(out result);
				break;
			case QualifierType.PropertyQualifier:
				num = this.parent.wbemObject.GetPropertyQualifierSet_(this.propertyOrMethodName, out result);
				break;
			case QualifierType.MethodQualifier:
				num = this.parent.wbemObject.GetMethodQualifierSet_(this.propertyOrMethodName, out result);
				break;
			default:
				throw new ManagementException(ManagementStatus.Unexpected, null, null);
			}
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
			return result;
		}

		/// <summary>Gets the number of <see cref="T:System.Management.QualifierData" /> objects in the <see cref="T:System.Management.QualifierDataCollection" />.          </summary>
		/// <returns>Returns an <see cref="T:System.Int32" /> value representing the number of objects in the collection.</returns>
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000FAAC File Offset: 0x0000DCAC
		public int Count
		{
			get
			{
				string[] array = null;
				IWbemQualifierSetFreeThreaded typeQualifierSet;
				try
				{
					typeQualifierSet = this.GetTypeQualifierSet();
				}
				catch (ManagementException ex)
				{
					if (this.qualifierSetType == QualifierType.PropertyQualifier && ex.ErrorCode == ManagementStatus.SystemProperty)
					{
						return 0;
					}
					throw;
				}
				int names_ = typeQualifierSet.GetNames_(0, out array);
				if (names_ < 0)
				{
					if (((long)names_ & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)names_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(names_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				return array.Length;
			}
		}

		/// <summary>Gets a value indicating whether the object is synchronized.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether the object is synchronized.</returns>
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000F034 File Offset: 0x0000D234
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the object to be used for synchronization.          </summary>
		/// <returns>Returns an <see cref="T:System.Object" /> value to be used for synchronization.</returns>
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000F037 File Offset: 0x0000D237
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Copies the <see cref="T:System.Management.QualifierDataCollection" /> into an array.          </summary>
		/// <param name="array">The array to which to copy the <see cref="T:System.Management.QualifierDataCollection" />. </param>
		/// <param name="index">The index from which to start copying. </param>
		// Token: 0x060002C0 RID: 704 RVA: 0x0000FB30 File Offset: 0x0000DD30
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < array.GetLowerBound(0) || index > array.GetUpperBound(0))
			{
				throw new ArgumentOutOfRangeException("index");
			}
			string[] array2 = null;
			IWbemQualifierSetFreeThreaded typeQualifierSet;
			try
			{
				typeQualifierSet = this.GetTypeQualifierSet();
			}
			catch (ManagementException ex)
			{
				if (this.qualifierSetType == QualifierType.PropertyQualifier && ex.ErrorCode == ManagementStatus.SystemProperty)
				{
					return;
				}
				throw;
			}
			int names_ = typeQualifierSet.GetNames_(0, out array2);
			if (names_ < 0)
			{
				if (((long)names_ & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)names_);
				}
				else
				{
					Marshal.ThrowExceptionForHR(names_, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			if (index + array2.Length > array.Length)
			{
				throw new ArgumentException(null, "index");
			}
			foreach (string qualName in array2)
			{
				array.SetValue(new QualifierData(this.parent, this.propertyOrMethodName, qualName, this.qualifierSetType), index++);
			}
			return;
		}

		/// <summary>Copies the <see cref="T:System.Management.QualifierDataCollection" /> into a specialized              <see cref="T:System.Management.QualifierData" /> array.          </summary>
		/// <param name="qualifierArray">The specialized array of <see cref="T:System.Management.QualifierData" /> objects to which to copy the <see cref="T:System.Management.QualifierDataCollection" />.</param>
		/// <param name="index">The index from which to start copying. </param>
		// Token: 0x060002C1 RID: 705 RVA: 0x0000FC38 File Offset: 0x0000DE38
		public void CopyTo(QualifierData[] qualifierArray, int index)
		{
			this.CopyTo(qualifierArray, index);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> that iterates through the <see cref="T:System.Management.QualifierDataCollection" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Management.QualifierDataCollection" />.</returns>
		// Token: 0x060002C2 RID: 706 RVA: 0x0000FC42 File Offset: 0x0000DE42
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new QualifierDataCollection.QualifierDataEnumerator(this.parent, this.propertyOrMethodName, this.qualifierSetType);
		}

		/// <summary>Returns an enumerator for the <see cref="T:System.Management.QualifierDataCollection" />. This method is strongly typed.          </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x060002C3 RID: 707 RVA: 0x0000FC42 File Offset: 0x0000DE42
		public QualifierDataCollection.QualifierDataEnumerator GetEnumerator()
		{
			return new QualifierDataCollection.QualifierDataEnumerator(this.parent, this.propertyOrMethodName, this.qualifierSetType);
		}

		/// <summary>Gets the specified <see cref="T:System.Management.QualifierData" /> from the <see cref="T:System.Management.QualifierDataCollection" />.          </summary>
		/// <param name="qualifierName">The name of the <see cref="T:System.Management.QualifierData" /> to access in the <see cref="T:System.Management.QualifierDataCollection" />. </param>
		/// <returns>Returns a <see cref="T:System.Management.QualifierData" /> containing the data for a specified qualifier in the collection.</returns>
		// Token: 0x1700008C RID: 140
		public virtual QualifierData this[string qualifierName]
		{
			get
			{
				if (qualifierName == null)
				{
					throw new ArgumentNullException("qualifierName");
				}
				return new QualifierData(this.parent, this.propertyOrMethodName, qualifierName, this.qualifierSetType);
			}
		}

		/// <summary>Removes a <see cref="T:System.Management.QualifierData" /> from the <see cref="T:System.Management.QualifierDataCollection" /> by name.          </summary>
		/// <param name="qualifierName">The name of the <see cref="T:System.Management.QualifierData" /> to remove. </param>
		// Token: 0x060002C5 RID: 709 RVA: 0x0000FC84 File Offset: 0x0000DE84
		public virtual void Remove(string qualifierName)
		{
			int num = this.GetTypeQualifierSet().Delete_(qualifierName);
			if (num < 0)
			{
				if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		/// <summary>Adds a <see cref="T:System.Management.QualifierData" /> to the <see cref="T:System.Management.QualifierDataCollection" />. This overload specifies the qualifier name and value.          </summary>
		/// <param name="qualifierName">The name of the <see cref="T:System.Management.QualifierData" /> to be added to the <see cref="T:System.Management.QualifierDataCollection" />. </param>
		/// <param name="qualifierValue">The value for the new qualifier. </param>
		// Token: 0x060002C6 RID: 710 RVA: 0x0000FCCA File Offset: 0x0000DECA
		public virtual void Add(string qualifierName, object qualifierValue)
		{
			this.Add(qualifierName, qualifierValue, false, false, false, true);
		}

		/// <summary>Adds a <see cref="T:System.Management.QualifierData" /> to the <see cref="T:System.Management.QualifierDataCollection" />. This overload specifies all property values for a <see cref="T:System.Management.QualifierData" />.          </summary>
		/// <param name="qualifierName">The qualifier name. </param>
		/// <param name="qualifierValue">The qualifier value. </param>
		/// <param name="isAmended">
		///       <see langword="true" /> to specify that this qualifier is amended (<paramref name="flavor" />); otherwise, <see langword="false" />. </param>
		/// <param name="propagatesToInstance">
		///       <see langword="true" /> to propagate this qualifier to instances; otherwise, <see langword="false" />. </param>
		/// <param name="propagatesToSubclass">
		///       <see langword="true" /> to propagate this qualifier to subclasses; otherwise, <see langword="false" />. </param>
		/// <param name="isOverridable">
		///       <see langword="true" /> to specify that this qualifier's value is overridable in instances of subclasses; otherwise, <see langword="false" />. </param>
		// Token: 0x060002C7 RID: 711 RVA: 0x0000FCD8 File Offset: 0x0000DED8
		public virtual void Add(string qualifierName, object qualifierValue, bool isAmended, bool propagatesToInstance, bool propagatesToSubclass, bool isOverridable)
		{
			int num = 0;
			if (isAmended)
			{
				num |= 128;
			}
			if (propagatesToInstance)
			{
				num |= 1;
			}
			if (propagatesToSubclass)
			{
				num |= 2;
			}
			if (!isOverridable)
			{
				num |= 16;
			}
			int num2 = this.GetTypeQualifierSet().Put_(qualifierName, ref qualifierValue, num);
			if (num2 < 0)
			{
				if (((long)num2 & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x040001D5 RID: 469
		private ManagementBaseObject parent;

		// Token: 0x040001D6 RID: 470
		private string propertyOrMethodName;

		// Token: 0x040001D7 RID: 471
		private QualifierType qualifierSetType;

		/// <summary>Represents the enumerator for <see cref="T:System.Management.QualifierData" /> objects in the <see cref="T:System.Management.QualifierDataCollection" />. </summary>
		// Token: 0x020000FC RID: 252
		public class QualifierDataEnumerator : IEnumerator
		{
			// Token: 0x06000655 RID: 1621 RVA: 0x000271F0 File Offset: 0x000253F0
			internal QualifierDataEnumerator(ManagementBaseObject parent, string propertyOrMethodName, QualifierType qualifierType)
			{
				this.parent = parent;
				this.propertyOrMethodName = propertyOrMethodName;
				this.qualifierType = qualifierType;
				this.qualifierNames = null;
				IWbemQualifierSetFreeThreaded wbemQualifierSetFreeThreaded = null;
				int num;
				switch (qualifierType)
				{
				case QualifierType.ObjectQualifier:
					num = parent.wbemObject.GetQualifierSet_(out wbemQualifierSetFreeThreaded);
					break;
				case QualifierType.PropertyQualifier:
					num = parent.wbemObject.GetPropertyQualifierSet_(propertyOrMethodName, out wbemQualifierSetFreeThreaded);
					break;
				case QualifierType.MethodQualifier:
					num = parent.wbemObject.GetMethodQualifierSet_(propertyOrMethodName, out wbemQualifierSetFreeThreaded);
					break;
				default:
					throw new ManagementException(ManagementStatus.Unexpected, null, null);
				}
				if (num < 0)
				{
					this.qualifierNames = new string[0];
					return;
				}
				num = wbemQualifierSetFreeThreaded.GetNames_(0, out this.qualifierNames);
				if (num < 0)
				{
					if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
						return;
					}
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}

			/// <summary>Gets the current object in the collection.</summary>
			/// <returns>Returns the current element in the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
			// Token: 0x170000E2 RID: 226
			// (get) Token: 0x06000656 RID: 1622 RVA: 0x000272C8 File Offset: 0x000254C8
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			/// <summary>Gets or sets the current <see cref="T:System.Management.QualifierData" /> in the <see cref="T:System.Management.QualifierDataCollection" /> enumeration.</summary>
			/// <returns>The current <see cref="T:System.Management.QualifierData" /> element in the collection.</returns>
			// Token: 0x170000E3 RID: 227
			// (get) Token: 0x06000657 RID: 1623 RVA: 0x000272D0 File Offset: 0x000254D0
			public QualifierData Current
			{
				get
				{
					if (this.index == -1 || this.index == this.qualifierNames.Length)
					{
						throw new InvalidOperationException();
					}
					return new QualifierData(this.parent, this.propertyOrMethodName, this.qualifierNames[this.index], this.qualifierType);
				}
			}

			/// <summary>Moves to the next element in the <see cref="T:System.Management.QualifierDataCollection" /> enumeration.</summary>
			/// <returns>
			///     <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
			// Token: 0x06000658 RID: 1624 RVA: 0x00027320 File Offset: 0x00025520
			public bool MoveNext()
			{
				if (this.index == this.qualifierNames.Length)
				{
					return false;
				}
				this.index++;
				return this.index != this.qualifierNames.Length;
			}

			/// <summary>Resets the enumerator to the beginning of the <see cref="T:System.Management.QualifierDataCollection" /> enumeration.</summary>
			// Token: 0x06000659 RID: 1625 RVA: 0x00027355 File Offset: 0x00025555
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x0400054C RID: 1356
			private ManagementBaseObject parent;

			// Token: 0x0400054D RID: 1357
			private string propertyOrMethodName;

			// Token: 0x0400054E RID: 1358
			private QualifierType qualifierType;

			// Token: 0x0400054F RID: 1359
			private string[] qualifierNames;

			// Token: 0x04000550 RID: 1360
			private int index = -1;
		}
	}
}
