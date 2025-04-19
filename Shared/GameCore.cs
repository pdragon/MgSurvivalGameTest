using System.Numerics;
using System;
using System.Drawing;
namespace Shared
{
    public class GameCore
    {
        //protected readonly IInputService Input;
        public TileMap Map;
        public Player Player;

        //protected GameCore(IInputService input)
        public GameCore()
        {
            //Input = input;
            //Map = new TileMap(width: 100, height: 100, tileSize: 32);
            Player = new Player(startX: 5, startY: 5, speed: 200f);
        }

        public virtual void Initialize()
        {
            
        }

        public void Update(float deltaTime, Point? click)
        {
            if (click.HasValue)
                Player.SetDestination(click.Value);
            Player.Update(deltaTime, Map);
        }

        //public abstract void Draw(SpriteBatch spriteBatch);
    }
}
