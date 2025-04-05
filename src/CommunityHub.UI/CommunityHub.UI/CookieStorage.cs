namespace CommunityHub.UI
{
    public class CookieStorage : ICookieStorage
    {
        private readonly ISession _session;

        public CookieStorage(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public void SetCookie(string name, string value, DateTime expiry)
        {
            _session.SetString(name, value);
            _session.SetString(name + "_expiry", expiry.ToString());
        }

        public bool IsCookiePresent()
        {
            return _session.Keys.Any(k => !k.EndsWith("_expiry") && _session.GetString(k) != null);
        }

        public string GetCookie(string name)
        {
            return _session.GetString(name);
        }

        public void ClearCookies()
        {
            _session.Clear();
        }
    }
}