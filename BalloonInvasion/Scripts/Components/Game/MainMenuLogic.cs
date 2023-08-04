using BalloonInvasion.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonInvasion
{
    internal class MainMenuLogic : Component
    {
        public SpriteFont Texter = null;
        public int MenuIndex = 0;
        public int SelectedElement = 0;
        public int Elements = 0;

        public MainMenuLogic() 
        {
            Texter = new SpriteFont();
            Texter.Font = ContentManager.GetSprite("DebugFont");
            OpenMenu(0);
        }

        public void OpenMenu(int Index)
        {
            MenuIndex = Index;
            SelectedElement = 0;
            if (Index == 0)
            {
                Elements = 2;
            }else if(Index == 1)
            {
                Elements = 6;
            } else if (Index == 2)
            {
                Elements = 3;
            } else if (Index == 3)
            {
                Elements = 0;
            }
        }


        public void ProcessControls()
        {
            if (Input.KeyPressed(Keys.Up) || Input.KeyPressed(Keys.W))
            {
                if(SelectedElement > 0)
                {
                    SelectedElement--;
                }
            }
            else if (Input.KeyPressed(Keys.Down) || Input.KeyPressed(Keys.S))
            {
                if (SelectedElement < Elements)
                {
                    SelectedElement++;
                }
            }
            else if (Input.KeyPressed(Keys.Left) || Input.KeyPressed(Keys.A))
            {
                if(MenuIndex == 0)
                {
                    if(SelectedElement == 0)
                    {
                        OpenMenu(2);
                    } else if (SelectedElement == 1)
                    {
                        GameManager.Game.Exit();
                    }
                }
                else if (MenuIndex == 2)
                {
                    if (SelectedElement == 0)
                    {
                        LevelManager.StartLevel("Game");
                    } else if (SelectedElement == 1)
                    {
                        Network.StartServer();
                        LevelManager.StartLevel("Game");
                    } else if (SelectedElement == 2)
                    {
                        Network.StartClient();
                        OpenMenu(3);
                    }else if(SelectedElement == 3)
                    {
                        OpenMenu(0);
                    }
                }
            }
            else if (Input.KeyPressed(Keys.Right) || Input.KeyPressed(Keys.D))
            {
                if (MenuIndex == 0)
                {
                    if (SelectedElement == 0)
                    {
                        OpenMenu(2);
                    } else if (SelectedElement == 1)
                    {
                        GameManager.Game.Exit();
                    }
                }else if(MenuIndex == 2)
                {
                    if (SelectedElement == 0)
                    {
                        LevelManager.StartLevel("Game");
                    }else if(SelectedElement == 1)
                    {
                        Network.StartServer();
                        LevelManager.StartLevel("Game");
                    } else if(SelectedElement == 2)
                    {
                        Network.StartClient();
                        OpenMenu(3);
                        Network.GetClient().Connect("127.0.0.1", Network.DefaultPort);
                    } else if (SelectedElement == 3)
                    {
                        OpenMenu(0);
                    }
                }
            }
        }

        public string SelectMarker(int Index)
        {
            if(SelectedElement == Index)
            {
                return "> ";
            } else
            {
                return "";
            }
        }


        public override void Update(GameTime gameTime)
        {
            ProcessControls();
            string MenuText = "";
            if(Texter != null)
            {
                MenuText = "\nSimple Menu" + "\n";

                if(MenuIndex == 0)
                {
                    MenuText += "\n" + SelectMarker(0) + "Start"
                    + "\n" + SelectMarker(1) + "Exit";
                } else if (MenuIndex == 2)
                {
                    MenuText += "\n" + "Mode" + "\n"
                    + "\n" + SelectMarker(0) + "Singleplayer"
                    + "\n" + SelectMarker(1) + "Host"
                    + "\n" + SelectMarker(2) + "Connect"
                    + "\n\n" + SelectMarker(3) + "Back";
                } else if (MenuIndex == 3)
                {
                    MenuText += "\n" + "Connecting...";
                }
                Texter.SetText(MenuText);
            }
        }

        public override void OnDestroy()
        {
            Texter.Destory();
        }
    }
}
