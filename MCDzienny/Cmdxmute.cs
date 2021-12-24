using System;
using System.Threading;

namespace MCDzienny
{
	public class CmdXmute : Command
	{
		public override string name { get { return "xmute"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "mod"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
		public override void Use(Player p, string message)
		{
            Player who = Player.Find(message.Split(' ')[0]);

            if (who == null)
            {
                Player.SendMessage(p, "Player not found.");
                return;
            }
            else if (who == p)
            {
                Player.SendMessage(p, "You can't mute yourself!");
                return;
            }
            else if (who.group.Permission > p.group.Permission)
            {
                Player.SendMessage(p, "You can't mute someone of equal or higher rank!");
                return;
            }
            else if (who.muted)
            {
                Player.SendMessage(p, "&cThat player is already muted!");
                return;
            }

            string smTime = message.Split(' ')[1];

            if (smTime == "")
            {
                smTime = "60";
            }

            int mTime = Convert.ToInt16(smTime);
            Command.all.Find("mute").Use(p, who.name);
            Player.GlobalMessage(p.color + p.PublicName + " &cmuted " + who.color + who.PublicName + " &efor&c " + mTime + " &eseconds.");
            Thread.Sleep(mTime * 1000);
            if (who.muted == true)
            {
                Command.all.Find("mute").Use(null, who.name);
            }
		}

		public override void Help(Player p)
		{
			Player.SendMessage(p, "/xmute - Mutes the given player for a given amount of time.");
            Player.SendMessage(p, "/xmute {player} {time}");
		}
	}
}