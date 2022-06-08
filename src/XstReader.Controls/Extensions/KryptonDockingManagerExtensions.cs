using Krypton.Docking;
using Krypton.Navigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XstReader.App.Controls.Helpers;

namespace XstReader.App.Controls
{
    public static class KryptonDockingManagerExtensions
    {

        public static KryptonDockingDockspace AddXstDockSpaceInTabs(this KryptonDockingManager manager, DockingEdge dockingEdge, params Control[] controls)
        {
            return manager.AddDockspace("Control", dockingEdge, controls.Select(c => KryptonDockingHelper.NewPage(c)).ToArray());
        }

        public static KryptonDockingDockspace AddXstDockSpaceInStack(this KryptonDockingManager manager, DockingEdge dockingEdge, Control control, params Control[] controls)
        {
            return manager.AddDockspace("Control", dockingEdge, new KryptonPage[] { KryptonDockingHelper.NewPage(control) }, controls.Select(c => KryptonDockingHelper.NewPage(c)).ToArray());
        }

    }
}
