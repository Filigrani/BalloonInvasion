using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;

namespace BalloonInvasion
{
    internal class CursorComponent : Component
    {
        public Renderer MyRenderer = null;
        public DebugRenderer MyDebugRenderer = null;
        public int CursorType = 0;
        public Vector2 ColisionBounds = new Vector2(24, 24);
        public Vector2 ColisionOffset = new Vector2(4, 4);
        public Vector2 Offset = new Vector2(-16, -16);
        public PlayerController Controller = null;

        public override void OnAttached()
        {
            ChangeCursor(Controller.MyShooter.CurrentWeapon.Info.CursorType);
        }

        public void ChangeCursor(string Type)
        {
            if (Type == "Crosshair" || Type == "CrosshairPartly")
            {
                MyRenderer.SetSprite(ContentManager.GetSprite(Type), new Microsoft.Xna.Framework.Rectangle(0, 0, 32, 32));
                Offset = new Vector2(-16, -16);
                MyRenderer.FrameOffset = 32;
                ColisionBounds = new Vector2(24, 24);
                ColisionOffset = new Vector2(4, 4);
            } else if(Type == "CrosshairBig")
            {
                MyRenderer.SetSprite(ContentManager.GetSprite(Type), new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64));
                Offset = new Vector2(-32, -32);
                MyRenderer.FrameOffset = 64;
                ColisionBounds = new Vector2(56, 56);
            } else if (Type == "CrosshairHorizontal")
            {
                MyRenderer.SetSprite(ContentManager.GetSprite(Type), new Microsoft.Xna.Framework.Rectangle(0, 0, 56, 16));
                Offset = new Vector2(-28, -8);
                MyRenderer.FrameOffset = 56;
                ColisionBounds = new Vector2(56, 32);
                ColisionOffset = new Vector2(0, -8);
            }
            MyDebugRenderer.RenderOffset = ColisionOffset;
            MyDebugRenderer.RenderBounds = new Microsoft.Xna.Framework.Rectangle(0, 0, (int)ColisionBounds.X, (int)ColisionBounds.Y);
        }

        public override void Update(GameTime gameTime)
        {
            Microsoft.Xna.Framework.Point MosPos = Mouse.GetState().Position;
            Object.Position = new Vector2(MosPos.X+Offset.X, MosPos.Y+Offset.Y);

            MyRenderer.Visible = CursorType != 0;
            if (Controller.MyShooter.CurrentWeapon.CanShot())
            {
                MyRenderer.Frame = 0;
            } else
            {
                MyRenderer.Frame = 1;
            }
        }
        public RectangleF GetPhysicalColision()
        {
            Vector2 ColOffset = ColisionOffset * LayersManager.Scaler;
            Vector2 TempBound = ColisionBounds;
            if (Object != null)
            {
                return new RectangleF(Object.Position.X + ColOffset.X, Object.Position.Y + ColOffset.Y, TempBound.X, TempBound.Y);
            }
            return new RectangleF(0, 0, 0, 0);
        }
    }
}
