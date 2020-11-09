using GenHelper.MainLoop.Services.WinApi.Members;
using System.Collections.Generic;
using WindowsInput;
using WindowsInput.Native;

namespace GenHelper.MainLoop.Services
{
    internal sealed class MouseTranslator
    {
        private static readonly InputSimulator sim = new InputSimulator();
        private delegate bool HitRangeDelegate(Win32Point point, Rect bounds);

        private sealed class Border
        {
            public VirtualKeyCode Key { get; }
            public HitRangeDelegate Predicate { get; }

            private bool _state = false;

            public Border(VirtualKeyCode key, HitRangeDelegate predicate)
            {
                Key = key;
                Predicate = predicate;
            }

            public void EnsureState(bool state)
            {
                if (state && !_state)
                {
                    _state = true;
                    sim.Keyboard.KeyDown(Key);
                }
                else if (!state && _state)
                {
                    _state = false;
                    sim.Keyboard.KeyUp(Key);
                }
            }
        }

        private readonly IReadOnlyList<Border> _borders;

        public MouseTranslator(int margin)
        {
            _borders = new List<Border>
            {
                new Border(VirtualKeyCode.LEFT, (p, r) => p.X < r.Left + margin),
                new Border(VirtualKeyCode.UP, (p, r) => p.Y < r.Top + margin),
                new Border(VirtualKeyCode.RIGHT, (p, r) => p.X > r.Right - margin),
                new Border(VirtualKeyCode.DOWN, (p, r) => p.Y > r.Bottom - margin)
            };
        }

        public void Tick(Win32Point point, Rect gameArea)
        {
            foreach (Border border in _borders)
            {
                bool hitTest = border.Predicate(point, gameArea);
                border.EnsureState(hitTest);
            }

        }
        public void Release()
        {
            foreach (Border border in _borders)
            {
                border.EnsureState(false);
            }
        }
    }
}
