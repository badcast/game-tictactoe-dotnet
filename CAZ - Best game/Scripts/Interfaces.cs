using System.Windows;

namespace CAZ
{
    public interface IScreen : IStyle
    {
        void Update();
        void Created();
        void Showed();
        void Closed();
        UIElement GetElement();
	}


    public interface IStyle
    {
        void SetStyle(DesignManager design);
	}
}
