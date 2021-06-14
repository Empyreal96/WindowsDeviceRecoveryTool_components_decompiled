using System;
using System.Globalization;
using System.Linq;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200013C RID: 316
	public sealed class EntityProperty : IEquatable<EntityProperty>
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x0004E0D5 File Offset: 0x0004C2D5
		// (set) Token: 0x06001452 RID: 5202 RVA: 0x0004E0DD File Offset: 0x0004C2DD
		public object PropertyAsObject
		{
			get
			{
				return this.propertyAsObject;
			}
			internal set
			{
				this.IsNull = (value == null);
				this.propertyAsObject = value;
			}
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0004E0F0 File Offset: 0x0004C2F0
		public static EntityProperty GeneratePropertyForDateTimeOffset(DateTimeOffset? input)
		{
			return new EntityProperty(input);
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0004E0F8 File Offset: 0x0004C2F8
		public static EntityProperty GeneratePropertyForByteArray(byte[] input)
		{
			return new EntityProperty(input);
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0004E100 File Offset: 0x0004C300
		public static EntityProperty GeneratePropertyForBool(bool? input)
		{
			return new EntityProperty(input);
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0004E108 File Offset: 0x0004C308
		public static EntityProperty GeneratePropertyForDouble(double? input)
		{
			return new EntityProperty(input);
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0004E110 File Offset: 0x0004C310
		public static EntityProperty GeneratePropertyForGuid(Guid? input)
		{
			return new EntityProperty(input);
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0004E118 File Offset: 0x0004C318
		public static EntityProperty GeneratePropertyForInt(int? input)
		{
			return new EntityProperty(input);
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0004E120 File Offset: 0x0004C320
		public static EntityProperty GeneratePropertyForLong(long? input)
		{
			return new EntityProperty(input);
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0004E128 File Offset: 0x0004C328
		public static EntityProperty GeneratePropertyForString(string input)
		{
			return new EntityProperty(input);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0004E130 File Offset: 0x0004C330
		public EntityProperty(byte[] input) : this(EdmType.Binary)
		{
			this.PropertyAsObject = input;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0004E140 File Offset: 0x0004C340
		public EntityProperty(bool? input) : this(EdmType.Boolean)
		{
			this.IsNull = (input == null);
			this.PropertyAsObject = input;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0004E168 File Offset: 0x0004C368
		public EntityProperty(DateTimeOffset? input) : this(EdmType.DateTime)
		{
			if (input != null)
			{
				this.PropertyAsObject = input.Value.UtcDateTime;
				return;
			}
			this.IsNull = true;
			this.PropertyAsObject = null;
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0004E1BB File Offset: 0x0004C3BB
		public EntityProperty(DateTime? input) : this(EdmType.DateTime)
		{
			this.IsNull = (input == null);
			this.PropertyAsObject = input;
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0004E1E0 File Offset: 0x0004C3E0
		public EntityProperty(double? input) : this(EdmType.Double)
		{
			this.IsNull = (input == null);
			this.PropertyAsObject = input;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0004E205 File Offset: 0x0004C405
		public EntityProperty(Guid? input) : this(EdmType.Guid)
		{
			this.IsNull = (input == null);
			this.PropertyAsObject = input;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0004E22A File Offset: 0x0004C42A
		public EntityProperty(int? input) : this(EdmType.Int32)
		{
			this.IsNull = (input == null);
			this.PropertyAsObject = input;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0004E24F File Offset: 0x0004C44F
		public EntityProperty(long? input) : this(EdmType.Int64)
		{
			this.IsNull = (input == null);
			this.PropertyAsObject = input;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0004E274 File Offset: 0x0004C474
		public EntityProperty(string input) : this(EdmType.String)
		{
			this.PropertyAsObject = input;
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0004E284 File Offset: 0x0004C484
		private EntityProperty(EdmType propertyType)
		{
			this.PropertyType = propertyType;
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x0004E293 File Offset: 0x0004C493
		// (set) Token: 0x06001466 RID: 5222 RVA: 0x0004E29B File Offset: 0x0004C49B
		public EdmType PropertyType { get; private set; }

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001467 RID: 5223 RVA: 0x0004E2A4 File Offset: 0x0004C4A4
		// (set) Token: 0x06001468 RID: 5224 RVA: 0x0004E2C0 File Offset: 0x0004C4C0
		public byte[] BinaryValue
		{
			get
			{
				if (!this.IsNull)
				{
					this.EnforceType(EdmType.Binary);
				}
				return (byte[])this.PropertyAsObject;
			}
			set
			{
				if (value != null)
				{
					this.EnforceType(EdmType.Binary);
				}
				this.PropertyAsObject = value;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x0004E2D3 File Offset: 0x0004C4D3
		// (set) Token: 0x0600146A RID: 5226 RVA: 0x0004E2EF File Offset: 0x0004C4EF
		public bool? BooleanValue
		{
			get
			{
				if (!this.IsNull)
				{
					this.EnforceType(EdmType.Boolean);
				}
				return (bool?)this.PropertyAsObject;
			}
			set
			{
				if (value != null)
				{
					this.EnforceType(EdmType.Boolean);
				}
				this.PropertyAsObject = value;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600146B RID: 5227 RVA: 0x0004E30D File Offset: 0x0004C50D
		// (set) Token: 0x0600146C RID: 5228 RVA: 0x0004E329 File Offset: 0x0004C529
		public DateTime? DateTime
		{
			get
			{
				if (!this.IsNull)
				{
					this.EnforceType(EdmType.DateTime);
				}
				return (DateTime?)this.PropertyAsObject;
			}
			set
			{
				if (value != null)
				{
					this.EnforceType(EdmType.DateTime);
				}
				this.PropertyAsObject = value;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600146D RID: 5229 RVA: 0x0004E348 File Offset: 0x0004C548
		// (set) Token: 0x0600146E RID: 5230 RVA: 0x0004E38C File Offset: 0x0004C58C
		public DateTimeOffset? DateTimeOffsetValue
		{
			get
			{
				if (!this.IsNull)
				{
					this.EnforceType(EdmType.DateTime);
				}
				if (this.PropertyAsObject == null)
				{
					return null;
				}
				return new DateTimeOffset?(new DateTimeOffset((DateTime)this.PropertyAsObject));
			}
			set
			{
				if (value != null)
				{
					this.EnforceType(EdmType.DateTime);
					this.PropertyAsObject = value.Value.UtcDateTime;
					return;
				}
				this.PropertyAsObject = null;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x0600146F RID: 5231 RVA: 0x0004E3D8 File Offset: 0x0004C5D8
		// (set) Token: 0x06001470 RID: 5232 RVA: 0x0004E3F4 File Offset: 0x0004C5F4
		public double? DoubleValue
		{
			get
			{
				if (!this.IsNull)
				{
					this.EnforceType(EdmType.Double);
				}
				return (double?)this.PropertyAsObject;
			}
			set
			{
				if (value != null)
				{
					this.EnforceType(EdmType.Double);
				}
				this.PropertyAsObject = value;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001471 RID: 5233 RVA: 0x0004E412 File Offset: 0x0004C612
		// (set) Token: 0x06001472 RID: 5234 RVA: 0x0004E42E File Offset: 0x0004C62E
		public Guid? GuidValue
		{
			get
			{
				if (!this.IsNull)
				{
					this.EnforceType(EdmType.Guid);
				}
				return (Guid?)this.PropertyAsObject;
			}
			set
			{
				if (value != null)
				{
					this.EnforceType(EdmType.Guid);
				}
				this.PropertyAsObject = value;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001473 RID: 5235 RVA: 0x0004E44C File Offset: 0x0004C64C
		// (set) Token: 0x06001474 RID: 5236 RVA: 0x0004E468 File Offset: 0x0004C668
		public int? Int32Value
		{
			get
			{
				if (!this.IsNull)
				{
					this.EnforceType(EdmType.Int32);
				}
				return (int?)this.PropertyAsObject;
			}
			set
			{
				if (value != null)
				{
					this.EnforceType(EdmType.Int32);
				}
				this.PropertyAsObject = value;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001475 RID: 5237 RVA: 0x0004E486 File Offset: 0x0004C686
		// (set) Token: 0x06001476 RID: 5238 RVA: 0x0004E4A2 File Offset: 0x0004C6A2
		public long? Int64Value
		{
			get
			{
				if (!this.IsNull)
				{
					this.EnforceType(EdmType.Int64);
				}
				return (long?)this.PropertyAsObject;
			}
			set
			{
				if (value != null)
				{
					this.EnforceType(EdmType.Int64);
				}
				this.PropertyAsObject = value;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001477 RID: 5239 RVA: 0x0004E4C0 File Offset: 0x0004C6C0
		// (set) Token: 0x06001478 RID: 5240 RVA: 0x0004E4DC File Offset: 0x0004C6DC
		public string StringValue
		{
			get
			{
				if (!this.IsNull)
				{
					this.EnforceType(EdmType.String);
				}
				return (string)this.PropertyAsObject;
			}
			set
			{
				if (value != null)
				{
					this.EnforceType(EdmType.String);
				}
				this.PropertyAsObject = value;
			}
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0004E4F0 File Offset: 0x0004C6F0
		public override bool Equals(object obj)
		{
			EntityProperty other = obj as EntityProperty;
			return this.Equals(other);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0004E50C File Offset: 0x0004C70C
		public bool Equals(EntityProperty other)
		{
			if (other == null)
			{
				return false;
			}
			if (this.PropertyType != other.PropertyType)
			{
				return false;
			}
			if (this.IsNull && other.IsNull)
			{
				return true;
			}
			switch (this.PropertyType)
			{
			case EdmType.String:
				return string.Equals(this.StringValue, other.StringValue, StringComparison.Ordinal);
			case EdmType.Binary:
				return this.BinaryValue.Length == other.BinaryValue.Length && this.BinaryValue.SequenceEqual(other.BinaryValue);
			case EdmType.Boolean:
				return this.BooleanValue == other.BooleanValue;
			case EdmType.DateTime:
				return this.DateTime == other.DateTime;
			case EdmType.Double:
			{
				double? doubleValue = this.DoubleValue;
				double? doubleValue2 = other.DoubleValue;
				return doubleValue.GetValueOrDefault() == doubleValue2.GetValueOrDefault() && doubleValue != null == (doubleValue2 != null);
			}
			case EdmType.Guid:
				return this.GuidValue == other.GuidValue;
			case EdmType.Int32:
				return this.Int32Value == other.Int32Value;
			case EdmType.Int64:
				return this.Int64Value == other.Int64Value;
			default:
				return this.PropertyAsObject.Equals(other.PropertyAsObject);
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0004E70C File Offset: 0x0004C90C
		public override int GetHashCode()
		{
			int num;
			if (this.PropertyAsObject == null)
			{
				num = 0;
			}
			else if (this.PropertyType == EdmType.Binary)
			{
				num = 0;
				byte[] array = (byte[])this.PropertyAsObject;
				if (array.Length != 0)
				{
					for (int i = 0; i < Math.Min(array.Length - 4, 1024); i += 4)
					{
						num ^= BitConverter.ToInt32(array, i);
					}
				}
			}
			else
			{
				num = this.PropertyAsObject.GetHashCode();
			}
			return num ^ this.PropertyType.GetHashCode() ^ this.IsNull.GetHashCode();
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x0004E794 File Offset: 0x0004C994
		// (set) Token: 0x0600147D RID: 5245 RVA: 0x0004E79C File Offset: 0x0004C99C
		internal bool IsNull { get; set; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x0004E7A5 File Offset: 0x0004C9A5
		// (set) Token: 0x0600147F RID: 5247 RVA: 0x0004E7AD File Offset: 0x0004C9AD
		internal bool IsEncrypted { get; set; }

		// Token: 0x06001480 RID: 5248 RVA: 0x0004E7B6 File Offset: 0x0004C9B6
		public static EntityProperty CreateEntityPropertyFromObject(object entityValue)
		{
			return EntityProperty.CreateEntityPropertyFromObject(entityValue, true);
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0004E7C0 File Offset: 0x0004C9C0
		internal static EntityProperty CreateEntityPropertyFromObject(object value, bool allowUnknownTypes)
		{
			if (value is string)
			{
				return new EntityProperty((string)value);
			}
			if (value is byte[])
			{
				return new EntityProperty((byte[])value);
			}
			if (value is bool)
			{
				return new EntityProperty(new bool?((bool)value));
			}
			if (value is bool?)
			{
				return new EntityProperty((bool?)value);
			}
			if (value is DateTime)
			{
				return new EntityProperty(new DateTime?((DateTime)value));
			}
			if (value is DateTime?)
			{
				return new EntityProperty((DateTime?)value);
			}
			if (value is DateTimeOffset)
			{
				return new EntityProperty(new DateTimeOffset?((DateTimeOffset)value));
			}
			if (value is DateTimeOffset?)
			{
				return new EntityProperty((DateTimeOffset?)value);
			}
			if (value is double)
			{
				return new EntityProperty(new double?((double)value));
			}
			if (value is double?)
			{
				return new EntityProperty((double?)value);
			}
			if (value is Guid?)
			{
				return new EntityProperty((Guid?)value);
			}
			if (value is Guid)
			{
				return new EntityProperty(new Guid?((Guid)value));
			}
			if (value is int)
			{
				return new EntityProperty(new int?((int)value));
			}
			if (value is int?)
			{
				return new EntityProperty((int?)value);
			}
			if (value is long)
			{
				return new EntityProperty(new long?((long)value));
			}
			if (value is long?)
			{
				return new EntityProperty((long?)value);
			}
			if (value == null)
			{
				return new EntityProperty(null);
			}
			if (allowUnknownTypes)
			{
				return new EntityProperty(value.ToString());
			}
			return null;
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0004E94C File Offset: 0x0004CB4C
		internal static EntityProperty CreateEntityPropertyFromObject(object value, Type type)
		{
			if (type == typeof(string))
			{
				return new EntityProperty((string)value);
			}
			if (type == typeof(byte[]))
			{
				return new EntityProperty((byte[])value);
			}
			if (type == typeof(bool))
			{
				return new EntityProperty(new bool?((bool)value));
			}
			if (type == typeof(bool?))
			{
				return new EntityProperty((bool?)value);
			}
			if (type == typeof(DateTime))
			{
				return new EntityProperty(new DateTime?((DateTime)value));
			}
			if (type == typeof(DateTime?))
			{
				return new EntityProperty((DateTime?)value);
			}
			if (type == typeof(DateTimeOffset))
			{
				return new EntityProperty(new DateTimeOffset?((DateTimeOffset)value));
			}
			if (type == typeof(DateTimeOffset?))
			{
				return new EntityProperty((DateTimeOffset?)value);
			}
			if (type == typeof(double))
			{
				return new EntityProperty(new double?((double)value));
			}
			if (type == typeof(double?))
			{
				return new EntityProperty((double?)value);
			}
			if (type == typeof(Guid?))
			{
				return new EntityProperty((Guid?)value);
			}
			if (type == typeof(Guid))
			{
				return new EntityProperty(new Guid?((Guid)value));
			}
			if (type == typeof(int))
			{
				return new EntityProperty(new int?((int)value));
			}
			if (type == typeof(int?))
			{
				return new EntityProperty((int?)value);
			}
			if (type == typeof(long))
			{
				return new EntityProperty(new long?((long)value));
			}
			if (type == typeof(long?))
			{
				return new EntityProperty((long?)value);
			}
			return null;
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0004EB60 File Offset: 0x0004CD60
		internal static EntityProperty CreateEntityPropertyFromObject(object value, EdmType type)
		{
			if (type == EdmType.String)
			{
				return new EntityProperty((string)value);
			}
			if (type == EdmType.Binary)
			{
				return new EntityProperty(Convert.FromBase64String((string)value));
			}
			if (type == EdmType.Boolean)
			{
				return new EntityProperty(new bool?(bool.Parse((string)value)));
			}
			if (type == EdmType.DateTime)
			{
				return new EntityProperty(new DateTimeOffset?(DateTimeOffset.Parse((string)value, CultureInfo.InvariantCulture)));
			}
			if (type == EdmType.Double)
			{
				return new EntityProperty(new double?(double.Parse((string)value, CultureInfo.InvariantCulture)));
			}
			if (type == EdmType.Guid)
			{
				return new EntityProperty(new Guid?(Guid.Parse((string)value)));
			}
			if (type == EdmType.Int32)
			{
				return new EntityProperty(new int?(int.Parse((string)value, CultureInfo.InvariantCulture)));
			}
			if (type == EdmType.Int64)
			{
				return new EntityProperty(new long?(long.Parse((string)value, CultureInfo.InvariantCulture)));
			}
			return null;
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0004EC44 File Offset: 0x0004CE44
		private void EnforceType(EdmType requestedType)
		{
			if (this.PropertyType != requestedType)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot return {0} type for a {1} typed property.", new object[]
				{
					requestedType,
					this.PropertyType
				}));
			}
		}

		// Token: 0x040007E8 RID: 2024
		private object propertyAsObject;
	}
}
