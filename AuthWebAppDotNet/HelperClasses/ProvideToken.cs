using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses
{
    public class ProvideToken
    {
        ITokenProvider _tokenProvider;
        public ProvideToken(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;

        }

        public async Task<string> GetTokenAsync()
        {
            return await _tokenProvider.getTokenAsync();
        }
    }
}
