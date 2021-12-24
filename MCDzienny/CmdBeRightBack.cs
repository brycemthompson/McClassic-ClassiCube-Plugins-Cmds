using System;

namespace MCDzienny
{
    public class CmdBrb : Command
    {
        public override string name { get { return "brb"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            if (p.muted == false)
            {
                Player.GlobalMessage("--" + p.color + p.PublicName + Server.DefaultColor + "-- will be back soon.");
                return;
            }
            Player.SendMessage(p, "Nice try, but I filtered this command when you're muted.");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/brb -- Announce that you'll be back soon ");
        }
    }
}