using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Gateway
{
    public interface IContext
    {
        void TransferText(string text);
    }
}
