using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics;

namespace Microsoft.Data.Edm
{
	// Token: 0x020001BB RID: 443
	internal static class EdmUtil
	{
		// Token: 0x06000AAC RID: 2732 RVA: 0x0001F73D File Offset: 0x0001D93D
		public static bool IsNullOrWhiteSpaceInternal(string value)
		{
			return value == null || value.ToCharArray().All(new Func<char, bool>(char.IsWhiteSpace));
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0001F75C File Offset: 0x0001D95C
		public static string JoinInternal<T>(string separator, IEnumerable<T> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (separator == null)
			{
				separator = string.Empty;
			}
			string result;
			using (IEnumerator<T> enumerator = values.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = string.Empty;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (enumerator.Current != null)
					{
						T t = enumerator.Current;
						string text = t.ToString();
						if (text != null)
						{
							stringBuilder.Append(text);
						}
					}
					while (enumerator.MoveNext())
					{
						stringBuilder.Append(separator);
						if (enumerator.Current != null)
						{
							T t2 = enumerator.Current;
							string text2 = t2.ToString();
							if (text2 != null)
							{
								stringBuilder.Append(text2);
							}
						}
					}
					result = stringBuilder.ToString();
				}
			}
			return result;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0001F838 File Offset: 0x0001DA38
		public static bool IsQualifiedName(string name)
		{
			string[] array = name.Split(new char[]
			{
				'.'
			});
			if (array.Count<string>() < 2)
			{
				return false;
			}
			foreach (string value in array)
			{
				if (EdmUtil.IsNullOrWhiteSpaceInternal(value))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0001F88F File Offset: 0x0001DA8F
		public static bool IsValidUndottedName(string name)
		{
			return !string.IsNullOrEmpty(name) && EdmUtil.UndottedNameValidator.IsMatch(name);
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0001F8A8 File Offset: 0x0001DAA8
		public static bool IsValidDottedName(string name)
		{
			return name.Split(new char[]
			{
				'.'
			}).All(new Func<string, bool>(EdmUtil.IsValidUndottedName));
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0001F8DC File Offset: 0x0001DADC
		public static string ParameterizedName(IEdmFunctionBase function)
		{
			int num = 0;
			int num2 = function.Parameters.Count<IEdmFunctionParameter>();
			StringBuilder stringBuilder = new StringBuilder();
			UnresolvedFunction unresolvedFunction = function as UnresolvedFunction;
			if (unresolvedFunction != null)
			{
				stringBuilder.Append(unresolvedFunction.Namespace);
				stringBuilder.Append("/");
				stringBuilder.Append(unresolvedFunction.Name);
				return stringBuilder.ToString();
			}
			IEdmSchemaElement edmSchemaElement = function as IEdmSchemaElement;
			if (edmSchemaElement != null)
			{
				stringBuilder.Append(edmSchemaElement.Namespace);
				stringBuilder.Append(".");
			}
			stringBuilder.Append(function.Name);
			stringBuilder.Append("(");
			foreach (IEdmFunctionParameter edmFunctionParameter in function.Parameters)
			{
				string value;
				if (edmFunctionParameter.Type.IsCollection())
				{
					value = "Collection(" + edmFunctionParameter.Type.AsCollection().ElementType().FullName() + ")";
				}
				else if (edmFunctionParameter.Type.IsEntityReference())
				{
					value = "Ref(" + edmFunctionParameter.Type.AsEntityReference().EntityType().FullName() + ")";
				}
				else
				{
					value = edmFunctionParameter.Type.FullName();
				}
				stringBuilder.Append(value);
				num++;
				if (num < num2)
				{
					stringBuilder.Append(", ");
				}
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0001FA6C File Offset: 0x0001DC6C
		public static bool TryGetNamespaceNameFromQualifiedName(string qualifiedName, out string namespaceName, out string name)
		{
			int num = qualifiedName.LastIndexOf('/');
			if (num >= 0)
			{
				namespaceName = qualifiedName.Substring(0, num);
				name = qualifiedName.Substring(num + 1);
				return true;
			}
			int num2 = qualifiedName.LastIndexOf('.');
			if (num2 < 0)
			{
				namespaceName = string.Empty;
				name = qualifiedName;
				return false;
			}
			namespaceName = qualifiedName.Substring(0, num2);
			name = qualifiedName.Substring(num2 + 1);
			return true;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0001FACC File Offset: 0x0001DCCC
		public static string FullyQualifiedName(IEdmVocabularyAnnotatable element)
		{
			IEdmSchemaElement edmSchemaElement = element as IEdmSchemaElement;
			if (edmSchemaElement != null)
			{
				IEdmFunction edmFunction = edmSchemaElement as IEdmFunction;
				if (edmFunction != null)
				{
					return EdmUtil.ParameterizedName(edmFunction);
				}
				return edmSchemaElement.FullName();
			}
			else
			{
				IEdmEntityContainerElement edmEntityContainerElement = element as IEdmEntityContainerElement;
				if (edmEntityContainerElement == null)
				{
					IEdmProperty edmProperty = element as IEdmProperty;
					if (edmProperty != null)
					{
						IEdmSchemaType edmSchemaType = edmProperty.DeclaringType as IEdmSchemaType;
						if (edmSchemaType != null)
						{
							string text = EdmUtil.FullyQualifiedName(edmSchemaType);
							if (text != null)
							{
								return text + "/" + edmProperty.Name;
							}
						}
					}
					else
					{
						IEdmFunctionParameter edmFunctionParameter = element as IEdmFunctionParameter;
						if (edmFunctionParameter != null)
						{
							string text2 = EdmUtil.FullyQualifiedName(edmFunctionParameter.DeclaringFunction);
							if (text2 != null)
							{
								return text2 + "/" + edmFunctionParameter.Name;
							}
						}
					}
					return null;
				}
				IEdmFunctionImport edmFunctionImport = edmEntityContainerElement as IEdmFunctionImport;
				if (edmFunctionImport != null)
				{
					return edmFunctionImport.Container.FullName() + "/" + EdmUtil.ParameterizedName(edmFunctionImport);
				}
				return edmEntityContainerElement.Container.FullName() + "/" + edmEntityContainerElement.Name;
			}
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0001FBB9 File Offset: 0x0001DDB9
		public static T CheckArgumentNull<T>([EdmUtil.ValidatedNotNullAttribute] T value, string parameterName) where T : class
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			return value;
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0001FBCB File Offset: 0x0001DDCB
		public static bool EqualsOrdinal(this string string1, string string2)
		{
			return string.Equals(string1, string2, StringComparison.Ordinal);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0001FBD5 File Offset: 0x0001DDD5
		public static bool EqualsOrdinalIgnoreCase(this string string1, string string2)
		{
			return string.Equals(string1, string2, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x040004EB RID: 1259
		private const string StartCharacterExp = "[\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Lm}\\p{Nl}]";

		// Token: 0x040004EC RID: 1260
		private const string OtherCharacterExp = "[\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Lm}\\p{Nl}\\p{Mn}\\p{Mc}\\p{Nd}\\p{Pc}\\p{Cf}]";

		// Token: 0x040004ED RID: 1261
		private const string NameExp = "[\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Lm}\\p{Nl}][\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Lm}\\p{Nl}\\p{Mn}\\p{Mc}\\p{Nd}\\p{Pc}\\p{Cf}]{0,}";

		// Token: 0x040004EE RID: 1262
		private static Regex UndottedNameValidator = PlatformHelper.CreateCompiled("^[\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Lm}\\p{Nl}][\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Lm}\\p{Nl}\\p{Mn}\\p{Mc}\\p{Nd}\\p{Pc}\\p{Cf}]{0,}$", RegexOptions.Singleline);

		// Token: 0x020001BC RID: 444
		private sealed class ValidatedNotNullAttribute : Attribute
		{
		}
	}
}
