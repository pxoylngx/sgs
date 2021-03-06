﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Sanguosha.UI.Animations;

namespace Sanguosha.UI.Controls
{
    /// <summary>
    /// Interaction logic for LoseHealthAnimation.xaml
    /// </summary>
    public class BigEquipBorder : FrameBasedAnimation
    {
        static List<ImageSource> frames;

        static BigEquipBorder()
        {
            frames = LoadFrames("pack://application:,,,/Controls;component/Views/Resources/EquipButton/border", 10);
        }

        public BigEquipBorder()
        {
            this.Loaded += new System.Windows.RoutedEventHandler(BigEquipBorder_Loaded);
        }

        void BigEquipBorder_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            IsActive = true;
        }

        public override List<ImageSource> Frames
        {
            get
            {
                return frames;
            }
        }
    }

}
