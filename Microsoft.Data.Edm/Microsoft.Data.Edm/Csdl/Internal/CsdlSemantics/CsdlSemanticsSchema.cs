using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001AD RID: 429
	internal class CsdlSemanticsSchema : CsdlSemanticsElement, IEdmCheckable
	{
		// Token: 0x06000955 RID: 2389 RVA: 0x000192BC File Offset: 0x000174BC
		public CsdlSemanticsSchema(CsdlSemanticsModel model, CsdlSchema schema) : base(schema)
		{
			this.model = model;
			this.schema = schema;
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x00019336 File Offset: 0x00017536
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x0001933E File Offset: 0x0001753E
		public override CsdlElement Element
		{
			get
			{
				return this.schema;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x00019346 File Offset: 0x00017546
		public IEnumerable<IEdmSchemaType> Types
		{
			get
			{
				return this.typesCache.GetValue(this, CsdlSemanticsSchema.ComputeTypesFunc, null);
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x0001935A File Offset: 0x0001755A
		public IEnumerable<CsdlSemanticsAssociation> Associations
		{
			get
			{
				return this.associationsCache.GetValue(this, CsdlSemanticsSchema.ComputeAssociationsFunc, null);
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x0001936E File Offset: 0x0001756E
		public IEnumerable<IEdmFunction> Functions
		{
			get
			{
				return this.functionsCache.GetValue(this, CsdlSemanticsSchema.ComputeFunctionsFunc, null);
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x00019382 File Offset: 0x00017582
		public IEnumerable<IEdmValueTerm> ValueTerms
		{
			get
			{
				return this.valueTermsCache.GetValue(this, CsdlSemanticsSchema.ComputeValueTermsFunc, null);
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x00019396 File Offset: 0x00017596
		public IEnumerable<IEdmEntityContainer> EntityContainers
		{
			get
			{
				return this.entityContainersCache.GetValue(this, CsdlSemanticsSchema.ComputeEntityContainersFunc, null);
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x000193AA File Offset: 0x000175AA
		public string Namespace
		{
			get
			{
				return this.schema.Namespace;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x000193B8 File Offset: 0x000175B8
		public IEnumerable<EdmError> Errors
		{
			get
			{
				HashSetInternal<string> hashSetInternal = new HashSetInternal<string>();
				if (this.schema.Alias != null)
				{
					hashSetInternal.Add(this.schema.Alias);
				}
				foreach (CsdlUsing csdlUsing in this.schema.Usings)
				{
					if (!hashSetInternal.Add(csdlUsing.Alias))
					{
						return new EdmError[]
						{
							new EdmError(base.Location, EdmErrorCode.DuplicateAlias, Strings.CsdlSemantics_DuplicateAlias(this.Namespace, csdlUsing.Alias))
						};
					}
				}
				return Enumerable.Empty<EdmError>();
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x00019470 File Offset: 0x00017670
		private Dictionary<string, object> LabeledExpressions
		{
			get
			{
				return this.labeledExpressionsCache.GetValue(this, CsdlSemanticsSchema.ComputeLabeledExpressionsFunc, null);
			}
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00019484 File Offset: 0x00017684
		public IEdmAssociation FindAssociation(string name)
		{
			return this.FindSchemaElement<IEdmAssociation>(name, new Func<IEdmModel, string, IEdmAssociation>(CsdlSemanticsSchema.FindAssociation));
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00019499 File Offset: 0x00017699
		public IEnumerable<IEdmFunction> FindFunctions(string name)
		{
			return this.FindSchemaElement<IEnumerable<IEdmFunction>>(name, new Func<IEdmModel, string, IEnumerable<IEdmFunction>>(CsdlSemanticsSchema.FindFunctions));
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x000194AE File Offset: 0x000176AE
		public IEdmSchemaType FindType(string name)
		{
			return this.FindSchemaElement<IEdmSchemaType>(name, new Func<IEdmModel, string, IEdmSchemaType>(CsdlSemanticsSchema.FindType));
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x000194C3 File Offset: 0x000176C3
		public IEdmValueTerm FindValueTerm(string name)
		{
			return this.FindSchemaElement<IEdmValueTerm>(name, new Func<IEdmModel, string, IEdmValueTerm>(CsdlSemanticsSchema.FindValueTerm));
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x000194D8 File Offset: 0x000176D8
		public IEdmEntityContainer FindEntityContainer(string name)
		{
			return this.FindSchemaElement<IEdmEntityContainer>(name, new Func<IEdmModel, string, IEdmEntityContainer>(CsdlSemanticsSchema.FindEntityContainer));
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x000194F0 File Offset: 0x000176F0
		public T FindSchemaElement<T>(string name, Func<IEdmModel, string, T> modelFinder)
		{
			string text = this.ReplaceAlias(name);
			if (text == null)
			{
				text = name;
			}
			return modelFinder(this.model, text);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00019518 File Offset: 0x00017718
		public string ReplaceAlias(string name)
		{
			string text = CsdlSemanticsSchema.ReplaceAlias(this.Namespace, this.schema.Alias, name);
			if (text == null)
			{
				foreach (CsdlUsing csdlUsing in this.schema.Usings)
				{
					text = CsdlSemanticsSchema.ReplaceAlias(csdlUsing.Namespace, csdlUsing.Alias, name);
					if (text != null)
					{
						break;
					}
				}
			}
			return text;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00019598 File Offset: 0x00017798
		public string UnresolvedName(string qualifiedName)
		{
			if (qualifiedName == null)
			{
				return null;
			}
			return this.ReplaceAlias(qualifiedName) ?? qualifiedName;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x000195AC File Offset: 0x000177AC
		public IEdmLabeledExpression FindLabeledElement(string label, IEdmEntityType bindingContext)
		{
			object obj;
			if (!this.LabeledExpressions.TryGetValue(label, out obj))
			{
				return null;
			}
			CsdlLabeledExpression csdlLabeledExpression = obj as CsdlLabeledExpression;
			if (csdlLabeledExpression != null)
			{
				return this.WrapLabeledElement(csdlLabeledExpression, bindingContext);
			}
			return this.WrapLabeledElementList((List<CsdlLabeledExpression>)obj, bindingContext);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x000195EC File Offset: 0x000177EC
		public IEdmLabeledExpression WrapLabeledElement(CsdlLabeledExpression labeledElement, IEdmEntityType bindingContext)
		{
			IEdmLabeledExpression edmLabeledExpression;
			if (!this.semanticsLabeledElements.TryGetValue(labeledElement, out edmLabeledExpression))
			{
				edmLabeledExpression = new CsdlSemanticsLabeledExpression(labeledElement.Label, labeledElement.Element, bindingContext, this);
				this.semanticsLabeledElements[labeledElement] = edmLabeledExpression;
			}
			return edmLabeledExpression;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001962C File Offset: 0x0001782C
		private static string ReplaceAlias(string namespaceName, string namespaceAlias, string name)
		{
			if (namespaceAlias != null && name.Length > namespaceAlias.Length && name.StartsWith(namespaceAlias, StringComparison.Ordinal) && name[namespaceAlias.Length] == '.')
			{
				return (namespaceName ?? string.Empty) + name.Substring(namespaceAlias.Length);
			}
			return null;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00019681 File Offset: 0x00017881
		private static IEdmAssociation FindAssociation(IEdmModel model, string name)
		{
			return ((CsdlSemanticsModel)model).FindAssociation(name);
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0001968F File Offset: 0x0001788F
		private static IEnumerable<IEdmFunction> FindFunctions(IEdmModel model, string name)
		{
			return model.FindFunctions(name);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00019698 File Offset: 0x00017898
		private static IEdmSchemaType FindType(IEdmModel model, string name)
		{
			return model.FindType(name);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x000196A1 File Offset: 0x000178A1
		private static IEdmValueTerm FindValueTerm(IEdmModel model, string name)
		{
			return model.FindValueTerm(name);
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000196AA File Offset: 0x000178AA
		private static IEdmEntityContainer FindEntityContainer(IEdmModel model, string name)
		{
			return model.FindEntityContainer(name);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x000196B4 File Offset: 0x000178B4
		private static void AddLabeledExpressions(CsdlExpressionBase expression, Dictionary<string, object> result)
		{
			if (expression == null)
			{
				return;
			}
			EdmExpressionKind expressionKind = expression.ExpressionKind;
			switch (expressionKind)
			{
			case EdmExpressionKind.Record:
				IL_11A:
				using (IEnumerator<CsdlPropertyValue> enumerator = ((CsdlRecordExpression)expression).PropertyValues.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CsdlPropertyValue csdlPropertyValue = enumerator.Current;
						CsdlSemanticsSchema.AddLabeledExpressions(csdlPropertyValue.Expression, result);
					}
					return;
				}
				goto IL_15B;
			case EdmExpressionKind.Collection:
				using (IEnumerator<CsdlExpressionBase> enumerator2 = ((CsdlCollectionExpression)expression).ElementValues.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						CsdlExpressionBase expression2 = enumerator2.Current;
						CsdlSemanticsSchema.AddLabeledExpressions(expression2, result);
					}
					return;
				}
				break;
			default:
				switch (expressionKind)
				{
				case EdmExpressionKind.If:
					goto IL_15B;
				case EdmExpressionKind.AssertType:
					CsdlSemanticsSchema.AddLabeledExpressions(((CsdlAssertTypeExpression)expression).Operand, result);
					return;
				case EdmExpressionKind.IsType:
					CsdlSemanticsSchema.AddLabeledExpressions(((CsdlIsTypeExpression)expression).Operand, result);
					return;
				case EdmExpressionKind.FunctionApplication:
					break;
				case EdmExpressionKind.LabeledExpressionReference:
					return;
				case EdmExpressionKind.Labeled:
				{
					CsdlLabeledExpression csdlLabeledExpression = (CsdlLabeledExpression)expression;
					string label = csdlLabeledExpression.Label;
					object obj;
					if (result.TryGetValue(label, out obj))
					{
						List<CsdlLabeledExpression> list = obj as List<CsdlLabeledExpression>;
						if (list == null)
						{
							list = new List<CsdlLabeledExpression>();
							list.Add((CsdlLabeledExpression)obj);
							result[label] = list;
						}
						list.Add(csdlLabeledExpression);
					}
					else
					{
						result[label] = csdlLabeledExpression;
					}
					CsdlSemanticsSchema.AddLabeledExpressions(csdlLabeledExpression.Element, result);
					return;
				}
				default:
					return;
				}
				break;
			}
			using (IEnumerator<CsdlExpressionBase> enumerator3 = ((CsdlApplyExpression)expression).Arguments.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					CsdlExpressionBase expression3 = enumerator3.Current;
					CsdlSemanticsSchema.AddLabeledExpressions(expression3, result);
				}
				return;
			}
			goto IL_11A;
			IL_15B:
			CsdlIfExpression csdlIfExpression = (CsdlIfExpression)expression;
			CsdlSemanticsSchema.AddLabeledExpressions(csdlIfExpression.Test, result);
			CsdlSemanticsSchema.AddLabeledExpressions(csdlIfExpression.IfTrue, result);
			CsdlSemanticsSchema.AddLabeledExpressions(csdlIfExpression.IfFalse, result);
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00019898 File Offset: 0x00017A98
		private static void AddLabeledExpressions(IEnumerable<CsdlVocabularyAnnotationBase> annotations, Dictionary<string, object> result)
		{
			foreach (CsdlVocabularyAnnotationBase csdlVocabularyAnnotationBase in annotations)
			{
				CsdlValueAnnotation csdlValueAnnotation = csdlVocabularyAnnotationBase as CsdlValueAnnotation;
				if (csdlValueAnnotation != null)
				{
					CsdlSemanticsSchema.AddLabeledExpressions(csdlValueAnnotation.Expression, result);
				}
				else
				{
					CsdlTypeAnnotation csdlTypeAnnotation = csdlVocabularyAnnotationBase as CsdlTypeAnnotation;
					if (csdlTypeAnnotation != null)
					{
						foreach (CsdlPropertyValue csdlPropertyValue in csdlTypeAnnotation.Properties)
						{
							CsdlSemanticsSchema.AddLabeledExpressions(csdlPropertyValue.Expression, result);
						}
					}
				}
			}
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00019948 File Offset: 0x00017B48
		private IEdmLabeledExpression WrapLabeledElementList(List<CsdlLabeledExpression> labeledExpressions, IEdmEntityType bindingContext)
		{
			IEdmLabeledExpression edmLabeledExpression;
			if (!this.ambiguousLabeledExpressions.TryGetValue(labeledExpressions, out edmLabeledExpression))
			{
				foreach (CsdlLabeledExpression labeledElement in labeledExpressions)
				{
					IEdmLabeledExpression edmLabeledExpression2 = this.WrapLabeledElement(labeledElement, bindingContext);
					edmLabeledExpression = ((edmLabeledExpression == null) ? edmLabeledExpression2 : new AmbiguousLabeledExpressionBinding(edmLabeledExpression, edmLabeledExpression2));
				}
				this.ambiguousLabeledExpressions[labeledExpressions] = edmLabeledExpression;
			}
			return edmLabeledExpression;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x000199C4 File Offset: 0x00017BC4
		private IEnumerable<IEdmValueTerm> ComputeValueTerms()
		{
			List<IEdmValueTerm> list = new List<IEdmValueTerm>();
			foreach (CsdlValueTerm valueTerm in this.schema.ValueTerms)
			{
				list.Add(new CsdlSemanticsValueTerm(this, valueTerm));
			}
			return list;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00019A24 File Offset: 0x00017C24
		private IEnumerable<IEdmEntityContainer> ComputeEntityContainers()
		{
			List<IEdmEntityContainer> list = new List<IEdmEntityContainer>();
			foreach (CsdlEntityContainer entityContainer in this.schema.EntityContainers)
			{
				list.Add(new CsdlSemanticsEntityContainer(this, entityContainer));
			}
			return list;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00019A84 File Offset: 0x00017C84
		private IEnumerable<CsdlSemanticsAssociation> ComputeAssociations()
		{
			List<CsdlSemanticsAssociation> list = new List<CsdlSemanticsAssociation>();
			foreach (CsdlAssociation association in this.schema.Associations)
			{
				list.Add(new CsdlSemanticsAssociation(this, association));
			}
			return list;
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00019AE4 File Offset: 0x00017CE4
		private IEnumerable<IEdmFunction> ComputeFunctions()
		{
			List<IEdmFunction> list = new List<IEdmFunction>();
			foreach (CsdlFunction function in this.schema.Functions)
			{
				list.Add(new CsdlSemanticsFunction(this, function));
			}
			return list;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00019B44 File Offset: 0x00017D44
		private IEnumerable<IEdmSchemaType> ComputeTypes()
		{
			List<IEdmSchemaType> list = new List<IEdmSchemaType>();
			foreach (CsdlStructuredType csdlStructuredType in this.schema.StructuredTypes)
			{
				CsdlEntityType csdlEntityType = csdlStructuredType as CsdlEntityType;
				if (csdlEntityType != null)
				{
					list.Add(new CsdlSemanticsEntityTypeDefinition(this, csdlEntityType));
				}
				else
				{
					CsdlComplexType csdlComplexType = csdlStructuredType as CsdlComplexType;
					if (csdlComplexType != null)
					{
						list.Add(new CsdlSemanticsComplexTypeDefinition(this, csdlComplexType));
					}
				}
			}
			foreach (CsdlEnumType enumeration in this.schema.EnumTypes)
			{
				list.Add(new CsdlSemanticsEnumTypeDefinition(this, enumeration));
			}
			return list;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00019C1C File Offset: 0x00017E1C
		private Dictionary<string, object> ComputeLabeledExpressions()
		{
			Dictionary<string, object> result = new Dictionary<string, object>();
			foreach (CsdlAnnotations csdlAnnotations in this.schema.OutOfLineAnnotations)
			{
				CsdlSemanticsSchema.AddLabeledExpressions(csdlAnnotations.Annotations, result);
			}
			foreach (CsdlStructuredType csdlStructuredType in this.schema.StructuredTypes)
			{
				CsdlSemanticsSchema.AddLabeledExpressions(csdlStructuredType.VocabularyAnnotations, result);
				foreach (CsdlProperty csdlProperty in csdlStructuredType.Properties)
				{
					CsdlSemanticsSchema.AddLabeledExpressions(csdlProperty.VocabularyAnnotations, result);
				}
			}
			foreach (CsdlFunction csdlFunction in this.schema.Functions)
			{
				CsdlSemanticsSchema.AddLabeledExpressions(csdlFunction.VocabularyAnnotations, result);
				foreach (CsdlFunctionParameter csdlFunctionParameter in csdlFunction.Parameters)
				{
					CsdlSemanticsSchema.AddLabeledExpressions(csdlFunctionParameter.VocabularyAnnotations, result);
				}
			}
			foreach (CsdlValueTerm csdlValueTerm in this.schema.ValueTerms)
			{
				CsdlSemanticsSchema.AddLabeledExpressions(csdlValueTerm.VocabularyAnnotations, result);
			}
			foreach (CsdlEntityContainer csdlEntityContainer in this.schema.EntityContainers)
			{
				CsdlSemanticsSchema.AddLabeledExpressions(csdlEntityContainer.VocabularyAnnotations, result);
				foreach (CsdlEntitySet csdlEntitySet in csdlEntityContainer.EntitySets)
				{
					CsdlSemanticsSchema.AddLabeledExpressions(csdlEntitySet.VocabularyAnnotations, result);
				}
				foreach (CsdlFunctionImport csdlFunctionImport in csdlEntityContainer.FunctionImports)
				{
					CsdlSemanticsSchema.AddLabeledExpressions(csdlFunctionImport.VocabularyAnnotations, result);
					foreach (CsdlFunctionParameter csdlFunctionParameter2 in csdlFunctionImport.Parameters)
					{
						CsdlSemanticsSchema.AddLabeledExpressions(csdlFunctionParameter2.VocabularyAnnotations, result);
					}
				}
			}
			return result;
		}

		// Token: 0x0400048F RID: 1167
		private readonly CsdlSemanticsModel model;

		// Token: 0x04000490 RID: 1168
		private readonly CsdlSchema schema;

		// Token: 0x04000491 RID: 1169
		private readonly Cache<CsdlSemanticsSchema, IEnumerable<IEdmSchemaType>> typesCache = new Cache<CsdlSemanticsSchema, IEnumerable<IEdmSchemaType>>();

		// Token: 0x04000492 RID: 1170
		private static readonly Func<CsdlSemanticsSchema, IEnumerable<IEdmSchemaType>> ComputeTypesFunc = (CsdlSemanticsSchema me) => me.ComputeTypes();

		// Token: 0x04000493 RID: 1171
		private readonly Cache<CsdlSemanticsSchema, IEnumerable<CsdlSemanticsAssociation>> associationsCache = new Cache<CsdlSemanticsSchema, IEnumerable<CsdlSemanticsAssociation>>();

		// Token: 0x04000494 RID: 1172
		private static readonly Func<CsdlSemanticsSchema, IEnumerable<CsdlSemanticsAssociation>> ComputeAssociationsFunc = (CsdlSemanticsSchema me) => me.ComputeAssociations();

		// Token: 0x04000495 RID: 1173
		private readonly Cache<CsdlSemanticsSchema, IEnumerable<IEdmFunction>> functionsCache = new Cache<CsdlSemanticsSchema, IEnumerable<IEdmFunction>>();

		// Token: 0x04000496 RID: 1174
		private static readonly Func<CsdlSemanticsSchema, IEnumerable<IEdmFunction>> ComputeFunctionsFunc = (CsdlSemanticsSchema me) => me.ComputeFunctions();

		// Token: 0x04000497 RID: 1175
		private readonly Cache<CsdlSemanticsSchema, IEnumerable<IEdmEntityContainer>> entityContainersCache = new Cache<CsdlSemanticsSchema, IEnumerable<IEdmEntityContainer>>();

		// Token: 0x04000498 RID: 1176
		private static readonly Func<CsdlSemanticsSchema, IEnumerable<IEdmEntityContainer>> ComputeEntityContainersFunc = (CsdlSemanticsSchema me) => me.ComputeEntityContainers();

		// Token: 0x04000499 RID: 1177
		private readonly Cache<CsdlSemanticsSchema, IEnumerable<IEdmValueTerm>> valueTermsCache = new Cache<CsdlSemanticsSchema, IEnumerable<IEdmValueTerm>>();

		// Token: 0x0400049A RID: 1178
		private static readonly Func<CsdlSemanticsSchema, IEnumerable<IEdmValueTerm>> ComputeValueTermsFunc = (CsdlSemanticsSchema me) => me.ComputeValueTerms();

		// Token: 0x0400049B RID: 1179
		private readonly Cache<CsdlSemanticsSchema, Dictionary<string, object>> labeledExpressionsCache = new Cache<CsdlSemanticsSchema, Dictionary<string, object>>();

		// Token: 0x0400049C RID: 1180
		private static readonly Func<CsdlSemanticsSchema, Dictionary<string, object>> ComputeLabeledExpressionsFunc = (CsdlSemanticsSchema me) => me.ComputeLabeledExpressions();

		// Token: 0x0400049D RID: 1181
		private readonly Dictionary<CsdlLabeledExpression, IEdmLabeledExpression> semanticsLabeledElements = new Dictionary<CsdlLabeledExpression, IEdmLabeledExpression>();

		// Token: 0x0400049E RID: 1182
		private readonly Dictionary<List<CsdlLabeledExpression>, IEdmLabeledExpression> ambiguousLabeledExpressions = new Dictionary<List<CsdlLabeledExpression>, IEdmLabeledExpression>();
	}
}
