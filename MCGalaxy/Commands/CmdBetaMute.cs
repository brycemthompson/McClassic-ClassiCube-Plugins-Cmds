/*
CmdBetaMute - Workaround command to mute-kick betacraft users to enforce a working mute.
@author Panda
*/
using MCGalaxy.Commands.Chatting;

namespace MCGalaxy
{
	public class CmdBetaMute : MessageCmd
	{
		public override string name { get { return "BetaMute"; } }
		public override string shortcut { get { return "bm"; } }
		public override string type { get { return "moderation"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
		public override void Use(Player p, string message)
		{
			string[] args = message.Split(' ');
			if (args.Length < 2) { Help(p); return; }

			string targetUsername = args[0];
			string muteDuration = args[1];
			string reason = string.Empty;

			if (args.Length > 2) {
				reason = args[2];
			}

			string target = PlayerInfo.FindMatchesPreferOnline(p, targetUsername);
			if (target.CaselessEq(string.Empty)) {
				p.Message("%cPlease be sure to provide a valid player.");
				Help(p);
				return;
            }

			Player who = PlayerInfo.FindExact(target);
			if (who == null) {
				p.Message(string.Format("Player {0} was not found. Are you sure you spelled the name correctly?"), target);
				Help(p);
				return;
            }

			Command mute = Command.Find("mute");
			Command kick = Command.Find("kick");
			if (p.CanUse(mute) && p.CanUse(kick)) {
				Command.Find("mute").Use(p, who.truename + " " + muteDuration + (reason.CaselessEq(string.Empty) ? "" : reason));
				Command.Find("kick").Use(p, who.truename + " " + (reason.CaselessEq(string.Empty) ? "" : reason));
			}
			else
            {
				p.Message("%cYou do not have the required permissions for this command.");
				p.Message("Permissions needed: %cmute, kick.");
				Help(p);
            }
        }

        public override void Help(Player p)
		{
			p.Message("/BetaMute - Workaround command to mute-kick betacraft users.");
			p.Message("Usage - /betamute <username> <duration> <reason>");
			p.Message("Shortcut - /bm <username> <duration> <reason>");
		}
	}
}
