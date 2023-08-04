using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BalloonInvasion.Scripts;

namespace BalloonInvasion
{
    internal class PlayerController : Component
    {
        public Ghost MyGhost = null;
        public Shooter MyShooter = null;
        public int PreviousScroll = 0;
        public bool ClickPressed = false;


        public void CheckHit()
        {
            Microsoft.Xna.Framework.Point MousePos = Mouse.GetState().Position;
            RectangleF Aim = GameManager.CursorComp.GetPhysicalColision();
            Weapon.WeaponInfo Info = MyShooter.CurrentWeapon.Info;
            for (int i = Balloon.Balloons.Count - 1; i >= 0; i--)
            {
                Balloon balloon = Balloon.Balloons[i];
                if (balloon != null && balloon.Object != null)
                {
                    if (RectangleF.Intersect(Aim, balloon.GetPhysicalColision()) != RectangleF.Empty)
                    {
                        balloon.Hit(Info.Damage, Info.Knockback);
                        if (!MyShooter.CurrentWeapon.Info.MultiHit)
                        {
                            return;
                        }
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            bool Click = Mouse.GetState().LeftButton == ButtonState.Pressed;
            bool Click2 = Mouse.GetState().RightButton == ButtonState.Pressed;
            int Scroll = Mouse.GetState().ScrollWheelValue;

            if (PreviousScroll != Scroll)
            {
                PreviousScroll = Scroll;

                if (Scroll < 0)
                {
                    MyShooter.NextWeapon(gameTime);
                } else if (Scroll > 0)
                {
                    MyShooter.PreviousWeapon(gameTime);
                }
            }


            if (MyShooter.CurrentWeapon.Info.Auto)
            {
                if (Click)
                {
                    if (MyShooter.Shoot(gameTime))
                    {
                        CheckHit();
                    }
                }
            } else
            {
                if (!ClickPressed && Click)
                {
                    ClickPressed = true;
                    if (MyShooter.Shoot(gameTime))
                    {
                        CheckHit();
                    }
                }
                if (!Click && ClickPressed)
                {
                    ClickPressed = false;
                }
            }
            if (Click2)
            {
                MyShooter.Reload(gameTime);
            }

            MyShooter.Update(gameTime);
        }
    }
}
