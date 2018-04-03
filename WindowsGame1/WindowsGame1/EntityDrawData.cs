using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace repulse
{
    public class EntityDrawData
    {
        /*
        TODO: 
        add ps3 controller
        make sure two ps3 controllers work
        add who got highscore
        animations :( wamp wamp wampppp
        
        ~~
        write high scores from AI mode
        changing all timers to real time.
        adding a new font kinda
        adding a timer 
        add type of controller select
        add a correct timer counter/for AI
        add bot mode - started
        background selector - help/effort SCALE
        add skip for intructions
        Add grey things for text - help SHADOW
        
        Questions: 
        ~~
        where do i find xbox controller software?
        .gitignore?? 
        can i changed texture sizes mid game / check
        how to do the grey highlight thing for text - can i get the text space? / check
        */



        //public Vector2[] swordSpeed = new Vector2[4];
        //int[] array = new int[] { 1, 2, 3 };

        //the rest of variables
        public DirectionEnum attackDirection = DirectionEnum.Blank;
        public DirectionEnum blockDirection = DirectionEnum.Blank;
        public CharacterEnum characterChoice;
        public CharacterEnum p1Character;
        public CharacterEnum p2Character;
        public double timer = 0;
        public int timerLimit = 8000;
        public double attackTimer = 0;
        public int attackTimerLimit = 1400;
        public int attackDelayTimerLimit = 1000;
        public double attackDelayTimer = 0;
        public double choiceTimer = 0;
        public int choiceTimerLimit = 500;
        public double generalTimerStage1 = 0;
        public double moveChoiceTimer = 0;
        public int moveChoiceTimerLimit = 250;
        public bool attack = false;
        public bool newAttacker = true;
        public bool attackCast = false;
        public bool pressedExtention= false;
        public string[] alive = new string[2];
        public string[] Dead = new string[2];
        public string[] deaddead = new string[2];
        public SpriteFont font;
        public SpriteFont fontDejaVu;
        //represents the stage in which the game in is (selections screens playings, ending screens)
        public StageEnum Stage = StageEnum.ControllerTypeSelect;
        public int gameMode = 0;
        public bool BlockAnimation;
        public bool DamageAnimation;
        public double hitTimer = 0;
        public bool weaponSpeedNeedReset = true;
        public bool initCompleted = false;
        //debugging variables:
        public int debug = -1;
        public int ChangedAttackerRan = 0;
        public double debugDelay = 0;
        public int debugDelayLimit = 250;
        public int pause = -1;
        public double pauseDelay = 0;
        public int pauseDelayLimit = 250;
        public double informationTimer = 0;
        public int informationTimerLimit = 5000;
        public Color fontColor = Color.Khaki;

        //private variables
        private GraphicsDevice graphicsDevice;
        private ContentManager contentManager;
        private SortedList<string, SpriteFont> fonts;
        private double elapsedTime;
        private double reactionTime;
        private int totalFrames;
        private int fps;
        private Player _p1;
        private Player _p2;
        private int _victor = 0;
        public int Player1Controller = -1;
        public int Player2Controller = -1;
        public Controller WASDcontroller;
        public Controller IJKLcontroller;
        public Controller Arrowcontroller;
        public Controller NumPadcontroller;
        public Controller LeftSidecontroller;
        public Controller RightSidecontroller;
        private SelectionBox chosen;
        private Background bg;
        private Gamemode twoPlayers;
        private Gamemode onePlayer;
        private Random _r = new Random();
        private HighScore _hs;
        //private StreamReader _scoreReader = new StreamReader("score.txt");
        //private StreamWriter _scoreWriter = new StreamWriter("score.txt");
        //lists
        private List<Controller> _controllers = new List<Controller>();
        //private Dictionary<ControllerEnum, Controller> _controllers = new Dictionary<ControllerEnum, Controller>();
        private List<Entity> _entities = new List<Entity>();
        private Dictionary<DirectionEnum, Arrow> _arrows = new Dictionary<DirectionEnum, Arrow>();
        private Dictionary<DirectionEnum, Weapon> _weapons1 = new Dictionary<DirectionEnum, Weapon>();
        private Dictionary<DirectionEnum, Weapon> _weapons2 = new Dictionary<DirectionEnum, Weapon>();
        private Dictionary<CharacterEnum, Character> _characters = new Dictionary<CharacterEnum, Character>();
        private Dictionary<int, Background> _bgs = new Dictionary<int, Background>();
        private Dictionary<ControllerEnum, ControllerIcon> _controllerIcons = new Dictionary<ControllerEnum, ControllerIcon>();
        SortedList<string, Texture2D> textures;

        private int _currentController = -1;
        private bool _lastStand = false;
        private bool _initLastStand = false;
        private bool _chosenAttack = false;
        private bool _p1chosen = false;
        //private string 

        public EntityDrawData(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            this.graphicsDevice = graphicsDevice;
            this.contentManager = contentManager;
            textures = new SortedList<string, Texture2D>();
            fonts = new SortedList<string, SpriteFont>();

            for (int i = 1; i < 7; i++)
            {
                LoadBackgrounds(i);
            }

            _hs = new HighScore(this);
            onePlayer = new Gamemode(this, "onePlayer", 2);
            twoPlayers = new Gamemode(this, "twoPlayers", 1);
            _entities.Add(onePlayer);
            _entities.Add(twoPlayers);

            font = LoadFont("SpriteFont_sml");
            fontDejaVu = LoadFont("SpriteFont1");

            
            LoadControllers(WASDcontroller, ControllerEnum.WASD);
            LoadControllers(IJKLcontroller, ControllerEnum.IJKL);
            LoadControllers(Arrowcontroller, ControllerEnum.Arrow);
            LoadControllers(NumPadcontroller, ControllerEnum.NumPad);
            LoadControllers(LeftSidecontroller, ControllerEnum.LeftSide);
            LoadControllers(RightSidecontroller, ControllerEnum.RightSide);

            LoadControllerIcons(ControllerEnum.WASD);
            LoadControllerIcons(ControllerEnum.IJKL);
            LoadControllerIcons(ControllerEnum.Arrow);
            LoadControllerIcons(ControllerEnum.NumPad);
            LoadControllerIcons(ControllerEnum.LeftSide);
            LoadControllerIcons(ControllerEnum.RightSide);

            chosen = new SelectionBox(this, "selected");
            _entities.Add(chosen);

            AddCharacter(CharacterEnum.PixelGenji);
            AddCharacter(CharacterEnum.CuteGenji);
            AddCharacter(CharacterEnum.EvilGenji);
            AddCharacter(CharacterEnum.Mercy);
            AddCharacter(CharacterEnum.Reinhardt);
            AddCharacter(CharacterEnum.Torbjorn);
            
            precharacter();
            /*
            p1controller.Character += Controller_Character;
            p2controller.Character += Controller_Character;
            */
        }

        private void Controller_Action(Controller controller, ActionEnum act, bool pressed)
        {

            //string con = controller;
            //takes actions of the enter key
            if (choiceTimer >= choiceTimerLimit)
            {
                
                if (Stage == StageEnum.GamemodeSelect)
                {
                    playerGamemodeChoice();
                    stageChange();
                    
                }
                else if (Stage == StageEnum.ControllerTypeSelect)
                {
                    //Console.WriteLine(_controllers.IndexOf(controller) + " xd " + Player1Controller + " <-p1p2-> " + Player2Controller);
                    bool keyboardEnter = false;
                    if(Player1Controller <= 3 && Player1Controller >= 0 || Player2Controller <= 3 && Player2Controller >= 0)
                    {
                        keyboardEnter = true;
                    }
                    if(_controllers.IndexOf(controller) == Player1Controller || _controllers.IndexOf(controller) == Player2Controller || keyboardEnter)
                    {
                        stageChange();
                        _currentController = Player1Controller;
                        UpdateControllerIcons();
                    }
                    
                }
                else if (Stage == StageEnum.PlayerInformationScreen)
                {
                    stageChange();

                }
                else if (Stage == StageEnum.BackgroundSelect)
                {
                    playerBackgroundChoice();
                    stageChange();
                }
                else if (Stage == StageEnum.CharacterSelect)
                {
                    if (gameMode == 1)
                    {
                        if (_controllers.IndexOf(controller) == _currentController || _currentController <= 3)
                        {
                            if (_currentController == Player1Controller)
                            {
                                Controller_Character(characterChoice, true, 0);
                                Change_currentController();
                                p1Character = characterChoice;
                                choiceTimer = 0;

                            }
                            else if (_currentController == Player2Controller)
                            {
                                Controller_Character(characterChoice, true, 1);
                                p2Character = characterChoice;

                                if (initCompleted == false) stage2Declair();
                                else {
                                    if (_currentController == Player2Controller)
                                    {
                                        _p1.attacker = false;
                                        _p2.attacker = true;
                                    }
                                    else
                                    {
                                        _p1.attacker = true;
                                        _p2.attacker = false;
                                    }
                                    _p1.CharacterChanging(p1Character);
                                    _p2.CharacterChanging(p2Character);
                                    for (int i = 0; i < _weapons1.Count; i++)
                                    {
                                        DirectionEnum dir = DirectionEnum.Blank;
                                        if (i == 0) dir = DirectionEnum.Down;
                                        if (i == 1) dir = DirectionEnum.Up;
                                        if (i == 2) dir = DirectionEnum.Left;
                                        if (i == 3) dir = DirectionEnum.Right;

                                        _weapons1[dir].characterChanging(p1Character, dir);
                                        _weapons2[dir].characterChanging(p2Character, dir);
                                    }
                                }

                                stageChange();
                                pauseDelay = 0;
                            }

                        }

                        else if (_controllers.IndexOf(controller) != _currentController)
                        {

                        }
                    }
                    else if (gameMode == 2)
                    {
                        if (_currentController == Player1Controller && _p1chosen == false)
                        {
                            Controller_Character(characterChoice, true, 0);
                            Change_currentController();
                            _p1chosen = true;
                            p1Character = characterChoice;
                        }
                        else if (_p1chosen == true)
                        {
                            Controller_Character(characterChoice, true, 1);
                            p2Character = characterChoice;

                            if (initCompleted == false) stage2Declair();
                            else {
                                _p1.CharacterChanging(p1Character);
                                _p2.CharacterChanging(p2Character);
                                for (int i = 0; i < _weapons2.Count; i++)
                                {
                                    DirectionEnum dir = DirectionEnum.Blank;
                                    if (i == 0) dir = DirectionEnum.Down;
                                    if (i == 1) dir = DirectionEnum.Up;
                                    if (i == 2) dir = DirectionEnum.Left;
                                    if (i == 3) dir = DirectionEnum.Right;

                                    _weapons2[dir].characterChanging(p2Character, dir);
                                }
                            }

                            stageChange();
                            pauseDelay = 0;
                        }


                    }

                }
                else if (Stage == StageEnum.MainGameplay)
                {

                }
                else if (Stage == StageEnum.EndScreen)
                {
                    if (gameMode == 1)
                    {
                        resetGame();
                    }
                    else if (gameMode == 2)
                    {
                        resetGame();
                    }
                    
                }
                choiceTimer = 0;
            }
            
        }
        private void Controller_Direction(Controller controller, DirectionEnum dir, bool pressed)
        {
            //takes directional inputs
            if (Stage == StageEnum.ControllerTypeSelect && moveChoiceTimer >= moveChoiceTimerLimit)
            {
                if (dir == DirectionEnum.Up && Player1Controller == -1)
                {
                    Player1Controller = _controllers.IndexOf(controller);
                    //Console.WriteLine(Player1Controller);
                }
                else if (dir == DirectionEnum.Up && Player2Controller == -1)
                {
                    Player2Controller = _controllers.IndexOf(controller);
                    
                    if (Player2Controller == Player1Controller)
                    {
                        Player2Controller = -1;
                    }
                }
                else if (dir == DirectionEnum.Down)
                {
                    Player1Controller = -1;
                    Player2Controller = -1;
                }
                UpdateControllerIcons();
            }
            if(_controllers.IndexOf(controller) == Player2Controller || _controllers.IndexOf(controller) == Player1Controller)
            {
                //if you cannot attack return
                if (Stage == StageEnum.GamemodeSelect && moveChoiceTimer >= moveChoiceTimerLimit)
                {
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

                
                if (gameMode == 1)
                {
                    if (Stage == StageEnum.BackgroundSelect && moveChoiceTimer >= moveChoiceTimerLimit)
                    {
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
                    else if (Stage == StageEnum.CharacterSelect)
                    {
                        if (_controllers.IndexOf(controller) == _currentController && moveChoiceTimer >= moveChoiceTimerLimit)
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
                        else if (_controllers.IndexOf(controller) != _currentController)
                        {
                            // defender
                        }

                    }
                    else if (Stage == StageEnum.MainGameplay)
                    {
                        if (_controllers.IndexOf(controller) == _currentController && newAttacker == false && attackDirection == DirectionEnum.Blank)
                        {

                            // attacker
                            if (pressed) pressedExtention = true;

                            _arrows[dir].Toggle(pressedExtention);
                            attackDirection = dir;
                            attack = true;
                        }
                        else if (_controllers.IndexOf(controller) != _currentController)
                        {
                            // defender
                            blockDirection = dir;
                        }
                    }
                }
                else if (gameMode == 2)
                {
                    if (Stage == StageEnum.BackgroundSelect && moveChoiceTimer >= moveChoiceTimerLimit)
                    {
                        if (moveChoiceTimer >= moveChoiceTimerLimit && _controllers.IndexOf(controller) == _currentController)
                        {
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
                    }
                    else if (Stage == StageEnum.CharacterSelect)
                    {
                        if (moveChoiceTimer >= moveChoiceTimerLimit && _controllers.IndexOf(controller) == _currentController)
                        {
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


                    }
                    else if (Stage == StageEnum.MainGameplay)
                    {
                        if (moveChoiceTimer >= moveChoiceTimerLimit && _controllers.IndexOf(controller) == _currentController) blockDirection = dir;
                    }
                    else if (Stage == StageEnum.EndScreen)
                    {
                        if (moveChoiceTimer >= moveChoiceTimerLimit && _controllers.IndexOf(controller) == _currentController)
                        {
                            if (dir == DirectionEnum.Up)
                            {
                                _hs.IncreaseHighScoreLetter();
                            } 
                            else if (dir == DirectionEnum.Down)
                            {
                                _hs.DecreaseHighScoreLetter();
                            }
                            else if (dir == DirectionEnum.Right)
                            {
                                _hs.ShiftLetterPosition("+");
                            }
                            else if (dir == DirectionEnum.Left)
                            {
                                _hs.ShiftLetterPosition("-");
                            }
                        }
                    }
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
            alive[1] = "genji1";
            Dead[1] = "genji2Dead";
            deaddead[1] = "genji3deaddead";
            alive[0] = "torbjorn";
            Dead[0] = "mercyDead";
            deaddead[0] = "reinhardtdeaddead";
            

        }
        public void LoadControllers(Controller controller, ControllerEnum Style)
        {
            switch (Style)
            {
                case ControllerEnum.WASD:
                    controller = new KeyboardController(KeyboardController.KeyboardStyleEnum.WASD);
                    break;
                case ControllerEnum.IJKL:
                    controller = new KeyboardController(KeyboardController.KeyboardStyleEnum.IJKL);
                    break;
                case ControllerEnum.Arrow:
                    controller = new KeyboardController(KeyboardController.KeyboardStyleEnum.Arrow);
                    break;
                case ControllerEnum.NumPad:
                    controller = new KeyboardController(KeyboardController.KeyboardStyleEnum.NumPad);
                    break;
                case ControllerEnum.LeftSide:
                    controller = new PS3Controller(PS3Controller.PS3StyleEnum.LeftSide);
                    break;
                case ControllerEnum.RightSide:
                    controller = new PS3Controller(PS3Controller.PS3StyleEnum.RightSide);
                    break;
            }
            _controllers.Add(controller);
            controller.Direction += Controller_Direction;

            controller.Action += Controller_Action;
        }

        public void LoadControllerIcons(ControllerEnum _con)
        {
            string texture = "";
            switch (_con)
            {
                case ControllerEnum.WASD:
                    texture = "wasd";
                    break;
                case ControllerEnum.IJKL:
                    texture = "ijkl";
                    break;
                case ControllerEnum.Arrow:
                    texture = "arrow";
                    break;
                case ControllerEnum.NumPad:
                    texture = "numpad";
                    break;
                case ControllerEnum.LeftSide:
                    texture = "leftside";
                    break;
                case ControllerEnum.RightSide:
                    texture = "rightside";
                    break;

            }
            var conIcon = new ControllerIcon(this, texture, _con);
            _controllerIcons.Add(_con, conIcon);
            //Console.WriteLine(texture + _con);
            AddEntity(conIcon);
        }
        public void LoadBackgrounds(int position)
        {
           //loads the different possible backgrounds
            
            string texture;

            switch (position)
            {
                case 1:
                    texture = "background1";
                    break;
                case 2:
                    texture = "background2";
                    break;
                case 3:
                    texture = "background3";
                    break;
                case 4:
                    texture = "background4";
                    break;
                case 5:
                    texture = "background5";
                    break;
                case 6:
                    texture = "background6";
                    break;
                default:
                    texture = null;
                    break;
            }

            var bg = new Background(this, texture, position, false);
            _bgs.Add(position, bg);
            AddEntity(bg);
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
            //loads textures xd rwar
            Texture2D texture;
            if (textures.TryGetValue(assetName, out texture))
                return texture;
            texture = contentManager.Load<Texture2D>(assetName);
            textures.Add(assetName, texture);
            return texture;
        }

        public SpriteFont LoadFont(string assetName)
        {
            //loads specific fonts
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
            //pauses the game
            if (pauseDelay > pauseDelayLimit)
            {

                pause *= -1;
                pauseDelay = 0;
            }
        }

        public void damage()
        {
            //checks if attack was blocked and says what to do
            if(gameMode == 1)
            {
                if (attackDirection != blockDirection && attackDirection != DirectionEnum.Blank)
                {
                    if (_p1.attacker)
                    {
                        _p2.health--;
                    }
                    else if (_p2.attacker)
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
                        _lastStand = false;
                    }

                    DamageAnimation = true;

                }
                else if (blockDirection == attackDirection && blockDirection != DirectionEnum.Blank)
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
                
                ChangeAttacker();
                if (_p2.health == 0) lastStand(2);
                if (_p1.health == 0) lastStand(1);
            }
            else if (gameMode == 2)
            {
                if (attackDirection != blockDirection && attackDirection != DirectionEnum.Blank)
                {
                    _p1.health--;
                    DamageAnimation = true;
                }
                else if (blockDirection == attackDirection && blockDirection != DirectionEnum.Blank)
                {
                    BlockAnimation = true;
                }
                if (_p1.health == 0 && _victor == 0)
                {
                    _victor = 1;
                    _p1.health--;

                }
            }




            attackDirection = DirectionEnum.Blank;
            blockDirection = DirectionEnum.Blank;
            timer = 0;
            weaponSpeedNeedReset = true;


        }
        public void Change_currentController()
        {
            if(gameMode == 1)
            {
                if (_currentController == Player1Controller)
                {
                    _currentController = Player2Controller;
                }
                else if (_currentController == Player2Controller)
                {
                    _currentController = Player1Controller;
                }
            }
            
            //Console.WriteLine(Player2Controller + " " + Player1Controller);
        }
        
        public void ChangeAttacker()
        {
            //changes whos attacking

            Change_currentController();
            newAttacker = true;
            ChangedAttackerRan++;
            
            
            if (_lastStand )
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
            if (_victor == 0)
            {
                if (_p2.health == -1) _victor = 1;
                else if (_p1.health == -1) _victor = 2;
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
            
            attackTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            reactionTime += gameTime.ElapsedGameTime.TotalMilliseconds;


            if (gameMode == 1)
            {
                attackCast = true;
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
                    attackTimerLimit = Convert.ToInt32(attackTimerLimit / 1.1) - 1;
                    if (attackTimerLimit <= 1)
                    {
                        attackTimerLimit = 2;
                    }
                    if (attackDirection != DirectionEnum.Blank)
                    {
                        if (_currentController == Player1Controller)
                        {
                            _weapons1[attackDirection].resetWeaponPosition(attackDirection);
                        }
                        else if (_currentController == Player2Controller)
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
                        for (int i = 0; i < _weapons1.Count; i++)
                        {
                            DirectionEnum dir = DirectionEnum.Blank;
                            if (i == 0) dir = DirectionEnum.Down;
                            if (i == 1) dir = DirectionEnum.Up;
                            if (i == 2) dir = DirectionEnum.Left;
                            if (i == 3) dir = DirectionEnum.Right;

                            _weapons1[dir].resetWeaponSpeed(attackDirection, attackTimerLimit);
                            _weapons2[dir].resetWeaponSpeed(attackDirection, attackTimerLimit);
                        }
                        weaponSpeedNeedReset = false;
                    }
                    if (_currentController == Player1Controller)
                    {
                        _weapons1[attackDirection].weaponMovement(gameTime);
                    }
                    else if (_currentController == Player2Controller)
                    {
                        _weapons2[attackDirection].weaponMovement(gameTime);
                    }
                }
            }
            else if (gameMode == 2)
            {
                if(_chosenAttack == false) ChooseAttackDirection();
                

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
                    attackTimerLimit = Convert.ToInt32(attackTimerLimit / 1.1) - 1;
                    if (attackTimerLimit <= 1)
                    {
                        attackTimerLimit = 2;
                    }


                    if (attackDirection != DirectionEnum.Blank)
                    {
                        _weapons2[attackDirection].resetWeaponPosition(attackDirection);
                    }


                    _chosenAttack = false;
                    
                    damage();




                }
                if (attackCast == true)
                {
                    if (weaponSpeedNeedReset)
                    {
                        for (int i = 0; i < _weapons2.Count; i++)
                        {
                            DirectionEnum dir = DirectionEnum.Blank;
                            if (i == 0) dir = DirectionEnum.Down;
                            if (i == 1) dir = DirectionEnum.Up;
                            if (i == 2) dir = DirectionEnum.Left;
                            if (i == 3) dir = DirectionEnum.Right;
                            
                            _weapons2[dir].resetWeaponSpeed(attackDirection, attackTimerLimit);
                        }
                        weaponSpeedNeedReset = false;
                    }
                    _weapons2[attackDirection].weaponMovement(gameTime);
                }
            }

        }

        public void newAttackerDelay(GameTime gameTime){
            //adds a delay so you cant hold down a key
            if (newAttacker == true)
            {
                attackDelayTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (attackDelayTimer >= attackDelayTimerLimit)
            {
                newAttacker = false;
                attackDelayTimer = 0;
            }
        }

        public void lastStand(int dyingPlayer)
        {
            //activates the last stand for a player
            _lastStand = true;

            if (_initLastStand == true) _initLastStand = false;

            _initLastStand = true;
                
        }

        public void ChooseAttackDirection()
        {
            int attackDirectionInt;
            attackCast = true;
            attackDirectionInt = _r.Next(1, 5);
            switch (attackDirectionInt)
            {
                case 1:
                    attackDirection = DirectionEnum.Up;
                    break;
                case 2:
                    attackDirection = DirectionEnum.Down;
                    break;
                case 3:
                    attackDirection = DirectionEnum.Left;
                    break;
                case 4:
                    attackDirection = DirectionEnum.Right;
                    break;
            }

            _chosenAttack = true;

            reactionTime = 0;
        }

        public void resetGame()
        {
            //resets the game after finishing
            stageChange();
            _victor = 0;
            choiceTimer = 0;
            _p1.health = 2;
            _p2.health = 2;
            attackTimerLimit = 1400;
            _currentController = Player1Controller;
            
        }

        

        public void stage2Declair()
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
            //changes the stage
            switch (Stage){
                case StageEnum.ControllerTypeSelect:
                    Stage = StageEnum.GamemodeSelect;
                    break;
                case StageEnum.GamemodeSelect:
                Stage = StageEnum.PlayerInformationScreen;
                    break;
                case StageEnum.PlayerInformationScreen:
                    Stage = StageEnum.BackgroundSelect;
                    break;
                case StageEnum.BackgroundSelect:
                    Stage = StageEnum.CharacterSelect;
                    break;
                case StageEnum.CharacterSelect:
                    Stage = StageEnum.MainGameplay;
                    break;
                case StageEnum.MainGameplay:
                    Stage = StageEnum.EndScreen;
                    break;
                case StageEnum.EndScreen:
                    Stage = StageEnum.BackgroundSelect;
                    break;
            }
            
            //hi
            //hey whatcha doing
        }

        public void playerGamemodeChoice()
        {
            //picks the gamemode
            if (chosen.x == 0)
            {
                gameMode = 1;
            }
            else if (chosen.x == 1)
            {
                gameMode = 2;
            }

        }

        public void playerBackgroundChoice()
        {
            //picks the background
            if (chosen.x == 0)
            {
                if (chosen.y == 0)
                {
                    _bgs[1].chosen = true;

                }
                else if (chosen.y == 1)
                {
                    _bgs[4].chosen = true; 
                }
            }
            else if (chosen.x == 1)
            {
                if (chosen.y == 0)
                {
                    _bgs[2].chosen = true;
                }
                else if (chosen.y == 1)
                {
                    _bgs[5].chosen = true;
                }
            }
            else if (chosen.x == 2)
            {
                if (chosen.y == 0)
                {
                    _bgs[3].chosen = true;
                }
                else if (chosen.y == 1)
                {
                    _bgs[6].chosen = true;
                }
            }
        }

        public void playersCharacterChoice()
        {
            //converts selection box coorditate to a CharacterEnum
            
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

        public void UpdateControllerIcons()
        {
            _controllerIcons[ControllerEnum.WASD].player = 0;
            _controllerIcons[ControllerEnum.IJKL].player = 0;
            _controllerIcons[ControllerEnum.Arrow].player = 0;
            _controllerIcons[ControllerEnum.NumPad].player = 0;
            _controllerIcons[ControllerEnum.LeftSide].player = 0;
            _controllerIcons[ControllerEnum.RightSide].player = 0;

            switch (Player1Controller)
            {

                case 0:
                    _controllerIcons[ControllerEnum.WASD].player = 1;
                    break;
                case 1:
                    _controllerIcons[ControllerEnum.IJKL].player = 1;
                    break;
                case 2:
                    _controllerIcons[ControllerEnum.Arrow].player = 1;
                    break;
                case 3:
                    _controllerIcons[ControllerEnum.NumPad].player = 1;
                    break;
                case 4:
                    _controllerIcons[ControllerEnum.LeftSide].player = 1;
                    break;
                case 5:
                    _controllerIcons[ControllerEnum.RightSide].player = 1;
                    break;
            }
            switch (Player2Controller)
            {
                case 0:
                    _controllerIcons[ControllerEnum.WASD].player = 2;
                    break;
                case 1:
                    _controllerIcons[ControllerEnum.IJKL].player = 2;
                    break;
                case 2:
                    _controllerIcons[ControllerEnum.Arrow].player = 2;
                    break;
                case 3:
                    _controllerIcons[ControllerEnum.NumPad].player = 2;
                    break;
                case 4:
                    _controllerIcons[ControllerEnum.LeftSide].player = 2;
                    break;
                case 5:
                    _controllerIcons[ControllerEnum.RightSide].player = 2;
                    break;
            }

            _controllerIcons[ControllerEnum.WASD].Position();
            _controllerIcons[ControllerEnum.IJKL].Position();
            _controllerIcons[ControllerEnum.Arrow].Position();
            _controllerIcons[ControllerEnum.NumPad].Position();
            _controllerIcons[ControllerEnum.LeftSide].Position();
            _controllerIcons[ControllerEnum.RightSide].Position();

        }

        public Vector2 TextShadowPosition(Vector2 position, float scale)
        {
            scale *= 1;
            position += new Vector2(scale, scale);
            return position;

        }

        public void Update(GameTime gameTime)
        {
            debugDelay += gameTime.ElapsedGameTime.TotalMilliseconds;
            pauseDelay += gameTime.ElapsedGameTime.TotalMilliseconds;
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (pause == -1)
            {
                if (Stage == StageEnum.GamemodeSelect)
                {
                    choiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                    moveChoiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                else if (Stage == StageEnum.ControllerTypeSelect)
                {
                    choiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                    moveChoiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;



                }
                else if (Stage == StageEnum.PlayerInformationScreen)
                {
                    choiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                    informationTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (informationTimer >= informationTimerLimit)
                    {
                        informationTimer = 0;
                        stageChange();
                    }
                }
                if (gameMode  == 1)
                {
                    
                    if (Stage == StageEnum.BackgroundSelect)
                    {
                        choiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        moveChoiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                    else if (Stage == StageEnum.CharacterSelect)
                    {
                        choiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        moveChoiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        playersCharacterChoice();
                    }
                    else if (Stage == StageEnum.MainGameplay)
                    {

                        // CharacterChoice.Update(gameTime);
                        timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        newAttackerDelay(gameTime);
                        if (_victor != 0)
                        {

                            stageChange();
                        }
                        if(attack == false && timer > timerLimit - attackTimer)
                        {
                            timer = 0;
                            damage();
                        }else if (attack == true && timer < timerLimit - attackTimer)
                        {
                            AttackAnimation(gameTime);
                        }
                        else if (attack == true && timer > timerLimit - attackTimer)
                        {
                            AttackAnimation(gameTime);
                        }
                        /*
                        if (timer > timerLimit)
                        {
                            
                        } else if (timer < timerLimit - attackTimer)
                        {

                            if (attack == true)
                            {
                            }
                        }
                        */
                    }
                    else if (Stage == StageEnum.EndScreen)
                    {

                    }
                }
                else if (gameMode == 2)
                {

                    if (Stage == StageEnum.BackgroundSelect)
                    {
                        choiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        moveChoiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                    else if (Stage == StageEnum.CharacterSelect)
                    {
                        choiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        moveChoiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        playersCharacterChoice();
                    }
                    else if (Stage == StageEnum.MainGameplay)
                    {

                        // CharacterChoice.Update(gameTime);
                        timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        newAttackerDelay(gameTime);
                        if (_victor != 0)
                        {
                            _hs.highScoreWrite(reactionTime);
                            stageChange();
                        }
                        if (timer > 150)
                        {

                            AttackAnimation(gameTime);
                        }

                    }
                    else if (Stage == StageEnum.EndScreen)
                    {
                        choiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        moveChoiceTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
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
            
            for (var i = 0; i < _entities.Count; ++i)
            {
                _entities[i].Draw(gameTime, spriteBatch);
            }

            if (Stage == StageEnum.GamemodeSelect)
            {
                if (debug > 0)
                {
                    const string debugInfo = @"
FPS: {0}
_currentController: {1}
chosen.x: {2}
chosen.y: {3}
choiceTimer: {4}
moveChoiceTimer: {5}
Stage: {6}
";
                    spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, chosen.x, chosen.y, choiceTimer, moveChoiceTimer, Stage), TextShadowPosition(new Vector2(5.0f, 0.0f), 1), Color.Black);
                    spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, chosen.x, chosen.y, choiceTimer, moveChoiceTimer, Stage), new Vector2(5.0f, 0.0f), fontColor);

                }

                const string startingScreen = @"
Hey! Welcome to Repulse! A Game about having good reaction times!
Now, Please select a gamemode!
";
                spriteBatch.DrawString(font, String.Format(startingScreen), TextShadowPosition(new Vector2(250.0f, 0.0f), 1), Color.Black);
                spriteBatch.DrawString(font, String.Format(startingScreen), new Vector2(250.0f, 0.0f), fontColor);

                const string startingScreen1 = "VS a Friend";
                spriteBatch.DrawString(font, String.Format(startingScreen1), TextShadowPosition(new Vector2(200.0f, 125.0f), 1), Color.Black);
                spriteBatch.DrawString(font, String.Format(startingScreen1), new Vector2(200.0f, 125.0f), fontColor);

                const string startingScreen2 = "Agaisnt an Bot";
                spriteBatch.DrawString(font, String.Format(startingScreen2), TextShadowPosition(new Vector2(650.0f, 125.0f), 1), Color.Black);
                spriteBatch.DrawString(font, String.Format(startingScreen2), new Vector2(650.0f, 125.0f), fontColor);

            }
            else if (Stage == StageEnum.ControllerTypeSelect)
            {

                if (debug > 0)
                {
                    const string debugInfo = @"
FPS: {0}
_currentController: {1}
chosen.x: {2}
chosen.y: {3}
choiceTimer: {4}
moveChoiceTimer: {5}
Stage: {6}
Player1Controller: {7}
Player2Controller: {8}
";
                    spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, chosen.x, chosen.y, choiceTimer, moveChoiceTimer, Stage, Player1Controller, Player2Controller), TextShadowPosition(new Vector2(5.0f, 0.0f), 1), Color.Black);
                    spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, chosen.x, chosen.y, choiceTimer, moveChoiceTimer, Stage, Player1Controller, Player2Controller), new Vector2(5.0f, 0.0f), fontColor);

                }
                const string startingScreencontroller = @"
Please Select your prefered type of controller (or keyboard placement)
By hitting the up key on said controller style
Hit any down key to reset the controllers
Finally, hit the action button of the controller to confirm
(Enter or Trigger)
";
                spriteBatch.DrawString(font, String.Format(startingScreencontroller), TextShadowPosition(new Vector2(300.0f, 0.0f), 1), Color.Black);
                spriteBatch.DrawString(font, String.Format(startingScreencontroller), new Vector2(300.0f, 0.0f), fontColor);

                const string startingScreen1 = "Player 1's Controller";
                spriteBatch.DrawString(font, String.Format(startingScreen1), TextShadowPosition(new Vector2(200.0f, 125.0f), 1), Color.Black);
                spriteBatch.DrawString(font, String.Format(startingScreen1), new Vector2(200.0f, 125.0f), fontColor);

                const string startingScreen2 = "Player 2's Controller";
                spriteBatch.DrawString(font, String.Format(startingScreen2), TextShadowPosition(new Vector2(650.0f, 125.0f), 1), Color.Black);
                spriteBatch.DrawString(font, String.Format(startingScreen2), new Vector2(650.0f, 125.0f), fontColor);

            }
            else if (Stage == StageEnum.PlayerInformationScreen)
            {
                if (gameMode == 1)
                {
                    const string startingScreenp21 = @"
You have chosen two player.
";
                    spriteBatch.DrawString(font, String.Format(startingScreenp21), TextShadowPosition(new Vector2(350.0f, 0.0f), 1), Color.Black);
                    spriteBatch.DrawString(font, String.Format(startingScreenp21), new Vector2(350.0f, 0.0f), fontColor);
                    
                }
                else if (gameMode == 2)
                {
                    const string startingScreenp22 = @"
You have chosen to play against a bot,
You cannot win this mode, 
There is only defending,
And training reaction time.
Current Highscore is: {0} Miliiseconds.
";
                    spriteBatch.DrawString(font, String.Format(startingScreenp22, _hs.highScoreRead()), TextShadowPosition(new Vector2(350.0f, 0.0f), 1), Color.Black);
                    spriteBatch.DrawString(font, String.Format(startingScreenp22, _hs.highScoreRead()), new Vector2(350.0f, 0.0f), fontColor);

                }
            }

            if (gameMode == 1)
            {
                if (Stage == StageEnum.BackgroundSelect)
                {
                    const string backgroundSelectInfo = "Please select a background.";
                    Vector2 _pos = new Vector2(5.0f, 0.0f);
                    spriteBatch.DrawString(font, String.Format(backgroundSelectInfo), TextShadowPosition(new Vector2(400.0f, 0.0f), 1), Color.Black);
                    spriteBatch.DrawString(font, String.Format(backgroundSelectInfo), new Vector2(400.0f, 0.0f), fontColor);

                }
                else if (Stage == StageEnum.CharacterSelect)
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
                        Vector2 _pos = new Vector2(5.0f, 0.0f);
                        spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, chosen.x, chosen.y, characterChoice, choiceTimer, moveChoiceTimer, Stage), TextShadowPosition(new Vector2(5.0f, 0.0f), 1), Color.Black);
                        spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, chosen.x, chosen.y, characterChoice, choiceTimer, moveChoiceTimer, Stage), new Vector2(5.0f, 0.0f), fontColor);

                    }
                    
                    const string instructions = "Player {0}: please choose a Character";
                    if (_currentController == Player1Controller)
                    {
                        spriteBatch.DrawString(font, String.Format(instructions, 1), TextShadowPosition(new Vector2(350, 0.0f), 1.25f), Color.Black, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, String.Format(instructions, 1), new Vector2(350, 0.0f), fontColor, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);

                    }
                    else if (_currentController == Player2Controller)
                    {
                        spriteBatch.DrawString(font, String.Format(instructions, 2), TextShadowPosition(new Vector2(350, 0.0f), 1.25f), Color.Black, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, String.Format(instructions, 2), new Vector2(350, 0.0f), fontColor, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
                    }
                }
                else if (Stage == StageEnum.MainGameplay)
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
                            ), TextShadowPosition(new Vector2(5.0f, 0.0f), 1), Color.Black);
                        spriteBatch.DrawString(font, String.Format(debugInfo, fps, _p1.attacker, attackDirection, blockDirection, timer, _p1.health,
                            _p2.health, newAttacker, attackDelayTimer, ChangedAttackerRan, _p2.attacker, attackTimerLimit, _currentController, Stage
                            ), new Vector2(5.0f, 0.0f), fontColor);
                    }
                    const string timeRemainingText = "{0}";
                    int timeRemaining = timerLimit - (int)timer - (int)attackTimer;
                    if (timeRemaining <= 0)
                    {
                        timeRemaining = 0;
                    }
                    spriteBatch.DrawString(fontDejaVu, String.Format(timeRemainingText, timeRemaining), TextShadowPosition(new Vector2(700.0f, -40.0f), 2), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(fontDejaVu, String.Format(timeRemainingText, timeRemaining), new Vector2(700.0f, -40.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                    if (BlockAnimation)
                    {
                        hitTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        const string blockText = "Block!!";
                        
                        spriteBatch.DrawString(fontDejaVu, String.Format(blockText), TextShadowPosition(new Vector2(445.0f, 50.0f), 2), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(fontDejaVu, String.Format(blockText), new Vector2(445.0f, 50.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        if (hitTimer > 500)
                        {
                            BlockAnimation = false;
                            hitTimer = 0;
                        }
                    }
                    if (DamageAnimation)
                    {
                        hitTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        const string damageText = "Hit!!";
                        spriteBatch.DrawString(fontDejaVu, String.Format(damageText), TextShadowPosition(new Vector2(450.0f, 50.0f), 1), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(fontDejaVu, String.Format(damageText), new Vector2(450.0f, 50.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        if (hitTimer > 500)
                        {
                            DamageAnimation = false;
                            hitTimer = 0;
                        }
                    }
                    if (_lastStand)
                    {
                        const string lastStandText = "Player {0} you must strike back or die!";
                        if(_currentController == Player1Controller)
                        {
                            spriteBatch.DrawString(fontDejaVu, String.Format(lastStandText, 1), TextShadowPosition(new Vector2(150.0f, 10.0f), 1), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                            spriteBatch.DrawString(fontDejaVu, String.Format(lastStandText, 1), new Vector2(150.0f, 10.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

                        }
                        else if (_currentController == Player2Controller)
                        {
                            spriteBatch.DrawString(fontDejaVu, String.Format(lastStandText, 2), TextShadowPosition(new Vector2(150.0f, 10.0f), 1), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                            spriteBatch.DrawString(fontDejaVu, String.Format(lastStandText, 2), new Vector2(150.0f, 10.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        }
                    }





                }
                else if (Stage == StageEnum.EndScreen)
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
                            ), TextShadowPosition(new Vector2(5.0f, 0.0f), 1), Color.Black);
                        spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, attackDirection, blockDirection, timer, _p1.health,
                            _p2.health, newAttacker, attackDelayTimer, ChangedAttackerRan, attackTimerLimit, Stage
                            ), new Vector2(5.0f, 0.0f), fontColor);
                    }
                    if (_victor != 0)
                    {
                        const string victoryText = "Player {0} Wins!!";
                        spriteBatch.DrawString(fontDejaVu, String.Format(victoryText, _victor), TextShadowPosition(new Vector2(350.0f, 0.0f), 1), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(fontDejaVu, String.Format(victoryText, _victor), new Vector2(350.0f, 0.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    }
                }
            }




            else if (gameMode == 2)
            {
                if (Stage == StageEnum.BackgroundSelect)
                {
                    const string backgroundSelectInfo = "Please select a background.";
                    Vector2 _pos = new Vector2(5.0f, 0.0f);
                    spriteBatch.DrawString(font, String.Format(backgroundSelectInfo), TextShadowPosition(new Vector2(400.0f, 0.0f), 1), Color.Black);
                    spriteBatch.DrawString(font, String.Format(backgroundSelectInfo), new Vector2(400.0f, 0.0f), fontColor);

                }
                else if (Stage == StageEnum.CharacterSelect)
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
                        Vector2 _pos = new Vector2(5.0f, 0.0f);
                        spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, chosen.x, chosen.y, characterChoice, choiceTimer, moveChoiceTimer, Stage), TextShadowPosition(new Vector2(5.0f, 0.0f), 1), Color.Black);
                        spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, chosen.x, chosen.y, characterChoice, choiceTimer, moveChoiceTimer, Stage), new Vector2(5.0f, 0.0f), fontColor);

                    }
                    string instructions = " ";
                    if (_p1chosen == false)
                    {
                        instructions = "Please choose your Character";
                    }
                    else if (_p1chosen == true)
                    {
                        instructions = "Please choose your opponent's Character";
                    }
                    spriteBatch.DrawString(font, String.Format(instructions), TextShadowPosition(new Vector2(400, 0.0f), 1.25f), Color.Black, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, String.Format(instructions), new Vector2(400, 0.0f), fontColor, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);

                }
                else if (Stage == StageEnum.MainGameplay)
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
                            ), TextShadowPosition(new Vector2(5.0f, 0.0f), 1), Color.Black);
                        spriteBatch.DrawString(font, String.Format(debugInfo, fps, _p1.attacker, attackDirection, blockDirection, timer, _p1.health,
                            _p2.health, newAttacker, attackDelayTimer, ChangedAttackerRan, _p2.attacker, attackTimerLimit, _currentController, Stage
                            ), new Vector2(5.0f, 0.0f), fontColor);
                    }

                    if (BlockAnimation)
                    {
                        hitTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        const string blockText = "Block!!";

                        spriteBatch.DrawString(fontDejaVu, String.Format(blockText), TextShadowPosition(new Vector2(445.0f, 50.0f), 2), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(fontDejaVu, String.Format(blockText), new Vector2(445.0f, 50.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        if (hitTimer > 500)
                        {
                            BlockAnimation = false;
                            hitTimer = 0;
                        }
                    }
                    if (DamageAnimation)
                    {
                        hitTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        const string damageText = "Hit!!";
                        spriteBatch.DrawString(fontDejaVu, String.Format(damageText), TextShadowPosition(new Vector2(450.0f, 50.0f), 1), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(fontDejaVu, String.Format(damageText), new Vector2(450.0f, 50.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        if (hitTimer > 500)
                        {
                            DamageAnimation = false;
                            hitTimer = 0;
                        }
                    }
                }
                else if (Stage == StageEnum.EndScreen)
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
                            ), TextShadowPosition(new Vector2(5.0f, 0.0f), 1), Color.Black);
                        spriteBatch.DrawString(font, String.Format(debugInfo, fps, _currentController, attackDirection, blockDirection, timer, _p1.health,
                            _p2.health, newAttacker, attackDelayTimer, ChangedAttackerRan, attackTimerLimit, Stage
                            ), new Vector2(5.0f, 0.0f), fontColor);
                    }
                    if (_victor != 0)
                    {
                        const string victoryText = "You lose, your ending reaction time was {0} milliseconds.";
                        spriteBatch.DrawString(font, String.Format(victoryText, reactionTime), TextShadowPosition(new Vector2(250.0f, 0.0f), 1), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, String.Format(victoryText, reactionTime), new Vector2(250.0f, 0.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        
                    }
                    string highscoreText;
                    if (_hs.newHighScore == true)
                    {
                        highscoreText = @"
You got a new highscore!!
Please Enter your Name!
";
                        string playerName = _hs.CurrentHighScoreName();
                        Console.WriteLine(playerName);
                        spriteBatch.DrawString(font, String.Format(playerName), TextShadowPosition(new Vector2(350.0f, 100.0f), 1), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, String.Format(playerName), new Vector2(350.0f, 100.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);





                    }
                    else
                    {
                        highscoreText = "You didn't get a new highscore";
                    }
                        spriteBatch.DrawString(font, String.Format(highscoreText), TextShadowPosition(new Vector2(350.0f, 50.0f), 1), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, String.Format(highscoreText), new Vector2(350.0f, 50.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);


                    
                }
            }

            if(pause == 1)
            {
                
                    const string pauseInfo = "Game is Paused";
                spriteBatch.DrawString(fontDejaVu, String.Format(pauseInfo), TextShadowPosition(new Vector2(650.0f, 500.0f), 1), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                spriteBatch.DrawString(fontDejaVu, String.Format(pauseInfo), new Vector2(650.0f, 500.0f), fontColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                
            }

            

        }
        public void DrawStrings(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 pos, Color colour, float rotation, Vector2 origin, float scale, SpriteEffects effect, float layer)
        {
            spriteBatch.DrawString(font, String.Format(text), TextShadowPosition(pos, scale), Color.Black, rotation, origin, scale, effect, layer);
            spriteBatch.DrawString(font, String.Format(text), pos, colour, rotation, origin, scale, effect, layer);

        }
    }
}