namespace CommunityHub.UI.Services
{
    namespace CommunityHub.UI.Services
    {
        public class CookieReaderService : ICookieReaderService
        {
            private readonly Dictionary<string, string> _cookies = new();
            private readonly Dictionary<string, DateTime> _cookieExpirations = new();

            public Dictionary<string, string> ReadCookiesFromResponse(HttpResponseMessage response)
            {
                _cookies.Clear();
                _cookieExpirations.Clear();

                if (response.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
                {
                    foreach (var cookie in cookieHeaders)
                    {
                        var cookieParts = cookie.Split(';')[0];
                        var keyValue = cookieParts.Split('=', 2);

                        if (keyValue.Length == 2)
                        {
                            _cookies[keyValue[0].Trim()] = keyValue[1].Trim();

                            var expiresPart = cookie.Split(';').FirstOrDefault(p => p.Trim().StartsWith("expires="));
                            if (!string.IsNullOrEmpty(expiresPart))
                            {
                                var expiresValue = expiresPart.Split('=')[1].Trim();
                                if (DateTime.TryParse(expiresValue, out var expiryDate))
                                {
                                    _cookieExpirations[keyValue[0].Trim()] = expiryDate;
                                }
                            }
                        }
                    }
                }

                return _cookies;
            }

            public DateTime GetCookieExpiry(string name)
            {
                if (_cookieExpirations.TryGetValue(name, out var expiryDate))
                {
                    return expiryDate;
                }

                throw new ArgumentNullException("Cookie expiry date ot set. ", nameof(name));
            }

            public string GetCookie(string name) => _cookies.TryGetValue(name, out var value) ? value : null;

            public void ClearCookies()
            {
                _cookies.Clear();
                _cookieExpirations.Clear();
            }
        }
    }
}
