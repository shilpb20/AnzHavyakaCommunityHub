namespace CommunityHub.Core.Messages
{
    public static class ErrorMessage
    {
        public const string InvalidData = "Invalid Data";
        public const string InvalidStateTransition = "Invalid state transition. Current state: {0}.";
        public const string ValueNullOrEmpty = "{0} cannot be null or empty.";

        public const string InvalidId = "Id must be a positive number";
    }
}
