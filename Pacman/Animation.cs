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

namespace Pacman
{
    public class Animation
    {
        int frameCounter, switchFrame;

        bool active;
        Vector2 position, amountOfFrames, currentFrame;
        Texture2D animationImage;
        Rectangle sourcRect;

        public bool Active 
        {
            get { return active; }
            set { active = value; }
        }

        public Vector2 CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        public Vector2 Position 
        {
            get { return position; }
            set { position = value; }
        }

        public int FrameWidth 
        {
            get { return animationImage.Width / (int)amountOfFrames.X; }
        }

        public int FrameHeight 
        {
            get { return animationImage.Height / (int)amountOfFrames.Y; }
        }

        public Texture2D AnimationImage 
        {
            set { animationImage = value; }
        }

        public void Init(Vector2 position,Vector2 amountOfFrames)
        {
            active = false;
            this.position = position;
            this.amountOfFrames = amountOfFrames;
        }

        public void Update(GameTime gameTime) 
        {
            switchFrame = 80;
            if(this.active)
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            else if(!this.active)
                frameCounter = 0;
            if (frameCounter > switchFrame)  
            {
                frameCounter = 0;
                currentFrame.X += FrameWidth;
                if (currentFrame.X >= animationImage.Width)
                    currentFrame.X = 0;
                
            }
            sourcRect = new Rectangle((int)currentFrame.X, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(animationImage, position, sourcRect, Color.White);
        }
    }
}
