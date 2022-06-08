using Krypton.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstReader.App.Controls.Helpers
{
    public static class KryptonDockingHelper
    {
        public static KryptonPage NewDocument(string name, Control control)
        {
            KryptonPage page = NewPage(name, control);

            // Document pages cannot be docked or auto hidden
            //page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);

            return page;
        }

        public static KryptonPage NewPage(Control content)
            => NewPage(content.Name, content);

        public static KryptonPage NewPage(string name, Control content)
        {
            // Create new page with title and image
            KryptonPage p = new()
            {
                Text = name,
                TextTitle = name,
                TextDescription = name,
                //ImageSmall = (Bitmap)imageListSmall.Images[image]
            };

            // Add the control for display inside the page
            content.Dock = DockStyle.Fill;
            p.Controls.Add(content);

            p.Flags = (int)(KryptonPageFlags.AllowConfigSave |
                      KryptonPageFlags.AllowPageDrag |
                      KryptonPageFlags.DockingAllowDocked |
                      KryptonPageFlags.AllowPageReorder |
                      KryptonPageFlags.DockingAllowAutoHidden);

            p.AllowDrop = true;

            p.MinimumSize = content.MinimumSize;
            return p;
        }

    }
}
