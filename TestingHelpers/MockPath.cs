using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// PathWrapper calls direct to Path but all this does is string manipulation so we can inherit directly from PathWrapper as no IO is done
    /// </summary>
    public class MockPath : PathWrapper
    {
        
    }
}
