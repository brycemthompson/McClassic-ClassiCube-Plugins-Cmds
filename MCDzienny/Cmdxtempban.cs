using System;
using System.IO;
using System.Threading;
namespace MCDzienny
{
	public class CmdXtempban : Command
	{
        
		public override string name { get { return "xtempban"; } }
		public override string shortcut { get { return "xtb"; } }
		public override string type { get { return "mod"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message)
        {
            Player who = Player.Find(message.Split(' ')[0]);

            string reason = message.Split(' ')[1];
            string stbTime = message.Split(' ')[2];
            int tbTime;

            //If player is offline do this
            if (who == null)
            {
                Player.SendMessage(p, "Player is offline. Will tempban anyways.");

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

                if (stbTime == null)
                {
                    stbTime = "60";
                }

                tbTime = Convert.ToInt16(stbTime);
                Command.all.Find("ban").Use(p, offlinePlayer + " " + reason + " for: " + tbTime + " minutes.");
                Player.GlobalMessage(p.color + p.PublicName + " &0tempbanned &e(offline)&f" + offlinePlayer + " &efor&c " + tbTime + " &eminutes&e for:&c" + reason);

                if (!File.Exists("records/" + offlinePlayer + ".txt"))
                {
                    if (!Directory.Exists("records"))
                    {
                        Directory.CreateDirectory("records");
                    }
                    File.WriteAllText("records/" + offlinePlayer + ".txt", p.color + p.PublicName + " &0tempbanned&e (offline)&f" + offlinePlayer + " &efor:&c " + reason + "&f---" + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText("records/" + offlinePlayer + ".txt", p.color + p.PublicName + " &0tempbanned&e (offline)&f" + offlinePlayer + " &efor:&c " + reason + "&f---" + Environment.NewLine);
                }
                Thread.Sleep(tbTime * 60000);
                Command.all.Find("unban").Use(null, offlinePlayer);

            }
            else
            {
                //Do this if online
                if (who == p)
                {
                    Player.SendMessage(p, "You can't tempban yourself!");
                    return;
                }
                else if (who.group.Permission > p.group.Permission)
                {
                    Player.SendMessage(p, "You can't tempban someone of equal or greater rank!");
                    return;
                }

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

                if (stbTime == null)
                {
                    stbTime = "60";
                }

                tbTime = Convert.ToInt16(stbTime);
                Command.all.Find("kickban").Use(p, who.name + " " + reason + " for: " + tbTime + " minutes.");
                Player.GlobalMessage(p.color + p.PublicName + " &0tempbanned " + who.color + who.PublicName + " &efor&c " + tbTime + " &eminutes&e for:&c" + reason);

                if (!File.Exists("records/" + who.name + ".txt"))
                {
                    if (!Directory.Exists("records"))
                    {
                        Directory.CreateDirectory("records");
                    }
                    File.WriteAllText("records/" + who.name + ".txt", p.color + p.PublicName + " &0tempbanned " + who.color + who.name + " &efor:&c " + reason + "&f---" + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText("records/" + who.name + ".txt", p.color + p.PublicName + " &0tempbanned " + who.color + who.name + " &efor:&c " + reason + "&f---" + Environment.NewLine);
                }
                Thread.Sleep(tbTime * 60000);
                Command.all.Find("unban").Use(null, who.name);
            }
        }


		
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/xtempban - Tempbans a player with a reason and time.");
            Player.SendMessage(p, "/xtempban {name} {reason} {time}");
		}
	}
}