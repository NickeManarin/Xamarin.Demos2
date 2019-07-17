using System.Windows.Input;
using Sharpnado.Presentation.Forms.RenderedViews;
using Xamarin.Forms;

namespace TestApp.Controls
{
    public class ExtendedViewCell : DraggableViewCell
    {
        public static readonly BindableProperty OnTappedCommandProperty = BindableProperty.Create(nameof(OnTappedCommand), typeof(ICommand), typeof(ExtendedViewCell), null, propertyChanged: OnCommandPropertyChanged);
        public static readonly BindableProperty OnTappedCommandParameterProperty = BindableProperty.Create("OnTappedCommandParameter", typeof(object), typeof(ExtendedViewCell), null, propertyChanged: OnCommandParameterPropertyChanged);

        public ICommand OnTappedCommand
        {
            get => (ICommand)GetValue(OnTappedCommandProperty);
            set => SetValue(OnTappedCommandProperty, value);
        }

        public object OnTappedCommandParameter
        {
            get => (ICommand)GetValue(OnTappedCommandParameterProperty);
            set => SetValue(OnTappedCommandParameterProperty, value);
        }

        private static void OnCommandParameterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //Stuff to handle changes, not really required
        }
        private static void OnCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //More stuff to handle changes
        }

        protected override void OnTapped()
        {
            if (OnTappedCommand == null)
                return;

            OnTappedCommand.Execute(OnTappedCommandParameter);

            base.OnTapped();
        }

    }
}