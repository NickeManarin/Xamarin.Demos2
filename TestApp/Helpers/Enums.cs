namespace TestApp.Helpers
{
    public static class Enums
    {
        public enum GradientDirection
        {
            ToRight,
            ToLeft,
            ToTop,
            ToBottom,
            ToTopLeft,
            ToTopRight,
            ToBottomLeft,
            ToBottomRight
        }

        public enum StatusBandMode
        {
            Sucess,
            Info,
            Warning,
            Error,
        }

        public enum RoundedCorners
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 4,
            Bottom = 8,
            Vertical = Top | Bottom,
            Horizontal = Left | Right,
            All = Vertical | Horizontal
        }
    }
}