using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Infrastructure
{
    public interface IProgressFacade
    {
        event EventHandler OnProgress;
        void Update(double current, double total);
    }
}
