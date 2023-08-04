using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using BalloonInvasion.Scripts.Components.Game;
using BalloonInvasion.Scripts;

namespace BalloonInvasion
{
    internal class Ghost : Component
    {
        public Renderer RendererBody = null;
        public Renderer RendererHands = null;
        public Renderer RendererGun = null;
        public Movement Movement = null;
        public PlayerController Controller = null;

        public Vector2 KnockbackVelocity = new Vector2(0,0);
        public Vector2 ColisionBounds = new Vector2(60, 60);
        public Vector2 ColisionOffset = new Vector2(9, 13);
        public Animator AnimatorBody = null;
        public Animator AnimatorHands = null;
        public Animator AnimatorGun = null;
        public Vector2 MoveToPosition = new Vector2(0, 0);
        public float MovingSpeed = 2;
        public TimeSpan NextBlink = TimeSpan.Zero;
        public TimeSpan NextBob = TimeSpan.Zero;
        public TimeSpan StartIdleTime = TimeSpan.FromSeconds(10);
        public bool Side = true;
        public Vector2 HandsOffset = new Vector2(5, 9);
        public Vector2 GunOffset = new Vector2(51, 10);
        public static List<Ghost> Ghosts = new List<Ghost>();
        public Vector2 BobingOffset = new Vector2(0, 0);
        public int InsertAmmo = 0;
        public TimeSpan LastUpdate = TimeSpan.Zero;
        public bool Mirrored = false;
        public string GetIdle()
        {
            if (Side)
            {
                return "IdleSide";
            } else
            {
                return "Idle";
            }
        }
        public string GetShot()
        {
            if (Side)
            {
                return "ShotSide";
            } else
            {
                return "Shot";
            }
        }
        public string GetSideAffex()
        {
            if (Side)
            {
                return "Side";
            } else
            {
                return "";
            }
        }

        public override void OnAttached()
        {
            Ghosts.Add(this);
            AnimatorBody.Animations = GetAnimationsBody();
            AnimatorBody.PlayAnimation(GetIdle());

            AnimatorHands.Animations = GetAnimationsHands();
            AnimatorHands.DebugMode = false;
            AnimatorHands.ByFrameDebug = false;
            SwitchWeapon();
            AnimatorGun.Animations = GetAnimationsGun();
            AnimatorGun.PlayAnimation("Idle");
        }
        public override void OnDestroy()
        {
            Ghosts.Remove(this);
        }

        public string GetWeapon()
        {
            return Controller.MyShooter.CurrentWeapon.Info.Type.ToString();
        }

