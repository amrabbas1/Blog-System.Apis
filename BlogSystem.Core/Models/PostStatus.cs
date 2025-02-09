using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Models
{
    public enum PostStatus
    {
        [EnumMember(Value = "Published")]
        Published,
        [EnumMember(Value = "Draft")]
        Draft,
        [EnumMember(Value = "Archived")]
        Archived
    }
}
