using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BalloonInvasion
{
    internal class Collision : Component
    {
        public static List<Collision> Coliders = new List<Collision>();
        public static List<Cell> Cells = new List<Cell>();
        public List<int> BelongCells = new List<int>();
        public static int CellsRow = 0;
        public int Height = 0;
        public int Widgth = 0;

        public class Cell
        {
            public Rectangle Rect = Rectangle.Empty;
            public List<Collision> Objects = new List<Collision>();
            public int Index = 0;

            public Renderer DebugRenderer = null;

            public Cell(int x, int y, int endx, int endy)
            {
                Rect = new Rectangle(x, y, endx-x, endy-y);
            }
        }

        public void Spawned()
        {
            Point Position = Object.Position.ToPoint();
            Rectangle Rect = new Rectangle(Position.X, Position.Y, Widgth, Height);
            foreach (Cell C in Cells)
            {
                if(Rect.Intersects(C.Rect))
                {
                    C.Objects.Add(this);
                    BelongCells.Add(C.Index);
                }
            }
        }
        public static void Rebuild()
        {
            Coliders.Clear();
            Cells.Clear();
            LocateCells();
        }

        public static void LocateCells()
        {
            int SceneW = 7000;
            int SceneH = 7000;
            int CellW = 300;
            int CellH = 300;
            int X = 0;
            int Y = 0;
            CellsRow = (int)Math.Ceiling((double)SceneW / CellW);

            while (true)
            {
                int EndX = X + CellW - 1;
                int EndY = Y + CellH - 1;

                if (EndX >= SceneW)
                {
                    EndX = SceneW - 1;
                }
                if (EndY >= SceneH)
                {
                    EndX = SceneH - 1;
                }

                if (Y > SceneH - 1)
                {
                    break;
                }

                if (X > SceneW - 1)
                {
                    Y = EndY + 1;
                    X = 0;
                    continue;
                }

                Cell C = new Cell(X, Y, EndX, EndY);
                C.Index = Cells.Count - 1;
                Cells.Add(C);
                X = EndX + 1;
            }
        }

        public void TryMove(Vector2 DesiredPosition)
        {
            Point Position = DesiredPosition.ToPoint();
            Rectangle Rect = new Rectangle(Position.X, Position.Y, Widgth, Height);

            foreach (Collision C in Coliders)
            {
                if(C != this)
                {
                    Point OtherPosition = C.Object.Position.ToPoint();
                    Rectangle OtherRect = new Rectangle(OtherPosition.X, OtherPosition.Y, C.Widgth, C.Height);
                    if (OtherRect.Intersects(Rect))
                    {
                        return;
                    }
                }
            }

        }

        public override void Update(GameTime gameTime)
        {
            //Point Position = Object.Position.ToPoint();
            //Rectangle Rect = new Rectangle(Position.X, Position.Y, Widgth, Height);

            //for (int i = BelongCells.Count-1; i >= 0; i++)
            //{
            //    Cell C = Cells[BelongCells[i]];
            //    if (!C.Rect.Intersects(Rect))
            //    {
            //        BelongCells.RemoveAt(i);
            //    }
            //}
        }
    }
}
