using ReheeCmf.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReheeCmf.Commons.DTOs
{
	[DataContract]
	public class NavActionItemDTO
	{
		[DataMember]
		public bool IsHeader { get; set; }
		[DataMember]
		public string? Label { get; set; }
		[DataMember]
		public string? Icon { get; set; }
		[DataMember]
		public string? Link { get; set; }
		[DataMember]
		public bool ActiveMatchAll { get; set; }
		[DataMember]
		public bool Badge { get; set; }
		[DataMember]
		public int BadgeNumber { get; set; }
		[DataMember]
		public EnumBadgeType Type { get; set; }
		[DataMember]
		public IEnumerable<NavActionItemDTO>? SubItem { get; set; }
		[DataMember]
		public string? Permission { get; set; }
		[DataMember]
		public bool IsModule { get; set; }
		[DataMember]
		public string? ModuleName { get; set; }
		[DataMember]
		public bool IsReserved { get; set; }
	}
}
