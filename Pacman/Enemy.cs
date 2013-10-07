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
    public class Enemy
    {
        enum EnemyState
        {
            Normal, Run, Right, Left, Up, Down
        };

        EnemyState enemyState;
        Texture2D EnemyImage;
        Vector2 tempCurrentFrame,enemyPosition;

        List<Vector2> pathSegment,collisionIndex;

        float moveSpeed;
        int k;
        bool active,breakLoop,goalReached;

        Animation enemyAnimation = new Animation();
        Rectangle tempUpRect, enemyTempRect, tempDownRect, tempLeftRect, tempRightRect,tempGoalRect;

        public bool Active 
        {
            get { return active; }
            set { active = value; }
        }

        public void Init() 
        {
            pathSegment = new List<Vector2>();
            collisionIndex = new List<Vector2>();
            enemyState = EnemyState.Right;
            moveSpeed = 100f;
            enemyPosition = new Vector2(220, 220);
            tempCurrentFrame = Vector2.Zero;
            enemyAnimation.Init(enemyPosition, new Vector2(1, 1));
            active = true;
            breakLoop = false;
            goalReached = false;
            k = 0;

        }

        public void LoadContent(ContentManager Content) 
        {
            enemyAnimation.Position = new Vector2(560, 550);
            EnemyImage = Content.Load<Texture2D>("PacEnemyAnim");
            enemyAnimation.AnimationImage = EnemyImage;
        }

        public void Update(GameTime gameTime, Player player, Collision col, Layers layer) 
        {
            enemyAnimation.Active = true;
            
            if (active) 
            {
                tempGoalRect = new Rectangle((int)player.PlayerPosition.X, (int)player.PlayerPosition.Y, (int)layer.TileDimensions.X,(int)layer.TileDimensions.Y);
                for (int i = 0; i < col.CollisionMap.Count; i++) 
                {
                    for (int j = 0; j < col.CollisionMap[i].Count; j++) 
                    {

                        if (tempGoalRect.Intersects(new Rectangle((int)col.FoodCollisionMap[i][j].X, (int)col.FoodCollisionMap[i][j].Y, (int)layer.TileDimensions.X, (int)layer.TileDimensions.Y)))
                        {
                            collisionIndex.Add(new Vector2(i, j));
                            breakLoop = true;
                            break;
                        }
                    }
                    if (breakLoop) 
                    {
                        breakLoop = false;
                        break;
                    }
                }
                goalReached = false;
                enemyTempRect = new Rectangle((int)enemyPosition.X, (int)enemyPosition.Y, (int)layer.TileDimensions.X, (int)layer.TileDimensions.Y);
                while (!goalReached) 
                {
                    //tempLeftRect = new Rectangle((int)col.CollisionMap[(int)collisionIndex[k].X][(int)collisionIndex[k].Y -1].X, (int)col.CollisionMap[(int)collisionIndex[k].X][(int)collisionIndex[k].Y - 1].Y, (int)layer.TileDimensions.X, (int)layer.TileDimensions.Y);
                    //tempRightRect = new Rectangle((int)col.CollisionMap[(int)collisionIndex[k].X][(int)collisionIndex[k].Y +1].X, (int)col.CollisionMap[(int)collisionIndex[k].X][(int)collisionIndex[k].Y + 1].Y, (int)layer.TileDimensions.X, (int)layer.TileDimensions.Y);
                    //tempUpRect = new Rectangle((int)col.CollisionMap[(int)collisionIndex[k].X -1][(int)collisionIndex[k].Y].X, (int)col.CollisionMap[(int)collisionIndex[k].X -1][(int)collisionIndex[k].Y].Y, (int)layer.TileDimensions.X, (int)layer.TileDimensions.Y);
                    //tempDownRect = new Rectangle((int)col.CollisionMap[(int)collisionIndex[k].X -1][(int)collisionIndex[k].Y].X, (int)col.CollisionMap[(int)collisionIndex[k].X +1][(int)collisionIndex[k].Y].Y, (int)layer.TileDimensions.X, (int)layer.TileDimensions.Y);
                    
                    if (enemyPosition.X < player.PlayerPosition.X)                                          //Enemy är vänster om spelaren
                    {
                        if (col.Contents[(int)collisionIndex[k].X][(int)collisionIndex[k].Y -1] == "o" && collisionIndex[k].Y != 0) 
                        {
                            collisionIndex.Insert(0,(new Vector2(collisionIndex[k].X, collisionIndex[k].Y - 1)));
                        }
                        else if (col.Contents[(int)collisionIndex[k].X -1][(int)collisionIndex[k].Y] == "o" && collisionIndex[k].X != 0) 
                        {
                            collisionIndex.Insert(0,(new Vector2(collisionIndex[k].X - 1, collisionIndex[k].Y)));
                        }
                        else if (col.Contents[(int)collisionIndex[k].X +1][(int)collisionIndex[k].Y] == "o" && collisionIndex[k].X <= col.CollisionMap.Count) 
                        {
                            collisionIndex.Insert(0,(new Vector2(collisionIndex[k].X + 1, collisionIndex[k].Y)));
                        }
                    }
                    else if (enemyPosition.X > player.PlayerPosition.X)                                     //Enemy är höger om spelaren
                    {
                        if (col.Contents[(int)collisionIndex[k].X][(int)collisionIndex[k].Y + 1] == "o" && collisionIndex[k].Y <= col.CollisionMap[(int)collisionIndex[k].X].Count) 
                        {
                            collisionIndex.Insert(0,(new Vector2((int)collisionIndex[k].X, (int)collisionIndex[k].Y + 1)));
                        }
                        else if (col.Contents[(int)collisionIndex[k].X - 1][(int)collisionIndex[k].Y] == "o" && collisionIndex[k].X != 0) 
                        {
                            collisionIndex.Insert(0,(new Vector2((int)collisionIndex[k].X -1, (int)collisionIndex[k].Y)));
                        }
                        else if (col.Contents[(int)collisionIndex[k].X + 1][(int)collisionIndex[k].Y] == "o" && collisionIndex[k].X <= col.CollisionMap.Count) 
                        {
                            collisionIndex.Insert(0,(new Vector2((int)collisionIndex[k].X +1, (int)collisionIndex[k].Y)));
                        }
                    }
                    else if (enemyPosition.Y < player.PlayerPosition.Y)                                     //Enemy är över spelaren 
                    {
                        if (col.Contents[(int)collisionIndex[k].X][(int)collisionIndex[k].Y -1] == "o" && collisionIndex[k].Y != 0) 
                        {
                            collisionIndex.Insert(0, (new Vector2((int)collisionIndex[k].X, (int)collisionIndex[k].Y - 1)));
                        }
                        else if (col.Contents[(int)collisionIndex[k].X][(int)collisionIndex[k].Y + 1] == "o" && collisionIndex[k].Y <= col.CollisionMap[(int)collisionIndex[k].X].Count) 
                        {
                            collisionIndex.Insert(0, (new Vector2((int)collisionIndex[k].X, (int)collisionIndex[k].Y + 1)));
                        }
                        else if (col.Contents[(int)collisionIndex[k].X - 1][(int)collisionIndex[k].Y] == "o" && collisionIndex[k].X != 0) 
                        {
                            collisionIndex.Insert(0, (new Vector2((int)collisionIndex[k].X -1, (int)collisionIndex[k].Y)));
                        }
                    }
                    else if (enemyPosition.Y > player.PlayerPosition.Y)                                 
                    {
                        if (col.Contents[(int)collisionIndex[k].X][(int)collisionIndex[k].Y - 1] == "o" && collisionIndex[k].Y != 0) 
                        {
                            collisionIndex.Insert(0, (new Vector2((int)collisionIndex[k].X, (int)collisionIndex[k].Y - 1)));
                        }
                        else if (col.Contents[(int)collisionIndex[k].X][(int)collisionIndex[k].Y + 1] == "o" && collisionIndex[k].Y <= col.CollisionMap[(int)collisionIndex[k].X].Count) 
                        {
                            collisionIndex.Insert(0, (new Vector2((int)collisionIndex[k].X, (int)collisionIndex[k].Y + 1)));
                        }
                        else if (col.Contents[(int)collisionIndex[k].X + 1][(int)collisionIndex[k].Y] == "o" && collisionIndex[k].X <= col.CollisionMap.Count) 
                        {
                            collisionIndex.Insert(0, (new Vector2((int)collisionIndex[k].X, (int)collisionIndex[k].Y + 1)));
                        }
                    }
                    if (enemyTempRect.Intersects(new Rectangle((int)col.FoodCollisionMap[(int)collisionIndex[0].X][(int)collisionIndex[0].Y].X, (int)col.FoodCollisionMap[(int)collisionIndex[0].X][(int)collisionIndex[0].Y].Y, (int)layer.TileDimensions.X, (int)layer.TileDimensions.Y)))
                    {
                        goalReached = true;
                    }
                    else
                        k++;
                }
                if (enemyState == EnemyState.Right)
                {
                    enemyPosition.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    tempCurrentFrame.X = 0;
                }
                else if (enemyState == EnemyState.Left)
                {
                    enemyPosition.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    tempCurrentFrame.X = 0;
                }
                else if (enemyState == EnemyState.Down)
                {
                    enemyPosition.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    tempCurrentFrame.X = 0;
                }
                else if (enemyState == EnemyState.Up)
                {
                    enemyPosition.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    tempCurrentFrame.X = 0;
                }
                tempCurrentFrame.Y = enemyAnimation.CurrentFrame.Y;
                enemyAnimation.CurrentFrame = tempCurrentFrame;

                
                enemyAnimation.Position = enemyPosition;
                enemyAnimation.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            enemyAnimation.Draw(spriteBatch);
        }
    }
}
