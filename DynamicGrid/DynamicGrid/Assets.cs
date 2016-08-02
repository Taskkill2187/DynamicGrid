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
    public static class Assets
    {
        public static SpriteFont Font;
        public static Texture2D White;

        public static void DrawLine(Vector2 End1, Vector2 End2, int Thickness, Color Col, SpriteBatch SB)
        {
            //Vector2 Line = End2 - End1;
            //SB.Draw(White, new Rectangle((int)End1.X, (int)End1.Y, Thickness, (int)Line.Length()),
            //    new Rectangle(0, 0, 1, 1), Col, -(float)Math.Atan2(Line.X, Line.Y), new Vector2(0, 0), SpriteEffects.None, 0);

            Vector2 delta = End1 - End2;
            SB.Draw(White, End1, null, Col, -(float)Math.Atan2(delta.X, delta.Y) - (float)Math.PI / 2, new Vector2(0, 0.5f), new Vector2(delta.Length(), Thickness), SpriteEffects.None, 0f);
        }

        public static void DrawCircle(Vector2 Pos, float Radius, Color Col, SpriteBatch SB)
        {
            for (int i = -(int)Radius; i < (int)Radius; i++)
            {
                int HalfHeight = (int)Math.Sqrt(Radius * Radius - i * i);
                SB.Draw(White, new Rectangle((int)Pos.X + i, (int)Pos.Y - HalfHeight, 1, HalfHeight * 2), Col);
            }
        }

        public static void Load(ContentManager Content, GraphicsDevice GD)
        {
            White = new Texture2D(GD, 1, 1);
            Color[] Col = new Color[1];
            Col[0] = Color.White;
            White.SetData<Color>(Col);
            Font = Content.Load<SpriteFont>("Font");
        }
    }
}
