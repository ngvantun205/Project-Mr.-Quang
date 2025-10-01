﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TDEduEnglish.Converters {
    public class InverseBoolToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool isVisible) {
                return isVisible ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Visibility visibility) {
                return visibility != Visibility.Visible;
            }
            return true;
        }
    }
}
