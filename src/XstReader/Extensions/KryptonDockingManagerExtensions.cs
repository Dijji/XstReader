// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using Krypton.Docking;
using Krypton.Navigator;
using XstReader.App.Helpers;

namespace XstReader.App
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
