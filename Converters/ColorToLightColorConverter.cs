using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TDEduEnglish.Converters
{
    public class ColorToLightColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Color color) {
                // Tạo màu nhạt hơn bằng cách giảm opacity
                return Color.FromArgb(26, color.R, color.G, color.B); // ~10% opacity
            }

            if (value is SolidColorBrush brush) {
                var color1 = brush.Color;
                return new SolidColorBrush(Color.FromArgb(26, color1.R, color1.G, color1.B));
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
