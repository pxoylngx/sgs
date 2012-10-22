﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

using Sanguosha.Core.Cards;

namespace Sanguosha.UI.Controls
{
    public class CardToolTipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string convertType = parameter as string;
            if (convertType == null || value == null) return null;
            if (convertType == "Name" || convertType == "Description" || convertType == "Usage")
            {
                try
                {
                    return Application.Current.Resources[string.Format("Card.{0}.{1}", value.ToString(), convertType)];
                }
                catch (Exception)
                {
                    Trace.TraceWarning("Cannot find card {0}'s {1}", value.ToString(), convertType.ToLower());
                }
            }
            else if (convertType == "Suit")
            {
                try
                {
                    return Application.Current.Resources[string.Format("Card.Suit.{0}.SuitText", value.ToString(), convertType)];
                }
                catch (Exception)
                {
                    Trace.TraceWarning("Cannot find card suit: {0}", value.ToString());
                }
            }
            else if (convertType == "SuitColor")
            {
                try
                {
                    return Application.Current.Resources[string.Format("Card.Suit.{0}.SuitBrush", value.ToString(), convertType)];
                }
                catch (Exception)
                {
                    Trace.TraceWarning("Cannot find card suit color: {0}", value.ToString());
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
