using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonInvasion.Scripts.Components.Game
{
    internal class Movement : Component
    {
        public TimeSpan LastUpdate = TimeSpan.Zero;
        public TimeSpan DodgeEnds = TimeSpan.Zero;
        public TimeSpan NextDodge = TimeSpan.Zero;

        public float Acceleration = 0.25f;
        public float Decceleration = 0.17f;
        public float TopSeed = 2.5f;
        public float DodgeSpeed = 7f;
        public float CurrentSpeed = 0;
        public float DodgeDuration = 0.5f;
        public float DodgeCooldown = 3f;

        public Vector2 Velocity = new Vector2(0, 0);

        public bool KeyLeft = false;
        public bool KeyRight = false;
        public bool KeyUp = false;
        public bool KeyDown = false;
        public bool LastMirrored = false;

        public bool IsDodging()
        {
            return LastUpdate < DodgeEnds;
        }

        public bool CanDodge()
        {
            return NextDodge == TimeSpan.Zero || LastUpdate > NextDodge;
        }

        public bool Dodge()
        {
            if (CanDodge())
            {
                DodgeEnds = LastUpdate + TimeSpan.FromSeconds(DodgeDuration);
                NextDodge = DodgeEnds + TimeSpan.FromSeconds(DodgeCooldown);
                return true;
            }
            return false;
        }

        public void ProcessMovementKeys()
        {
            KeyLeft = Input.KeyDown(Keys.A);
            KeyRight = Input.KeyDown(Keys.D);
            KeyUp = Input.KeyDown(Keys.W);
            KeyDown = Input.KeyDown(Keys.S);
        }


        public override void Update(GameTime gameTime)
        {
            LastUpdate = gameTime.TotalGameTime;
            bool Dodging = IsDodging();

            bool Move = false;
            if (!Dodging)
            {
                ProcessMovementKeys();
            }
            bool Left = KeyLeft;
            bool Right = KeyRight;
            bool Up = KeyUp;
            bool Down = KeyDown;
            Move = Left || Right || Up || Down;
            if (Dodging && (!Move || Left && Right))
            {
                if (LastMirrored)
                {
                    Left = true;
                    Right = false;
                } else
                {
                    Right = true;
                    Left = false;
                }
                Move = true;
            }


            if (Move)
            {
                if (!Dodging)
                {
                    if (CurrentSpeed < TopSeed)
                    {
                        CurrentSpeed += Acceleration;
                    }
                } else
                {
                    CurrentSpeed = DodgeSpeed;
                }
            } else
            {
                if (CurrentSpeed > 0)
                {
                    CurrentSpeed -= Decceleration;
                }
            }

            if (!Dodging)
            {
                if (CurrentSpeed > TopSeed)
                {
                    CurrentSpeed = TopSeed;
                }
            }
            if (CurrentSpeed < 0)
            {
                CurrentSpeed = 0;
            }
            Velocity = Vector2.Zero;
            if (CurrentSpeed > 0)
            {
                bool DiagonalMovement = (Left || Right) && (Down || Up);
                if (Left && !Right)
                {
                    Velocity.X = -CurrentSpeed;
                } else if (Right && !Left)
                {
                    Velocity.X = CurrentSpeed;
                }
                if (Up && !Down)
                {
                    Velocity.Y = -CurrentSpeed;
                } else if (Down && !Up)
                {
                    Velocity.Y = CurrentSpeed;
                }
                if (DiagonalMovement)
                {
                    Velocity = Vector2.Normalize(Velocity) * CurrentSpeed;
                }
            }
            if (Velocity != Vector2.Zero)
            {
                Object.Position += Velocity;
            }
        }
    }
}
