using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Core.Enums
{
    public enum eDuplicateStatus
    {
        [Description("No Duplicate")]
        NoDuplicate,
        [Description("User email already exists.")]
        DuplicateUserEmail,
        [Description("User contact number already exists.")]
        DuplicateUserContact,
        [Description("Spouse email already exists.")]
        DuplicateSpouseEmail,
        [Description("Spouse contact number already exists.")]
        DuplicateSpouseContact
    }
}
