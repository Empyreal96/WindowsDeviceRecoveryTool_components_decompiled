using System;

namespace System.Management
{
	/// <summary>Represents a WQL ASSOCIATORS OF data query. It can be used for both instances and schema queries.           </summary>
	// Token: 0x0200003A RID: 58
	public class RelatedObjectQuery : WqlObjectQuery
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Management.RelatedObjectQuery" /> class. This is the default constructor.          </summary>
		// Token: 0x060001ED RID: 493 RVA: 0x0000A302 File Offset: 0x00008502
		public RelatedObjectQuery() : this(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.RelatedObjectQuery" /> class. If the specified string can be successfully parsed as a WQL query, it is considered to be the query string; otherwise, it is assumed to be the path of the source object for the query. In this case, the query is assumed to be an instance query.           </summary>
		/// <param name="queryOrSourceObject">The query string or the path of the source object.</param>
		// Token: 0x060001EE RID: 494 RVA: 0x0000A30C File Offset: 0x0000850C
		public RelatedObjectQuery(string queryOrSourceObject)
		{
			if (queryOrSourceObject == null)
			{
				return;
			}
			if (queryOrSourceObject.TrimStart(new char[0]).StartsWith(RelatedObjectQuery.tokenAssociators, StringComparison.OrdinalIgnoreCase))
			{
				this.QueryString = queryOrSourceObject;
				return;
			}
			ManagementPath managementPath = new ManagementPath(queryOrSourceObject);
			if ((managementPath.IsClass || managementPath.IsInstance) && managementPath.NamespacePath.Length == 0)
			{
				this.SourceObject = queryOrSourceObject;
				this.isSchemaQuery = false;
				return;
			}
			throw new ArgumentException(RC.GetString("INVALID_QUERY"), "queryOrSourceObject");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.RelatedObjectQuery" /> class for the given source object and related class. The query is assumed to be an instance query (as opposed to a schema query).          </summary>
		/// <param name="sourceObject">The path of the source object for this query.</param>
		/// <param name="relatedClass">The related objects' class.</param>
		// Token: 0x060001EF RID: 495 RVA: 0x0000A38C File Offset: 0x0000858C
		public RelatedObjectQuery(string sourceObject, string relatedClass) : this(sourceObject, relatedClass, null, null, null, null, null, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.RelatedObjectQuery" /> class for the given set of parameters. The query is assumed to be an instance query (as opposed to a schema query).          </summary>
		/// <param name="sourceObject">The path of the source object.</param>
		/// <param name="relatedClass">The related objects' required class.</param>
		/// <param name="relationshipClass">The relationship type.</param>
		/// <param name="relatedQualifier">The qualifier required to be present on the related objects.</param>
		/// <param name="relationshipQualifier">The qualifier required to be present on the relationships.</param>
		/// <param name="relatedRole">The role that the related objects are required to play in the relationship.</param>
		/// <param name="thisRole">The role that the source object is required to play in the relationship.</param>
		/// <param name="classDefinitionsOnly">
		///       <see langword="true" /> to return only the class definitions of the related objects; otherwise, false .</param>
		// Token: 0x060001F0 RID: 496 RVA: 0x0000A3A8 File Offset: 0x000085A8
		public RelatedObjectQuery(string sourceObject, string relatedClass, string relationshipClass, string relatedQualifier, string relationshipQualifier, string relatedRole, string thisRole, bool classDefinitionsOnly)
		{
			this.isSchemaQuery = false;
			this.sourceObject = sourceObject;
			this.relatedClass = relatedClass;
			this.relationshipClass = relationshipClass;
			this.relatedQualifier = relatedQualifier;
			this.relationshipQualifier = relationshipQualifier;
			this.relatedRole = relatedRole;
			this.thisRole = thisRole;
			this.classDefinitionsOnly = classDefinitionsOnly;
			this.BuildQuery();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.RelatedObjectQuery" /> class for a schema query using the given set of parameters. This constructor is used for schema queries only: the first parameter must be set to <see langword="true" />                .          </summary>
		/// <param name="isSchemaQuery">
		///       <see langword="true" /> to indicate that this is a schema query; otherwise, <see langword="false" /> .</param>
		/// <param name="sourceObject">The path of the source class.</param>
		/// <param name="relatedClass">The related objects' required base class.</param>
		/// <param name="relationshipClass">The relationship type.</param>
		/// <param name="relatedQualifier">The qualifier required to be present on the related objects.</param>
		/// <param name="relationshipQualifier">The qualifier required to be present on the relationships.</param>
		/// <param name="relatedRole">The role that the related objects are required to play in the relationship.</param>
		/// <param name="thisRole">The role that the source class is required to play in the relationship.</param>
		// Token: 0x060001F1 RID: 497 RVA: 0x0000A408 File Offset: 0x00008608
		public RelatedObjectQuery(bool isSchemaQuery, string sourceObject, string relatedClass, string relationshipClass, string relatedQualifier, string relationshipQualifier, string relatedRole, string thisRole)
		{
			if (!isSchemaQuery)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "isSchemaQuery");
			}
			this.isSchemaQuery = true;
			this.sourceObject = sourceObject;
			this.relatedClass = relatedClass;
			this.relationshipClass = relationshipClass;
			this.relatedQualifier = relatedQualifier;
			this.relationshipQualifier = relationshipQualifier;
			this.relatedRole = relatedRole;
			this.thisRole = thisRole;
			this.classDefinitionsOnly = false;
			this.BuildQuery();
		}

		/// <summary>Gets or sets a value indicating whether this is a schema query or an instance query.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether this is a schema query.</returns>
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000A47D File Offset: 0x0000867D
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000A485 File Offset: 0x00008685
		public bool IsSchemaQuery
		{
			get
			{
				return this.isSchemaQuery;
			}
			set
			{
				this.isSchemaQuery = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets the source object to be used for the query. For instance queries, this is typically an instance path. For schema queries, this is typically a class name.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the path of the object to be used for the query.</returns>
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000A49A File Offset: 0x0000869A
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000A4B0 File Offset: 0x000086B0
		public string SourceObject
		{
			get
			{
				if (this.sourceObject == null)
				{
					return string.Empty;
				}
				return this.sourceObject;
			}
			set
			{
				this.sourceObject = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets the class of the endpoint objects (the related class).          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the related class name.</returns>
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000A4C5 File Offset: 0x000086C5
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000A4DB File Offset: 0x000086DB
		public string RelatedClass
		{
			get
			{
				if (this.relatedClass == null)
				{
					return string.Empty;
				}
				return this.relatedClass;
			}
			set
			{
				this.relatedClass = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets the type of relationship (association).          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the relationship class name. </returns>
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000A4F0 File Offset: 0x000086F0
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000A506 File Offset: 0x00008706
		public string RelationshipClass
		{
			get
			{
				if (this.relationshipClass == null)
				{
					return string.Empty;
				}
				return this.relationshipClass;
			}
			set
			{
				this.relationshipClass = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets a qualifier required to be defined on the related objects.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the name of the qualifier required on the related object.</returns>
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000A51B File Offset: 0x0000871B
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000A531 File Offset: 0x00008731
		public string RelatedQualifier
		{
			get
			{
				if (this.relatedQualifier == null)
				{
					return string.Empty;
				}
				return this.relatedQualifier;
			}
			set
			{
				this.relatedQualifier = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets a qualifier required to be defined on the relationship objects.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the name of the qualifier required on the relationship objects.</returns>
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000A546 File Offset: 0x00008746
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000A55C File Offset: 0x0000875C
		public string RelationshipQualifier
		{
			get
			{
				if (this.relationshipQualifier == null)
				{
					return string.Empty;
				}
				return this.relationshipQualifier;
			}
			set
			{
				this.relationshipQualifier = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets the role that the related objects returned should be playing in the relationship.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the role of the related objects.</returns>
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000A571 File Offset: 0x00008771
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000A587 File Offset: 0x00008787
		public string RelatedRole
		{
			get
			{
				if (this.relatedRole == null)
				{
					return string.Empty;
				}
				return this.relatedRole;
			}
			set
			{
				this.relatedRole = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets the role that the source object should be playing in the relationship.          </summary>
		/// <returns>Returns a <see cref="T:System.String" /> value containing the role of this object.</returns>
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000A59C File Offset: 0x0000879C
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000A5B2 File Offset: 0x000087B2
		public string ThisRole
		{
			get
			{
				if (this.thisRole == null)
				{
					return string.Empty;
				}
				return this.thisRole;
			}
			set
			{
				this.thisRole = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Gets or sets a value indicating that for all instances that adhere to the query, only their class definitions be returned. This parameter is only valid for instance queries.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating that for all instances that adhere to the query, only their class definitions are to be returned.</returns>
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000A5C7 File Offset: 0x000087C7
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000A5CF File Offset: 0x000087CF
		public bool ClassDefinitionsOnly
		{
			get
			{
				return this.classDefinitionsOnly;
			}
			set
			{
				this.classDefinitionsOnly = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		/// <summary>Builds the query string according to the current property values.                       </summary>
		// Token: 0x06000204 RID: 516 RVA: 0x0000A5E4 File Offset: 0x000087E4
		protected internal void BuildQuery()
		{
			if (this.sourceObject == null)
			{
				base.SetQueryString(string.Empty);
			}
			if (this.sourceObject == null || this.sourceObject.Length == 0)
			{
				return;
			}
			string text = string.Concat(new string[]
			{
				RelatedObjectQuery.tokenAssociators,
				" ",
				RelatedObjectQuery.tokenOf,
				" {",
				this.sourceObject,
				"}"
			});
			if (this.RelatedClass.Length != 0 || this.RelationshipClass.Length != 0 || this.RelatedQualifier.Length != 0 || this.RelationshipQualifier.Length != 0 || this.RelatedRole.Length != 0 || this.ThisRole.Length != 0 || this.classDefinitionsOnly || this.isSchemaQuery)
			{
				text = text + " " + RelatedObjectQuery.tokenWhere;
				if (this.RelatedClass.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenResultClass,
						" = ",
						this.relatedClass
					});
				}
				if (this.RelationshipClass.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenAssocClass,
						" = ",
						this.relationshipClass
					});
				}
				if (this.RelatedRole.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenResultRole,
						" = ",
						this.relatedRole
					});
				}
				if (this.ThisRole.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenRole,
						" = ",
						this.thisRole
					});
				}
				if (this.RelatedQualifier.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenRequiredQualifier,
						" = ",
						this.relatedQualifier
					});
				}
				if (this.RelationshipQualifier.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenRequiredAssocQualifier,
						" = ",
						this.relationshipQualifier
					});
				}
				if (!this.isSchemaQuery)
				{
					if (this.classDefinitionsOnly)
					{
						text = text + " " + RelatedObjectQuery.tokenClassDefsOnly;
					}
				}
				else
				{
					text = text + " " + RelatedObjectQuery.tokenSchemaOnly;
				}
			}
			base.SetQueryString(text);
		}

		/// <summary>Parses the query string and sets the property values accordingly.                       </summary>
		/// <param name="query">The query string to be parsed.</param>
		// Token: 0x06000205 RID: 517 RVA: 0x0000A878 File Offset: 0x00008A78
		protected internal override void ParseQuery(string query)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = null;
			string text6 = null;
			bool flag = false;
			bool flag2 = false;
			string text7 = query.Trim();
			if (string.Compare(text7, 0, RelatedObjectQuery.tokenAssociators, 0, RelatedObjectQuery.tokenAssociators.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "associators");
			}
			text7 = text7.Remove(0, RelatedObjectQuery.tokenAssociators.Length);
			if (text7.Length == 0 || !char.IsWhiteSpace(text7[0]))
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			text7 = text7.TrimStart(null);
			if (string.Compare(text7, 0, RelatedObjectQuery.tokenOf, 0, RelatedObjectQuery.tokenOf.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "of");
			}
			text7 = text7.Remove(0, RelatedObjectQuery.tokenOf.Length).TrimStart(null);
			if (text7.IndexOf('{') != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			text7 = text7.Remove(0, 1).TrimStart(null);
			int num;
			if (-1 == (num = text7.IndexOf('}')))
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			string text8 = text7.Substring(0, num).TrimEnd(null);
			text7 = text7.Remove(0, num + 1).TrimStart(null);
			if (0 < text7.Length)
			{
				if (string.Compare(text7, 0, RelatedObjectQuery.tokenWhere, 0, RelatedObjectQuery.tokenWhere.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "where");
				}
				text7 = text7.Remove(0, RelatedObjectQuery.tokenWhere.Length);
				if (text7.Length == 0 || !char.IsWhiteSpace(text7[0]))
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				text7 = text7.TrimStart(null);
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				bool flag8 = false;
				bool flag9 = false;
				bool flag10 = false;
				for (;;)
				{
					if (text7.Length >= RelatedObjectQuery.tokenResultClass.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenResultClass, 0, RelatedObjectQuery.tokenResultClass.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenResultClass, "=", ref flag3, ref text);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenAssocClass.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenAssocClass, 0, RelatedObjectQuery.tokenAssocClass.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenAssocClass, "=", ref flag4, ref text2);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenResultRole.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenResultRole, 0, RelatedObjectQuery.tokenResultRole.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenResultRole, "=", ref flag5, ref text3);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenRole.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenRole, 0, RelatedObjectQuery.tokenRole.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenRole, "=", ref flag6, ref text4);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenRequiredQualifier.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenRequiredQualifier, 0, RelatedObjectQuery.tokenRequiredQualifier.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenRequiredQualifier, "=", ref flag7, ref text5);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenRequiredAssocQualifier.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenRequiredAssocQualifier, 0, RelatedObjectQuery.tokenRequiredAssocQualifier.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenRequiredAssocQualifier, "=", ref flag8, ref text6);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenSchemaOnly.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenSchemaOnly, 0, RelatedObjectQuery.tokenSchemaOnly.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenSchemaOnly, ref flag10);
						flag2 = true;
					}
					else
					{
						if (text7.Length < RelatedObjectQuery.tokenClassDefsOnly.Length || string.Compare(text7, 0, RelatedObjectQuery.tokenClassDefsOnly, 0, RelatedObjectQuery.tokenClassDefsOnly.Length, StringComparison.OrdinalIgnoreCase) != 0)
						{
							break;
						}
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenClassDefsOnly, ref flag9);
						flag = true;
					}
				}
				if (text7.Length != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				if (flag10 && flag9)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
			}
			this.sourceObject = text8;
			this.relatedClass = text;
			this.relationshipClass = text2;
			this.relatedRole = text3;
			this.thisRole = text4;
			this.relatedQualifier = text5;
			this.relationshipQualifier = text6;
			this.classDefinitionsOnly = flag;
			this.isSchemaQuery = flag2;
		}

		/// <summary>Creates a copy of the object.          </summary>
		/// <returns>The copied object.             </returns>
		// Token: 0x06000206 RID: 518 RVA: 0x0000AD18 File Offset: 0x00008F18
		public override object Clone()
		{
			if (!this.isSchemaQuery)
			{
				return new RelatedObjectQuery(this.sourceObject, this.relatedClass, this.relationshipClass, this.relatedQualifier, this.relationshipQualifier, this.relatedRole, this.thisRole, this.classDefinitionsOnly);
			}
			return new RelatedObjectQuery(true, this.sourceObject, this.relatedClass, this.relationshipClass, this.relatedQualifier, this.relationshipQualifier, this.relatedRole, this.thisRole);
		}

		// Token: 0x04000155 RID: 341
		private static readonly string tokenAssociators = "associators";

		// Token: 0x04000156 RID: 342
		private static readonly string tokenOf = "of";

		// Token: 0x04000157 RID: 343
		private static readonly string tokenWhere = "where";

		// Token: 0x04000158 RID: 344
		private static readonly string tokenResultClass = "resultclass";

		// Token: 0x04000159 RID: 345
		private static readonly string tokenAssocClass = "assocclass";

		// Token: 0x0400015A RID: 346
		private static readonly string tokenResultRole = "resultrole";

		// Token: 0x0400015B RID: 347
		private static readonly string tokenRole = "role";

		// Token: 0x0400015C RID: 348
		private static readonly string tokenRequiredQualifier = "requiredqualifier";

		// Token: 0x0400015D RID: 349
		private static readonly string tokenRequiredAssocQualifier = "requiredassocqualifier";

		// Token: 0x0400015E RID: 350
		private static readonly string tokenClassDefsOnly = "classdefsonly";

		// Token: 0x0400015F RID: 351
		private static readonly string tokenSchemaOnly = "schemaonly";

		// Token: 0x04000160 RID: 352
		private bool isSchemaQuery;

		// Token: 0x04000161 RID: 353
		private string sourceObject;

		// Token: 0x04000162 RID: 354
		private string relatedClass;

		// Token: 0x04000163 RID: 355
		private string relationshipClass;

		// Token: 0x04000164 RID: 356
		private string relatedQualifier;

		// Token: 0x04000165 RID: 357
		private string relationshipQualifier;

		// Token: 0x04000166 RID: 358
		private string relatedRole;

		// Token: 0x04000167 RID: 359
		private string thisRole;

		// Token: 0x04000168 RID: 360
		private bool classDefinitionsOnly;
	}
}
