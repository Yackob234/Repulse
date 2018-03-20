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

            //the rest of variables
        public DirectionEnum attackDirection = DirectionEnum.Blank;
        public DirectionEnum blockDirection = DirectionEnum.Blank;
        public CharacterEnum characterChoice;
        public CharacterEnum p1Character;
        public CharacterEnum p2Character;
        public int timer = 0;
        public int timerLimit = 500;
        public int attackTimer = 0;
        public int attackTimerLimit = 70;
        public int attackDelayTimerLimit = 50;
        public int attackDelayTimer = 0;
        public int choiceTimer = 0;
        public int choiceTimerLimit = 50;
        public int generalTimerStage1 = 0;
        public int moveChoiceTimer = 0;
        public int moveChoiceTimerLimit = 25;
        public bool attack = false;
        public bool newAttacker = true;
        public bool attackCast = false;
        public bool pressedExtention= false;
        public string[] alive = new string[2];
        public string[] Dead = new string[2];
        public string[] deaddead = new string[2];
        public SpriteFont font;
        public int Stage = 1;
        public int xChoice = 0;
        public int yChoice = 0;
        public bool BlockAnimation;
        public bool DamageAnimation;
        public int hitTimer = 0;
        public bool weaponSpeedNeedReset = true;
        public bool initCompleted = false;
        //debugging variables:
        public int debug = -1;
        public int ChangedAttackerRan = 0;
        public int debugDelay = 0;
        public int debugDelayLimit = 25;
        public int pause = -1;
        public int pauseDelay = 0;
        public int pauseDelayLimit = 25;

        //private variables
        private GraphicsDevice graphicsDevice;
        private ContentManager contentManager;
        private SortedList<string, SpriteFont> fonts;
        private double elapsedTime;
        private int totalFrames;
        private int fps;
        private Player _p1;
        private Player _p2;
        private int _victor = 0;
        public Controller p1controller;
        public Controller p2controller;
        private SelectionBox chosen;

        //lists
        private List<Controller> _controllers = new List<Controller>();
        private List<Entity> _entities = new List<Entity>();
        private Dictionary<DirectionEnum, Arrow> _arrows = new Dictionary<DirectionEnum, Arrow>();
        private Dictionary<DirectionEnum, Weapon> _weapons1 = new Dictionary<DirectionEnum, Weapon>();
        private Dictionary<DirectionEnum, Weapon> _weapons2 = new Dictionary<DirectionEnum, Weapon>();
        private Dictionary<CharacterEnum, Character> _characters = new Dictionary<CharacterEnum, Character>();
        SortedList<string, Texture2D> textures;

        private int _currentController = 1;
        private bool _lastStand = false;
        private bool _initLastStand = false;
        
        public EntityDrawData(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            this.graphicsDevice = graphicsDevice;
            this.contentManager = contentManager;
            textures = new SortedList<string, Texture2D>();
            fonts = new SortedList<string, SpriteFont>();
            font = LoadFont("SpriteFont_sml");
            
            p1controller = new KeyboardController(KeyboardController.KeyboardStyleEnum.WSAD);
            p2controller = new KeyboardController(KeyboardController.KeyboardStyleEnum.Arrow);
            _controllers.Add(p1controller);
            _controllers.Add(p2controller);

            chosen = new SelectionBox(this, "selected");

            _entities.Add(chosen);

            AddCharacter(CharacterEnum.PixelGenji);
            AddCharacter(CharacterEnum.CuteGenji);
            AddCharacter(CharacterEnum.EvilGenji);
            AddCharacter(CharacterEnum.Mercy);
            AddCharacter(CharacterEnum.Reinhardt);
            AddCharacter(CharacterEnum.Torbjorn);

            p1controller.Direction += Controller_Direction;
            p2controller.Direction += Controller_Direction;
            p1controller.Action += Controller_Action;
            p2controller.Action += Controller_Action;
            precharacter();
            /*
            p1controller.Character += Controller_Character;
            p2controller.Character += Controller_Character;
            */



            

        }

        private void Controller_Action(Controller controller, ActionEnum act, bool pressed)
        {
            //takes actions of the enter key
            if (Stage == 1)
            {
                if (_controllers.IndexOf(controller) == _currentController - 1 && choiceTimer >= choiceTimerLimit)
                {
                    if (_currentController == 1)
                    {
                        Controller_Character(characterChoice, true, _currentController - 1);
                        _currentController++;
                        choiceTimer = 0;
                        p1Character = characterChoice;

                    }
                    else if (_currentController == 2)
                    {
                        Controller_Character(characterChoice, true, _currentController - 1);
                        choiceTimer = 0;
                        p2Character = characterChoice;
                        
                        if(initCompleted == false) mainGameDeclair();
                        stageChange();
                        pauseDelay = 0;
                    }

                }

                else if (_controllers.IndexOf(controller) != _currentController - 1)
                {

                }
            } else if (Stage == 2)
            {
                
            } else if (Stage == 3)
            {
                resetGame();
            }
        }
        private void Controller_Direction(Controller controller, DirectionEnum dir, bool pressed)
        {
            //if you cannot attack return
            if (Stage == 1 && moveChoiceTimer >= moveChoiceTimerLimit)
            {
                if (_controllers.IndexOf(controller) == _currentController - 1)
                {

                    // attacker
                    //Console.WriteLine("attacker");
                    switch (dir)
                    {
                        case DirectionEnum.Up:
                            chosen.y--;
                            break;
                        case DirectionEnum.Down:
                            chosen.y++;
                            break;
                        case DirectionEnum.Left:
                            chosen.x--;
                            break;
                        case DirectionEnum.Right:
                            chosen.x++;
                            break;
                    }
                    moveChoiceTimer = 0;
                }
                else if (_controllers.IndexOf(controller) != _currentController - 1)
                {
                    // defender
                }
                
            }
            else if (Stage == 2)
            {
                if (_controllers.IndexOf(controller) == _currentController - 1 && newAttacker == false && attackDirection == DirectionEnum.Blank)
                {

                    // attacker
                    // Console.WriteLine("attacker");
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
        }

        
        public void precharacter()
        {
            //declears stuff before so it does crash :)

            /*
            for (int chosenCharacter = 0; chosenCharacter < 2; chosenCharacter++)
            {
                alive[chosenCharacter] = "mercy";
                Dead[chosenCharacter] = "mercyDead";
                deaddead[chosenCharacter] = "mercydeaddead";
            }
            */
            alive[1] = "torbjorn";
            Dead[1] = "torbjornDead";
            deaddead[1] = "torbjorndeaddead";
            alive[0] = "reinhardt";
            Dead[0] = "reinhardtDead";
            deaddead[0] = "reinhardtdeaddead";
            

        }



        private void Controller_Character(CharacterEnum cha, bool pressed, int chosenCharacter)
        {
            //chooses the sprite for each player

            //if you cannot attack return
            
            
                //for errors
                alive[chosenCharacter] = "mercy";
                Dead[chosenCharacter] = "mercyDead";
                deaddead[chosenCharacter] = "mercydeaddead";
                //choosing stuff
                switch (cha)
                {
                    case CharacterEnum.CuteGenji:
                        alive[chosenCharacter] = "genji1";
                        Dead[chosenCharacter] = "genji1Dead";
                        deaddead[chosenCharacter] = "genji1deaddead";
                        break;
                    case CharacterEnum.EvilGenji:
                        alive[chosenCharacter] = "genji2";
                        Dead[chosenCharacter] = "genji2Dead";
                        deaddead[chosenCharacter] = "genji2deaddead";
                        break;
                    case CharacterEnum.PixelGenji:
                        alive[chosenCharacter] = "genji3";
                        Dead[chosenCharacter] = "genji3Dead";
                        deaddead[chosenCharacter] = "genji3deaddead";
                        break;
                    case CharacterEnum.Reinhardt:
                        alive[chosenCharacter] = "reinhardt";
                        Dead[chosenCharacter] = "reinhardtDead";
                        deaddead[chosenCharacter] = "reinhardtdeaddead";
                        break;
                    case CharacterEnum.Torbjorn:
                        alive[chosenCharacter] = "torbjorn";
                        Dead[chosenCharacter] = "torbjornDead";
                        deaddead[chosenCharacter] = "torbjorndeaddead";
                        break;
                    case CharacterEnum.Mercy:
                        alive[chosenCharacter] = "mercy";
                        Dead[chosenCharacter] = "mercyDead";
                        deaddead[chosenCharacter] = "mercydeaddead";
                        break;
                    default:
                        alive[chosenCharacter] = "mercy";
                        Dead[chosenCharacter] = "mercyDead";
                        deaddead[chosenCharacter] = "mercydeaddead";
                        break;
                }


                //Console.WriteLine(alive[chosenCharacter]);
                
            
        }

        public GraphicsDevice GraphicsDevice { get { return graphicsDevice; } }

        private Entity AddEntity(Entity entity)
        {
            //adds sprites to a list of entities
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

        private void AddWeapon(DirectionEnum dir, CharacterEnum cha, int player)
        {
            //adds weapons to a list, and gets the right sprites
            string _Texture;
            //swordR works for both down and Right, down was thus deleted to save space

            switch (cha)
            {
                case CharacterEnum.Mercy:
                    switch (dir)
                    {
                        case DirectionEnum.Up:
                            _Texture = "mercystaffU";
                            break;
                        case DirectionEnum.Down:
                            _Texture = "mercystaffR";
                            break;
                        case DirectionEnum.Left:
                            _Texture = "mercystaffL";
                            break;
                        case DirectionEnum.Right:
                            _Texture = "mercystaffR";
                            break;
                        default:
                            _Texture = null;
                            break;
                    }
                    break;
                case CharacterEnum.Reinhardt:
                    switch (dir)
                    {
                        case DirectionEnum.Up:
                            _Texture = "ReinHammerU";
                            break;
                        case DirectionEnum.Down:
                            _Texture = "ReinHammerD";
                            break;
                        case DirectionEnum.Left:
                            _Texture = "ReinHammerL";
                            break;
                        case DirectionEnum.Right:
                            _Texture = "ReinHammerR";
                            break;
                        default:
                            _Texture = null;
                            break;
                    }
                    break;
                case CharacterEnum.Torbjorn:
                    switch (dir)
                    {
                        case DirectionEnum.Up:
                            _Texture = "TorbHammerU";
                            break;
                        case DirectionEnum.Down:
                            _Texture = "TorbHammerD";
                            break;
                        case DirectionEnum.Left:
                            _Texture = "TorbHammerL";
                            break;
                        case DirectionEnum.Right:
                            _Texture = "TorbHammerR";
                            break;
                        default:
                            _Texture = null;
                            break;
                    }
                    break;
                default:
                    switch (dir)
                    {
                        case DirectionEnum.Up:
                            _Texture = "swordU";
                            break;
                        case DirectionEnum.Down:
                            _Texture = "swordR";
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
                    break;

            }
            var weapon = new Weapon(this, dir, _Texture, cha);
            if (player == 1)
            {
                _weapons1.Add(dir, weapon);
            } else if (player == 2)
            {
                _weapons2.Add(dir, weapon);
            }
            AddEntity(weapon);
        }
        private void AddCharacter(CharacterEnum cha)
        {
            //adds a picture for the selection screen for each CharacterEnum
            string Texture;

            switch (cha)
            {
                case CharacterEnum.PixelGenji:
                    Texture = "genji3";
                    break;
                case CharacterEnum.CuteGenji:
                    Texture = "genji1";
                    break;
                case CharacterEnum.EvilGenji:
                    Texture = "genji2";
                    break;
                case CharacterEnum.Mercy:
                    Texture = "mercy";
                    break;
                case CharacterEnum.Reinhardt:
                    Texture = "reinhardt";
                    break;
                case CharacterEnum.Torbjorn:
                    Texture = "torbjorn";
                    break;
                default:
                    Texture = null;
                    break;
            }

            var character = new Character(this, Texture, cha);
            _characters.Add(cha, character);
            AddEntity(character);
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

        public void pauseGame()
        {
            if (pauseDelay > pauseDelayLimit)
            {

                pause *= -1;
                pauseDelay = 0;
            }
        }
        public void damage()
        {
            //checks if attack was blocked and says what to do
            if (attackDirection != blockDirection && attackDirection != DirectionEnum.Blank)
            {
                if (_p1.attacker)
                {
                    _p2.health--;
                } else if (_p2.attacker)
                {
                    _p1.health--;
                }

                if (_lastStand)
                {
                    if (_p1.attacker)
                    {
                        _p1.health++;
                    }
                    else if (_p2.attacker)
                    {
                        _p2.health++;
                    }
                    attackTimerLimit += 10;
                    _lastStand = false;
                }

                DamageAnimation = true;

            } else if (blockDirection == attackDirection && blockDirection != DirectionEnum.Blank)
            {
                if (_lastStand)
                {
                    if (_p1.attacker)
                    {
                        _p1.health--;
                    }
                    else if (_p2.attacker)
                    {
                        _p2.health--;
                    }
                    _lastStand = false;
                }
                BlockAnimation = true;
            }

            

            if (_victor == 0) {
                if (_p2.health == 0) lastStand(2);
                if (_p1.health == 0) lastStand(1);

                if (_p2.health == -1) _victor = 1;
                else if (_p1.health == -1) _victor = 2;
            }
            ChangeAttacker();

            attackDirection = DirectionEnum.Blank;
            blockDirection = DirectionEnum.Blank;
            timer = 0;
            weaponSpeedNeedReset = true;


        }
        
        
        public void ChangeAttacker()
        {
            //changes whos attacking
            newAttacker = true;
            ChangedAttackerRan++;
            ++_currentController;
            if (_lastStand && _initLastStand == false)
            {
                if (_p1.attacker)
                {
                    _p2.health--;
                }
                else if (_p2.attacker)
                {
                    _p1.health--;
                }
                _lastStand = false;
            }
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
            //logic for the attacking
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
                attackTimerLimit = Convert.ToInt32(attackTimerLimit / 1.1) -1;
                if(attackTimerLimit <= 1)
                {
                    attackTimerLimit = 2;
                }
                if (attackDirection != DirectionEnum.Blank)
                {
                    if (_currentController == 1)
                    {
                        _weapons1[attackDirection].resetWeaponPosition(attackDirection);
                    } else if (_currentController == 2)
                    {
                        _weapons2[attackDirection].resetWeaponPosition(attackDirection);
                    }

                }
                
                damage();
            }
            if (attackCast == true)
            {
                if (weaponSpeedNeedReset)
                {
                    _weapons1[attackDirection].resetWeaponSpeed(attackDirection, attackTimerLimit);
                    _weapons2[attackDirection].resetWeaponSpeed(attackDirection, attackTimerLimit);
                    weaponSpeedNeedReset = false;
                }
                if (_currentController == 1)
                {
                    _weapons1[attackDirection].weaponMovement(gameTime);
                }
                else if (_currentController == 2)
                {
                    _weapons2[attackDirection].weaponMovement(gameTime);
                }
            }

        }

        public void newAttackerDelay(){
            //adds a delay so you cant hold down a key
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

        public void lastStand(int dyingPlayer)
        {
            _lastStand = true;

            if (_initLastStand == true) _initLastStand = false;

            _initLastStand = true;
                
        }

        public void resetGame()
        {
            stageChange();
            _victor = 0;
            _p1.health = 2;
            _p2.health = 2;
            attackTimerLimit = 70;
            _currentController = 1;
            xChoice = 0;
            yChoice = 0;
        }



        public void mainGameDeclair()
        {
            //declairs the main game after the character choosing has finished
            _p1 = new Player(this, alive[0], Dead[0], deaddead[0], 2, false, p1Character);
            _p2 = new Player(this, alive[1], Dead[1], deaddead[1], 2, true, p2Character);

            _entities.Add(_p1);
            _entities.Add(_p2);

            AddArrow(DirectionEnum.Up);
            AddArrow(DirectionEnum.Down);
            AddArrow(DirectionEnum.Left);
            AddArrow(DirectionEnum.Right);

            AddWeapon(DirectionEnum.Up, _p1._character, 1);
            AddWeapon(DirectionEnum.Down, _p1._character, 1);
            AddWeapon(DirectionEnum.Left, _p1._character, 1);
            AddWeapon(DirectionEnum.Right, _p1._character, 1);
            AddWeapon(DirectionEnum.Up, _p2._character, 2);
            AddWeapon(DirectionEnum.Down, _p2._character, 2);
            AddWeapon(DirectionEnum.Left, _p2._character, 2);
            AddWeapon(DirectionEnum.Right, _p2._character, 2);



            _weapons1[DirectionEnum.Up].resetWeaponPosition(DirectionEnum.Up);
            _weapons1[DirectionEnum.Left].resetWeaponPosition(DirectionEnum.Left);
            _weapons1[DirectionEnum.Down].resetWeaponPosition(DirectionEnum.Down);
            _weapons1[DirectionEnum.Right].resetWeaponPosition(DirectionEnum.Right);
            _weapons1[DirectionEnum.Up].resetWeaponSpeed(DirectionEnum.Up, attackTimerLimit);
            _weapons1[DirectionEnum.Left].resetWeaponSpeed(DirectionEnum.Left, attackTimerLimit);
            _weapons1[DirectionEnum.Down].resetWeaponSpeed(DirectionEnum.Down, attackTimerLimit);
            _weapons1[DirectionEnum.Right].resetWeaponSpeed(DirectionEnum.Right, attackTimerLimit);

            _weapons2[DirectionEnum.Up].resetWeaponPosition(DirectionEnum.Up);
            _weapons2[DirectionEnum.Left].resetWeaponPosition(DirectionEnum.Left);
            _weapons2[DirectionEnum.Down].resetWeaponPosition(DirectionEnum.Down);
            _weapons2[DirectionEnum.Right].resetWeaponPosition(DirectionEnum.Right);
            _weapons2[DirectionEnum.Up].resetWeaponSpeed(DirectionEnum.Up, attackTimerLimit);
            _weapons2[DirectionEnum.Left].resetWeaponSpeed(DirectionEnum.Left, attackTimerLimit);
            _weapons2[DirectionEnum.Down].resetWeaponSpeed(DirectionEnum.Down, attackTimerLimit);
            _weapons2[DirectionEnum.Right].resetWeaponSpeed(DirectionEnum.Right, attackTimerLimit);

            
            initCompleted = true;
        }

        public void stageChange()
        {
            Stage++;
            for (var i = 0; i < _entities.Count; i++)
            {
                _entities[i].StageUpdate();
            }
            if(Stage == 4)
            {
                Stage = 1;
            }
            //hi
        }

        public void playersCharacterChoice()
        {
            //converts selection box coorditate to a CharacterEnum
            if (moveChoiceTimer >= moveChoiceTimerLimit)
            {
                if (chosen.x == 0)
                {
                    if (chosen.y == 0)
                    {
                        characterChoice = CharacterEnum.PixelGenji;
                    }
                    else if (chosen.y == 1)
                    {
                        characterChoice = CharacterEnum.Mercy;
                    }
                }
                else if (chosen.x == 1)
                {
                    if (chosen.y == 0)
                    {
                        characterChoice = CharacterEnum.CuteGenji;
                    }
                    else if (chosen.y == 1)
                    {
                        characterChoice = CharacterEnum.Reinhardt;
                    }
                }
                else if (chosen.x == 2)
                {
                    if (chosen.y == 0)
                    {
                        characterChoice = CharacterEnum.EvilGenji;
                    }
                    else if (chosen.y == 1)
                    {
                        characterChoice = CharacterEnum.Torbjorn;
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            debugDelay++;
            pauseDelay++;
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (pause == -1)
            {
                
                if (Stage == 1)
                {
                    choiceTimer++;
                    moveChoiceTimer++;
                    playersCharacterChoice();
                }
                else if (Stage == 2)
                {

                    // CharacterChoice.Update(gameTime);
                    timer++;
                    newAttackerDelay();
                    if (_victor != 0)
                    {

                        stageChange();
                    }
                    if (attack == true)
                    {
                        AttackAnimation(gameTime);
                    }
                    if (timer > timerLimit)
                    {
                        timer = 0;
                        ChangeAttacker();
                    }

                }
                else if (Stage == 3)
                {

                }
                else
                {
                    Stage = 1;
                }
                for (var i = 0; i < _entities.Count; ++i)
                {
                    _entities[i].Update(gameTime);
                }
                
                for (var i = 0; i < _controllers.Count; ++i)
                {
                    _controllers[i].Update(gameTime);
                }
                if (_currentController >= 3)
                    _currentController = 1;
            }
            if (elapsedTime >= 1000.0f)
            {
                fps = totalFrames;
                totalFrames = 0;
                elapsedTime = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ++totalFrames;

                if (Stage == 1)
                {   
                    if (debug > 0)
                    {
                        const string debugInfo = @"
FPS: {0}
_currentController: {1}
chosen.x: {2}
chosen.y: {3}
characterChoice: {4}
choiceTimer: {5}
moveChoiceTimer: {6}
Stage: {7}
";
                        spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, chosen.x, chosen.y, characterChoice, choiceTimer, moveChoiceTimer, Stage), new Vector2(5.0f, 0.0f), Color.White);
                    }

                    const string instructions = @"
Player {0}: please choose a Character
";
                    spriteBatch.DrawString(font, String.Format(instructions, _currentController), new Vector2(350, 0.0f), Color.White, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
                }
                else if (Stage == 2)
                {
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
Stage: {13}
";
                        spriteBatch.DrawString(font, String.Format(debugInfo, fps, _p1.attacker, attackDirection, blockDirection, timer, _p1.health,
                            _p2.health, newAttacker, attackDelayTimer, ChangedAttackerRan, _p2.attacker, attackTimerLimit, _currentController, Stage
                            ), new Vector2(5.0f, 0.0f), Color.White);
                    }

                    if (BlockAnimation)
                    {
                        hitTimer++;
                        const string blockText = @"
Block!!
";
                        spriteBatch.DrawString(font, String.Format(blockText, _currentController), new Vector2(445.0f, 50.0f), Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                        if (hitTimer > 50)
                        {
                            BlockAnimation = false;
                            hitTimer = 0;
                        }
                    }
                    if (DamageAnimation)
                    {
                        hitTimer++;
                        const string damageText = @"
Hit!!
";
                        spriteBatch.DrawString(font, String.Format(damageText, _currentController), new Vector2(450.0f, 50.0f), Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                        if (hitTimer > 50)
                        {
                            DamageAnimation = false;
                            hitTimer = 0;
                        }
                    }
                    if (_lastStand)
                    {
                        const string lastStandText = @"
Player {0} you must strike back or die!
";
                        spriteBatch.DrawString(font, String.Format(lastStandText, _currentController), new Vector2(250.0f, 25.0f), Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                    }





                }
                else if (Stage == 3)
                {
                    if (debug > 0)
                    {
                        const string debugInfo = @"
FPS: {0}
Attack Direction: {2}
Block Direction: {3}
Timer: {4}
P1 Health: {5}
P2 Health: {6}
New Attacker: {7}
Attack Delay Timer: {8}
Changed Attacker Ran: {9} times
AttackTimerLimit:{10}
_currentController: {1}
Stage: {11}
";
                        spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, attackDirection, blockDirection, timer, _p1.health,
                            _p2.health, newAttacker, attackDelayTimer, ChangedAttackerRan, attackTimerLimit, Stage
                            ), new Vector2(5.0f, 0.0f), Color.White);
                    }
                    if (_victor != 0)
                    {
                        const string victoryText = @"
Player {0} Wins!!
";
                        spriteBatch.DrawString(font, String.Format(victoryText, _victor), new Vector2(400.0f, 0.0f), Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                    }
                }

            if(pause == 1)
            {
                
                    const string pauseInfo = @"
Game is Paused
";
                    spriteBatch.DrawString(font, String.Format(pauseInfo), new Vector2(800.0f, 0.0f), Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                
            }

            for (var i = 0; i < _entities.Count; ++i)
            {
                _entities[i].Draw(gameTime, spriteBatch);
            }

            }
        
    }
}