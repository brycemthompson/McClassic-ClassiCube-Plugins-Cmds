using System;
using System.IO;
using System.Threading;
namespace MCDzienny
{
	public class CmdWarn : Command
	{
        public string playername;
        string warnedby;
        string reason;
		public override string name { get { return "warn"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "mod"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }     //  bob reason
        public override void Use(Player p, string message)
        {
            Player who = Player.Find(message.Split(' ')[0]);
            who.name = "bob"

            //Do this if offline
            if (who == null)
            {
                Player.SendMessage(p, "Player is offline. Adding a record anyways.");
                string offlinePlayer = message.Split(' ')[0];

                reason = message.Split(' ')[1];

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

                if (!File.Exists("records/" + offlinePlayer + ".txt"))
                {
                    if (!Directory.Exists("records"))
                    {
                        Directory.CreateDirectory("records");
                    }
                    File.WriteAllText("records/" + offlinePlayer + ".txt", p.color + p.PublicName + " &cwarned&e (offline)&f" + offlinePlayer + " &efor:&c " + reason + "&f---" + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText("records/" + offlinePlayer + ".txt", p.color + p.PublicName + " &cwarned&e (offline)&f" + offlinePlayer + " &efor:&c " + reason + "&f---" + Environment.NewLine);
                }

                Player.GlobalMessage(p.color + p.PublicName + Server.DefaultColor + " &cwarned &e(offline)" + offlinePlayer + " for:");
                Player.GlobalMessage("%c" + reason + ".");
            }

            //Do this if online

            else
            {
                if (who.group.Permission >= p.group.Permission)
                {
                    Player.SendMessage(p, "You cannot warn a player of equal or higher rank.");
                    return;
                }

                if (who == p)
                {
                    Player.SendMessage(p, "You can't warn yourself!");
                    return;
                }

                reason = message.Substring(message.IndexOf(' ') + 1).Trim();

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


                if (message.Split(' ').Length == 1)
                {
                    Player.SendMessage(p, "Please use @r(rule number) to warn someone");
                    return;
                }
                string textfile;

                warnedby = (p == null) ? "<CONSOLE>" : p.color + p.name;
                Player.GlobalMessage(warnedby + Server.DefaultColor + " warned " + who.color + who.name + " for:");
                Player.GlobalMessage("%c" + reason + ".");

                if (!File.Exists("records/" + who.name + ".txt/"))
                {
                    if (!Directory.Exists("records/"))
                    {
                        Directory.CreateDirectory("records/");
                    }
                    File.WriteAllText("records/" + who.name + ".txt", p.color + p.PublicName + " &cwarned " + who.color + who.name + Server.DefaultColor + " for: " + reason + "&f---" + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText("records/" + who.name + ".txt", p.color + warnedby + " &cwarned " + who.color + who.name + Server.DefaultColor + " for: " + reason + "&f---" + Environment.NewLine);

                }
            }
        }
                

		public override void Help(Player p)
		{
			Player.SendMessage(p, "/warn - Warn a player for breaking a server rule.");
            Player.SendMessage(p, "/warn {player} {reason}");
		}
	}
}