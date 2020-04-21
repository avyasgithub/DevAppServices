using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses
{
   public interface ITokenProvider
    {
         Task<string> getTokenAsync();
        
    }
}
