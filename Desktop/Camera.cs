using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

public class Camera
{
    private GraphicsDevice GraphicsDevice;
    public Matrix ViewMatrix { get; private set; } = Matrix.Identity;
    protected Vector2 Position = Vector2.Zero;

    private int MapWidth, MapHeight; // в пикселях

    public Camera(GraphicsDevice graphicsDevice, int mapWidth, int mapHeight)
    {
        GraphicsDevice = graphicsDevice;
        MapWidth = mapWidth;
        MapHeight = mapHeight;
    }

    public void Move(Vector2 delta)
    {
        Position += delta;
        ClampToBounds();
        UpdateMatrix();
    }

    private void ClampToBounds()
    {
        int screenWidth = GraphicsDevice.Viewport.Width;
        int screenHeight = GraphicsDevice.Viewport.Height;

        Position.X = MathHelper.Clamp(Position.X, 0, Math.Max(0, MapWidth - screenWidth));
        Position.Y = MathHelper.Clamp(Position.Y, 0, Math.Max(0, MapHeight - screenHeight));
    }

    private void UpdateMatrix()
    {
        ViewMatrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0);
    }
}
