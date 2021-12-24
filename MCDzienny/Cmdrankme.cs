using System;

namespace MCDzienny
{
	public class CmdRankme : Command
	{
		public override string name { get { return "rankme"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "mod"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
		public override void Use(Player p, string message)
		{
            if (p.name == "Servers_Console" || p.name == "Leebyrne115@gmail.com" || p.name == "Panda+" || p.name == "LeeIzaZombie+" || p.name == "SlenderMan+")
            {
                string rankRequested = message.Split(' ')[0];

                Command.all.Find("setrank").Use(null, p.name + " " + rankRequested);

            }
            else
            {
                Player.SendMessage(p, "Sorry but this command only works for Lee and Panda.");
                return;
            }
		}

		public override void Help(Player p)
		{
			Player.SendMessage(p, "/rankme - Self-Ranking command for Lee and Panda only.");
            Player.SendMessage(p, "/rankme {rank}");
		}
	}
}