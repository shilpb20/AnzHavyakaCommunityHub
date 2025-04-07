public static partial class UiRoute
{
    public static class Error
    {
        public const string ByStatusCode = $"{UiRoutePrefix.Error}/status/{{statusCode}}";
        public const string GeneralError = $"{UiRoutePrefix.Error}";
        public const string PageNotFound = $"{UiRoutePrefix.Error}/status/404";
    }
}
