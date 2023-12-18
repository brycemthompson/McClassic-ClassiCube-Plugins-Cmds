/*
CmdBanAll - Bans all users for 1 day if we hit 420.69 randomly. Ported from MCGalaxy to MCDzienny
@author Panda
*/
using System;
using System.Threading;
using System.Collections.Generic;

namespace MCDzienny
{
	public class CmdBanAll : Command
	{
		public override string name { get { return "banall"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
		public override void Use(Player p, string message)
		{
			if (p.muted) return;
			if (p.group.Permission >= LevelPermission.Admin) {
				double value = RandomRealBanAllChance(p);
				Player.GlobalMessage(string.Format("%c[{0}] {4}{1}:{3} Real BanAll chance calculated: %c{2}. {3}Needed: %a420.69", p.group.Permission, p.name, value, Server.DefaultColor, p.color));
			}
			else {
				string suffix = RandomMessage();
				Player.GlobalMessage(p.color + p.name + Server.DefaultColor + " ISSUED /BANALL " + suffix);
			}
		}

		public override void Help(Player p)
		{
			Player.SendMessage(p, "/Banall - Ban everyone... Literally.");
		}

		public string RandomMessage()
		{
			Random random = new Random();
			int val = random.Next(12);

			switch(val)
			{
				case 0:
					return "%cbut banned themself.";
				case 1:
					return "%cbut spelled it wrong.";
				case 2:
					return "%cbut forgot they're not staff.";
				case 3:
					return "%cbut on the wrong server.";
				case 4:
					return "%cbut broke their nail typing it.";
				case 5:
					return "%cbut forgot Panda didn't import it yet.";
				case 6:
					return "%cthinking they could ACTUALLY ban everyone LMAO!";
				case 7:
					return "%cbut cancelled it JUST IN TIME!";
				case 8:			  
					return "%cbut finally realized it won't ban ANYONE.";
				case 9:
					return "%cand farted at the same time.";
				case 10:
					return "%cbut hacked Panda's code so only Panda gets banned.";
				case 11:
					return "%cbut LegoSpaceGuy hacked it into /lick.";
				default:
					return "%cbut shit hit the fan. %cThis is an error message, contact @Panda";
			}
		}

		/// <summary>
		/// Function to bring about the real possibility of "the yeetening"
		/// </summary>
		/// <param name="p"></param>
		public double RandomRealBanAllChance(Player p)
		{
			// Calculate a random value between 0 and 10,000 (0.01% chance of occurrance)
			Random random = new Random();
			double value = Math.Round((random.NextDouble() * 10001), 2);

			// If we hit this god-forsaken value... let Panda have his fun :D
			if (value == 420.69) {

				Player.GlobalMessage(string.Format("{1}{0} %cHAS ACTIVATED THE REAL BANALL (%A0.01% CHANCE FOR ADMINS+%c).", p.name, p.color));
				Player.GlobalMessage("%cALL ONLINE PLAYERS WILL BE TEMPBANNED IN 3 SECONDS.");
				Thread.Sleep(3000); // 3 second grace period to logout before banall

				// Ban all online users for 1 day
				List<Player> onlineUsers = new List<Player>(Player.players);
				foreach(Player user in onlineUsers)
				{
					Command.all.Find("tempban").Use(null, user.name + " 1d BANALL by " + p.name + ".");
					Command.all.Find("kick").Use(null, user.name + " Banned for 1d by " + p.name + " using BANALL!");
				}
			}
			return value;
		}
	}
}