        public Dictionary<string, Animator.AnimationData> GetAnimationsBody()
        {
            Dictionary<string, Animator.AnimationData> Dict = new Dictionary<string, Animator.AnimationData>();

            Animator.AnimationData IdleAnim = new Animator.AnimationData("Idle");
            IdleAnim.DefaultLoop = true;
            Animator.AnimationData IdleSideAnim = new Animator.AnimationData("IdleSide");
            IdleSideAnim.DefaultLoop = true;
            Animator.AnimationData BlinkAnim = new Animator.AnimationData("Blink");
            Animator.AnimationData BlinkSideAnim = new Animator.AnimationData("BlinkSide");
            Animator.AnimationData ShotAnim = new Animator.AnimationData("Shot");
            Animator.AnimationData ShotSideAnim = new Animator.AnimationData("ShotSide");
            Texture2D SpriteSheet = ContentManager.GetSprite("Ghost");
            for (int i = 0; i <= 0; i++) // Idle
            {
                IdleAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * i, 0, 64, 80), 200);
            }
            Dict.Add(IdleAnim.Name, IdleAnim);
            for (int i = 5; i <= 5; i++) // IdleSide
            {
                IdleSideAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * i, 0, 64, 80), 200);
            }
            Dict.Add(IdleSideAnim.Name, IdleSideAnim);

            BlinkAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 0, 0, 64, 80), 100);
            BlinkAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 1, 0, 64, 80), 100);
            BlinkAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 2, 0, 64, 80), 100);
            BlinkAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 3, 0, 64, 80), 100);
            BlinkAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 2, 0, 64, 80), 100);
            BlinkAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 1, 0, 64, 80), 100);
            BlinkAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 0, 0, 64, 80), 100);
            Dict.Add(BlinkAnim.Name, BlinkAnim);

            BlinkSideAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 5, 0, 64, 80), 100);
            BlinkSideAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 6, 0, 64, 80), 100);
            BlinkSideAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 7, 0, 64, 80), 100);
            BlinkSideAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 7, 0, 64, 80), 100);
            BlinkSideAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 6, 0, 64, 80), 100);
            BlinkSideAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 5, 0, 64, 80), 100);
            Dict.Add(BlinkSideAnim.Name, BlinkSideAnim);

            ShotAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 4, 0, 64, 80), 300);
            ShotAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 2, 0, 64, 80), 70);
            ShotAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 1, 0, 64, 80), 70);
            ShotAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 0, 0, 64, 80), 70);
            Dict.Add(ShotAnim.Name, ShotAnim);

            ShotSideAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 9, 0, 64, 80), 300);
            ShotSideAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 7, 0, 64, 80), 70);
            ShotSideAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 6, 0, 64, 80), 70);
            ShotSideAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 5, 0, 64, 80), 70);
            Dict.Add(ShotSideAnim.Name, ShotSideAnim);


            Animator.AnimationData DodgeStartAnim = new Animator.AnimationData("DodgeStart");
            DodgeStartAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 9, 0, 64, 80), 10);
            DodgeStartAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 10, 0, 64, 80), 10);
            DodgeStartAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 11, 0, 64, 80), 10);
            DodgeStartAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 12, 0, 64, 80), 10);
            DodgeStartAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 13, 0, 64, 80), 10);
            Dict.Add(DodgeStartAnim.Name, DodgeStartAnim);
            Animator.AnimationData DodgeAnim = new Animator.AnimationData("Dodge");
            DodgeAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 14, 0, 64, 80), 50);
            DodgeAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 15, 0, 64, 80), 50);
            DodgeAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 16, 0, 64, 80), 50);
            DodgeAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 17, 0, 64, 80), 50);
            DodgeAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 18, 0, 64, 80), 50);
            DodgeAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 19, 0, 64, 80), 50);
            DodgeAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 20, 0, 64, 80), 50);
            DodgeAnim.DefaultLoop = true;
            Dict.Add(DodgeAnim.Name, DodgeAnim);

            Animator.AnimationData MeltAnim = new Animator.AnimationData("Melt");
            MeltAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 5, 0, 64, 80), 80);
            MeltAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 21, 0, 64, 80),80);
            MeltAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 22, 0, 64, 80),80);
            MeltAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 23, 0, 64, 80),80);
            MeltAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 24, 0, 64, 80),80);
            MeltAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 25, 0, 64, 80),80);
            MeltAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 26, 0, 64, 80),80);
            MeltAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 27, 0, 64, 80),80);
            MeltAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 28, 0, 64, 80),80);
            MeltAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 29, 0, 64, 80),80);
            MeltAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(64 * 30, 0, 64, 80),80);
            Dict.Add(MeltAnim.Name, MeltAnim);
            return Dict;
        }
        public Dictionary<string, Animator.AnimationData> GetAnimationsHands()
        {
            Dictionary<string, Animator.AnimationData> Dict = new Dictionary<string, Animator.AnimationData>();

            Animator.AnimationData IdleAnim = new Animator.AnimationData("Idle");
            IdleAnim.DefaultLoop = true;
            Texture2D SpriteSheet = ContentManager.GetSprite("GhostHands_Shotgun");
            Texture2D SpriteSheet2 = ContentManager.GetSprite("GhostHands_Pistol");
            Texture2D SpriteSheet3 = ContentManager.GetSprite("GhostHands_Nailgun");
            for (int i = 0; i <= 0; i++) // Idle
            {
                IdleAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * i, 0, 69, 66), 200);
            }
            Dict.Add(IdleAnim.Name, IdleAnim);
            Animator.AnimationData IdleShotgunAnim = new Animator.AnimationData("Idle_Shotgun");
            IdleShotgunAnim.DefaultLoop = true;
            Animator.AnimationData ShotgunShotAnim = new Animator.AnimationData("Shot_Shotgun");
            Animator.AnimationData ShotgunReloadAnim = new Animator.AnimationData("Reload_Shotgun");
            Animator.AnimationData ShotgunInsertAnim = new Animator.AnimationData("ReloadInsert_Shotgun");
            Animator.AnimationData ShotgunInsertLastAnim = new Animator.AnimationData("ReloadInsertLast_Shotgun");
            for (int i = 1; i <= 1; i++) // Idle
            {
                IdleShotgunAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * i, 0, 69, 66), 200);
            }
            Dict.Add(IdleShotgunAnim.Name, IdleShotgunAnim);

            ShotgunShotAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 1, 0, 69, 66), 100);
            ShotgunShotAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 2, 0, 69, 66), 100);
            ShotgunShotAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 3, 0, 69, 66), 100);
            ShotgunShotAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 2, 0, 69, 66), 100);
            ShotgunShotAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 1, 0, 69, 66), 100);
            Dict.Add(ShotgunShotAnim.Name, ShotgunShotAnim);

            // Empty hand takes first ammo
            ShotgunReloadAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 1, 0, 69, 66), 100);
            ShotgunReloadAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 5, 0, 69, 66), 100);
            ShotgunReloadAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 6, 0, 69, 66), 100);
            ShotgunReloadAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 7, 0, 69, 66), 100);
            Dict.Add(ShotgunReloadAnim.Name, ShotgunReloadAnim);

            ShotgunInsertAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 8, 0, 69, 66), 100);
            ShotgunInsertAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 9, 0, 69, 66), 100);
            ShotgunInsertAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 10, 0, 69, 66), 100);
            ShotgunInsertAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 11, 0, 69, 66), 100);
            ShotgunInsertAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 12, 0, 69, 66), 100);
            ShotgunInsertAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 13, 0, 69, 66), 100);
            Dict.Add(ShotgunInsertAnim.Name, ShotgunInsertAnim);

            ShotgunInsertLastAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 8, 0, 69, 66), 100);
            ShotgunInsertLastAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 9, 0, 69, 66), 100);
            ShotgunInsertLastAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 10, 0, 69, 66), 100);
            ShotgunInsertLastAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 11, 0, 69, 66), 100);
            ShotgunInsertLastAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 12, 0, 69, 66), 100);
            ShotgunInsertLastAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 6, 0, 69, 66), 100);
            ShotgunInsertLastAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(69 * 5, 0, 69, 66), 100);
            Dict.Add(ShotgunInsertLastAnim.Name, ShotgunInsertLastAnim);

            Animator.AnimationData IdlePistolSideAnim = new Animator.AnimationData("Idle_PistolSide");
            IdlePistolSideAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 0, 0, 69, 66), 100);
            Dict.Add(IdlePistolSideAnim.Name, IdlePistolSideAnim);

            Animator.AnimationData IdlePistolAnim = new Animator.AnimationData("Idle_Pistol");
            IdlePistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 16, 0, 69, 66), 100);
            Dict.Add(IdlePistolAnim.Name, IdlePistolAnim);

            Animator.AnimationData ShotPistolAnim = new Animator.AnimationData("Shot_Pistol");
            ShotPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 0, 0, 69, 66), 30);
            ShotPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 1, 0, 69, 66), 30);
            ShotPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 2, 0, 69, 66), 60);
            ShotPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 1, 0, 69, 66), 30);
            ShotPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 0, 0, 69, 66), 30);
            Dict.Add(ShotPistolAnim.Name, ShotPistolAnim);

            Animator.AnimationData ReloadPistolAnim = new Animator.AnimationData("Reload_Pistol");
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 0, 0, 69, 66), 60);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 1, 0, 69, 66), 60);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 2, 0, 69, 66), 60);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 3, 0, 69, 66), 60);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 7, 0, 69, 66), 120);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 8, 0, 69, 66), 70);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 6, 0, 69, 66), 70);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 5, 0, 69, 66), 70);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 4, 0, 69, 66), 70);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 3, 0, 69, 66), 70);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 9, 0, 69, 66), 70);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 10, 0, 69, 66), 60);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 11, 0, 69, 66), 60);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 12, 0, 69, 66), 60);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 13, 0, 69, 66), 60);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 14, 0, 69, 66), 100);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 15, 0, 69, 66), 100);
            ReloadPistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 0, 0, 69, 66), 100);
            Dict.Add(ReloadPistolAnim.Name, ReloadPistolAnim);

            Animator.AnimationData FixJamistolAnim = new Animator.AnimationData("FixJam_Pistol");
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 0, 0, 69, 66), 60);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 1, 0, 69, 66), 60);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 2, 0, 69, 66), 60);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 3, 0, 69, 66), 60);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 9, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 10, 0, 69, 66), 60);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 11, 0, 69, 66), 60);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 12, 0, 69, 66), 60);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 13, 0, 69, 66), 60);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 11, 0, 69, 66), 89);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 12, 0, 69, 66), 89);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 13, 0, 69, 66), 89);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 12, 0, 69, 66), 89);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 11, 0, 69, 66), 89);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 12, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 13, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 18, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 17, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 18, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 19, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 18, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 17, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 18, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 19, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 18, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 17, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 18, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 19, 0, 69, 66), 70);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 13, 0, 69, 66), 140);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 14, 0, 69, 66), 100);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 15, 0, 69, 66), 100);
            FixJamistolAnim.AddFrame(SpriteSheet2, new Microsoft.Xna.Framework.Rectangle(69 * 0, 0, 69, 66), 100);
            Dict.Add(FixJamistolAnim.Name, FixJamistolAnim);
            Animator.AnimationData IdleNailgunAnim = new Animator.AnimationData("Idle_Nailgun");
            IdleNailgunAnim.AddFrame(SpriteSheet3, new Microsoft.Xna.Framework.Rectangle(69 * 0, 0, 69, 66), 100);
            Dict.Add(IdleNailgunAnim.Name, IdleNailgunAnim);

            return Dict;
        }
        public Dictionary<string, Animator.AnimationData> GetAnimationsGun()
        {
            Dictionary<string, Animator.AnimationData> Dict = new Dictionary<string, Animator.AnimationData>();

            Animator.AnimationData IdleAnim = new Animator.AnimationData("Idle");
            IdleAnim.DefaultLoop = true;
            Animator.AnimationData ShotAnim = new Animator.AnimationData("Shot");
            ShotAnim.DefaultLoop = false;
            Texture2D SpriteSheet = ContentManager.GetSprite("Shot");
            for (int i = 0; i <= 0; i++) // Idle
            {
                IdleAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0), 200);
            }
            Dict.Add(IdleAnim.Name, IdleAnim);
            for (int i = 0; i <= 0; i++) // Reload
            {
                ShotAnim.AddFrame(SpriteSheet, new Microsoft.Xna.Framework.Rectangle(0, 0, 43, 37), 110);
            }
            Dict.Add(ShotAnim.Name, ShotAnim);

            return Dict;
        }

        public void MoveTo(Vector2 pos)
        {
            MoveToPosition = new Vector2(pos.X-30,pos.Y-30);
        }

        public void DoShot()
        {
            CancleIdle();
            Weapon wp = Controller.MyShooter.CurrentWeapon;
            if(wp.Info.Type != Weapon.WeaponType.Nailgun)
            {
                AnimatorGun.PlayAnimation("Shot");
            }
            if (!wp.IsReloading)
            {
                AnimatorHands.PlayAnimation("Shot_"+ GetWeapon());
            }
            
            AnimatorBody.PlayAnimation(GameManager.MyGhost.GetShot());
            Random RNG = new Random();
            RendererHands.RenderOffset += new Vector2(RNG.Next(-wp.Info.Recoil, wp.Info.Recoil), RNG.Next(-wp.Info.Recoil, wp.Info.Recoil));
            
        }
        public void DoReload(int AddAmmo, int Speed = 1)
        {
            CancleIdle();
            InsertAmmo = AddAmmo;
            AnimatorHands.PlayAnimation("Reload_"+ GetWeapon());
        }

        public void SwitchWeapon()
        {
            AnimatorHands.PlayAnimation(AnimatorHands.GetAnimationNameIfExist("Idle_"+GetWeapon()+GetSideAffex(), "Idle_" + GetWeapon()));
        }

        public void SetWeaponOffsets()
        {
            string Weapon = GetWeapon();

            if (!Mirrored)
            {
                if (Weapon == "Shotgun")
                {
                    HandsOffset = new Vector2(5, 9);
                    GunOffset = new Vector2(46, 1);
                } else if (Weapon == "Pistol")
                {
                    HandsOffset = new Vector2(5, 9);
                    GunOffset = new Vector2(36, 5);
                }
            } else
            {
                if (Weapon == "Shotgun")
                {
                    HandsOffset = new Vector2(-5, 9);
                    GunOffset = new Vector2(-23, 1);
                } else if (Weapon == "Pistol")
                {
                    HandsOffset = new Vector2(-10, 9);
                    GunOffset = new Vector2(-18, 5);
                }
            }
        }

        public void StartIdle()
        {
            Side = false;
            if (AnimatorBody.CurrentAnimation.Data.Name.StartsWith("Idle"))
            {
                AnimatorBody.PlayAnimation(GetIdle());
            }
            if (AnimatorHands.CurrentAnimation.Data.Name.StartsWith("Idle"))
            {
                AnimatorHands.PlayAnimation(AnimatorHands.GetAnimationNameIfExist("Idle_" + GetWeapon() + GetSideAffex(), "Idle_" + GetWeapon()));
            }
        }
        public void CancleIdle()
        {
            StartIdleTime = LastUpdate + TimeSpan.FromSeconds(24);
            Side = true;
            if (AnimatorBody.CurrentAnimation.Data.Name.StartsWith("Idle"))
            {
                AnimatorBody.PlayAnimation(GetIdle());
            }
            if (AnimatorHands.CurrentAnimation.Data.Name.StartsWith("Idle"))
            {
                AnimatorHands.PlayAnimation(AnimatorHands.GetAnimationNameIfExist("Idle_" + GetWeapon() + GetSideAffex(), "Idle_" + GetWeapon()));
            }
        }

        public bool SetMirrored(bool mirrored)
        {
            if(Mirrored != mirrored)
            {
                SpriteEffects Eff;
                if (mirrored)
                {
                    Eff = SpriteEffects.FlipHorizontally;
                } else
                {
                    Eff = SpriteEffects.None;
                }

                RendererBody.SpriteEffect = Eff;
                RendererHands.SpriteEffect = Eff;
                RendererGun.SpriteEffect = Eff;
                Mirrored = mirrored;
                return true;
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            LastUpdate = gameTime.TotalGameTime;
            if (NextBlink < gameTime.TotalGameTime)
            {
                NextBlink = gameTime.TotalGameTime + TimeSpan.FromSeconds(new Random().Next(5,10));
                if (Side)
                {
                    AnimatorBody.PlayAnimation("BlinkSide");
                } else
                {
                    AnimatorBody.PlayAnimation("Blink");
                }
            }

            if(StartIdleTime != TimeSpan.Zero)
            {
                if (StartIdleTime < gameTime.TotalGameTime)
                {
                    StartIdleTime = TimeSpan.Zero;
                    StartIdle();
                }
            }

            bool Dodging = Movement.IsDodging();

            if (!Dodging)
            {
                if (Input.KeyPressed(Keys.Space))
                {
                    if (Movement.CanDodge())
                    {
                        CancleIdle();
                        if (Movement.Dodge())
                        {
                            AnimatorBody.PlayAnimation("DodgeStart");
                        }
                    }
                }

                if (AnimatorBody.CurrentAnimation.Data.Name == "Dodge")
                {
                    AnimatorBody.PlayAnimation(GetIdle());
                }
            }

            bool MirroringChanged = false;

            if(Movement.Velocity.X < 0)
            {
                MirroringChanged = SetMirrored(true);
            } else if(Movement.Velocity.X > 0)
            {
                MirroringChanged = SetMirrored(false);
            }
            Movement.LastMirrored = Mirrored;

            SetWeaponOffsets();
            float dt = gameTime.ElapsedGameTime.Milliseconds;

            if (!MirroringChanged)
            {
                RendererHands.RenderOffset = Vector2.Lerp(RendererHands.RenderOffset, HandsOffset, dt * 0.1f);
                RendererGun.RenderOffset = new Vector2(RendererHands.RenderOffset.X + GunOffset.X, RendererHands.RenderOffset.Y + GunOffset.Y);
            } else
            {
                RendererHands.RenderOffset = HandsOffset;
                RendererGun.RenderOffset = GunOffset;
            }

            if (Input.KeyPressed(Keys.M))
            {
                FunnyMelt();
            }
        }

        public RectangleF GetPhysicalColision()
        {
            Vector2 ColOffset = ColisionOffset * LayersManager.Scaler;
            Vector2 TempBound = ColisionBounds;
            if (Object != null)
            {
                return new RectangleF(Object.Position.X + ColOffset.X, Object.Position.Y + ColOffset.Y, TempBound.X, TempBound.Y);
            }
            return new RectangleF(0,0,0,0);
        }

        public void FunnyMelt()
        {
            AnimatorBody.PlayAnimation("Melt");
            RendererHands.Visible = false;
        }

        public override void OnAnimationFinished(GameTime gameTime, string AnimationName, string SenderName)
        {
            if(SenderName == "AnimatorGun")
            {
                if(AnimationName == "Shot")
                {
                    AnimatorGun.PlayAnimation("Idle");
                }
            }
            else if (SenderName == "AnimatorHands")
            {
                if (AnimationName == "Shot_Shotgun")
                {
                    AnimatorHands.PlayAnimation("Idle_Shotgun");
                }
                if (AnimationName == "Reload_Shotgun")
                {
                    if(InsertAmmo > 1)
                    {
                        AnimatorHands.PlayAnimation("ReloadInsert_Shotgun");
                    } else
                    {
                        AnimatorHands.PlayAnimation("ReloadInsertLast_Shotgun");
                    }
                }
                if (AnimationName == "ReloadInsert_Shotgun")
                {
                    InsertAmmo--;
                    if (InsertAmmo == 1)
                    {
                        AnimatorHands.PlayAnimation("ReloadInsertLast_Shotgun");
                    }else
                    {
                        AnimatorHands.PlayAnimation("ReloadInsert_Shotgun");
                    }
                }
                if (AnimationName == "ReloadInsertLast_Shotgun")
                {
                    InsertAmmo--;
                    AnimatorHands.PlayAnimation("Shot_Shotgun");
                }
                if (AnimationName == "Shot_Pistol")
                {
                    AnimatorHands.PlayAnimation(AnimatorHands.GetAnimationNameIfExist("Idle_Pistol" + GetSideAffex(), "Idle_Pistol"));
                }
                if (AnimationName == "Reload_Pistol")
                {
                    AnimatorHands.PlayAnimation(AnimatorHands.GetAnimationNameIfExist("Idle_Pistol" + GetSideAffex(), "Idle_Pistol"));
                }
            } else if (SenderName == "AnimatorBody")
            {
                if (AnimationName == "Blink" || AnimationName == "BlinkSide" || AnimationName == "Shot" || AnimationName == "ShotSide" || AnimationName == "Dodge")
                {
                    AnimatorBody.PlayAnimation(GetIdle());
                }
                if (AnimationName == "DodgeStart")
                {
                    AnimatorBody.PlayAnimation("Dodge");
                }
            }
        }
    }
}
