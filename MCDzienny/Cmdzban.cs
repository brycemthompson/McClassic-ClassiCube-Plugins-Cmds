using System;
using System.IO;

namespace MCDzienny
{
	public class CmdZban : Command
	{
		public override string name { get { return "zban"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "mod"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
		public override void Use(Player p, string message)
		{
            string reason = message.Split(' ')[1];
            Player who = Player.Find(message.Split(' ')[0]);

            //Do this if offline
            if (who == null)
            {
                Player.SendMessage(p, "Player is offline. Banning and adding a record anyways.");

                string offlinePlayer = message.Split(' ')[0];

                if (reason == "@r1")
                {
                    reason = "Rule 1: Do not use bad language avoiding the chat filter!";
                }
                else if (reason == "@r2")
                {
                    reason = "Rule 2: Be nice to other players/No Advertising.";
                }
                else if (reason == "@r3")
                {
                    reason = "Rule 3: No cyber bullying/Spamming/Flooding";
                }
                else if (reason == "@r4")
                {
                    reason = "Rule 4: Respect the staff/Play fair/Do not evade punishments!";
                }
                else if (reason == "@r5")
                {
                    reason = "Rule 5: No hacked clients!";
                }
                else if (reason == "@r6")
                {
                    reason = "Rule 6: Do not block glitch and you must be reachable!";
                }
                else if (reason == "@r7")
                {
                    reason = "Rule 7: What the owner says, is law!";
                }
                else
                {
                    Player.SendMessage(p, "You need to use /warn <player> @r(rule number) to warn someone");
                    return;
                }

                Command.all.Find("ban").Use(p, offlinePlayer + " for: " + reason);

                if (!File.Exists("records/" + offlinePlayer + ".txt"))
                {
                    if (!Directory.Exists("records"))
                    {
                        Directory.CreateDirectory("records");
                    }
                    File.WriteAllText("records/" + offlinePlayer + ".txt", p.color + p.PublicName + " &0banned&e (offline)&f" + offlinePlayer + " &efor:&c " + reason + "&f---" + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText("records/" + offlinePlayer + ".txt", p.color + p.PublicName + " &0banned&e (offline)&f" + offlinePlayer + " &efor:&c " + reason + "&f---" + Environment.NewLine);
                }
            }

            //Do this if online
            else
            {
                if (who == p)
                {
                    Player.SendMessage(p, "You can't ban yourself!");
                    return;
                }
                else if (who.group.Permission > p.group.Permission)
                {
                    Player.SendMessage(p, "You can't ban someone of equal or greater rank!");
                    return;
                }

                Command.all.Find("kickban").Use(p, who.name + " for: " + reason);

                if (!File.Exists("records/" + who.name + ".txt/"))
                {
                    if (!Directory.Exists("records/"))
                    {
                        Directory.CreateDirectory("records/");
                    }
                    File.WriteAllText("records/" + who.name + ".txt", p.color + p.PublicName + " &0banned " + who.color + who.name + Server.DefaultColor + " for: " + reason + "&f---" + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText("records/" + who.name + ".txt", p.color + p.PublicName + " &0banned " + who.color + who.name + Server.DefaultColor + " for: " + reason + "&f---" + Environment.NewLine);

                }
            }
		}

		public override void Help(Player p)
		{
			Player.SendMessage(p, "/zban - Bans a player while adding a record.");
            Player.SendMessage(p, "/zban {name} {reason}");
		}
	}
}