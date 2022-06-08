using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstReader.App.Common
{
    public interface IXstElementDoubleClickable<T> : IXstElementSelectable<T>
         where T : XstElement
    {
        public event EventHandler<XstElementEventArgs>? DoubleClickItem;
    }
}
