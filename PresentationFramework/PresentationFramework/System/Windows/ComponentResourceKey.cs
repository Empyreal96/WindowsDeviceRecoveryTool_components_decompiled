using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Defines or references resource keys based on class names in external assemblies, as well as an additional identifier.</summary>
	// Token: 0x020000A2 RID: 162
	[TypeConverter(typeof(ComponentResourceKeyConverter))]
	public class ComponentResourceKey : ResourceKey
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.ComponentResourceKey" /> class.</summary>
		// Token: 0x06000341 RID: 833 RVA: 0x00009370 File Offset: 0x00007570
		public ComponentResourceKey()
		{
		}

		/// <summary>Initializes a new instance of a <see cref="T:System.Windows.ComponentResourceKey" /> , specifying the <see cref="T:System.Type" /> that defines the key, and an object to use as an additional resource identifier.</summary>
		/// <param name="typeInTargetAssembly">The type that defines the resource key.</param>
		/// <param name="resourceId">A unique identifier to differentiate this <see cref="T:System.Windows.ComponentResourceKey" /> from others associated with the <paramref name="typeInTargetAssembly" /> type.</param>
		// Token: 0x06000342 RID: 834 RVA: 0x00009378 File Offset: 0x00007578
		public ComponentResourceKey(Type typeInTargetAssembly, object resourceId)
		{
			if (typeInTargetAssembly == null)
			{
				throw new ArgumentNullException("typeInTargetAssembly");
			}
			if (resourceId == null)
			{
				throw new ArgumentNullException("resourceId");
			}
			this._typeInTargetAssembly = typeInTargetAssembly;
			this._typeInTargetAssemblyInitialized = true;
			this._resourceId = resourceId;
			this._resourceIdInitialized = true;
		}

		/// <summary>Gets or sets the <see cref="T:System.Type" /> that defines the resource key.</summary>
		/// <returns>The type that defines the resource key.</returns>
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000343 RID: 835 RVA: 0x000093C9 File Offset: 0x000075C9
		// (set) Token: 0x06000344 RID: 836 RVA: 0x000093D1 File Offset: 0x000075D1
		public Type TypeInTargetAssembly
		{
			get
			{
				return this._typeInTargetAssembly;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._typeInTargetAssemblyInitialized)
				{
					throw new InvalidOperationException(SR.Get("ChangingTypeNotAllowed"));
				}
				this._typeInTargetAssembly = value;
				this._typeInTargetAssemblyInitialized = true;
			}
		}

		/// <summary>Gets the assembly object that indicates which assembly's dictionary to look in for the value associated with this key.</summary>
		/// <returns>The retrieved assembly, as a reflection class.</returns>
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000940D File Offset: 0x0000760D
		public override Assembly Assembly
		{
			get
			{
				if (!(this._typeInTargetAssembly != null))
				{
					return null;
				}
				return this._typeInTargetAssembly.Assembly;
			}
		}

		/// <summary>Gets or sets a unique identifier to differentiate this key from others associated with this type.</summary>
		/// <returns>A unique identifier. Typically this is a string.</returns>
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000942A File Offset: 0x0000762A
		// (set) Token: 0x06000347 RID: 839 RVA: 0x00009432 File Offset: 0x00007632
		public object ResourceId
		{
			get
			{
				return this._resourceId;
			}
			set
			{
				if (this._resourceIdInitialized)
				{
					throw new InvalidOperationException(SR.Get("ChangingIdNotAllowed"));
				}
				this._resourceId = value;
				this._resourceIdInitialized = true;
			}
		}

		/// <summary>Determines whether the provided object is equal to the current <see cref="T:System.Windows.ComponentResourceKey" />. </summary>
		/// <param name="o">Object to compare with the current <see cref="T:System.Windows.ComponentResourceKey" />.</param>
		/// <returns>
		///     <see langword="true" /> if the objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000348 RID: 840 RVA: 0x0000945C File Offset: 0x0000765C
		public override bool Equals(object o)
		{
			ComponentResourceKey componentResourceKey = o as ComponentResourceKey;
			if (componentResourceKey == null)
			{
				return false;
			}
			if (!((componentResourceKey._typeInTargetAssembly != null) ? componentResourceKey._typeInTargetAssembly.Equals(this._typeInTargetAssembly) : (this._typeInTargetAssembly == null)))
			{
				return false;
			}
			if (componentResourceKey._resourceId == null)
			{
				return this._resourceId == null;
			}
			return componentResourceKey._resourceId.Equals(this._resourceId);
		}

		/// <summary>Returns a hash code for this <see cref="T:System.Windows.ComponentResourceKey" />. </summary>
		/// <returns>A signed 32-bit integer value.</returns>
		// Token: 0x06000349 RID: 841 RVA: 0x000094C9 File Offset: 0x000076C9
		public override int GetHashCode()
		{
			return ((this._typeInTargetAssembly != null) ? this._typeInTargetAssembly.GetHashCode() : 0) ^ ((this._resourceId != null) ? this._resourceId.GetHashCode() : 0);
		}

		/// <summary>Gets the string representation of a <see cref="T:System.Windows.ComponentResourceKey" />. </summary>
		/// <returns>The string representation.</returns>
		// Token: 0x0600034A RID: 842 RVA: 0x00009500 File Offset: 0x00007700
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			stringBuilder.Append("TargetType=");
			stringBuilder.Append((this._typeInTargetAssembly != null) ? this._typeInTargetAssembly.FullName : "null");
			stringBuilder.Append(" ID=");
			stringBuilder.Append((this._resourceId != null) ? this._resourceId.ToString() : "null");
			return stringBuilder.ToString();
		}

		// Token: 0x040005D9 RID: 1497
		private Type _typeInTargetAssembly;

		// Token: 0x040005DA RID: 1498
		private bool _typeInTargetAssemblyInitialized;

		// Token: 0x040005DB RID: 1499
		private object _resourceId;

		// Token: 0x040005DC RID: 1500
		private bool _resourceIdInitialized;
	}
}
