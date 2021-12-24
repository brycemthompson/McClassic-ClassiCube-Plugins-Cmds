/*
	Auto-generated command skeleton class.

	Use this as a basis for custom commands implemented via the MCDzienny scripting framework.
	File and class should be named a specific way.  For example, /update is named 'CmdUpdate.cs' for the file, and 'CmdUpdate' for the class.
*/

// Add any other using statements you need up here, of course.
// As a note, MCDzienny is designed for .NET 3.5.
using System;

namespace MCDzienny
{
	public class CmdSendrules : Command
	{
		// The command's name, in all lowercase.  What you'll be putting behind the slash when using it.
		public override string name { get { return "sendrules"; } }

		// Command's shortcut (please take care not to use an existing one, or you may have issues.
		public override string shortcut { get { return "sr"; } }

		// Determines which submenu the command displays in under /help.
		public override string type { get { return "mod"; } }

		// Determines whether or not this command can be used in a museum.  Block/map altering commands should be made false to avoid errors.
		public override bool museumUsable { get { return false; } }

		// Determines the command's default rank.  Valid values are:
		// LevelPermission.Nobody, LevelPermission.Banned, LevelPermission.Guest
		// LevelPermission.Builder, LevelPermission.AdvBuilder, LevelPermission.Operator, LevelPermission.Admin
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

		// This is where the magic happens, naturally.
		// p is the player object for the player executing the command.  message is everything after the command invocation itself.
        public override void Use(Player p, string message)
        {
            var split = message.Split(' ');
            Player who = Player.Find(split[0]);
            if (who == null)
            {
                Player.SendMessage(p, "Couldn't find player.");
                return;
            }


            if (p != null && who.group.Permission > p.group.Permission)
            {
                Player.SendMessage(p, "You cannot use this command someone ranked higher than you.");
                return;
            }
            if (p == who)
            {
                Player.SendMessage(p, "You cannot use this command on yourself.");
                return;
            }
            Command.all.Find("make").Use(p, who.name + " rules");
            Player.SendMessage(p, "You've sent the rules to " + who.color + who.name + "!");
        }



		// This one controls what happens when you use /help [commandname].
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/sendrules (player). - Sends rules to (player)");
		}
	}
}