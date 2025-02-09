using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Models
{
    public enum UserRole
    {
        [EnumMember(Value = "Admin")]
        Admin,
        [EnumMember(Value = "Editor")]
        Editor,
        [EnumMember(Value = "Reader")]
        Reader
    }
}
