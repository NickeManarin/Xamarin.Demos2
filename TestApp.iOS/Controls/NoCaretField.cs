using CoreGraphics;
using UIKit;

namespace TestApp.iOS.Controls
{
    internal sealed class NoCaretField : UITextField
    {
        public NoCaretField() : base(new CGRect())
        {
            SpellCheckingType = UITextSpellCheckingType.No;
            AutocorrectionType = UITextAutocorrectionType.No;
            AutocapitalizationType = UITextAutocapitalizationType.None;
        }

        public override CGRect GetCaretRectForPosition(UITextPosition position)
        {
            return new CGRect();
        }
    }
}