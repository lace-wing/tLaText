using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tLaText.Core
{
    internal abstract class LaAction
    {
        public abstract string Trigger { get; }

        public virtual void Action(LaText text, string[] args)
        {

        }
    }
}
