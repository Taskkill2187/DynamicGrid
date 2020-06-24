using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DynamicGrid
{
    public class GridSpring
    {
        public GridPoint End1;
        public GridPoint End2;
        private float Length0;

        public GridSpring(GridPoint End1, GridPoint End2)
        {
            this.End1 = End1;
            this.End2 = End2;
            Length0 = (End1.Pos - End2.Pos).Length();
        }

        public void Update()
        {
            lock (End1)
            {
                lock (End2)
                {
                    End1.ApplyForce((End2.Pos - End1.Pos) / 10);
                    End2.ApplyForce((End1.Pos - End2.Pos) / 10);
                    //End1.Update();
                    //End2.Update();
                }
            }
        }
        public void Draw(SpriteBatch SB)
        {
            lock (End1)
            {
                lock (End2)
                {
                    Assets.DrawLine(End1.Pos, End2.Pos, 1, Color.Blue, SB);
                }
            }
            //SB.DrawString(Assets.Font, ((int)(End1.Pos - End2.Pos).Length()).ToString(), (End1.Pos + End2.Pos) / 2, Color.Blue);
        }
        public void DrawColored(SpriteBatch SB)
        {
            lock (End1)
            {
                lock (End2)
                {
                    float Length = (End1.Pos - End2.Pos).Length();
                    if (Length > 0)
                    {
                        if ((Length - Length0) < (Length0 / 2))
                            Assets.DrawLine(End1.Pos, End2.Pos, 1, Color.Lerp(Color.Green, Color.Yellow, (Length - Length0) / (Length0 / 2)), SB);
                        else
                            Assets.DrawLine(End1.Pos, End2.Pos, 1, Color.Lerp(Color.Yellow, Color.Red, (Length - Length0 * 1.5f) / (Length0)), SB);
                    }
                }
            }
            //SB.DrawString(Assets.Font, ((int)(End1.Pos - End2.Pos).Length()).ToString(), (End1.Pos + End2.Pos) / 2, Color.Blue);
        }
    }
}
