using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using System.Threading;

namespace DynamicGrid
{
    public class DynamicGrid
    {
        GridPoint[,] Points;
        List<GridSpring> VertSprings = new List<GridSpring>();
        List<GridSpring> HorzSprings = new List<GridSpring>();
        Rectangle Field;

        public DynamicGrid(Rectangle Field, int PointSpacing)
        {
            // Create the Points
            Points = new GridPoint[Field.Width / PointSpacing + 2, Field.Height / PointSpacing + 2];
            for (int ix = 0; ix < Points.GetLength(0); ix++)
            {
                for (int iy = 0; iy < Points.GetLength(1); iy++)
                {
                    if (ix == 0 || ix == Points.GetLength(0) - 1 || iy == 0 || iy == Points.GetLength(1) - 1)
                    {
                        Points[ix, iy] = new GridPoint(new Vector2(ix * PointSpacing + Field.X, iy * PointSpacing + Field.Y), false);
                    }
                    else
                    {
                        Points[ix, iy] = new GridPoint(new Vector2(ix * PointSpacing + Field.X, iy * PointSpacing + Field.Y), true);
                    }
                }
            }

            // Create the horz Springs
            for (int ix = 0; ix < Points.GetLength(0) - 1; ix++)
            {
                for (int iy = 0; iy < Points.GetLength(1); iy++)
                {
                    HorzSprings.Add(new GridSpring(Points[ix, iy], Points[ix + 1, iy]));
                }
            }

            // Create the vert Springs
            for (int ix = 0; ix < Points.GetLength(0); ix++)
            {
                for (int iy = 0; iy < Points.GetLength(1) - 1; iy++)
                {
                    VertSprings.Add(new GridSpring(Points[ix, iy], Points[ix, iy + 1]));
                }
            }

            this.Field = new Rectangle(Field.X, Field.Y, Points.GetLength(0) * PointSpacing, Points.GetLength(1) * PointSpacing);
        }

        public void ApplyForce(Vector2 Pos, float Strength)
        {
            lock (Points)
            {
                for (int ix = 0; ix < Points.GetLength(0); ix++)
                {
                    for (int iy = 0; iy < Points.GetLength(1); iy++)
                    {
                        Points[ix, iy].GetPulledBy(Control.GetMouseVector(), Strength * 25);
                    }
                }
            }
        }
        public void Twist(Vector2 Pos, float Strength, float DeviationAngle)
        {
            lock (Points)
            {
                for (int ix = 0; ix < Points.GetLength(0); ix++)
                {
                    for (int iy = 0; iy < Points.GetLength(1); iy++)
                    {
                        Points[ix, iy].OrbitAround(Control.GetMouseVector(), Strength * 25, DeviationAngle);
                    }
                }
            }
        }

        private void UpdatePoints()
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            lock (Points)
            {
                for (int ix = 0; ix < Points.GetLength(0); ix++)
                {
                    for (int iy = 0; iy < Points.GetLength(1); iy++)
                    {
                        Points[ix, iy].Update();
                    }
                }
            }
        }
        private void UpdateHorzSprings()
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            lock (HorzSprings)
            {
                for (int i = 0; i < HorzSprings.Count; i++)
                {
                    HorzSprings[i].Update();
                }
            }
        }
        private void UpdateVertSprings()
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            lock (VertSprings)
            {
                for (int i = 0; i < VertSprings.Count; i++)
                {
                    VertSprings[i].Update();
                }
            }
        }

        public void Update()
        {
            //Task.Factory.StartNew(() => UpdateHorzSprings());
            //Task.Factory.StartNew(() => UpdateVertSprings());
            UpdateHorzSprings();
            UpdateVertSprings();
            UpdatePoints();
        }
        public void Draw(SpriteBatch SB)
        {
            lock (HorzSprings)
            {
                for (int i = 0; i < HorzSprings.Count; i++)
                {
                    if (i < HorzSprings.Count)
                    {
                        HorzSprings[i].Draw(SB);
                    }
                }
            }
            lock (VertSprings)
            {
                for (int i = 0; i < VertSprings.Count; i++)
                {
                    if (i < VertSprings.Count)
                    {
                        VertSprings[i].Draw(SB);
                    }
                }
            }
            lock (Points)
            {
                for (int ix = 0; ix < Points.GetLength(0); ix++)
                {
                    for (int iy = 0; iy < Points.GetLength(1); iy++)
                    {
                        Points[ix, iy].Draw(SB);
                    }
                }
            }
        }
    }
}
