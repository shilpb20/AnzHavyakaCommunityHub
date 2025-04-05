using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Core
{
    public static class ErrorCode
    {
        public const string UnknownError = "Unknown Error";
        public const string ResponseParseError = "Response Parse Error";
        public const string HttpError = "Http Error";
        public const string RequestError = "Request Error";

        public const string InvalidData = "Invalid Data";

        public const string DuplicateUser = "Duplicate User";
    }
}
