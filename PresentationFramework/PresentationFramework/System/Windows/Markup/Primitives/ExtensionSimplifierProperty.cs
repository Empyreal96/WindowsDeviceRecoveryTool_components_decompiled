using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000286 RID: 646
	internal class ExtensionSimplifierProperty : MarkupPropertyWrapper
	{
		// Token: 0x06002477 RID: 9335 RVA: 0x000B0C22 File Offset: 0x000AEE22
		public ExtensionSimplifierProperty(MarkupProperty baseProperty, IValueSerializerContext context) : base(baseProperty)
		{
			this._context = context;
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x000B0C34 File Offset: 0x000AEE34
		public override bool IsComposite
		{
			get
			{
				if (!base.IsComposite)
				{
					return false;
				}
				if (base.IsCollectionProperty)
				{
					return true;
				}
				bool flag = true;
				foreach (MarkupObject markupObject in this.Items)
				{
					if (!flag || !typeof(MarkupExtension).IsAssignableFrom(markupObject.ObjectType))
					{
						return true;
					}
					flag = false;
					markupObject.AssignRootContext(this._context);
					foreach (MarkupProperty markupProperty in markupObject.Properties)
					{
						if (markupProperty.IsComposite)
						{
							return true;
						}
					}
				}
				return flag;
			}
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x000B0D08 File Offset: 0x000AEF08
		private IEnumerable<MarkupObject> GetBaseItems()
		{
			return base.Items;
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x0600247A RID: 9338 RVA: 0x000B0D10 File Offset: 0x000AEF10
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				foreach (MarkupObject baseObject in this.GetBaseItems())
				{
					ExtensionSimplifierMarkupObject extensionSimplifierMarkupObject = new ExtensionSimplifierMarkupObject(baseObject, this._context);
					extensionSimplifierMarkupObject.AssignRootContext(this._context);
					yield return extensionSimplifierMarkupObject;
				}
				IEnumerator<MarkupObject> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x0600247B RID: 9339 RVA: 0x000B0D30 File Offset: 0x000AEF30
		public override string StringValue
		{
			get
			{
				string text = null;
				if (!base.IsComposite)
				{
					text = MarkupExtensionParser.AddEscapeToLiteralString(base.StringValue);
				}
				else
				{
					using (IEnumerator<MarkupObject> enumerator = this.Items.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							MarkupObject item = enumerator.Current;
							text = this.ConvertMarkupItemToString(item);
						}
					}
					if (text == null)
					{
						text = "";
					}
				}
				return text;
			}
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x000B0DA4 File Offset: 0x000AEFA4
		private string ConvertMarkupItemToString(MarkupObject item)
		{
			ValueSerializer valueSerializerFor = this._context.GetValueSerializerFor(typeof(Type));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('{');
			string text = valueSerializerFor.ConvertToString(item.ObjectType, this._context);
			if (text.EndsWith("Extension", StringComparison.Ordinal))
			{
				stringBuilder.Append(text, 0, text.Length - 9);
			}
			else
			{
				stringBuilder.Append(text);
			}
			bool flag = true;
			foreach (MarkupProperty markupProperty in item.Properties)
			{
				stringBuilder.Append(flag ? " " : ", ");
				flag = false;
				if (!markupProperty.IsConstructorArgument)
				{
					stringBuilder.Append(markupProperty.Name);
					stringBuilder.Append('=');
				}
				string text2 = markupProperty.StringValue;
				if (text2 != null && text2.Length > 0)
				{
					if (text2[0] == '{')
					{
						if (text2.Length <= 1 || text2[1] != '}')
						{
							stringBuilder.Append(text2);
							continue;
						}
						text2 = text2.Substring(2);
					}
					foreach (char c in text2)
					{
						if (c != ',')
						{
							if (c != '{')
							{
								if (c != '}')
								{
									stringBuilder.Append(c);
								}
								else
								{
									stringBuilder.Append("\\}");
								}
							}
							else
							{
								stringBuilder.Append("\\{");
							}
						}
						else
						{
							stringBuilder.Append("\\,");
						}
					}
				}
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000B0F74 File Offset: 0x000AF174
		internal override void VerifyOnlySerializableTypes()
		{
			base.VerifyOnlySerializableTypes();
			if (base.IsComposite)
			{
				foreach (MarkupObject markupObject in this.Items)
				{
					MarkupWriter.VerifyTypeIsSerializable(markupObject.ObjectType);
					foreach (MarkupProperty markupProperty in markupObject.Properties)
					{
						markupProperty.VerifyOnlySerializableTypes();
					}
				}
			}
		}

		// Token: 0x04001B34 RID: 6964
		private IValueSerializerContext _context;

		// Token: 0x04001B35 RID: 6965
		private const int EXTENSIONLENGTH = 9;
	}
}
