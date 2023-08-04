using BalloonInvasion.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonInvasion.Networking
{
    internal class ServerHandler
    {
        public static void WELCOME(int _fromClient, Packet _packet)
        {
            string WelcomeMessage = _packet.ReadString();
            GameObject Ghost = GameManager.CreateGhost();

            using (Packet Pack = new Packet((int)PacketsIDs.JOIN))
            {
                Pack.Write(Ghost.GUID);
                Network.GetServer().SendData(Pack, _fromClient);
            }
        }

        public static void PLAYERDATA(int _fromClient, Packet _packet)
        {
            string PlayerGUID = _packet.ReadString();
            Vector2 Position = _packet.ReadVector2();
            GameObjectManager.GetObject(PlayerGUID).Position = Position;
        }
    }
}
