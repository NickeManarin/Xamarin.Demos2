using System;
using System.Collections.Generic;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using TestApp.Controls;
using TestApp.iOS.Controls;
using TestApp.iOS.Helpers;
using TestApp.iOS.Renderers;
using TestApp.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRenderer))]
namespace TestApp.iOS.Renderers
{
    public class ExtendedDatePickerRenderer : ViewRenderer<ExtendedDatePicker, UITextField>
    {
        private UIDatePicker _picker;
        private UIColor _defaultTextColor;
        private bool _disposed;
        private bool _useLegacyColorManagement;
        internal static readonly UIColor SeventyPercentGrey = new UIColor(0.7f, 0.7f, 0.7f, 1);
        private readonly Color _defaultPlaceholderColor = SeventyPercentGrey.ToColor();

        private IElementController ElementController => Element;

        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedDatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            if (Control == null)
            {
                var entry = new NoCaretField { BorderStyle = UITextBorderStyle.RoundedRect };
                entry.EditingDidBegin += OnStarted;
                entry.EditingDidEnd += OnEnded;

                _picker = new UIDatePicker
                {
                    Mode = UIDatePickerMode.Date,
                    TimeZone = new NSTimeZone("UTC")
                };
                _picker.ValueChanged += HandleValueChanged;

                var uiToolbar = new UIToolbar(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 44))
                {
                    BarStyle = UIBarStyle.Default,
                    Translucent = true
                };

                var clearButton = new UIBarButtonItem(UIBarButtonSystemItem.Trash, (o, a) =>
                {
                    //Clear the date.
                    Element.SelectedDate = null;
                    Element.UpdateDate();
                    SetText(false);

                    entry.ResignFirstResponder();
                });
                clearButton.Title = "Clear";

