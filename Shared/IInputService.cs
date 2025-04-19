using System.Drawing;
using System.Numerics;
namespace Shared
{
    public interface IInputService { Point? GetClick(); }
    public interface IRenderService
    {
        void DrawTile(Rectangle dst);
        void DrawPlayer(Vector2 pos);
    }
}
