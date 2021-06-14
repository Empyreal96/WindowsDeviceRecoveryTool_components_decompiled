using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x0200015E RID: 350
	internal sealed class SelectedPropertiesNode
	{
		// Token: 0x06000997 RID: 2455 RVA: 0x0001DCF8 File Offset: 0x0001BEF8
		internal SelectedPropertiesNode(string selectClause) : this(SelectedPropertiesNode.SelectionType.PartialSubtree)
		{
			string[] array = selectClause.Split(new char[]
			{
				','
			});
			foreach (string text in array)
			{
				string[] segments = text.Split(new char[]
				{
					'/'
				});
				this.ParsePathSegment(segments, 0);
			}
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0001DD5B File Offset: 0x0001BF5B
		private SelectedPropertiesNode(SelectedPropertiesNode.SelectionType selectionType)
		{
			this.selectionType = selectionType;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0001DD6A File Offset: 0x0001BF6A
		internal static SelectedPropertiesNode Create(string selectQueryOption)
		{
			if (selectQueryOption == null)
			{
				return SelectedPropertiesNode.EntireSubtree;
			}
			selectQueryOption = selectQueryOption.Trim();
			if (selectQueryOption.Length == 0)
			{
				return SelectedPropertiesNode.Empty;
			}
			return new SelectedPropertiesNode(selectQueryOption);
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0001DD94 File Offset: 0x0001BF94
		internal static SelectedPropertiesNode CombineNodes(SelectedPropertiesNode left, SelectedPropertiesNode right)
		{
			if (left.selectionType == SelectedPropertiesNode.SelectionType.EntireSubtree || right.selectionType == SelectedPropertiesNode.SelectionType.EntireSubtree)
			{
				return SelectedPropertiesNode.EntireSubtree;
			}
			if (left.selectionType == SelectedPropertiesNode.SelectionType.Empty)
			{
				return right;
			}
			if (right.selectionType == SelectedPropertiesNode.SelectionType.Empty)
			{
				return left;
			}
			SelectedPropertiesNode selectedPropertiesNode = new SelectedPropertiesNode(SelectedPropertiesNode.SelectionType.PartialSubtree)
			{
				hasWildcard = (left.hasWildcard | right.hasWildcard)
			};
			if (left.selectedProperties != null && right.selectedProperties != null)
			{
				selectedPropertiesNode.selectedProperties = SelectedPropertiesNode.CreateSelectedPropertiesHashSet(left.selectedProperties.AsEnumerable<string>().Concat(right.selectedProperties));
			}
			else if (left.selectedProperties != null)
			{
				selectedPropertiesNode.selectedProperties = SelectedPropertiesNode.CreateSelectedPropertiesHashSet(left.selectedProperties);
			}
			else if (right.selectedProperties != null)
			{
				selectedPropertiesNode.selectedProperties = SelectedPropertiesNode.CreateSelectedPropertiesHashSet(right.selectedProperties);
			}
			if (left.children != null && right.children != null)
			{
				selectedPropertiesNode.children = new Dictionary<string, SelectedPropertiesNode>(left.children);
				using (Dictionary<string, SelectedPropertiesNode>.Enumerator enumerator = right.children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, SelectedPropertiesNode> keyValuePair = enumerator.Current;
						SelectedPropertiesNode left2;
						if (selectedPropertiesNode.children.TryGetValue(keyValuePair.Key, out left2))
						{
							selectedPropertiesNode.children[keyValuePair.Key] = SelectedPropertiesNode.CombineNodes(left2, keyValuePair.Value);
						}
						else
						{
							selectedPropertiesNode.children[keyValuePair.Key] = keyValuePair.Value;
						}
					}
					return selectedPropertiesNode;
				}
			}
			if (left.children != null)
			{
				selectedPropertiesNode.children = new Dictionary<string, SelectedPropertiesNode>(left.children);
			}
			else if (right.children != null)
			{
				selectedPropertiesNode.children = new Dictionary<string, SelectedPropertiesNode>(right.children);
			}
			return selectedPropertiesNode;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0001DF58 File Offset: 0x0001C158
		internal SelectedPropertiesNode GetSelectedPropertiesForNavigationProperty(IEdmEntityType entityType, string navigationPropertyName)
		{
			if (this.selectionType == SelectedPropertiesNode.SelectionType.Empty)
			{
				return SelectedPropertiesNode.Empty;
			}
			if (this.selectionType == SelectedPropertiesNode.SelectionType.EntireSubtree)
			{
				return SelectedPropertiesNode.EntireSubtree;
			}
			if (entityType == null)
			{
				return SelectedPropertiesNode.EntireSubtree;
			}
			if (this.selectedProperties.Contains(navigationPropertyName))
			{
				return SelectedPropertiesNode.EntireSubtree;
			}
			if (this.children != null)
			{
				SelectedPropertiesNode empty;
				if (!this.children.TryGetValue(navigationPropertyName, out empty))
				{
					empty = SelectedPropertiesNode.Empty;
				}
				return (from typeSegmentChild in this.GetMatchingTypeSegments(entityType)
				select typeSegmentChild.GetSelectedPropertiesForNavigationProperty(entityType, navigationPropertyName)).Aggregate(empty, new Func<SelectedPropertiesNode, SelectedPropertiesNode, SelectedPropertiesNode>(SelectedPropertiesNode.CombineNodes));
			}
			return SelectedPropertiesNode.Empty;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0001E01C File Offset: 0x0001C21C
		internal IEnumerable<IEdmNavigationProperty> GetSelectedNavigationProperties(IEdmEntityType entityType)
		{
			if (this.selectionType == SelectedPropertiesNode.SelectionType.Empty)
			{
				return SelectedPropertiesNode.EmptyNavigationProperties;
			}
			if (entityType == null)
			{
				return SelectedPropertiesNode.EmptyNavigationProperties;
			}
			if (this.selectionType == SelectedPropertiesNode.SelectionType.EntireSubtree || this.hasWildcard)
			{
				return entityType.NavigationProperties();
			}
			IEnumerable<string> enumerable = this.selectedProperties;
			if (this.children != null)
			{
				enumerable = this.children.Keys.Concat(enumerable);
			}
			IEnumerable<IEdmNavigationProperty> enumerable2 = enumerable.Select(new Func<string, IEdmProperty>(entityType.FindProperty)).OfType<IEdmNavigationProperty>();
			foreach (SelectedPropertiesNode selectedPropertiesNode in this.GetMatchingTypeSegments(entityType))
			{
				enumerable2 = enumerable2.Concat(selectedPropertiesNode.GetSelectedNavigationProperties(entityType));
			}
			return enumerable2.Distinct<IEdmNavigationProperty>();
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0001E10C File Offset: 0x0001C30C
		internal IDictionary<string, IEdmStructuralProperty> GetSelectedStreamProperties(IEdmEntityType entityType)
		{
			if (this.selectionType == SelectedPropertiesNode.SelectionType.Empty)
			{
				return SelectedPropertiesNode.EmptyStreamProperties;
			}
			if (entityType == null)
			{
				return SelectedPropertiesNode.EmptyStreamProperties;
			}
			if (this.selectionType == SelectedPropertiesNode.SelectionType.EntireSubtree || this.hasWildcard)
			{
				return (from sp in entityType.StructuralProperties()
				where sp.Type.IsStream()
				select sp).ToDictionary((IEdmStructuralProperty sp) => sp.Name, StringComparer.Ordinal);
			}
			IDictionary<string, IEdmStructuralProperty> dictionary = (from p in this.selectedProperties.Select(new Func<string, IEdmProperty>(entityType.FindProperty)).OfType<IEdmStructuralProperty>()
			where p.Type.IsStream()
			select p).ToDictionary((IEdmStructuralProperty p) => p.Name, StringComparer.Ordinal);
			foreach (SelectedPropertiesNode selectedPropertiesNode in this.GetMatchingTypeSegments(entityType))
			{
				IDictionary<string, IEdmStructuralProperty> selectedStreamProperties = selectedPropertiesNode.GetSelectedStreamProperties(entityType);
				foreach (KeyValuePair<string, IEdmStructuralProperty> keyValuePair in selectedStreamProperties)
				{
					dictionary[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			return dictionary;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0001E2A8 File Offset: 0x0001C4A8
		internal bool IsOperationSelected(IEdmEntityType entityType, IEdmFunctionImport operation, bool mustBeContainerQualified)
		{
			mustBeContainerQualified = (mustBeContainerQualified || entityType.FindProperty(operation.Name) != null);
			return this.IsOperationSelectedAtThisLevel(operation, mustBeContainerQualified) || this.GetMatchingTypeSegments(entityType).Any((SelectedPropertiesNode typeSegment) => typeSegment.IsOperationSelectedAtThisLevel(operation, mustBeContainerQualified));
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0001E418 File Offset: 0x0001C618
		private static IEnumerable<IEdmEntityType> GetBaseTypesAndSelf(IEdmEntityType entityType)
		{
			for (IEdmEntityType currentType = entityType; currentType != null; currentType = currentType.BaseEntityType())
			{
				yield return currentType;
			}
			yield break;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0001E438 File Offset: 0x0001C638
		private static HashSet<string> CreateSelectedPropertiesHashSet(IEnumerable<string> properties)
		{
			HashSet<string> hashSet = SelectedPropertiesNode.CreateSelectedPropertiesHashSet();
			foreach (string item in properties)
			{
				hashSet.Add(item);
			}
			return hashSet;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001E488 File Offset: 0x0001C688
		private static HashSet<string> CreateSelectedPropertiesHashSet()
		{
			return new HashSet<string>(StringComparer.Ordinal);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0001E700 File Offset: 0x0001C900
		private static IEnumerable<string> GetPossibleMatchesForSelectedOperation(IEdmFunctionImport operation, bool mustBeContainerQualified)
		{
			string operationName = operation.Name;
			string operationNameWithParameters = operation.NameWithParameters();
			if (!mustBeContainerQualified)
			{
				yield return operationName;
			}
			yield return operationNameWithParameters;
			string containerName = operation.Container.Name + ".";
			yield return containerName + "*";
			yield return containerName + operationName;
			yield return containerName + operationNameWithParameters;
			string qualifiedContainerName = operation.Container.FullName() + ".";
			yield return qualifiedContainerName + "*";
			yield return qualifiedContainerName + operationName;
			yield return qualifiedContainerName + operationNameWithParameters;
			yield break;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0001E910 File Offset: 0x0001CB10
		private IEnumerable<SelectedPropertiesNode> GetMatchingTypeSegments(IEdmEntityType entityType)
		{
			if (this.children != null)
			{
				foreach (IEdmEntityType currentType in SelectedPropertiesNode.GetBaseTypesAndSelf(entityType))
				{
					SelectedPropertiesNode typeSegmentChild;
					if (this.children.TryGetValue(currentType.FullName(), out typeSegmentChild))
					{
						if (typeSegmentChild.hasWildcard)
						{
							throw new ODataException(Strings.SelectedPropertiesNode_StarSegmentAfterTypeSegment);
						}
						yield return typeSegmentChild;
					}
				}
			}
			yield break;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001E934 File Offset: 0x0001CB34
		private void ParsePathSegment(string[] segments, int index)
		{
			string text = segments[index].Trim();
			if (this.selectedProperties == null)
			{
				this.selectedProperties = SelectedPropertiesNode.CreateSelectedPropertiesHashSet();
			}
			bool flag = string.CompareOrdinal("*", text) == 0;
			if (index != segments.Length - 1)
			{
				if (flag)
				{
					throw new ODataException(Strings.SelectedPropertiesNode_StarSegmentNotLastSegment);
				}
				SelectedPropertiesNode selectedPropertiesNode = this.EnsureChildAnnotation(text);
				selectedPropertiesNode.ParsePathSegment(segments, index + 1);
			}
			else
			{
				this.selectedProperties.Add(text);
			}
			this.hasWildcard = (this.hasWildcard || flag);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0001E9B8 File Offset: 0x0001CBB8
		private SelectedPropertiesNode EnsureChildAnnotation(string segmentName)
		{
			if (this.children == null)
			{
				this.children = new Dictionary<string, SelectedPropertiesNode>(StringComparer.Ordinal);
			}
			SelectedPropertiesNode selectedPropertiesNode;
			if (!this.children.TryGetValue(segmentName, out selectedPropertiesNode))
			{
				selectedPropertiesNode = new SelectedPropertiesNode(SelectedPropertiesNode.SelectionType.PartialSubtree);
				this.children.Add(segmentName, selectedPropertiesNode);
			}
			return selectedPropertiesNode;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0001EA10 File Offset: 0x0001CC10
		private bool IsOperationSelectedAtThisLevel(IEdmFunctionImport operation, bool mustBeContainerQualified)
		{
			return this.selectionType != SelectedPropertiesNode.SelectionType.Empty && (this.selectionType == SelectedPropertiesNode.SelectionType.EntireSubtree || SelectedPropertiesNode.GetPossibleMatchesForSelectedOperation(operation, mustBeContainerQualified).Any((string possibleMatch) => this.selectedProperties.Contains(possibleMatch)));
		}

		// Token: 0x04000383 RID: 899
		private const char PathSeparator = '/';

		// Token: 0x04000384 RID: 900
		private const char ItemSeparator = ',';

		// Token: 0x04000385 RID: 901
		private static readonly SelectedPropertiesNode Empty = new SelectedPropertiesNode(SelectedPropertiesNode.SelectionType.Empty);

		// Token: 0x04000386 RID: 902
		private static readonly SelectedPropertiesNode EntireSubtree = new SelectedPropertiesNode(SelectedPropertiesNode.SelectionType.EntireSubtree);

		// Token: 0x04000387 RID: 903
		private static readonly Dictionary<string, IEdmStructuralProperty> EmptyStreamProperties = new Dictionary<string, IEdmStructuralProperty>(StringComparer.Ordinal);

		// Token: 0x04000388 RID: 904
		private static readonly IEnumerable<IEdmNavigationProperty> EmptyNavigationProperties = Enumerable.Empty<IEdmNavigationProperty>();

		// Token: 0x04000389 RID: 905
		private readonly SelectedPropertiesNode.SelectionType selectionType;

		// Token: 0x0400038A RID: 906
		private HashSet<string> selectedProperties;

		// Token: 0x0400038B RID: 907
		private Dictionary<string, SelectedPropertiesNode> children;

		// Token: 0x0400038C RID: 908
		private bool hasWildcard;

		// Token: 0x0200015F RID: 351
		private enum SelectionType
		{
			// Token: 0x04000392 RID: 914
			Empty,
			// Token: 0x04000393 RID: 915
			EntireSubtree,
			// Token: 0x04000394 RID: 916
			PartialSubtree
		}
	}
}