                var space = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
                var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, a) =>
                {
                    entry.ResignFirstResponder();
                });
                var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
                {
                    //Set the date.
                    Element.SelectedDate = _picker.Date.ToDateTime();
                    Element.UpdateDate();
                    SetText(false);

                    entry.ResignFirstResponder();
                });

                uiToolbar.SetItems(new[] { clearButton, space, cancelButton, doneButton }, false);

                entry.InputView = _picker;
                entry.InputAccessoryView = uiToolbar;
                entry.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
                entry.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
                entry.InputAssistantItem.LeadingBarButtonGroups = null;
                entry.InputAssistantItem.TrailingBarButtonGroups = null;

                _defaultTextColor = entry.TextColor;
                _useLegacyColorManagement = e.NewElement.UseLegacyColorManagement<DatePicker>();
                SetNativeControl(entry);
            }

            SetText(false);
            UpdateFont();
            UpdateMaximumDate();
            UpdateMinimumDate();
            UpdateTextColor();
            UpdateFlowDirection();
            UpdatePlaceholder();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == DatePicker.DateProperty.PropertyName || e.PropertyName == DatePicker.FormatProperty.PropertyName)
                SetText(true);
            else if (e.PropertyName == DatePicker.MinimumDateProperty.PropertyName)
                UpdateMinimumDate();
            else if (e.PropertyName == DatePicker.MaximumDateProperty.PropertyName)
                UpdateMaximumDate();
            else if (e.PropertyName == DatePicker.TextColorProperty.PropertyName || e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
                UpdateTextColor();
            else if (e.PropertyName == VisualElement.FlowDirectionProperty.PropertyName)
                UpdateFlowDirection();
            else if (e.PropertyName == DatePicker.FontAttributesProperty.PropertyName || e.PropertyName == DatePicker.FontFamilyProperty.PropertyName || e.PropertyName == DatePicker.FontSizeProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == ExtendedDatePicker.PlaceholderProperty.PropertyName || e.PropertyName == ExtendedDatePicker.PlaceholderColorProperty.PropertyName)
                UpdatePlaceholder();
        }

        private void OnEnded(object sender, EventArgs eventArgs)
        {
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        private void OnStarted(object sender, EventArgs eventArgs)
        {
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            //TODO
            //ElementController?.SetValueFromRenderer(DatePicker.DateProperty, _picker.Date.ToDateTime().Date);
        }

        private void SetText(bool animate)
        {
            _picker.SetDate((Element.SelectedDate ?? DateTime.Now).ToNSDate(), animate);

            Control.Text = Element.SelectedDate == null ? string.Empty : Element.SelectedDate.Value.ToString(Element.Format);
        }

        private void UpdateFlowDirection()
        {
            if (((IVisualElementController)ElementController).EffectiveFlowDirection.IsRightToLeft())
            {
                Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
                Control.TextAlignment = UITextAlignment.Right;
            }
            else
            {
                if (!((IVisualElementController)ElementController).EffectiveFlowDirection.IsLeftToRight())
                    return;

                Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
                Control.TextAlignment = UITextAlignment.Left;
            }
        }

        private void UpdateFont()
        {
            var hasBold = Element.FontAttributes.HasFlag(FontAttributes.Bold);
            var hasItalic = Element.FontAttributes.HasFlag(FontAttributes.Italic);

            if (hasBold || hasItalic)
            {
                var withFamily = new UIFontDescriptor().CreateWithFamily(Element.FontFamily);
                var symbolicTraits = UIFontDescriptorSymbolicTraits.ClassUnknown;

                if (hasBold)
                    symbolicTraits |= UIFontDescriptorSymbolicTraits.Bold;
                if (hasItalic)
                    symbolicTraits |= UIFontDescriptorSymbolicTraits.Italic;

                Control.Font = UIFont.FromDescriptor(withFamily.CreateWithTraits(symbolicTraits), (nfloat)Element.FontSize);
                return;
            }

            //Font without any Bold/Italic attribute.
            Control.Font = UIFont.FromName(Element.FontFamily, (float)Element.FontSize);
        }

        private void UpdateMaximumDate()
        {
            _picker.MaximumDate = Element.MaximumDate.ToNSDate();
        }

        private void UpdateMinimumDate()
        {
            _picker.MinimumDate = Element.MinimumDate.ToNSDate();
        }

        private void UpdateTextColor()
        {
            var textColor = Element.TextColor;

            if (textColor.IsDefault || !Element.IsEnabled && _useLegacyColorManagement)
                Control.TextColor = _defaultTextColor;
            else
                Control.TextColor = textColor.ToUIColor();

            Control.Text = Control.Text;
        }

        private void UpdatePlaceholder()
        {
            var placeholder = (FormattedString)Element.Placeholder;

            if (placeholder == null)
                return;

            var placeholderColor = Element.PlaceholderColor;
            var font = Font.OfSize(Element.FontFamily, (float)Element.FontSize);

            if (_useLegacyColorManagement)
            {
                var defaultForegroundColor = placeholderColor.IsDefault || !Element.IsEnabled ? _defaultPlaceholderColor : placeholderColor;

                Control.AttributedPlaceholder = placeholder.ToAttributed(font, defaultForegroundColor);
            }
            else
            {
                var defaultForegroundColor = placeholderColor.IsDefault ? _defaultPlaceholderColor : placeholderColor;

                Control.AttributedPlaceholder = placeholder.ToAttributed(font, defaultForegroundColor);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
            {
                _defaultTextColor = null;

                if (_picker != null)
                {
                    _picker.RemoveFromSuperview();
                    _picker.ValueChanged -= HandleValueChanged;
                    _picker.Dispose();
                    _picker = null;
                }

                if (Control != null)
                {
                    Control.EditingDidBegin -= OnStarted;
                    Control.EditingDidEnd -= OnEnded;
                }
            }

            base.Dispose(disposing);
        }

        private void AddClearButton()
        {
            if (!(Control.InputAccessoryView is UIToolbar originalToolbar) || originalToolbar.Items.Length > 2)
                return;

            var clearButton = new UIBarButtonItem("Clear", UIBarButtonItemStyle.Plain, (sender, ev) =>
            {
                var baseDatePicker = Element as ExtendedDatePicker;

                Element.Unfocus();
                Element.Date = DateTime.Now;
                //Element.
                //baseDatePicker.ClearDate();
            });

            var cancelButton = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, (sender, ev) =>
            {
                Element.Unfocus();
            });

            var doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, (sender, ev) =>
            {
                //var baseDatePicker = Element as ExtendedDatePicker;

                Element.Unfocus();
                Element.Date = DateTime.Now;
            });

            var newItems = new List<UIBarButtonItem> { clearButton, cancelButton, doneButton };
            //newItems.AddRange(originalToolbar.Items);

            originalToolbar.Items = newItems.ToArray();
            originalToolbar.SetNeedsDisplay();
        }
    }
}