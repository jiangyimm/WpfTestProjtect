using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfTestProject.Models
{
    public class InfoItemDataTemplateSelector: DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;
            if (element != null)
            {
                var key = (item as Info)?.TemplateKey;
                if (!string.IsNullOrWhiteSpace(key))
                {
                    return element.FindResource(key) as DataTemplate;
                }
               
                if (item is Info)
                {
                    return element.FindResource("InfoItemNone") as DataTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
