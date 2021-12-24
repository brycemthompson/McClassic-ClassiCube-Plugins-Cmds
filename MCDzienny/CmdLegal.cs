//Created By Bboy505
using System;

namespace MCDzienny
{
	public class CmdLegal : Command
	{
		public override string name { get { return "legal"; } }
		public override string shortcut { get { return "l"; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
		public override void Use(Player p, string message)
		{
            Player.GlobalMessage("&a--------------------------------------------");
            Player.GlobalMessage("");
            Player.GlobalMessage("%eLast players are %cLegal!");
            Player.GlobalMessage("%2Last check done by: " + p.color + p.name);
            Player.GlobalMessage("&6Good Luck! &c:D");
            Player.GlobalMessage("");
            Player.GlobalMessage("&a--------------------------------------------");
						
		}
		
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/legal - Announce if last alive is legal!");
		}
	}
}