using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagPan
{
    interface IConsoleSelElement
    {
        List<int> getCorrespondingSel();
        string getCorrespondingStr();
    }
}
