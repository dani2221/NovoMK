using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace VestiMK.Helpers
{
    public class NavigationPageGradientHeader : NavigationPage
    {
        public NavigationPageGradientHeader(Page root) : base(root)
        {
        }

        public static readonly BindableProperty RightColorProperty =
          BindableProperty.Create(propertyName: nameof(RightColor),
              returnType: typeof(Color),
              declaringType: typeof(NavigationPageGradientHeader),
              defaultValue: Color.Accent);

        public static readonly BindableProperty LeftColorProperty =
           BindableProperty.Create(propertyName: nameof(LeftColor),
               returnType: typeof(Color),
               declaringType: typeof(NavigationPageGradientHeader),
               defaultValue: Color.Accent);

        public Color RightColor
        {
            get { return (Color)GetValue(RightColorProperty); }
            set { SetValue(RightColorProperty, value); }
        }

        public Color LeftColor
        {
            get { return (Color)GetValue(LeftColorProperty); }
            set { SetValue(LeftColorProperty, value); }
        }
    }
}
