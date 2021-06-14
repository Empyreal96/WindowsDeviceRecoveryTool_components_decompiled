using System;
using System.IO.Packaging;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200066F RID: 1647
	internal class CorePropertyEnumerator
	{
		// Token: 0x06006CD5 RID: 27861 RVA: 0x001F4FC0 File Offset: 0x001F31C0
		internal CorePropertyEnumerator(PackageProperties coreProperties, IFILTER_INIT grfFlags, ManagedFullPropSpec[] attributes)
		{
			if (attributes != null && attributes.Length != 0)
			{
				this._attributes = attributes;
			}
			else if ((grfFlags & IFILTER_INIT.IFILTER_INIT_APPLY_INDEX_ATTRIBUTES) == IFILTER_INIT.IFILTER_INIT_APPLY_INDEX_ATTRIBUTES)
			{
				this._attributes = new ManagedFullPropSpec[]
				{
					new ManagedFullPropSpec(FormatId.SummaryInformation, 2U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 3U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 4U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 5U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 6U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 8U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 9U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 11U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 12U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 13U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 2U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 18U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 26U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 27U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 28U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 29U)
				};
			}
			this._coreProperties = coreProperties;
			this._currentIndex = -1;
		}

		// Token: 0x06006CD6 RID: 27862 RVA: 0x001F50FC File Offset: 0x001F32FC
		internal bool MoveNext()
		{
			if (this._attributes == null)
			{
				return false;
			}
			this._currentIndex++;
			while (this._currentIndex < this._attributes.Length)
			{
				if (this._attributes[this._currentIndex].Property.PropType == PropSpecType.Id && this.CurrentValue != null)
				{
					return true;
				}
				this._currentIndex++;
			}
			return false;
		}

		// Token: 0x17001A00 RID: 6656
		// (get) Token: 0x06006CD7 RID: 27863 RVA: 0x001F5166 File Offset: 0x001F3366
		internal Guid CurrentGuid
		{
			get
			{
				this.ValidateCurrent();
				return this._attributes[this._currentIndex].Guid;
			}
		}

		// Token: 0x17001A01 RID: 6657
		// (get) Token: 0x06006CD8 RID: 27864 RVA: 0x001F5180 File Offset: 0x001F3380
		internal uint CurrentPropId
		{
			get
			{
				this.ValidateCurrent();
				return this._attributes[this._currentIndex].Property.PropId;
			}
		}

		// Token: 0x17001A02 RID: 6658
		// (get) Token: 0x06006CD9 RID: 27865 RVA: 0x001F519F File Offset: 0x001F339F
		internal object CurrentValue
		{
			get
			{
				this.ValidateCurrent();
				return this.GetValue(this.CurrentGuid, this.CurrentPropId);
			}
		}

		// Token: 0x06006CDA RID: 27866 RVA: 0x001F51B9 File Offset: 0x001F33B9
		private void ValidateCurrent()
		{
			if (this._currentIndex < 0 || this._currentIndex >= this._attributes.Length)
			{
				throw new InvalidOperationException(SR.Get("CorePropertyEnumeratorPositionedOutOfBounds"));
			}
		}

		// Token: 0x06006CDB RID: 27867 RVA: 0x001F51E4 File Offset: 0x001F33E4
		private object GetValue(Guid guid, uint propId)
		{
			if (guid == FormatId.SummaryInformation)
			{
				switch (propId)
				{
				case 2U:
					return this._coreProperties.Title;
				case 3U:
					return this._coreProperties.Subject;
				case 4U:
					return this._coreProperties.Creator;
				case 5U:
					return this._coreProperties.Keywords;
				case 6U:
					return this._coreProperties.Description;
				case 8U:
					return this._coreProperties.LastModifiedBy;
				case 9U:
					return this._coreProperties.Revision;
				case 11U:
					if (this._coreProperties.LastPrinted != null)
					{
						return this._coreProperties.LastPrinted.Value;
					}
					return null;
				case 12U:
					if (this._coreProperties.Created != null)
					{
						return this._coreProperties.Created.Value;
					}
					return null;
				case 13U:
					if (this._coreProperties.Modified != null)
					{
						return this._coreProperties.Modified.Value;
					}
					return null;
				}
			}
			else if (guid == FormatId.DocumentSummaryInformation)
			{
				if (propId == 2U)
				{
					return this._coreProperties.Category;
				}
				if (propId == 18U)
				{
					return this._coreProperties.Identifier;
				}
				switch (propId)
				{
				case 26U:
					return this._coreProperties.ContentType;
				case 27U:
					return this._coreProperties.Language;
				case 28U:
					return this._coreProperties.Version;
				case 29U:
					return this._coreProperties.ContentStatus;
				}
			}
			return null;
		}

		// Token: 0x04003575 RID: 13685
		private PackageProperties _coreProperties;

		// Token: 0x04003576 RID: 13686
		private ManagedFullPropSpec[] _attributes;

		// Token: 0x04003577 RID: 13687
		private int _currentIndex;
	}
}
