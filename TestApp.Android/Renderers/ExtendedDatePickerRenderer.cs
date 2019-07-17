using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Graphics;
using TestApp.Controls;
using TestApp.Droid.Renderers;
using TestApp.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using DatePicker = Xamarin.Forms.DatePicker;

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRenderer))]
namespace TestApp.Droid.Renderers
{
    public class ExtendedDatePickerRenderer : ViewRenderer<ExtendedDatePicker, PickerEditText>
    {
        private bool _isShowing;
        private DatePickerDialog _dialog;

        public ExtendedDatePickerRenderer(Context context) : base(context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedDatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            if (Control == null)
                SetNativeControl(new PickerEditText(Context));

            SetText();
            SetPlaceholder();
            SetPlaceholderColor();
            SetIsEnabled();
            SetFont();
            SetColor();
            UpdateMinimumDate();
            UpdateMaximumDate();

            Control.KeyListener = null;
            Control.Click += Picker_Click;
            //Control.FocusChange += Picker_FocusChanged;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ExtendedDatePicker.SelectedDateProperty.PropertyName)
                SetText();
            else if (e.PropertyName == DatePicker.MinimumDateProperty.PropertyName)
                UpdateMinimumDate();
            else if (e.PropertyName == DatePicker.MaximumDateProperty.PropertyName)
                UpdateMaximumDate();
            else if (e.PropertyName == ExtendedDatePicker.PlaceholderProperty.PropertyName)
                SetPlaceholder();
            else if (e.PropertyName == ExtendedDatePicker.PlaceholderColorProperty.PropertyName)
                SetPlaceholderColor();
            else if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
                SetPlaceholderColor();
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName || e.PropertyName == DatePicker.TextColorProperty.PropertyName)
                SetColor();
            else if (e.PropertyName == DatePicker.FontFamilyProperty.PropertyName || e.PropertyName == DatePicker.FontSizeProperty.PropertyName)
                SetFont();
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.Click -= Picker_Click;
                //Control.FocusChange -= Picker_FocusChanged;
            }

            if (_dialog != null)
            {
                _dialog.Hide();
                _dialog.Dispose();
                _dialog = null;
            }

            base.Dispose(disposing);
        }


        private void SetText()
        {
            Control.Text = Element.SelectedDate == null ? string.Empty : Element.SelectedDate.Value.ToString(Element.Format);
        }

        private void UpdateMaximumDate()
        {
            if (_dialog == null)
                return;

            var datePicker = _dialog.DatePicker;
            var dateTime = Element.MaximumDate;
            dateTime = dateTime.ToUniversalTime();
            var totalMilliseconds = (long)dateTime.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            datePicker.MaxDate = totalMilliseconds;
        }

        private void UpdateMinimumDate()
        {
            if (_dialog == null)
                return;

            var datePicker = _dialog.DatePicker;
            var dateTime = Element.MinimumDate;
            dateTime = dateTime.ToUniversalTime();
            var totalMilliseconds = (long)dateTime.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            datePicker.MinDate = totalMilliseconds;
        }

        private void SetPlaceholder()
        {
            Control.Hint = Element.Placeholder;
        }

        private void SetPlaceholderColor()
        {
            if (Element.PlaceholderColor != Color.Default)
                Control.SetHintTextColor(Element.PlaceholderColor.ToAndroid());
        }

        private void SetIsEnabled()
        {
            Control.Enabled = Element.IsEnabled;
        }

        private void SetFont()
        {
            Control.TextSize = (float)Element.FontSize;
            Control.Typeface = Helpers.FontExtensions.ToTypeface(Element);
        }

        private void SetColor()
        {
            Control.SetTextColor(Element.TextColor.ToAndroid());
            //Control.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
        }


        private void Picker_Click(object sender, EventArgs e)
        {
            if (_isShowing)
                return;

            ShowDatePicker();
        }

        private void Picker_FocusChanged(object sender, FocusChangeEventArgs e)
        {
            if (e.HasFocus)
                ShowDatePicker();
        }

        private void ShowDatePicker()
        {
            _isShowing = true;

            _dialog = new DatePickerDialog(Context, DatePicker_DateSet, Element.Date.Year, Element.Date.Month - 1, Element.Date.Day);

            _dialog.SetButton("Done", (sender, e) =>
            {
                Element.SelectedDate = _dialog.DatePicker.DateTime;
                Element.UpdateDate();
                SetText();
            });

            _dialog.SetButton2("Cancel", (sender, e) =>
            {
                _dialog.Hide();
            });

            _dialog.SetButton3("Clear", (sender, e) =>
            {
                Element.SelectedDate = null;
                Element.UpdateDate();
                SetText();
            });

            _dialog.DatePicker.DateTime = Element.SelectedDate ?? DateTime.Now;
            _dialog.DismissEvent += (sender, args) => _isShowing = false;
            _dialog.Show();
        }

        private void DatePicker_DateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            //view.Date = e.Date;
            ((IElementController)Element).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            Control.ClearFocus();

            _dialog.Dispose();
            _dialog = null;
        }
    }
}