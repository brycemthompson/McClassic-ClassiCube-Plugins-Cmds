using System;
using System.IO;
namespace MCDzienny
{
	public class CmdWarnkick : Command
	{
		public override string name { get { return "warnkick"; } }
		public override string shortcut { get { return "wk"; } }
		public override string type { get { return "mod"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        string reason;
		public override void Use(Player p, string message)            
		{
            Player who = Player.Find(message.Split(' ')[0]);

            //Do this if offline
            if (who == null)
            {
                Player.SendMessage(p, "Player is not online. Adding a warnkick record anyways.");

                string offlinePlayer = message.Split(' ')[0];
                reason = message.Split(' ')[1];

                if (reason == "@r1")
                {
                    reason = "Rule 1: Do not use bad language avoiding the chat filter!";
                }
                else if (reason == "@r2")
                {
                    reason = "Rule 2: Be nice to other players/No advertising.";
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
                    Player.SendMessage(p, "You need to use @r(rule number) to warnkick someone.");
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

                Player.GlobalMessage(p.color + p.PublicName + " &cwarnkicked&e (offline)" + offlinePlayer + " for:&c " + reason);
            }

            //Do this if online
            else
            {
                if (who == p)
                {
                    Player.SendMessage(p, "You cannot warnkick yourself!");
                    return;
                }

                if (who.group.Permission >= p.group.Permission)
                {
                    Player.SendMessage(p, "You cannot warnkick someone of equal or higher rank.");
                    return;
                }

                reason = message.Substring(message.IndexOf(' ') + 1).Trim();

                if (reason == "@r1")
                {
                    reason = "Rule 1: Do not use bad language avoiding the chat filter!";
                }
                else if (reason == "@r2")
                {
                    reason = "Rule 2: Be nice to other players/No advertising.";
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
                    Player.SendMessage(p, "You need to use @r(rule number) to warnkick someone.");
                    return;
                }

                Command.all.Find("kick").Use(p, who.name + " " + reason);

                if (!File.Exists("records/" + who.name + ".txt"))
                {
                    if (!Directory.Exists("records"))
                    {
                        Directory.CreateDirectory("records");
                    }
                    File.WriteAllText("records/" + who.name + ".txt", p.color + p.name + " &cwarnkicked " + who.color + who.name + Server.DefaultColor + " for:&c " + reason + "&f---" + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText("records/" + who.name + ".txt", p.color + p.name + " &cwarnkicked " + who.color + who.name + Server.DefaultColor + " for:&c " + reason + "&f---" + Environment.NewLine);
                }
            }
		}
          
    

		public override void Help(Player p)
		{
			Player.SendMessage(p, "/warnkick - Kicks a player for breaking a rule.");
            Player.SendMessage(p, "Ex: /warnkick {player} {@r(rulenumber)}");
		}
	}
}