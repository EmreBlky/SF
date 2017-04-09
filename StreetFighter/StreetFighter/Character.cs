using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StreetFighter
{
    class Character 
    {

        public KeysToMove CharacterKeysToMove = new KeysToMove();

        public GamePadState currentGamePadState ;
     
        public Vector2 playerPosition;
        public Vector2 velocity;
        private SpriteEffects rotationSprite;

        public bool TuslarıKitle=false;

        public int moveVelocity;

        public Rectangle mainRectangle;
        public Texture2D mainTexture2D;

        public Rectangle playerRectangleStay;
        public Rectangle playerRectangleWalk;
        public Rectangle playerRectangleJump;
        public Rectangle playerRectangleLean;

        private int attackController=0;

        public Rectangle playerRectangleAttack1;
        public Rectangle playerRectangleAttack2;
        public Rectangle playerRectangleAttack3;
        public Rectangle playerRectangleAttack4;

        public Texture2D playerStayTexture;
        public Texture2D playerWalkTexture;
        public Texture2D playerJumpTexture;
        public Texture2D playerLeanTexture;

        public Texture2D playerAttack1Texture;
        public Texture2D playerAttack2Texture;
        public Texture2D playerAttack3Texture;
        public Texture2D playerAttack4Texture;

        public int[] frameDelay = new int[8];

        public int[] frameNumber = new int[8];

        /* frameDelay & frameNumber
        0  playerRectangleStay;
        1  playerRectangleWalk;
        2  playerRectangleJump;
        3  playerRectangleLean;
        4  playerRectangleAttack1;//Tekme
        5  playerRectangleAttack2;
        6  playerRectangleAttack3;
        7  playerRectangleAttack4;
        */

        public bool PAUSE = false;

        public bool inputs = false;
        // false - Keyboard
        // true - Gamepad

        //Zıplamada Yön Bilgisi : Sağa doğru zıplama - Sola Doğru zıplama
        public bool yon;//0 Sol 1 Sag;
        private bool hareketliZıplama = false;

        private bool hasAttack1= false, 
                     hasAttack2= false, 
                     hasAttack3= false, 
                     hasAttack4 = false;

        private bool hasRight = false,
                     hasLeft = false, 
                     hasLean = false, 
                     hasJump = false;

        private bool hasWalk()
        {
            if (hasRight || hasLeft )
                return true;
            else
                return false;
        }

        private bool hasStay(){
            if (hasWalk() || hasLean || hasJump || !noAttack())//hepsi False ise Stay Durumu gerçekleşir
                return false;
            else
                return true;
        }

        public bool noAttack()
        {
            if (hasAttack1 || hasAttack2 /*|| hasAttack3 || hasAttack4*/)//Herhangi biri True ise True ,hepsi False ise False
                return false;
            else
                return true;
        }
        
        private bool tuslarSerbest()
        {
            if (inputs == false)
            {
                if (Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Right) &&
                    Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Left) &&
                    Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Lean) &&
                    Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Jump) &&
                    Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Attack1) &&
                    Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Attack2) /*&&
                    Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Attack3) &&
                    Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Attack4)*/)
                    return false;
                else
                    return true;
            }
            else
            {
                if ((currentGamePadState.Buttons.X == ButtonState.Pressed) &&//Tekme
                    (currentGamePadState.Buttons.B == ButtonState.Pressed) &&//Yumruk
                    (currentGamePadState.DPad.Left == ButtonState.Pressed) &&//Sola
                     (currentGamePadState.DPad.Right == ButtonState.Pressed) &&//Sağa
                     (currentGamePadState.DPad.Up == ButtonState.Pressed) &&//Zıplama
                     (currentGamePadState.DPad.Down == ButtonState.Pressed))//Eğilme
                    return false;
                else
                    return true;
            }
        }

        private Timer timerRyuJump = new Timer ();
        private Timer timerRyuStay = new Timer();
        private Timer timerRyuWalk = new Timer();
        private Timer timerRyuAttack1 = new Timer();
        private Timer timerRyuAttack2 = new Timer();
        private Timer timerRyuAttack3 = new Timer();//dead

       
        
        public void Update(GameTime gameTime)
        {

            if (PAUSE == false)
            {

                playerPosition += velocity;

                //Timer Handle
                timerRyuJump.Tick += new Timer.TimerTick(spriteRyuJump);
                timerRyuStay.Tick += new Timer.TimerTick(spriteRyuStay);
                timerRyuWalk.Tick += new Timer.TimerTick(spriteRyuWalk);
                timerRyuAttack1.Tick += new Timer.TimerTick(spriteRyuAttack1);
                timerRyuAttack2.Tick += new Timer.TimerTick(spriteRyuAttack2);
                timerRyuAttack3.Tick += new Timer.TimerTick(spriteRyuAttack3);//dead

                timerRyuJump.frameDelay = 135f;
                timerRyuStay.frameDelay = 80f;
                timerRyuWalk.frameDelay = 50f;
                timerRyuAttack1.frameDelay = 75f;
                timerRyuAttack2.frameDelay = 130f;
                timerRyuAttack3.frameDelay = 600f;
                //

                if (tuslarSerbest())
                {
                    //hasAttack1= false;
                    //hasAttack2= false; 
                    //hasAttack3= false;
                    //hasAttack4 = false;

                    hasRight = false;
                    hasLeft = false;
                    hasLean = false;
                    //hasJump = false;
                }

                // Hareketler
                if (TuslarıKitle == false)
                {

                    if ((hasJump && hasAttack1 && hasAttack2 /*&& hasAttack3 && hasAttack4*/) == false)
                    {
                        /*###########################################################################################*/
                        #region Sağa Yürüme
                        if (inputs == false)
                        {
                            if (!Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Jump) && hasJump == false &&
                                Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Right) && !Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Attack1)
                                                                                         && !Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Attack2)
                                                                                         && hasAttack1 == false
                                                                                         && hasAttack2 == false
                                /*&& hasAttack3 == false
                                && hasAttack4 == false*/)
                            {//Sağa Yürü
                                yon = true;//Sag
                                hasRight = true;
                                timerRyuWalk.Enabled = true;

                                velocity.X = +moveVelocity;
                                rotationSprite = SpriteEffects.None;
                            }
                        }
                        else
                        {
                            
                            //GamePad
                            if (!(currentGamePadState.DPad.Up == ButtonState.Pressed) && hasJump == false &&
                                (currentGamePadState.DPad.Right == ButtonState.Pressed) && !(currentGamePadState.Buttons.X == ButtonState.Pressed)
                                                                     && !(currentGamePadState.Buttons.B == ButtonState.Pressed)
                                                                     && hasAttack1 == false
                                                                     && hasAttack2 == false
                                /*&& hasAttack3 == false
                                && hasAttack4 == false*/)
                            {//Sağa Yürü
                                yon = true;//Sag
                                hasRight = true;
                                timerRyuWalk.Enabled = true;

                                velocity.X = +moveVelocity;
                                rotationSprite = SpriteEffects.None;
                            }
                        }
                        #endregion
                        /*###########################################################################################*/
                        #region Sola Yürüme
                        if (inputs == false)
                        {
                            if (!Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Jump) && hasJump == false
                                                                                         && Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Left)
                                                                                     && !Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Attack1)
                                                                                         && !Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Attack2)
                                                                                         && hasAttack1 == false
                                                                                         && hasAttack2 == false
                                /* && hasAttack3 == false
                                 && hasAttack4 == false*/)
                            {//Sola Yürü
                                yon = false;//Sol
                                hasLeft = true;
                                timerRyuWalk.Enabled = true;

                                velocity.X = -moveVelocity;
                                rotationSprite = SpriteEffects.FlipHorizontally;
                            }
                        }
                        else
                        {
                            if (!(currentGamePadState.DPad.Up == ButtonState.Pressed) && hasJump == false
                                                                                         && (currentGamePadState.DPad.Left == ButtonState.Pressed)
                                                                                         && !(currentGamePadState.Buttons.X == ButtonState.Pressed)
                                                                                         && !(currentGamePadState.Buttons.B == ButtonState.Pressed)
                                                                                         && hasAttack1 == false
                                                                                         && hasAttack2 == false
                                /*&& hasAttack3 == false
                                && hasAttack4 == false*/)
                            {//Sola Yürü
                                yon = false;//Sol
                                hasLeft = true;
                                timerRyuWalk.Enabled = true;

                                velocity.X = -moveVelocity;
                                rotationSprite = SpriteEffects.FlipHorizontally;
                            }
                        }
                        #endregion
                        /*###########################################################################################*/
                        #region Eğilme
                        if (inputs == false)
                        {
                            if (Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Lean) && hasWalk() == false //Eğilirken Adım Atmayı sağlar
                                                                                         && hasJump == false // Eğilirken zıplayamaz ve atak yapamaz
                                                                                         && hasAttack1 == false
                                                                                         && hasAttack2 == false
                                /*&& hasAttack3 == false
                                && hasAttack4 == false*/)
                            {//Eğilme

                                timerRyuWalk.Enabled = false;//Yürüme Görsellerini Durdur.
                                hasLean = true;
                                frameDelay[3] = 0;
                                mainRectangle = playerRectangleLean;
                                mainTexture2D = playerLeanTexture;
                                playerPosition.Y = 155f;//Eğildiğinde Y konumunu ayarla
                            }
                        }
                        else
                        {
                            if ((currentGamePadState.DPad.Down == ButtonState.Pressed) && hasWalk() == false //Eğilirken Adım Atmayı sağlar
                                                                     && hasJump == false // Eğilirken zıplayamaz ve atak yapamaz
                                                                     && hasAttack1 == false
                                                                     && hasAttack2 == false
                                /*&& hasAttack3 == false
                                && hasAttack4 == false*/)
                            {//Eğilme

                                timerRyuWalk.Enabled = false;//Yürüme Görsellerini Durdur.
                                hasLean = true;
                                frameDelay[3] = 0;
                                mainRectangle = playerRectangleLean;
                                mainTexture2D = playerLeanTexture;
                                playerPosition.Y = 155f;//Eğildiğinde Y konumunu ayarla
                            }
                        }
                        #endregion
                        /*###########################################################################################*/
                        #region Zıplama - Sadece Yukarı Zıplama

                        if (inputs == false)
                        {
                            if (!Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Left) &&
                                !Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Right) &&
                                 Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Jump) && hasLean == false
                                                                                          && hasJump == false
                                                                                          && hasAttack1 == false
                                                                                          && hasAttack2 == false
                                /*&& hasAttack3 == false
                                && hasAttack4 == false*/)
                            {
                                hasJump = true;
                                timerRyuJump.Enabled = true;
                                velocity.Y = -14f; // Maksimum Zıplama Yükskliği
                            }
                        }
                        else
                        {
                            if (!(currentGamePadState.DPad.Left == ButtonState.Pressed) &&
                                !(currentGamePadState.DPad.Right == ButtonState.Pressed) &&
                                 (currentGamePadState.DPad.Up == ButtonState.Pressed) && hasLean == false
                                                                                          && hasJump == false
                                                                                          && hasAttack1 == false
                                                                                          && hasAttack2 == false
                                /*&& hasAttack3 == false
                                && hasAttack4 == false*/)
                            {
                                hasJump = true;
                                timerRyuJump.Enabled = true;
                                velocity.Y = -14f; // Maksimum Zıplama Yükskliği
                            }
                        }
                        #endregion
                        /*###########################################################################################*/
                        #region Sola doğru Zıplama
                        if (inputs == false)
                        {
                            if (Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Jump) &&
                                Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Left))
                            {
                                if (hasJump == false)
                                {
                                    timerRyuWalk.Enabled = false;//Yürüme Görsellerini Durdur.
                                    hareketliZıplama = true;
                                    yon = false;//Sol
                                    hasJump = true;
                                    timerRyuJump.Enabled = true;
                                    velocity.Y = -14f; // Maksimum Zıplama Yükskliği
                                }
                            }
                        }
                        else
                        {
                            if ((currentGamePadState.DPad.Up == ButtonState.Pressed) &&
                                 (currentGamePadState.DPad.Left == ButtonState.Pressed))
                            {
                                if (hasJump == false)
                                {
                                    timerRyuWalk.Enabled = false;//Yürüme Görsellerini Durdur.
                                    hareketliZıplama = true;
                                    yon = false;//Sol
                                    hasJump = true;
                                    timerRyuJump.Enabled = true;
                                    velocity.Y = -14f; // Maksimum Zıplama Yükskliği
                                }
                            }
                        }
                        #endregion
                        /*###########################################################################################*/
                        #region Sağa doğru Zıplama
                        if (inputs == false)
                        {
                            if (Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Jump) &&
                                 Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Right))
                            {
                                if (hasJump == false)
                                {
                                    timerRyuWalk.Enabled = false;//Yürüme Görsellerini Durdur.    
                                    hareketliZıplama = true;
                                    yon = true;//Sag
                                    hasJump = true;
                                    timerRyuJump.Enabled = true;
                                    velocity.Y = -14f; // Maksimum Zıplama Yükskliği
                                }
                            }
                        }
                        else
                        {
                            if ((currentGamePadState.DPad.Up == ButtonState.Pressed) &&
                                (currentGamePadState.DPad.Right == ButtonState.Pressed))
                            {
                                if (hasJump == false)
                                {
                                    timerRyuWalk.Enabled = false;//Yürüme Görsellerini Durdur.    
                                    hareketliZıplama = true;
                                    yon = true;//Sag
                                    hasJump = true;
                                    timerRyuJump.Enabled = true;
                                    velocity.Y = -14f; // Maksimum Zıplama Yükskliği
                                }
                            }
                        }
                        #endregion
                        /*###########################################################################################*/

                        //ATAKLAR
                        /*###########################################################################################*/
                        if (inputs == false)
                        {
                            if (Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Attack1) && !Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Jump)
                                                                                           && !Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Right)
                                                                                           && !Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Left)
                                                                                           && hasWalk() == false
                                                                                           && hasJump == false
                                                                                           && hasAttack2 == false
                                /*&& hasAttack3 == false
                                && hasAttack4 == false*/)
                            {

                                hasAttack1 = true;
                                timerRyuAttack1.Enabled = true;

                            }
                        }
                        else
                        {
                            if ((currentGamePadState.Buttons.X == ButtonState.Pressed) && !(currentGamePadState.DPad.Up == ButtonState.Pressed)
                                                                                           && !(currentGamePadState.DPad.Right == ButtonState.Pressed)
                                                                                           && !(currentGamePadState.DPad.Left == ButtonState.Pressed)
                                                                                           && hasWalk() == false
                                                                                           && hasJump == false
                                                                                           && hasAttack2 == false
                                /* && hasAttack3 == false
                                 && hasAttack4 == false*/)
                            {
                                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                                hasAttack1 = true;
                                timerRyuAttack1.Enabled = true;
                            }
                        }


                        if (inputs == false)
                        {
                            if (Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Attack2) && !Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Jump)
                                                                           && !Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Right)
                                                                           && !Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Left)
                                                                             && hasWalk() == false
                                                                           && hasJump == false
                                                                           && hasAttack1 == false
                                /*&& hasAttack3 == false
                                && hasAttack4 == false*/)
                            {

                                hasAttack2 = true;
                                timerRyuAttack2.Enabled = true;
                            }
                        }
                        else
                        {
                            if ((currentGamePadState.Buttons.B == ButtonState.Pressed) && !(currentGamePadState.DPad.Up == ButtonState.Pressed)
                                                                           && !(currentGamePadState.DPad.Right == ButtonState.Pressed)
                                                                           && !(currentGamePadState.DPad.Left == ButtonState.Pressed)
                                                                             && hasWalk() == false
                                                                           && hasJump == false
                                                                           && hasAttack1 == false
                                /*&& hasAttack3 == false
                                && hasAttack4 == false*/)
                            {

                                hasAttack2 = true;
                                timerRyuAttack2.Enabled = true;
                            }
                        }


                        //if (Keyboard.GetState().IsKeyDown(CharacterKeysToMove.Attack3))
                        //{

                        //}

                    }//End

                }

                if (tuslarSerbest() && hasStay())
                {
                    timerRyuWalk.Enabled = false;//Yürüme Görsellerini/Efekt Durdur.
                    timerRyuJump.Enabled = false;//Zıplama Efektini Durdur.
                    timerRyuAttack1.Enabled = false;
                    timerRyuAttack2.Enabled = false;


                    if (yon)
                        rotationSprite = SpriteEffects.None;
                    else
                        rotationSprite = SpriteEffects.FlipHorizontally;

                    timerRyuStay.Enabled = true;
                    mainRectangle = playerRectangleStay;
                    mainTexture2D = playerStayTexture;
                }

                if (hasWalk() == false || hasLean)
                    velocity.X = 0;//Hızı 0'a Eşitle
                else
                    playerPosition.Y = 125f;

                #region Attack Kontrol Bölgesi
                if (hasAttack1)
                {

                    mainRectangle = playerRectangleAttack1;
                    mainTexture2D = playerAttack1Texture;
                    playerPosition.Y = 125f;
                    attackController++;

                    if (attackController > 40)
                    {
                        hasAttack1 = false;
                        timerRyuAttack1.Enabled = false;
                        attackController = 0;
                        frameDelay[4] = 0;

                    }

                }


                if (hasAttack2)
                {
                    mainRectangle = playerRectangleAttack2;
                    mainTexture2D = playerAttack2Texture;
                    playerPosition.Y = 125f;
                    attackController++;
                    if (attackController > 50)
                    {
                        hasAttack2 = false;
                        timerRyuAttack2.Enabled = false;
                        attackController = 0;
                        frameDelay[5] = 0;
                    }
                }

                if (hasAttack3)
                {
                    if (yon)
                    {
                        playerPosition.X -= 1f;
                        rotationSprite = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        playerPosition.X += 1f;
                        rotationSprite = SpriteEffects.None;
                    }
                    mainRectangle = playerRectangleAttack3;
                    mainTexture2D = playerAttack3Texture;
                    playerPosition.Y = 125f;
                    attackController++;
                    
                    if (attackController > 125)
                    {
                        hasAttack3 = false;
                        timerRyuAttack3.Enabled = false;
                        PAUSE = true;
                        attackController = 0;
                        frameDelay[6] = 0;
                    }
                }

                #endregion
                if (hasJump)
                {
                    velocity.Y += 0.83f; // Yerçekimi
                    if (hareketliZıplama)
                    {
                        if (yon)
                            velocity.X += 5f;
                        else
                            velocity.X -= 5f;
                    }

                }
                if (hasJump == false && hasLean == false)//Zıplamıyor ve eğilmiyorsa yada bekliyor veya beklemiyorsa
                {
                    velocity.Y = 0f;
                    hareketliZıplama = false;
                    playerPosition.Y = 125;
                }

                if (playerPosition.Y >= 125)//Karakterin Y pozisyonu 125'ten büyük ve eşitse zıplama durdurulsun
                {
                    hasJump = false;
                }

                if (playerPosition.X < 0) 
                    playerPosition.X = 0;
                else if (playerPosition.X > 700)
                    playerPosition.X = 700;
                

                timerRyuJump.Update(gameTime);
                timerRyuStay.Update(gameTime);
                timerRyuWalk.Update(gameTime);
                timerRyuAttack1.Update(gameTime);
                timerRyuAttack2.Update(gameTime);
                timerRyuAttack3.Update(gameTime);//dead
            }
        }//End Character Update End

        public void dieCharacter()
        {
            hasAttack3 = true;
            timerRyuAttack3.Enabled = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mainTexture2D, playerPosition, mainRectangle , Color.White, 0, new Vector2(0, 0), 1f, rotationSprite, 0);
        }

        private void spriteRyuStay()
        {
            frameDelay[0]++;
            if (frameDelay[0] > frameNumber[0])
                frameDelay[0] = 0;
        }

        private void spriteRyuWalk()
        {
            mainRectangle = playerRectangleWalk;
            mainTexture2D = playerWalkTexture;

            frameDelay[1]++;
            if (frameDelay[1] > frameNumber[1])
                frameDelay[1] = 0;
        }

        private void spriteRyuJump()
        {
            mainRectangle = playerRectangleJump;
            mainTexture2D = playerJumpTexture;

            frameDelay[2]++;
            if (frameDelay[2] > frameNumber[2])
                frameDelay[2] = 0;
        }

        private void spriteRyuAttack1()
        {
           
            frameDelay[4]++;
            if (frameDelay[4] > frameNumber[4])
            {      
                frameDelay[4] = 0;
            }
        }

        private void spriteRyuAttack2()
        {
            frameDelay[5]++;
            if (frameDelay[5] > frameNumber[5])
            {
                frameDelay[5] = 0;
            }
        }
        private void spriteRyuAttack3()
        {
            frameDelay[6]++;
            if (frameDelay[6] > frameNumber[6])
            {
                frameDelay[6] = 6;//dead
            }
        }


        
    }//EndClass

    public struct KeysToMove
    {
        public Keys Left;
        public Keys Right;
        public Keys Jump;
        public Keys Lean;

        public Keys Attack1;
        public Keys Attack2;
        public Keys Attack3;
        public Keys Attack4;
    }

}
