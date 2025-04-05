using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Core.Models
{
    public class PasswordResetResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static PasswordResetResult Failed(params string[] errors)
        {
            return new PasswordResetResult { Success = false, Errors = errors.ToList() };
        }

        public static PasswordResetResult SuccessResult()
        {
            return new PasswordResetResult { Success = true };
        }
    }

}
