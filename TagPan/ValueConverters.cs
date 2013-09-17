using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xaml;
using System.Windows.Converters;
using System.Windows.Markup;
using System.Windows.Data;

namespace TagPan
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    [ValueConversion(typeof(object), typeof(string))]
    public class IntToStringConverter : BaseConverter, IValueConverter    
    {        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)        
        {            
            string format = parameter as string;
            if (!string.IsNullOrEmpty(format))
            {
                return string.Format(culture, format, value);
            }
            else
            {
                return value.ToString();
            }
        }
 
        public object ConvertBack(object value, Type targetType, object parameter,
                        System.Globalization.CultureInfo culture)
        {
            return null; 
        }            
    }
}
