using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace repulse
{
    class EntityDrawData
    {
        //public Texture2D genji1;
        //public Texture2D genji1Dead;

        //public Texture2D onArrowUp;
        //public Arrows offArrows = new Arrows(new Vector2(3,2));
        public List<Texture2D> onArrow = new List<Texture2D>();
        public List<Texture2D> genjis = new List<Texture2D>();
        public Texture2D[] genji = new Texture2D[3];
        public Texture2D[] onArrows = new Texture2D[4];
        public Vector2[] onArrowPosition = new Vector2[4];
        public Texture2D[] offArrows = new Texture2D[4];
        public Vector2[] offArrowPosition = new Vector2[4];
        public Texture2D[] sword = new Texture2D[4];
        public Vector2[] swordPosition = new Vector2[4];
        public Vector2[] swordSpeed = new Vector2[4];
        //int[] array = new int[] { 1, 2, 3 };
        SortedList<string, Texture2D> textures;
        public int attackDirection = 0;
        public int blockDirection = 0;
        public Player p1 = new Player(2, false);
        public Player p2 = new Player(2, true);
        public int timer = 0;
        public int timerLimit = 500;
        public int attackTimer = 0;
        public int attackTimerLimit = 100;
        public int attackDelayTimerLimit = 100;
        public int attackDelayTimer = 0;
        public bool attack = false;
        public bool newAttacker = true;
        public int attackIndicator = 0;
        public float centreOfIndicatorsX = 500.0f;
        public float centreOfIndicatorsY = 300.0f;
        public int baseXShift = -3;
        public int baseYShift = -3;

        //debugging variables:
        public int debug = -1;
        public int ChangedAttackerRan = 0;
        

        //public Vector2 genji1Position;


        // Set the coordinates to draw the sprite at.
        public Vector2 genji1Position = new Vector2(200.0f, 200.0f);
        public Vector2 genji1deadPosition = Vector2.Zero;
        // Store some information about the sprite's motion.
        public Vector2 genji1Speed = new Vector2(50.0f, 50.0f);
        //int JumpDelay = 50;
        private GraphicsDevice graphicsDevice;
        private ContentManager contentManager;
        private SpriteFont font;
        private SortedList<string, SpriteFont> fonts;
        private double elapsedTime;
        private int totalFrames;
        private int fps;

        public EntityDrawData(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            this.graphicsDevice = graphicsDevice;
            this.contentManager = contentManager;
            textures = new SortedList<string, Texture2D>();
            fonts = new SortedList<string, SpriteFont>();
            font = LoadFont("SpriteFont_sml");
            genji[2] = LoadTexture("genji1"); 
            genji[1] = LoadTexture("genji1Dead");
            genji[0] = LoadTexture("genji1deaddead");
            sword[0] = LoadTexture("swordU");
            sword[1] = LoadTexture("swordR");
            sword[2] = LoadTexture("swordD");
            sword[3] = LoadTexture("swordL");
            offArrows[0] = LoadTexture("offArrowU");
            offArrows[1] = LoadTexture("offArrowR");
            offArrows[2] = LoadTexture("offArrowD");
            offArrows[3] = LoadTexture("offArrowL");
            onArrows[0] = LoadTexture("onArrowU");
            onArrows[1] = LoadTexture("onArrowR");
            onArrows[2] = LoadTexture("onArrowD");
            onArrows[3] = LoadTexture("onArrowL");
            swordPosition[0] = new Vector2(500.0f, 0.0f);
            swordPosition[1] = new Vector2(1000.0f, 300.0f);
            swordPosition[2] = new Vector2(500.0f, 600.0f);
            swordPosition[3] = new Vector2(0.0f, 300.0f);
            swordSpeed[0] = new Vector2(0.0f, -10.0f);
            swordSpeed[1] = new Vector2(-10.0f, 0.0f);
            swordSpeed[2] = new Vector2(0.0f, 10.0f);
            swordSpeed[3] = new Vector2(10.0f, 0.0f);
            for(int i = 0; i < 4; i++)
            {
                offArrowPosition[i] = new Vector2(IndiPos(i, 1), IndiPos(i, 2));
                onArrowPosition[i] = new Vector2(IndiPos(i, 1), IndiPos(i, 2));
            }



            //genjis.Add(LoadTexture("genji1deaddead"));
            //genjis.Add(LoadTexture("genji1Dead"));
            //genjis.Add(LoadTexture("genji1"));

            //genji1Dead = LoadTexture("genji1Dead");
            //genji1 = LoadTexture("genji1");


        }
        public float IndiPos(int direction, int XorY)
        {
            float position = 0;
            if (XorY == 1)
            {
                if (direction == 0)
                {
                    position = centreOfIndicatorsX + baseXShift;
                }
                else if (direction == 1)
                {
                    position = centreOfIndicatorsX + baseXShift + 10;
                }
                else if (direction == 2)
                {
                    position = centreOfIndicatorsX + baseXShift;
                }
                else if (direction == 3)
                {
                    position = centreOfIndicatorsX + baseXShift - 10;
                }
            }
            if (XorY == 2)
            {
                if (direction == 0)
                {
                    position = centreOfIndicatorsY + baseYShift - 10;
                }
                else if (direction == 1)
                {
                    position = centreOfIndicatorsY + baseYShift;
                }
                else if (direction == 2)
                {
                    position = centreOfIndicatorsY + baseYShift + 10;
                }
                else if (direction == 3)
                {
                    position = centreOfIndicatorsY + baseYShift;
                }
            }
            return position;
        }
        public Texture2D LoadTexture(string assetName)
        {
            Texture2D texture;
            if (textures.TryGetValue(assetName, out texture))
                return texture;
            texture = contentManager.Load<Texture2D>(assetName);
            textures.Add(assetName, texture);
            return texture;
        }

        public SpriteFont LoadFont(string assetName)
        {
            SpriteFont font;
            if (fonts.TryGetValue(assetName, out font))
                return font;
            font = contentManager.Load<SpriteFont>(assetName);
            fonts.Add(assetName, font);

            return font;
        }

        public void startDebug()
        {
            debug *= -1;
        }

        public void p2Left()
        {
            if(p2.attacker == false)
            {
                blockLeft();
            } else if(p2.attacker && attackDirection == 0 && newAttacker == false)
            {
                attackLeft();
            }
        }
        public void p2Up()
        {
            if (p2.attacker == false)
            {
                blockUp();
            }
            else if (p2.attacker && attackDirection == 0 && newAttacker == false)
            {
                attackUp();
            }
        }
        public void p2Right()
        {
            if (p2.attacker == false)
            {
                blockRight();
            }
            else if (p2.attacker && attackDirection == 0 && newAttacker == false)
            {
                attackRight();
            }
        }
        public void p2Down()
        {
            if (p2.attacker == false)
            {
                blockDown();
            }
            else if (p2.attacker && attackDirection == 0 && newAttacker == false)
            {
                attackDown();
            }
        }
        public void p1Left()
        {
            if (p1.attacker && attackDirection == 0 && newAttacker == false)
            {
                attackLeft();
            } else if (p1.attacker == false)
            {
                blockLeft();
            }
        }
        public void p1Up()
        {
            if (p1.attacker && attackDirection == 0 && newAttacker == false)
            {
                attackUp();
            }
            else if (p1.attacker == false)
            {
                blockUp();
            }
        }
        public void p1Right()
        {
            if (p1.attacker && attackDirection == 0 && newAttacker == false)
            {
                attackRight();
            }
            else if (p1.attacker == false)
            {
                blockRight();
            }
        }
        public void p1Down()
        {
            if (p1.attacker && attackDirection == 0 && newAttacker == false)
            {
                attackDown();
            }
            else if (p1.attacker == false)
            {
                blockDown();
            }
        }
        public void blockLeft()
        {
            blockDirection = 4;
        }
        public void blockUp()
        {
            blockDirection = 1;
        }
        public void blockRight()
        {
            blockDirection = 2;
        }
        public void blockDown()
        {
            blockDirection = 3;
        }
        public void attackLeft()
        {
            
            attack = true;
            attackDirection = 4;
            
        }
        public void attackRight()
        {
            
            attack = true;
            attackDirection = 2;
            
        }
        public void attackDown()
        {
            
            attackDirection = 3;
            attack = true;
            
        }
        public void attackUp()
        {
            
            attackDirection = 1;
            attack = true;
           
        }
        public void damage()
        {
            if (attackDirection != blockDirection && attackDirection != 0)
            {
                if(p1.attacker)
                {
                    p2.health--;
                } else if (p2.attacker)
                {
                    p1.health--;
                }
                
            }
            ChangeAttacker();
            attackDirection = 0;
            blockDirection = 0;
            timer = 0;
            

        }
        public void ChangeAttacker()
        {
            newAttacker = true;
            ChangedAttackerRan++;
            if (p1.attacker)
            {
                p1.attacker = false;
            } else if (p1.attacker == false)
            {
                p1.attacker = true;
            }
            if (p2.attacker)
            {
                p2.attacker = false;
            }
            else if (p2.attacker == false)
            {
                p2.attacker = true;
            }
        }
        public void AttackAnimation(int attackDirection, int blockDirection)
        {
            //animation
            
            attackTimer++;
            attackIndicator = attackDirection; 
            if (attackTimer >= attackTimerLimit)
            {
                damage();
                //DamageAnimation();
                attack = false;
                attackTimer = 0;
            }
            
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            timer++;
            if (attack == true)
            {
                
                AttackAnimation(attackDirection, blockDirection);
            }
            
            if (newAttacker == true)
            {
                attackDelayTimer++;
            } if (attackDelayTimer >= attackDelayTimerLimit)
            {
                newAttacker = false;
                attackDelayTimer = 0;
            }
            
            if (elapsedTime >= 1000.0f)
            {
                fps = totalFrames;
                totalFrames = 0;
                elapsedTime = 0;
            }
            if(timer > timerLimit)
            {
                timer = 0;
                ChangeAttacker();
            }
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ++totalFrames;

            if (debug > 0)
            {
                const string debugInfo = @"
FPS: {0}
Attacker is Player 1: {1}
Attacker is Player 2: {10}
Attack Direction: {2}
Block Direction: {3}
Timer: {4}
P1 Health: {5}
P2 Health: {6}
New Attacker: {7}
Attack Delay Timer: {8}
Changed Attacker Ran: {9} times
";
                spriteBatch.DrawString(font, String.Format(debugInfo, fps, p1.attacker, attackDirection, blockDirection, timer, 
                    p1.health, p2.health, newAttacker, attackDelayTimer, ChangedAttackerRan, p2.attacker), new Vector2(5.0f, 0.0f), Color.White);
            }
            if (p2.attacker)
            {
                //spriteBatch.Draw(genji[p2.health], genji1Position, Color.Aqua);
                
                spriteBatch.DrawString(font, String.Format("1"), new Vector2(220.0f, 220.0f), Color.White);
                if (p1.health == 2)
                {
                    spriteBatch.Draw(genji[2], genji1Position, Color.Aqua);
                }
                else if (p1.health == 1)
                {
                    spriteBatch.Draw(genji[1], genji1Position, Color.Aqua);
                }
                else if (p1.health == 0)
                {
                    spriteBatch.Draw(genji[0], genji1Position, Color.Aqua);
                }
            } else if (p1.attacker)
            {
                spriteBatch.DrawString(font, String.Format("2"), new Vector2(220.0f, 220.0f), Color.White);
                if (p2.health == 2)
                {
                    spriteBatch.Draw(genji[2], genji1Position, Color.CadetBlue);
                }
                else if (p2.health == 1)
                {
                    spriteBatch.Draw(genji[1], genji1Position, Color.CadetBlue);
                }
                else if (p2.health == 0)
                {
                    spriteBatch.Draw(genji[0], genji1Position, Color.CadetBlue);
                }

            }
            for(int i = 0; i < 4; i++)
            {
                spriteBatch.Draw(offArrows[i], offArrowPosition[i], Color.White);
            }
            if (attackDirection != 0)
            {
                spriteBatch.Draw(onArrows[attackDirection-1], onArrowPosition[attackDirection-1], Color.White);
            }


        }
    }
}
