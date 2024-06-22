using System;
namespace Inputs
{
    public interface IInputSystem
    {
        public event EventHandler<InputEventArgs> PointerDown;
        public event EventHandler<InputEventArgs> PointerDrag;
        public event EventHandler<InputEventArgs> PointerUp;
    }
}
