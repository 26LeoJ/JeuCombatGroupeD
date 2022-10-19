using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Battle_in_Draconia
{
    public class Game1 : Game
    {

        ////////////////////// GAME BACKGROUND ////////////////////

        #region game background

        Texture2D battleBackgroundSprite;
        Texture2D beginningSprite;
        Texture2D gameName;
        Texture2D blackFont;
        Texture2D statsBlankBattle;

        #endregion game background

        //////////////////////// CHARACTERS ////////////////////////
        #region declare characters related textures 
        Texture2D tankSprite;
        Texture2D healerSprite;
        Texture2D damagerSprite;
        Texture2D tankFlipSprite;
        Texture2D healerFlipSprite;
        Texture2D damagerFlipSprite;


        Texture2D tankHpAtkSprite;
        Texture2D healerHpAtkSprite;
        Texture2D damagerHpAtkSprite;

        Texture2D playerSprite;
        Texture2D playerHpAtkSprite;
        Texture2D iaSprite;
        Texture2D iaHpAtkSprite;
        #endregion declare characters related textures
        ////////////////////////////////////////////////////////////


        ///////////////////////// WRITTING /////////////////////////
        #region declare writting fonts

        SpriteFont gameFont; //main menu
        SpriteFont dialogsFont; //dialogs
        SpriteFont chooseFont; // choose characters text in choose menu
        SpriteFont speSkillsFont; // spe skils characters in choose menu
        SpriteFont nameCharFont; // class name charcters in choose menu
        SpriteFont battleTextFont; // text in battle
        SpriteFont buttonSprite; // button text in battle

        #endregion declare writting fonts
        ////////////////////////////////////////////////////////////

        ///////////////////////// DISPLAY //////////////////////////
        #region display

        bool display0 = true;
        bool display1 = false;
        bool display2 = false;

        #endregion display
        ////////////////////////////////////////////////////////////

        //////////////////// VARIABLES FOR GAME ////////////////////
        #region variables for game

        int playerChoice;
        string roleJoueur = "T";
        string roleIA = string.Empty;

        bool isActionIaChoosed = false;
        bool isActionJoueurChoosed = false;
        int actionJoueur;
        int actionIA;

        Tuple<int, int> resAction = new Tuple<int, int>(0, 0);

        int pvPlayer = 1;
        int pvIA = 1;

        int atkPlayer = 1;
        int atkIA = 1;

        #endregion variables for game
        ////////////////////////////////////////////////////////////

        ////////////////////////// SONGS ///////////////////////////
        #region songs

        Song batlleSong;
        bool isbattlesong = false;

        Song mainMenuSong;
        bool isMainMenu = false;

        Song selectSong;
        bool isSelectSong = false;

        #endregion songs

        ////////////////////////////////////////////////////////////

        /////////////////////////////AUTRE//////////////////////////
        #region autre

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        MouseState mouseState;
        KeyboardState keyboardState;

        #endregion autre

        //
        /////
        /////////////
        ////////////////
        ////////////////////
        ////////////////
        /////////////
        /////
        //

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //_graphics.ToggleFullScreen();
        }


        /////////
        protected override void Initialize() // initialisation dès le jeu lancé
        {

            var list = new List<string> { "T", "H", "D" };
            Random random = new Random();
            int index = random.Next(list.Count);
            roleIA = list[index];


            base.Initialize();
        }

        /////////
        protected override void LoadContent() // fonction qui charge les données
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // game background
            #region game background

            battleBackgroundSprite = Content.Load<Texture2D>("battleground/battleground5");
            beginningSprite = Content.Load<Texture2D>("start2");
            gameName = Content.Load<Texture2D>("gamename");
            statsBlankBattle = Content.Load<Texture2D>("battleground/statsBlankBattle");

            #endregion game background
            //

            // writting font
            #region writting font

            gameFont = Content.Load<SpriteFont>("galleryFont");
            dialogsFont = Content.Load<SpriteFont>("dialogsFont");
            blackFont = Content.Load<Texture2D>("black");
            chooseFont = Content.Load<SpriteFont>("chooseFont");
            speSkillsFont = Content.Load<SpriteFont>("speSkills");
            nameCharFont = Content.Load<SpriteFont>("nameChar");
            battleTextFont = Content.Load<SpriteFont>("battleTextFont");
            buttonSprite = Content.Load<SpriteFont>("buttonSprite");

            #endregion writting font
            //

            // characters sprites
            #region characters sprites
            tankSprite = Content.Load<Texture2D>("characters/tankSprite");
            healerSprite = Content.Load<Texture2D>("characters/healerSprite");
            damagerSprite = Content.Load<Texture2D>("characters/damagerSprite");

            tankFlipSprite = Content.Load<Texture2D>("characters/tankFlipSprite");
            healerFlipSprite = Content.Load<Texture2D>("characters/healerFlipSprite");
            damagerFlipSprite = Content.Load<Texture2D>("characters/damagerFlipSprite");

            tankHpAtkSprite = Content.Load<Texture2D>("characters/tankHpAtkSprite");
            healerHpAtkSprite = Content.Load<Texture2D>("characters/healerHpAtkSprite");
            damagerHpAtkSprite = Content.Load<Texture2D>("characters/damagerHpAtkSprite");
            #endregion characters sprites
            //

            // Songs
            #region songs

            batlleSong = Content.Load<Song>("song/8bit-battle");
            selectSong = Content.Load<Song>("song/selectSong");
            mainMenuSong = Content.Load<Song>("song/8bit-menu");

            #endregion songs
        }

        ///////////


        #region fonctions de coding room 
        static int DommageParRole(string role)
        {
            var charactersDM = new Dictionary<string, int>() { { "H", 1 }, { "T", 1 }, { "D", 2 } };
            return charactersDM[role];
        }

        static int HpParRole(string role)
        {
            var charactersDM = new Dictionary<string, int>() { { "H", 4 }, { "T", 5 }, { "D", 3 } };
            return charactersDM[role];
        }

        static Tuple<int, int> ResolutionAction(int actionJoueur, int actionAi, string roleJoueur, string roleIA)
        {

            int playerValueModifier = 0;
            int aiValueModifier = 0;

            bool playerSkillActivated = actionJoueur == 3;
            bool aiSkillActivated = actionAi == 3;

            bool playerDefending = actionJoueur == 2;
            bool aiDefending = actionAi == 2;


            //Tour du joueur

            switch (actionJoueur)
            {
                case 1:
                    aiValueModifier -= DommageParRole(roleJoueur);
                    break;
                case 3:
                    playerSkillActivated = true;
                    switch (roleJoueur)
                    {
                        case "D":
                            break;
                        case "H":
                            playerValueModifier += 2;
                            break;
                        case "T":
                            playerValueModifier -= 1;
                            aiValueModifier -= DommageParRole(roleJoueur) + 1;
                            break;
                    }
                    break;
            }

            switch (actionAi)
            {
                case 1:
                    playerValueModifier -= DommageParRole(roleIA);
                    break;
                case 3:
                    aiSkillActivated = true;
                    switch (roleIA)
                    {
                        case "D":
                            break;
                        case "H":
                            aiValueModifier += 2;
                            break;
                        case "T":
                            aiValueModifier -= 1;
                            playerValueModifier -= DommageParRole(roleIA) + 1;
                            break;
                    }
                    break;
            }

            if (playerDefending && playerValueModifier + 1 <= 0)
                playerValueModifier++;

            if (aiDefending && aiValueModifier + 1 <= 0)
                aiValueModifier++;

            if (playerSkillActivated && roleJoueur == "D")
            {
                var damageReceive = Math.Abs(playerValueModifier);
                aiValueModifier -= damageReceive;
            }

            if (aiSkillActivated && roleIA == "D")
            {
                var damageReceive = Math.Abs(aiValueModifier);
                playerValueModifier -= damageReceive;
            }

            return new Tuple<int, int>(playerValueModifier, aiValueModifier);

        }

        #endregion fonctions de coding room


        ///////////


        protected override void Update(GameTime gameTime) // s'effectue chaque frame ( 60 frame = 1s )
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();


            if (display0)
            {

                if (isMainMenu == false)
                {
                    MediaPlayer.Play(mainMenuSong);
                    MediaPlayer.Volume = 0.8f;//
                    isMainMenu = true;
                }

                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    display0 = false;
                    display1 = true;

                }
            }



            if (display1)
            {
                if (isSelectSong == false)
                {
                    MediaPlayer.Play(selectSong);
                    MediaPlayer.Volume = 0.8f;//
                    isSelectSong = true;
                }

                if (keyboardState.IsKeyDown(Keys.NumPad1))
                {
                    playerChoice = 1;
                    roleJoueur = "T";

                    display1 = false;
                    display2 = true;
                }

                else if (keyboardState.IsKeyDown(Keys.NumPad2))
                {
                    playerChoice = 2;
                    roleJoueur = "H";

                    display1 = false;
                    display2 = true;
                }

                else if (keyboardState.IsKeyDown(Keys.NumPad3))
                {
                    playerChoice = 3;
                    roleJoueur = "D";
                    display1 = false;
                    display2 = true;
                }

                pvPlayer = HpParRole(roleJoueur);

                atkPlayer = DommageParRole(roleJoueur);


            }

            
            if (display2)
            {

                if (isbattlesong == false)
                {
                    MediaPlayer.Play(batlleSong);
                    MediaPlayer.Volume = 0.8f;//
                    isbattlesong = true;
                    isMainMenu = false;
                }

                if (isActionIaChoosed == false)
                {
                    Random rand = new Random();
                    actionIA = rand.Next(1, 4);
                    isActionIaChoosed = true;
                }

                if (isActionJoueurChoosed == false)
                {
                    if (keyboardState.IsKeyDown(Keys.NumPad1))
                    {
                        actionJoueur = 1;
                        isActionJoueurChoosed = true;
                    }
                    else if (keyboardState.IsKeyDown(Keys.NumPad2))
                    {
                        actionJoueur = 2;
                        isActionJoueurChoosed = true;
                    }
                    else if (keyboardState.IsKeyDown(Keys.NumPad3))
                    {
                        actionJoueur = 3;
                        isActionJoueurChoosed = true;
                    }


                    //atkIA = DommageParRole(roleIA);
                    //pvIA = HpParRole(roleIA);
                }

                if (isActionIaChoosed == true && isActionJoueurChoosed == true)
                {
                    resAction = ResolutionAction(actionJoueur, actionIA, roleJoueur, roleIA);
                    pvPlayer += resAction.Item1;
                    pvIA += resAction.Item2;

                }

            }


            base.Update(gameTime);
        }

        /////////////


        protected override void Draw(GameTime gameTime) // permet de gérer uniquement le moment où l'on affiche
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();


            // GAME MAIN MENU 

            #region game main menu

            if (display0)
            {
                _spriteBatch.Draw(beginningSprite, new Rectangle(0, 0, 800, 500), Color.White);
                _spriteBatch.Draw(gameName, new Rectangle(10, 100, 600, 100), Color.Black);
                _spriteBatch.DrawString(gameFont, "Press Enter to Start your Adventure", new Vector2(10, 350), Color.White);
            }
            #endregion game main menu

            //


            // CHOOSE CHARACTER MENU 

            #region choose character menu

            if (display1)
            {
                //// SPRITE CHARACTERS ////
                
                _spriteBatch.Draw(blackFont, new Rectangle(0, 0, 1000, 1000), Color.Black);

                _spriteBatch.Draw(tankSprite, new Rectangle(100, 300, 100, 100), Color.White);
                _spriteBatch.Draw(tankHpAtkSprite, new Rectangle(110, 270, 90, 35), Color.White);


                _spriteBatch.Draw(healerSprite, new Rectangle(350, 300, 100, 100), Color.White);
                _spriteBatch.Draw(healerHpAtkSprite, new Rectangle(360, 270, 70, 30), Color.White);


                _spriteBatch.Draw(damagerSprite, new Rectangle(600, 300, 100, 100), Color.White);
                _spriteBatch.Draw(damagerHpAtkSprite, new Rectangle(620, 270, 50, 30), Color.White);


                ///// TEXT /////

                // speech
                string speechChoose = "??? : Oh, a new challenger.\n??? : Another one... \n??? : Do you wish to die that badly ? \n??? : Fine. You may choose a class, but remember, when you return,\n        you won't ever be the same.";
;               _spriteBatch.DrawString(dialogsFont, speechChoose, new Vector2(50, 10), Color.White);

                // select indication
                string damagerSelect = "PRESS 3";
                string healerSelect = "PRESS 2";
                string tankSelect = "PRESS 1";

                _spriteBatch.DrawString(chooseFont, damagerSelect, new Vector2(610, 430), Color.Red);
                _spriteBatch.DrawString(chooseFont, healerSelect, new Vector2(350, 430), Color.Red);
                _spriteBatch.DrawString(chooseFont, tankSelect, new Vector2(100, 430), Color.Red);


                // characters infos
                string tankSpe = "POWER : Sacrifice 1HP to get +1 ATK this turn only";
                _spriteBatch.DrawString(nameCharFont, "TANK", new Vector2(100, 200), Color.Red);
                _spriteBatch.DrawString(speSkillsFont, tankSpe, new Vector2(30, 250), Color.White);

                string healerSpe = "POWER : Heal 1HP";
                _spriteBatch.DrawString(nameCharFont, "HEALER", new Vector2(320, 200), Color.Red);
                _spriteBatch.DrawString(speSkillsFont, healerSpe, new Vector2(350, 250), Color.White);

                string damagerSpe = "POWER : Attack with the damages received during this turn";
                _spriteBatch.DrawString(nameCharFont, "DAMAGER", new Vector2(560, 200), Color.Red);
                _spriteBatch.DrawString(speSkillsFont, damagerSpe, new Vector2(505, 250), Color.White);

            }

            #endregion choose character menu

            //

            // IN GAME BATTLE 

            #region in game battle

            if (display2)
            {
                _spriteBatch.Draw(battleBackgroundSprite, new Rectangle(0, 0, 800, 500), Color.White);
                string messageBattle = "Argh ! An opponent !\nHe seems really strong, use :\n(1) Attack  (2) Defense  (3) Power against him !\n \n You can also run away using ESC, coward !";
                _spriteBatch.DrawString(dialogsFont, messageBattle, new Vector2(70, 70), Color.White);

                #region player


                if (playerChoice == 1)
                {
                    playerSprite = tankSprite;
                    playerHpAtkSprite = tankHpAtkSprite;
                }

                else if (playerChoice == 2)
                {
                    playerSprite = healerSprite;
                    playerHpAtkSprite = healerHpAtkSprite;
                }

                else if (playerChoice == 3)
                {
                    playerSprite = damagerSprite;
                    playerHpAtkSprite = damagerHpAtkSprite;
                }

                _spriteBatch.Draw(playerSprite, new Rectangle(150, 270, 140, 150), Color.White);
                _spriteBatch.Draw(playerHpAtkSprite, new Rectangle(160, 230, 100, 60), Color.White);

                #endregion player

                #region IA

                if (roleIA == "T")
                {
                    iaSprite = tankFlipSprite;
                    iaHpAtkSprite = tankHpAtkSprite;
                }

                else if (roleIA == "H")
                {
                    iaSprite = healerFlipSprite;
                    iaHpAtkSprite= healerHpAtkSprite;
                }

                else if (roleIA == "D")
                {
                    iaSprite = damagerFlipSprite;
                    iaHpAtkSprite = damagerHpAtkSprite;
                }

                _spriteBatch.Draw(iaSprite, new Rectangle(550, 100, 130, 140), Color.White);
  
                _spriteBatch.Draw(iaHpAtkSprite, new Rectangle(580, 50, 110, 60), Color.White);


                #endregion IA

                _spriteBatch.Draw(statsBlankBattle, new Rectangle(300, 220, 250,70), Color.White);
                _spriteBatch.DrawString(buttonSprite, "ATTACK DEFENSE POWER", new Vector2(315, 235), Color.Black);

            }

            #endregion in game battle

            //

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}