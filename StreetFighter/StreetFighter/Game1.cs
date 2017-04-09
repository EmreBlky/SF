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

namespace StreetFighter
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        SoundEffect kickSong;
        SoundEffect punchSong;
        SoundEffect fon,raund;
        SoundEffectInstance instance;

        Video video;
        VideoPlayer vplayer;
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Character Ryu =new Character();
        Character Ryu2 = new Character();

        SpriteFont Arial;
        SpriteFont Arial2;
       

        Texture2D background;

        Texture2D resRy1,resRy2,hbar,hbar2;
        Texture2D logo;

        Color[] RyuData;
        Color[] Ryu2Data;

        bool hasWin = false;
        string winner;

        bool presskey = false;
        bool presskeyApproval = false;
        int prova = 0;//pressenter color change

        bool ryu1AttackOk = false, ryu2AttackOk = false;

        int fullhealthyRyu1 = 200;
        int fullhealthyRyu2 = 200;


        private const int PENCERE_GENISLIK = 768;
        private const int PENCERE_YUKSEKLIK = 224;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = PENCERE_GENISLIK;
            graphics.PreferredBackBufferHeight = PENCERE_YUKSEKLIK;
          
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            for (int i = 0; i < 8; i++)
            {
                Ryu.frameDelay[i] = 0;
                Ryu2.frameDelay[i] = 0;
            }

            instance = fon.CreateInstance();
            instance.IsLooped = true;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
            Arial = Content.Load<SpriteFont>("Arial");
            Arial2 = Content.Load<SpriteFont>("Arial2");

            kickSong =Content.Load<SoundEffect>("Sound\\kick_1");
            punchSong = Content.Load<SoundEffect>("Sound\\punch_1");
            fon = Content.Load<SoundEffect>("Sound\\fon");
            raund = Content.Load<SoundEffect>("Sound\\raund_1");

            video = Content.Load<Video>("video\\sf2_1");
            vplayer = new VideoPlayer();

            vplayer.Play(video);


            background = Content.Load<Texture2D>("images\\bg1");
            logo = Content.Load<Texture2D>("images\\logo");

            resRy1 =   Content.Load<Texture2D>("images\\ryu");
            resRy2 =   Content.Load<Texture2D>("images\\ryu2");
            hbar = Content.Load<Texture2D>("images\\hbar");
            hbar2 = Content.Load<Texture2D>("images\\hbar2");

            Ryu.inputs = false;
            Ryu2.inputs = false;


            Ryu.frameNumber[0] = 4;//Stay
            Ryu.frameNumber[1] = 5;//walk
            Ryu.frameNumber[2] = 5;//jump
            Ryu.frameNumber[3] = 0;//lean
            Ryu.frameNumber[4] = 3;//tekme
            Ryu.frameNumber[5] = 5;//Yumruk
            Ryu.frameNumber[6] = 6;//Dead

            Ryu2.frameNumber[0] = 4;//Stay
            Ryu2.frameNumber[1] = 5;//walk
            Ryu2.frameNumber[2] = 5;//jump
            Ryu2.frameNumber[3] = 0;//lean
            Ryu2.frameNumber[4] = 3;//tekme
            Ryu2.frameNumber[5] = 5;//Yumruk
            Ryu2.frameNumber[6] = 6;//Dead

            Ryu.playerStayTexture = Content.Load<Texture2D>("images\\stayRyu");
            Ryu.playerWalkTexture = Content.Load<Texture2D>("images\\walkRyu");
            Ryu.playerJumpTexture = Content.Load<Texture2D>("images\\jumpRyu");
            Ryu.playerLeanTexture = Content.Load<Texture2D>("images\\RyuLean");

            Ryu2.playerStayTexture = Content.Load<Texture2D>("images\\stayRyu_1");
            Ryu2.playerWalkTexture = Content.Load<Texture2D>("images\\walkRyu_1");
            Ryu2.playerJumpTexture = Content.Load<Texture2D>("images\\jumpRyu_1");
            Ryu2.playerLeanTexture = Content.Load<Texture2D>("images\\RyuLean_1");

            Ryu.playerAttack1Texture = Content.Load<Texture2D>("images\\tekmeRyu");
            Ryu.playerAttack2Texture = Content.Load<Texture2D>("images\\atk2");
            Ryu.playerAttack3Texture = Content.Load<Texture2D>("images\\dead");
           

            Ryu2.playerAttack1Texture = Content.Load<Texture2D>("images\\tekmeRyu_1");
            Ryu2.playerAttack2Texture = Content.Load<Texture2D>("images\\atk2_1");
            Ryu2.playerAttack3Texture = Content.Load<Texture2D>("images\\dead_1");

            Ryu.CharacterKeysToMove.Left = Keys.A;
            Ryu.CharacterKeysToMove.Right = Keys.D;
            Ryu.CharacterKeysToMove.Jump = Keys.W;
            Ryu.CharacterKeysToMove.Lean = Keys.S;

            Ryu2.CharacterKeysToMove.Left = Keys.Left;
            Ryu2.CharacterKeysToMove.Right = Keys.Right;
            Ryu2.CharacterKeysToMove.Jump = Keys.Up;
            Ryu2.CharacterKeysToMove.Lean = Keys.Down;

            Ryu.CharacterKeysToMove.Attack1 = Keys.D1;
            Ryu.CharacterKeysToMove.Attack2 = Keys.D2;
            Ryu.CharacterKeysToMove.Attack3 = Keys.D3;

            Ryu2.CharacterKeysToMove.Attack1 = Keys.O;
            Ryu2.CharacterKeysToMove.Attack2 = Keys.P;
            Ryu2.CharacterKeysToMove.Attack3 = Keys.I;

            Ryu.moveVelocity = 5;
            Ryu.playerPosition = new Vector2(5, 125);
            Ryu.yon = true;

            Ryu2.moveVelocity = 5;
            Ryu2.playerPosition = new Vector2(680, 125);
            Ryu2.yon = false;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
         
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                //this.Exit();

            // TODO: Add your update logic here
            Ryu.currentGamePadState = GamePad.GetState(PlayerIndex.One);
            
            Ryu.Update(gameTime);
            Ryu2.Update(gameTime);

            RyuData = new Color[(Ryu.mainTexture2D.Width)* Ryu.mainTexture2D.Height];
            Ryu2Data = new Color[(Ryu2.mainTexture2D.Width) * Ryu2.mainTexture2D.Height];

            Ryu.mainTexture2D.GetData(RyuData);
            Ryu2.mainTexture2D.GetData(Ryu2Data);

            Rectangle ryRectangle = new Rectangle((int)Ryu.playerPosition.X, (int)Ryu.playerPosition.Y, Ryu.mainRectangle.Width, Ryu.mainRectangle.Height);
            Rectangle ry2Rectangle = new Rectangle((int)Ryu2.playerPosition.X, (int)Ryu2.playerPosition.Y, Ryu2.mainRectangle.Width, Ryu2.mainRectangle.Height);
            if (IntersectPixels(ryRectangle, RyuData, ry2Rectangle, Ryu2Data))
            {
                if (Ryu.noAttack())
                    ryu1AttackOk = false;

                if (Ryu2.noAttack())
                    ryu2AttackOk = false;

                if (Ryu.noAttack() == false)
                {
                    if(ryu1AttackOk==false)
                    {
                      fullhealthyRyu2 -= 25;
                      ryu1AttackOk = true;
                      kickSong.Play();
                    }
                }


                if (Ryu2.noAttack() == false)
                {
                    if(ryu2AttackOk==false)
                    {
                      fullhealthyRyu1 -= 25;
                      ryu2AttackOk = true;
                      punchSong.Play();
                    }
                }

            }
            if (hasWin == false)
            {

                if (fullhealthyRyu2 < fullhealthyRyu1)//Ryu1 Kazanýr
                {
                    if (fullhealthyRyu2 <= 0)
                    {

                        winner = "RYU1 KAZANDI";
                        hasWin = true;
                        Ryu.PAUSE = true;
                        Ryu2.TuslarýKitle = true;
                        Ryu2.dieCharacter();
                    }
                }
                else//Ryu2 Kazanýr
                {
                    if (fullhealthyRyu1 <= 0)
                    {

                        winner = "RYU2 KAZANDI";
                        hasWin = true;
                        Ryu2.PAUSE = true;
                        Ryu.TuslarýKitle = true;
                        Ryu.dieCharacter();
                    }
                }

            }


                //TargetElapsedTime = TimeSpan.FromSeconds(1 / Ryu.frameTime);

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F1))
                {
                    hasWin = false;
                    Ryu2.PAUSE = false;
                    Ryu.PAUSE = false;

                    fullhealthyRyu1 =fullhealthyRyu2 = 200;
               

                    Ryu.playerPosition = new Vector2(5, 125);
                    Ryu.yon = true;
                    Ryu.TuslarýKitle = false;

                    Ryu2.playerPosition = new Vector2(680, 125);
                    Ryu2.yon = false;
                    Ryu2.TuslarýKitle = false;

                    raund.Play();

                }

                if (presskeyApproval)
                {

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        presskey = true;
                        presskeyApproval = false;
                        raund.Play();
                        instance.Play();
                        
                    }
                }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

             Ryu.playerRectangleStay=new Rectangle (Ryu.frameDelay[0] * 71, 0, 68, 98);
             Ryu.playerRectangleWalk= new Rectangle(Ryu.frameDelay[1]*75, 0 , 75, 95);
             Ryu.playerRectangleJump = new Rectangle(Ryu.frameDelay[2] * 71, 0, 69, 110);
             Ryu.playerRectangleLean  = new Rectangle(Ryu.frameDelay[3] * 74, 0, 74, 68);
            

             Ryu.playerRectangleAttack1 = new Rectangle(Ryu.frameDelay[4] * 125, 0, 125, 105);
             Ryu.playerRectangleAttack2 = new Rectangle(Ryu.frameDelay[5] * 109, 0, 109, 105);
             Ryu.playerRectangleAttack3 = new Rectangle(Ryu.frameDelay[6] * 135, 0, 135, 99);


             Ryu2.playerRectangleStay = new Rectangle(Ryu2.frameDelay[0] * 71, 0, 68, 98);
             Ryu2.playerRectangleWalk = new Rectangle(Ryu2.frameDelay[1] * 75, 0, 75, 95);
             Ryu2.playerRectangleJump = new Rectangle(Ryu2.frameDelay[2] * 71, 0, 69, 110);
             Ryu2.playerRectangleLean = new Rectangle(Ryu2.frameDelay[3] * 74, 0, 74, 68);
        
             Ryu2.playerRectangleAttack1 = new Rectangle(Ryu2.frameDelay[4] * 125, 0, 125, 105);
             Ryu2.playerRectangleAttack2 = new Rectangle(Ryu2.frameDelay[5] * 109, 0, 109, 105);
             Ryu2.playerRectangleAttack3 = new Rectangle(Ryu2.frameDelay[6] * 135, 0, 135, 99);

            spriteBatch.Begin();


            spriteBatch.Draw(vplayer.GetTexture(), GraphicsDevice.Viewport.Bounds, Color.White);

            if (vplayer.State==MediaState.Stopped)
            {

                spriteBatch.Draw(logo, new Rectangle(0, 0, 768, 224), Color.White);
                if (prova < 20)
                    spriteBatch.DrawString(Arial, Convert.ToString("Press Enter"), new Vector2(340, 180), Color.Black);
                else if (prova >= 5 && prova < 40)
                    spriteBatch.DrawString(Arial, Convert.ToString("Press Enter"), new Vector2(340, 180), Color.White);
                else if (prova >= 40)
                    prova = 0;

                presskeyApproval = true;
                prova++;
            }

            if (presskey)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, 768, 224), Color.White);



                spriteBatch.Draw(resRy1, new Rectangle(1, 5, 50, 54), Color.White);
                spriteBatch.Draw(resRy2, new Rectangle(717, 5, 50, 54), Color.White);

                spriteBatch.Draw(hbar, new Rectangle(60, 5, fullhealthyRyu1, 18), Color.Red);
                spriteBatch.Draw(hbar2, new Rectangle(60, 5, 200, 18), Color.White);
                spriteBatch.DrawString(Arial, Convert.ToString(fullhealthyRyu1), new Vector2(266, 3), Color.White);

                spriteBatch.Draw(hbar, new Rectangle(500, 5, fullhealthyRyu2, 18), Color.Red);
                spriteBatch.Draw(hbar2, new Rectangle(500, 5, 200, 18), Color.White);
                spriteBatch.DrawString(Arial, Convert.ToString(fullhealthyRyu2), new Vector2(460, 3), Color.White);

                spriteBatch.DrawString(Arial, Convert.ToString("Ryu 1"), new Vector2(58, 25), Color.Orange);
                spriteBatch.DrawString(Arial, Convert.ToString("Ryu 2"), new Vector2(652, 25), Color.Orange);

                if (hasWin)
                    spriteBatch.DrawString(Arial2, Convert.ToString(winner), new Vector2(280, 50), Color.Yellow);

                Ryu.Draw(spriteBatch);

                Ryu2.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }



        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }
    }


}



