using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Management
{
	/// <summary>Represents the set of properties of a WMI object.</summary>
	// Token: 0x02000049 RID: 73
	public class PropertyDataCollection : ICollection, IEnumerable
	{
		// Token: 0x0600029D RID: 669 RVA: 0x0000EFB2 File Offset: 0x0000D1B2
		internal PropertyDataCollection(ManagementBaseObject parent, bool isSystem)
		{
			this.parent = parent;
			this.isSystem = isSystem;
		}

		/// <summary>Gets the number of objects in the <see cref="T:System.Management.PropertyDataCollection" />.          </summary>
		/// <returns>Returns an <see cref="T:System.Int32" /> value representing the number of objects in the collection.</returns>
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600029E RID: 670 RVA: 0x0000EFC8 File Offset: 0x0000D1C8
		public int Count
		{
			get
			{
				string[] array = null;
				object obj = null;
				int num;
				if (this.isSystem)
				{
					num = 48;
				}
				else
				{
					num = 64;
				}
				num |= 0;
				int names_ = this.parent.wbemObject.GetNames_(null, num, ref obj, out array);
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
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000F034 File Offset: 0x0000D234
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the object to be used for synchronization.          </summary>
		/// <returns>Returns an <see cref="T:System.Object" /> value containing the object to be used for synchronization.</returns>
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000F037 File Offset: 0x0000D237
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Copies the <see cref="T:System.Management.PropertyDataCollection" /> into an array.          </summary>
		/// <param name="array">The array to which to copy the <see cref="T:System.Management.PropertyDataCollection" />. </param>
		/// <param name="index">The index from which to start copying. </param>
		// Token: 0x060002A1 RID: 673 RVA: 0x0000F03C File Offset: 0x0000D23C
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
			object obj = null;
			int num = 0;
			if (this.isSystem)
			{
				num |= 48;
			}
			else
			{
				num |= 64;
			}
			num |= 0;
			int names_ = this.parent.wbemObject.GetNames_(null, num, ref obj, out array2);
			if (names_ >= 0)
			{
				if (index + array2.Length > array.Length)
				{
					throw new ArgumentException(null, "index");
				}
				foreach (string propName in array2)
				{
					array.SetValue(new PropertyData(this.parent, propName), index++);
				}
			}
			if (names_ < 0)
			{
				if (((long)names_ & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)names_);
					return;
				}
				Marshal.ThrowExceptionForHR(names_, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		/// <summary>Copies the <see cref="T:System.Management.PropertyDataCollection" /> to a specialized <see cref="T:System.Management.PropertyData" /> object array.          </summary>
		/// <param name="propertyArray">The destination array to contain the copied <see cref="T:System.Management.PropertyDataCollection" />.</param>
		/// <param name="index">The index in the destination array from which to start copying. </param>
		// Token: 0x060002A2 RID: 674 RVA: 0x0000F12A File Offset: 0x0000D32A
		public void CopyTo(PropertyData[] propertyArray, int index)
		{
			this.CopyTo(propertyArray, index);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> that iterates through the <see cref="T:System.Management.PropertyDataCollection" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Management.PropertyDataCollection" />.</returns>
		// Token: 0x060002A3 RID: 675 RVA: 0x0000F134 File Offset: 0x0000D334
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new PropertyDataCollection.PropertyDataEnumerator(this.parent, this.isSystem);
		}

		/// <summary>Returns the enumerator for this <see cref="T:System.Management.PropertyDataCollection" />.          </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x060002A4 RID: 676 RVA: 0x0000F134 File Offset: 0x0000D334
		public PropertyDataCollection.PropertyDataEnumerator GetEnumerator()
		{
			return new PropertyDataCollection.PropertyDataEnumerator(this.parent, this.isSystem);
		}

		/// <summary>Gets the specified property from the <see cref="T:System.Management.PropertyDataCollection" />, using [] syntax. This property is the indexer for the <see cref="T:System.Management.PropertyDataCollection" /> class.</summary>
		/// <param name="propertyName">The name of the property to retrieve.</param>
		/// <returns>Returns a <see cref="T:System.Management.PropertyData" /> containing the data for a specified property in the collection.</returns>
		// Token: 0x17000081 RID: 129
		public virtual PropertyData this[string propertyName]
		{
			get
			{
				if (propertyName == null)
				{
					throw new ArgumentNullException("propertyName");
				}
				return new PropertyData(this.parent, propertyName);
			}
		}

		/// <summary>Removes a <see cref="T:System.Management.PropertyData" /> from the <see cref="T:System.Management.PropertyDataCollection" />.          </summary>
		/// <param name="propertyName">The name of the property to be removed.</param>
		// Token: 0x060002A6 RID: 678 RVA: 0x0000F164 File Offset: 0x0000D364
		public virtual void Remove(string propertyName)
		{
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				ManagementClass managementClass = new ManagementClass(this.parent.ClassPath);
				this.parent.SetPropertyValue(propertyName, managementClass.GetPropertyValue(propertyName));
				return;
			}
			int num = this.parent.wbemObject.Delete_(propertyName);
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

		/// <summary>Adds a new <see cref="T:System.Management.PropertyData" /> with the specified value. The value cannot be null and must be convertible to a Common Information Model (CIM) type.          </summary>
		/// <param name="propertyName">The name of the new property.</param>
		/// <param name="propertyValue">The value of the property (cannot be null).</param>
		// Token: 0x060002A7 RID: 679 RVA: 0x0000F1F0 File Offset: 0x0000D3F0
		public virtual void Add(string propertyName, object propertyValue)
		{
			if (propertyValue == null)
			{
				throw new ArgumentNullException("propertyValue");
			}
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				throw new InvalidOperationException();
			}
			CimType cimType = CimType.None;
			bool flag = false;
			object obj = PropertyData.MapValueToWmiValue(propertyValue, out flag, out cimType);
			int num = (int)cimType;
			if (flag)
			{
				num |= 8192;
			}
			int num2 = this.parent.wbemObject.Put_(propertyName, 0, ref obj, num);
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

		/// <summary>Adds a new <see cref="T:System.Management.PropertyData" /> with the specified value and Common Information Model (CIM) type.          </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="propertyValue">The value of the property (which can be null).</param>
		/// <param name="propertyType">The CIM type of the property.</param>
		// Token: 0x060002A8 RID: 680 RVA: 0x0000F290 File Offset: 0x0000D490
		public void Add(string propertyName, object propertyValue, CimType propertyType)
		{
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				throw new InvalidOperationException();
			}
			int num = (int)propertyType;
			bool isArray = false;
			if (propertyValue != null && propertyValue.GetType().IsArray)
			{
				isArray = true;
				num |= 8192;
			}
			object obj = PropertyData.MapValueToWmiValue(propertyValue, propertyType, isArray);
			int num2 = this.parent.wbemObject.Put_(propertyName, 0, ref obj, num);
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

		/// <summary>Adds a new <see cref="T:System.Management.PropertyData" /> with no assigned value.          </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="propertyType">The Common Information Model (CIM) type of the property.</param>
		/// <param name="isArray">
		///       <see langword="true" /> to specify that the property is an array type; otherwise, <see langword="false" />.</param>
		// Token: 0x060002A9 RID: 681 RVA: 0x0000F338 File Offset: 0x0000D538
		public void Add(string propertyName, CimType propertyType, bool isArray)
		{
			if (propertyName == null)
			{
				throw new ArgumentNullException(propertyName);
			}
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				throw new InvalidOperationException();
			}
			int num = (int)propertyType;
			if (isArray)
			{
				num |= 8192;
			}
			object value = DBNull.Value;
			int num2 = this.parent.wbemObject.Put_(propertyName, 0, ref value, num);
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

		// Token: 0x040001CC RID: 460
		private ManagementBaseObject parent;

		// Token: 0x040001CD RID: 461
		private bool isSystem;

		/// <summary>Represents the enumerator for <see cref="T:System.Management.PropertyData" /> objects in the <see cref="T:System.Management.PropertyDataCollection" />. </summary>
		// Token: 0x020000FB RID: 251
		public class PropertyDataEnumerator : IEnumerator
		{
			// Token: 0x06000650 RID: 1616 RVA: 0x000270F4 File Offset: 0x000252F4
			internal PropertyDataEnumerator(ManagementBaseObject parent, bool isSystem)
			{
				this.parent = parent;
				this.propertyNames = null;
				this.index = -1;
				object obj = null;
				int num;
				if (isSystem)
				{
					num = 48;
				}
				else
				{
					num = 64;
				}
				num |= 0;
				int names_ = parent.wbemObject.GetNames_(null, num, ref obj, out this.propertyNames);
				if (names_ < 0)
				{
					if (((long)names_ & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)names_);
						return;
					}
					Marshal.ThrowExceptionForHR(names_, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}

			/// <summary>Gets the current object in the collection.</summary>
			/// <returns>Returns the current element in the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
			// Token: 0x170000E0 RID: 224
			// (get) Token: 0x06000651 RID: 1617 RVA: 0x0002716F File Offset: 0x0002536F
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			/// <summary>Gets the current <see cref="T:System.Management.PropertyData" /> in the <see cref="T:System.Management.PropertyDataCollection" /> enumeration.</summary>
			/// <returns>The current <see cref="T:System.Management.PropertyData" /> element in the collection.</returns>
			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x06000652 RID: 1618 RVA: 0x00027177 File Offset: 0x00025377
			public PropertyData Current
			{
				get
				{
					if (this.index == -1 || this.index == this.propertyNames.Length)
					{
						throw new InvalidOperationException();
					}
					return new PropertyData(this.parent, this.propertyNames[this.index]);
				}
			}

			/// <summary>Moves to the next element in the <see cref="T:System.Management.PropertyDataCollection" /> enumeration.</summary>
			/// <returns>
			///     <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
			// Token: 0x06000653 RID: 1619 RVA: 0x000271B0 File Offset: 0x000253B0
			public bool MoveNext()
			{
				if (this.index == this.propertyNames.Length)
				{
					return false;
				}
				this.index++;
				return this.index != this.propertyNames.Length;
			}

			/// <summary>Resets the enumerator to the beginning of the <see cref="T:System.Management.PropertyDataCollection" /> enumeration.</summary>
			// Token: 0x06000654 RID: 1620 RVA: 0x000271E5 File Offset: 0x000253E5
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x04000549 RID: 1353
			private ManagementBaseObject parent;

			// Token: 0x0400054A RID: 1354
			private string[] propertyNames;

			// Token: 0x0400054B RID: 1355
			private int index;
		}
	}
}
