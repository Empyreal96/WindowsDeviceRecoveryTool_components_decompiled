using System;

namespace System.Windows.Markup.Localizer
{
	/// <summary>Represents a key that is used to identify localizable resources in a <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />.</summary>
	// Token: 0x0200028F RID: 655
	public class BamlLocalizableResourceKey
	{
		// Token: 0x060024DB RID: 9435 RVA: 0x000B2A18 File Offset: 0x000B0C18
		internal BamlLocalizableResourceKey(string uid, string className, string propertyName, string assemblyName)
		{
			if (uid == null)
			{
				throw new ArgumentNullException("uid");
			}
			if (className == null)
			{
				throw new ArgumentNullException("className");
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			this._uid = uid;
			this._className = className;
			this._propertyName = propertyName;
			this._assemblyName = assemblyName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResourceKey" /> class with the supplied Uid, class name, and property name.</summary>
		/// <param name="uid">The Uid of an element that has a localizable resource.</param>
		/// <param name="className">The class name of a localizable resource in binary XAML (BAML).</param>
		/// <param name="propertyName">The property name of a localizable resource in BAML.</param>
		// Token: 0x060024DC RID: 9436 RVA: 0x000B2A72 File Offset: 0x000B0C72
		public BamlLocalizableResourceKey(string uid, string className, string propertyName) : this(uid, className, propertyName, null)
		{
		}

		/// <summary>Gets the Uid component of this <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResourceKey" />.</summary>
		/// <returns>The Uid component of this <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResourceKey" />.</returns>
		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x060024DD RID: 9437 RVA: 0x000B2A7E File Offset: 0x000B0C7E
		public string Uid
		{
			get
			{
				return this._uid;
			}
		}

		/// <summary>Gets the class name component of this <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResourceKey" />.</summary>
		/// <returns>The class name component of this <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResourceKey" />.</returns>
		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x060024DE RID: 9438 RVA: 0x000B2A86 File Offset: 0x000B0C86
		public string ClassName
		{
			get
			{
				return this._className;
			}
		}

		/// <summary>Gets the property name component of this <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResourceKey" />.</summary>
		/// <returns>The property name component of this <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResourceKey" />.</returns>
		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x060024DF RID: 9439 RVA: 0x000B2A8E File Offset: 0x000B0C8E
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		/// <summary>Gets the name of the assembly that defines the type of the localizable resource as declared by its <see cref="P:System.Windows.Markup.Localizer.BamlLocalizableResourceKey.ClassName" />.</summary>
		/// <returns>The name of the assembly.</returns>
		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x060024E0 RID: 9440 RVA: 0x000B2A96 File Offset: 0x000B0C96
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		/// <summary>Compares two instances of <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResourceKey" /> for equality.</summary>
		/// <param name="other">The other instance of <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResourceKey" /> to compare for equality.</param>
		/// <returns>
		///     <see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060024E1 RID: 9441 RVA: 0x000B2A9E File Offset: 0x000B0C9E
		public bool Equals(BamlLocalizableResourceKey other)
		{
			return other != null && (this._uid == other._uid && this._className == other._className) && this._propertyName == other._propertyName;
		}

		/// <summary>Compares an object to an instance of <see cref="T:System.Windows.Markup.Localizer.BamlLocalizableResourceKey" /> for equality.</summary>
		/// <param name="other">The object to compare for equality.</param>
		/// <returns>
		///     <see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060024E2 RID: 9442 RVA: 0x000B2ADE File Offset: 0x000B0CDE
		public override bool Equals(object other)
		{
			return this.Equals(other as BamlLocalizableResourceKey);
		}

		/// <summary>Returns an integer hash code representing this instance.</summary>
		/// <returns>An integer hash code.</returns>
		// Token: 0x060024E3 RID: 9443 RVA: 0x000B2AEC File Offset: 0x000B0CEC
		public override int GetHashCode()
		{
			return this._uid.GetHashCode() ^ this._className.GetHashCode() ^ this._propertyName.GetHashCode();
		}

		// Token: 0x04001B4C RID: 6988
		private string _uid;

		// Token: 0x04001B4D RID: 6989
		private string _className;

		// Token: 0x04001B4E RID: 6990
		private string _propertyName;

		// Token: 0x04001B4F RID: 6991
		private string _assemblyName;
	}
}
