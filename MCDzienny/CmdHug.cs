using System;
using System.IO;

namespace MCDzienny
{
    public class CmdHug : Command
    {
        public override string name { get { return "hug"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            if (p.muted)
            {
                Player.SendMessage(p, "%cError: %fYou may not use this as you are %bmuted%f.");
                return;
            }
            if (message == "") { Help(p); return; }
            string two = message.Split(' ')[0];
            Player who = Player.Find(two);

            if (who == null)
            {
                Player.SendMessage(p, "Player is not online.");
                return;
            }
            Random rnd = new Random();
               int msg = rnd.Next(1, 4);
               switch (msg)
               {
                   case 1:
                       Player.GlobalMessage(p.color + p.PublicName + "%f hugged " + who.color + who.PublicName);
                       break;
                   case 2:
                       Player.GlobalMessage(p.color + p.PublicName + "%f hugged " + who.color + who.PublicName + " %f VERY TIGHT!");
                       break;
                   case 3:
                       Player.GlobalMessage(p.color + p.PublicName + "%f hugged " + who.color + who.PublicName + " %f but it was a terrible hug.");
                       break;
                   case 4:
                       Player.GlobalMessage(p.color + p.PublicName + "%f gave " + who.color + who.PublicName + " %fa warm loving hug <3");
                       break;
               }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/hug (player) - Hug (player).");
        }
    }
}
