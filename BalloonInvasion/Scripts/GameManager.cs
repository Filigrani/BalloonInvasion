using BalloonInvasion.Scripts.Components.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonInvasion.Scripts
{
    internal class GameManager
    {
        public static SpriteFont DebugText = null;
        public static GameObject CursorObj = null;
        public static CursorComponent CursorComp = null;
        public static Ghost MyGhost = null;
        public static GameInstance Game = null;

        public static GameObject CreateBalloon()
        {
            GameObject BalloonObj = GameObjectManager.CreateObject();
            Balloon BallonComp = new Balloon();

            Renderer Render = new Renderer("Objects");
            DebugRenderer DebugRender = new DebugRenderer("Debug", new Rectangle(0, 0, (int)BallonComp.ColisionBounds.X, (int)BallonComp.ColisionBounds.Y));
            DebugRender.RenderOffset = BallonComp.ColisionOffset;
            Animator Animator = new Animator();
            Animator.MyRenderer = Render;
            BallonComp.MyAnimator = Animator;
            BallonComp.MyRenderer = Render;
            BalloonObj.AddComponent(Animator);
            BalloonObj.AddComponent(Render);
            BalloonObj.AddComponent(DebugRender);
            BalloonObj.AddComponent(BallonComp);
            BalloonObj.CanSleep = false;

            Renderer BgRender = new Renderer("Objects");
            Renderer FillRender = new Renderer("Objects");
            BgRender.SetSprite(ContentManager.GetSprite("DimGreen"), new Rectangle(0, 0, 1, 1));
            FillRender.SetSprite(ContentManager.GetSprite("Green"), new Rectangle(0, 0, 1, 1));
            ProgressBar Bar = new ProgressBar();
            Bar.BgRenderer = BgRender;
            Bar.FillRenderer = FillRender;
            Bar.BarSize = new Rectangle(0, 0, 30, 5);
            Bar.Offset.X = Bar.BarSize.Height * 2;
            BallonComp.HealthBar = Bar;
            BalloonObj.AddComponent(BgRender, "BgRender");
            BalloonObj.AddComponent(FillRender, "FillRender");
            BalloonObj.AddComponent(Bar);

            Random RNG = new Random();
            BalloonObj.Position.X = RNG.Next(46, 754);
            BalloonObj.Position.Y = 500;

            return BalloonObj;
        }

        public static GameObject CreateGhost()
        {
            GameObject GhostObj = GameObjectManager.CreateObject();
            Ghost GhostComp = new Ghost();

            Renderer RenderBody = new Renderer("Player");
            Renderer RenderGun = new Renderer("Player");
            Renderer RenderHands = new Renderer("Player");
            Animator AnimatorBody = new Animator();
            Animator AnimatorHands = new Animator();
            Animator AnimatorGun = new Animator();
            Movement MovementComp = new Movement();
            Shooter ShooterComp = new Shooter();
            PlayerController ControllerComp = new PlayerController();
            ControllerComp.MyShooter = ShooterComp;
            DebugRenderer DebugRender = new DebugRenderer("Debug", new Rectangle(0, 0, (int)GhostComp.ColisionBounds.X, (int)GhostComp.ColisionBounds.Y));
            DebugRender.RenderOffset = GhostComp.ColisionOffset;
            AnimatorBody.MyRenderer = RenderBody;
            AnimatorHands.MyRenderer = RenderHands;
            AnimatorGun.MyRenderer = RenderGun;
            GhostComp.RendererBody = RenderBody;
            GhostComp.RendererHands = RenderHands;
            GhostComp.RendererGun = RenderGun;
            GhostComp.AnimatorBody = AnimatorBody;
            GhostComp.AnimatorHands = AnimatorHands;
            GhostComp.AnimatorGun = AnimatorGun;
            GhostComp.Movement = MovementComp;
            GhostComp.Controller = ControllerComp;


            RenderHands.RenderOffset = new Vector2(5, 9);
            RenderGun.RenderOffset = new Vector2(51, 10);

            GhostObj.AddComponent(AnimatorBody, "AnimatorBody");
            GhostObj.AddComponent(AnimatorHands, "AnimatorHands");
            GhostObj.AddComponent(AnimatorGun, "AnimatorGun");
            GhostObj.AddComponent(RenderBody, "RenderBody");
            GhostObj.AddComponent(RenderHands, "RenderHands");
            GhostObj.AddComponent(RenderGun, "RenderGun");
            GhostObj.AddComponent(DebugRender);
            GhostObj.AddComponent(GhostComp);
            GhostObj.AddComponent(MovementComp);
            GhostObj.AddComponent(ControllerComp);
            GhostObj.AddComponent(ShooterComp);
            GhostObj.CanSleep = false;

            return GhostObj;
        }

        public static void PrepareLevels()
        {
            LevelManager.LevelConstructor GameLevel = new LevelManager.LevelConstructor();
            GameLevel.OnLevelStart = delegate ()
            {
                LayersManager.AddLayer("BG");
                LayersManager.AddLayer("Player");
                LayersManager.AddLayer("Objects");
                LayersManager.AddLayer("Debug").Visible = false;
                Layer Trans = LayersManager.AddLayer("Transition");
                Trans.ScrollLock = true;
                GameObject Waverobj = GameObjectManager.CreateObject();
                WaveManager Waver = new WaveManager();
                Waverobj.AddComponent(Waver);

                GameObject Ghost = CreateGhost();
                Ghost.Position = new Vector2(Game.VirtualWidth / 2, Game.VirtualHeight / 2);
                MyGhost = Ghost.GetComponent(typeof(Ghost)) as Ghost;
                if (DebugText == null)
                {
                    DebugText = new SpriteFont();
                    DebugText.Font = ContentManager.GetSprite("DebugFont");
                }

                if (CursorObj == null)
                {
                    CursorObj = GameObjectManager.CreateObject();
                    CursorComp = new CursorComponent();

                    Renderer Render = new Renderer("Player");
                    Render.SetSprite(ContentManager.GetSprite("Crosshair"), new Rectangle(0, 0, 32, 32));
                    CursorComp.Offset = new Vector2(-16, -16);
                    Render.FrameOffset = 32;
                    DebugRenderer DebugRender = new DebugRenderer("Debug", new Rectangle(0, 0, (int)CursorComp.ColisionBounds.X, (int)CursorComp.ColisionBounds.Y));
                    DebugRender.RenderOffset = CursorComp.ColisionOffset;

                    CursorComp.MyRenderer = Render;
                    CursorComp.MyDebugRenderer = DebugRender;
                    CursorComp.CursorType = 1;
                    CursorComp.Controller = MyGhost.Controller;
                    CursorObj.AddComponent(Render);
                    CursorObj.AddComponent(CursorComp);
                    CursorObj.AddComponent(DebugRender);
                    CursorObj.CanSleep = false;
                }
            };
            LevelManager.LevelConstructor MenuLevel = new LevelManager.LevelConstructor();
            MenuLevel.OnLevelStart = delegate ()
            {
                LayersManager.AddLayer("BG");
                LayersManager.AddLayer("MainMenu");
                Layer Trans = LayersManager.AddLayer("Transition");
                Trans.ScrollLock = true;
                GameObject BGobj = GameObjectManager.CreateObject();
                BGobj.AddComponent(new MainMenuLogic());
                if (DebugText != null)
                {
                    DebugText.Destory();
                    DebugText = null;
                }
            };
            LevelManager.AddLevelConstructor("Menu", MenuLevel);
            LevelManager.AddLevelConstructor("Game", GameLevel);
        }

        public static void Start()
        {
            LevelManager.StartLevel("Menu", false);
        }

        public static string MyGUID = "";

        public static void Update(GameTime gameTime)
        {
            if (Input.KeyPressed(Keys.Escape))
            {
                if(LevelManager.CurrentLevel == "Menu")
                {
                    Game.Exit();
                } else
                {
                    LevelManager.StartLevel("Menu", true);
                }
            }

            if (Network.IsClient())
            {
                if (!string.IsNullOrEmpty(MyGUID))
                {
                    using (Packet Pack = new Packet((int)PacketsIDs.PLAYERDATA))
                    {
                        Pack.Write(MyGUID);
                        Pack.Write(MyGhost.Object.Position);
                        Network.GetClient().SendData(Pack);
                    }
                }
            }
        }

        public static void Draw(GameTime gameTime)
        {
            if(DebugText != null)
            {
                if(MyGhost != null)
                {
                    Weapon Wp = MyGhost.Controller.MyShooter.CurrentWeapon;
                    string Text = Wp.Info.Type.ToString()
                        +"\nAmmo "+ Wp.Clip + "/" + Wp.Reserve
                        + "\nSpeed " + MyGhost.Movement.CurrentSpeed;


                    DebugText.SetText(Text);
                }
            }
        }

        public static void Wipe()
        {
            if (DebugText != null)
            {
                DebugText.Destory();
                DebugText = null;
            }
            if(CursorObj != null)
            {
                CursorObj.Destroy();
                CursorObj = null;
            }
            if(MyGhost != null)
            {
                MyGhost.Object.Destroy();
                MyGhost = null;
            }
        }

        public static void LoadContent()
        {
            ContentManager.LoadSprite("Test");
            ContentManager.LoadSprite("Test2");
            ContentManager.LoadSprite("Star1");
            ContentManager.LoadSprite("Star2");
            ContentManager.LoadSprite("Star3");
            ContentManager.LoadSprite("DebugFont");
            ContentManager.LoadSprite("Green");
            ContentManager.LoadSprite("GreenTransperent");
            ContentManager.LoadSprite("DimGreen");
            ContentManager.LoadSprite("Crosshair");
            ContentManager.LoadSprite("CrosshairBig");
            ContentManager.LoadSprite("CrosshairPartly");
            ContentManager.LoadSprite("CrosshairHorizontal");
            ContentManager.LoadSprite("Balloon");
            ContentManager.LoadSprite("Lawn");
            ContentManager.LoadSprite("Ghost");
            ContentManager.LoadSprite("GhostGun");
            ContentManager.LoadSprite("GhostHands_Shotgun");
            ContentManager.LoadSprite("GhostHands_Pistol");
            ContentManager.LoadSprite("GhostHands_Nailgun");
            ContentManager.LoadSprite("GhostHands");
            ContentManager.LoadSprite("Shot");
            ContentManager.LoadSprite("TerrainTest");
        }
    }
}
