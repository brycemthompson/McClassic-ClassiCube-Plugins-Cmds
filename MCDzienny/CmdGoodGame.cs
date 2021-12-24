using System;

namespace MCDzienny
{
    public class CmdGG : Command
    {
        public override string name { get { return "gg"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }

        public override void Use(Player p, string message)
        {
            if (p.muted == false)
            {
                Command.all.Find("impersonate").Use(null, p.name + " Good game");
                return;
            }
            Player.SendMessage(p, "Nice try, but I filtered this command when you're muted.");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/gg - Says Good game.");
        }
    }
}
