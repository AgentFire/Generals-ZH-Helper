using System.Runtime.InteropServices;

namespace GenHelper.MainLoop.Services.WinApi.Members
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner

        #region Lyrics

        public static bool operator ==(Rect one, Rect two)
        {
            return one.Left == two.Left && one.Right == two.Right && one.Top == two.Top && one.Bottom == two.Bottom;
        }
        public static bool operator !=(Rect one, Rect two)
        {
            return !(one == two);
        }
        public override int GetHashCode()
        {
            return unchecked((Left + 5) * (Top - 6) * Right * Bottom);
        }
        public override bool Equals(object obj)
        {
            return obj is Rect rect && this == rect;
        }

        #endregion
    }
}
