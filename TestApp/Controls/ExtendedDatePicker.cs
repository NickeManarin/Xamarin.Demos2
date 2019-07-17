using System;
using Xamarin.Forms;

namespace TestApp.Controls
{
    public class ExtendedDatePicker : DatePicker
    {
        #region Properties

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ExtendedDatePicker), "");
        public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(ExtendedDatePicker), null, BindingMode.TwoWay);
        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(ExtendedDatePicker), Color.Default);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set
            {
                SetValue(SelectedDateProperty, value);
                UpdateDate();
            }
        }

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        #endregion

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateDate();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsFocusedProperty.PropertyName)
            {
                if (IsFocused)
                {
                    if (!SelectedDate.HasValue)
                        Date = (DateTime)DateProperty.DefaultValue;
                }
                else
                {
                    OnPropertyChanged(DateProperty.PropertyName);
                }
            }

            if (propertyName == SelectedDateProperty.PropertyName)
            {
                if (SelectedDate.HasValue)
                    Date = SelectedDate.Value;
            }
        }

        public void UpdateDate()
        {
            if (SelectedDate.HasValue)
                Date = SelectedDate.Value;
            else
                Date = (DateTime)DateProperty.DefaultValue;
        }
    }
}