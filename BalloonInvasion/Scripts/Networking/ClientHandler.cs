using BalloonInvasion.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonInvasion.Networking
{
    internal class ClientHandler
    {
        public static void WELCOME(int _fromClient, Packet _packet)
        {
            using (Packet Pack = new Packet((int)PacketsIDs.WELCOME))
            {
                Pack.Write("Joined!");
                Network.GetClient().SendData(Pack);
            }
        }
        public static void JOIN(int _fromClient, Packet _packet)
        {
            string MYGUID = _packet.ReadString();
            GameManager.MyGUID = MYGUID;
            LevelManager.StartLevel("Game");
        }
    }
}
