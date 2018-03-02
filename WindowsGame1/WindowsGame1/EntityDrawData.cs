using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace repulse
{
    public class EntityDrawData
    {
        //public Texture2D genji1;
        //public Texture2D genji1Dead;

        //public Texture2D onArrowUp;
        //public Arrows offArrows = new Arrows(new Vector2(3,2));
        /*
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
        */
        //int[] array = new int[] { 1, 2, 3 };
        SortedList<string, Texture2D> textures;
        public DirectionEnum attackDirection = DirectionEnum.Blank;
        public DirectionEnum blockDirection = DirectionEnum.Blank;
        
        public int timer = 0;
        public int timerLimit = 500;
        public int attackTimer = 0;
        public int attackTimerLimit = 100;
        public int attackDelayTimerLimit = 50;
        public int attackDelayTimer = 0;
        public bool attack = false;
        public bool newAttacker = true;
        public bool attackCast = false;
        public bool pressedExtention= false;

        //debugging variables:
        public int debug = -1;
        public int ChangedAttackerRan = 0;
        public int debugDelay = 0;
        public int debugDelayLimit = 25;
        
        

        //public Vector2 genji1Position;


        // Set the coordinates to draw the sprite at.
        public Vector2 genji1Position = new Vector2(400.0f, 200.0f);
        // Store some information about the sprite's motion.
        //int JumpDelay = 50;
        private GraphicsDevice graphicsDevice;
        private ContentManager contentManager;
        private SpriteFont font;
        private SortedList<string, SpriteFont> fonts;
        private double elapsedTime;
        private int totalFrames;
        private int fps;
        private Player _p1;
        private Player _p2;
        private int _victor = 0;


        private List<Controller> _controllers = new List<Controller>();
        private List<Entity> _entities = new List<Entity>();
        private Dictionary<DirectionEnum, Arrow> _arrows = new Dictionary<DirectionEnum, Arrow>();
        private Dictionary<DirectionEnum, Sword> _swords = new Dictionary<DirectionEnum, Sword>();

        private int _currentController = 2;
        //private bool _allowAttack = true;
        
        public EntityDrawData(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            this.graphicsDevice = graphicsDevice;
            this.contentManager = contentManager;
            textures = new SortedList<string, Texture2D>();
            fonts = new SortedList<string, SpriteFont>();
            font = LoadFont("SpriteFont_sml");
           //genji[2] = LoadTexture("genji1"); 
            //genji[1] = LoadTexture("genji1Dead");
            //genji[0] = LoadTexture("genji1deaddead");
            /*
            sword[0] = LoadTexture("swordU");
            sword[1] = LoadTexture("swordR");
            sword[2] = LoadTexture("swordD");
            sword[3] = LoadTexture("swordL");
            
            _upSword = new Sword(this, "swordU", );
            _rightSword = new Sword(this, "swordR", );
            _downSword = new Sword(this, "swordD", );
            _leftSword = new Sword(this, "swordL", );
            */
            _p1 = new Player(this, "genji1", "genji1Dead", "genji1deaddead", 2, false, genji1Position);
            _p2 = new Player(this, "genji1", "genji1Dead", "genji1deaddead", 2, true, genji1Position);
            Controller p1controller = new KeyboardController(KeyboardController.KeyboardStyleEnum.WSAD);
            Controller p2controller = new KeyboardController(KeyboardController.KeyboardStyleEnum.IJKL);
            _controllers.Add(p1controller);
            _controllers.Add(p2controller);

            _entities.Add(_p1);
            _entities.Add(_p2);

            p1controller.Direction += Controller_Direction;
            p2controller.Direction += Controller_Direction;
           
            AddArrow(DirectionEnum.Up);
            AddArrow(DirectionEnum.Down);
            AddArrow(DirectionEnum.Left);
            AddArrow(DirectionEnum.Right);

            AddSword(DirectionEnum.Up);
            AddSword(DirectionEnum.Down);
            AddSword(DirectionEnum.Left);
            AddSword(DirectionEnum.Right);

            _swords[DirectionEnum.Up].resetSwordPosition(DirectionEnum.Up);
            _swords[DirectionEnum.Left].resetSwordPosition(DirectionEnum.Left);
            _swords[DirectionEnum.Down].resetSwordPosition(DirectionEnum.Down);
            _swords[DirectionEnum.Right].resetSwordPosition(DirectionEnum.Right);
            _swords[DirectionEnum.Up].resetSwordSpeed(DirectionEnum.Up, attackTimerLimit);
            _swords[DirectionEnum.Left].resetSwordSpeed(DirectionEnum.Left, attackTimerLimit);
            _swords[DirectionEnum.Down].resetSwordSpeed(DirectionEnum.Down, attackTimerLimit);
            _swords[DirectionEnum.Right].resetSwordSpeed(DirectionEnum.Right, attackTimerLimit);
            //genjis.Add(LoadTexture("genji1deaddead"));
            //genjis.Add(LoadTexture("genji1Dead"));
            //genjis.Add(LoadTexture("genji1"));


        }

        private void Controller_Direction(Controller controller, DirectionEnum dir, bool pressed)
        {
            //if you cannot attack return
            
            if (_controllers.IndexOf(controller) == _currentController-1 && newAttacker == false && attackDirection == DirectionEnum.Blank)
            {
                
                // attacker
                //Console.WriteLine("attacker");
                if (pressed) pressedExtention = true;
                
                _arrows[dir].Toggle(pressedExtention);
                attackDirection = dir;
                attack = true;
            }
            else if (_controllers.IndexOf(controller) != _currentController - 1)
            {
                // defender
                //Console.WriteLine("defender");
                blockDirection = dir;
            }
        }

        public GraphicsDevice GraphicsDevice { get { return graphicsDevice; } }

        private Entity AddEntity(Entity entity)
        {
            _entities.Add(entity);
            return entity;
        }

        private void AddArrow(DirectionEnum dir)
        {
            //adds arrows to a list, and gets the right sprites
            string onTexture, offTexture;

            switch (dir)
            {
                case DirectionEnum.Up:
                    onTexture = "onArrowU";
                    offTexture = "offArrowU";
                    break;
                case DirectionEnum.Down:
                    onTexture = "onArrowD";
                    offTexture = "offArrowD";
                    break;
                case DirectionEnum.Left:
                    onTexture = "onArrowL";
                    offTexture = "offArrowL";
                    break;
                case DirectionEnum.Right:
                    onTexture = "onArrowR";
                    offTexture = "offArrowR";
                    break;
                default:
                    onTexture = null;
                    offTexture = null;
                    break;
            }

            var arrow = new Arrow(this, dir, onTexture, offTexture);
            _arrows.Add(dir, arrow);
            AddEntity(arrow);
        }

        private void AddSword(DirectionEnum dir)
        {
            //adds swords to a list, and gets the right sprites
            string _Texture;

            switch (dir)
            {
                case DirectionEnum.Up:
                    _Texture = "swordU";
                    break;
                case DirectionEnum.Down:
                    _Texture = "swordD";
                    break;
                case DirectionEnum.Left:
                    _Texture = "swordL";
                    break;
                case DirectionEnum.Right:
                    _Texture = "swordR";
                    break;
                default:
                    _Texture = null;
                    break;
            }

            var sword = new Sword(this, dir, _Texture);
            _swords.Add(dir, sword);
            AddEntity(sword);
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
            //starts the debuging screen
            if (debugDelay>debugDelayLimit) {
                debug *= -1;
                debugDelay = 0;
            }
        }
        public void damage()
        {
            if (attackDirection != blockDirection && attackDirection != DirectionEnum.Blank)
            {
                if (_p1.attacker)
                {
                    _p2.health--;
                } else if (_p2.attacker)
                {
                    _p1.health--;
                }
                DamageAnimation();
            } else if (blockDirection == attackDirection && blockDirection != DirectionEnum.Blank)
            {
                BlockAnimation();
            }
            if (_victor == 0) {
                if (_p2.health == 0) _victor = 1;
                else if (_p1.health == 0) _victor = 2;
            }
            ChangeAttacker();

            attackDirection = DirectionEnum.Blank;
            blockDirection = DirectionEnum.Blank;
            timer = 0;


        }
        
        
        public void ChangeAttacker()
        {
            //changes whos attacking
            newAttacker = true;
            ChangedAttackerRan++;
            ++_currentController;
            if (_currentController >= 3)
                _currentController = 1;
            if (_p1.attacker)
            {
                _p1.attacker = false;
            } else if (_p1.attacker == false)
            {
                _p1.attacker = true;
            }
            if (_p2.attacker)
            {
                _p2.attacker = false;
            }
            else if (_p2.attacker == false)
            {
                _p2.attacker = true;
            }
        }
        
        public void AttackAnimation(GameTime gameTime)
        {
            //animation
            attackCast = true;
            attackTimer++;
            
            
            if (attackTimer >= attackTimerLimit)
            {
                


                foreach (var arrow in _arrows)
                {
                    arrow.Value.Toggle(false);
                }
                

                attack = false;
                pressedExtention = false;
                attackTimer = 0;
                attackCast = false;
                attackTimerLimit += -2;
                if (attackDirection != DirectionEnum.Blank)
                {
                    _swords[attackDirection].resetSwordPosition(attackDirection);
                    _swords[attackDirection].resetSwordSpeed(attackDirection, attackTimerLimit);
                }

                damage();
            }
            if (attackCast == true)
            {
                _swords[attackDirection].swordMovement(attackDirection, gameTime);
            }

        }
        public void DamageAnimation()
        {

        }
        public void BlockAnimation()
        {

        }

        public void newAttackerDelay(){
            if (newAttacker == true)
            {
                attackDelayTimer++;
            }
            if (attackDelayTimer >= attackDelayTimerLimit)
            {
                newAttacker = false;
                attackDelayTimer = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            timer++;
            debugDelay++;
            newAttackerDelay();
            if (attack == true)
            {
                AttackAnimation(gameTime);
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

            for (var i = 0; i < _controllers.Count; ++i)
            {
                _controllers[i].Update(gameTime);
            }

            for (var i = 0; i < _entities.Count; ++i)
            {
                _entities[i].Update(gameTime);
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
AttackTimerLimit:{11}
_currentController: {12}
";
                spriteBatch.DrawString(font, String.Format(debugInfo, fps, _p1.attacker, attackDirection, blockDirection, timer, _p1.health, 
                    _p2.health, newAttacker, attackDelayTimer, ChangedAttackerRan, _p2.attacker, attackTimerLimit, _currentController
                    ), new Vector2(5.0f, 0.0f), Color.White);
            }

            
            for (var i = 0; i < _entities.Count; ++i)
            {
                _entities[i].Draw(gameTime, spriteBatch);
            }
            /*
            switch (attackDirection)
            {
                case DirectionEnum.Up:
                    spriteBatch.Draw(sword[0], swordPosition[0], Color.White);
                    break;
                case DirectionEnum.Right:
                    spriteBatch.Draw(sword[1], swordPosition[1], Color.White);
                    break;
                case DirectionEnum.Down:
                    spriteBatch.Draw(sword[2], swordPosition[2], Color.White);
                    break;
                case DirectionEnum.Left:
                    spriteBatch.Draw(sword[3], swordPosition[3], Color.White);
                    break;
                case DirectionEnum.Blank:
                    break;
                    

            }
            */
            if(_victor != 0)
            {
                const string victoryText = @"
Player {0} Wins!!
";              spriteBatch.DrawString(font, String.Format(victoryText, _victor), new Vector2(450.0f, 0.0f), Color.White);
            }
       
        }
    }
}
