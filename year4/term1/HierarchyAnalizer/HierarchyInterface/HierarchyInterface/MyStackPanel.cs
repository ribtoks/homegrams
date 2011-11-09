using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace HierarchyInterface
{
    public class MyStackPanel : StackPanel
    {
        protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
        {
            return new ObservableUIElementCollection(this, logicalParent);
        }
    }
}
