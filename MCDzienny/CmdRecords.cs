using System;
using System.IO;
namespace MCDzienny
{
	public class CmdRecords : Command
	{
		public override string name { get { return "records"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
		public override void Use(Player p, string message)
		{            
            Player who = Player.Find(message);

            if (message == "") { Help(p); return; }

            if (who == null)
            {
                string offlinePlayer = message.Split(' ')[0];

                if (!File.Exists("records/" + offlinePlayer + ".txt"))
                {
                    Player.SendMessage(p, "No records are found for " + offlinePlayer + ".");
                    return;
                }
                else
                {
                    Player.SendMessage(p, "&b----------&eRecords of " + offlinePlayer + "&b----------");
                    string line;
                    System.IO.StreamReader file = new System.IO.StreamReader("records/" + offlinePlayer + ".txt");
                    while ((line = file.ReadLine()) != null)
                    {
                        Player.SendMessage(p, line); //Sending the records one line at a time
                    }
                    file.Close();
                }
            }
            else
            {

                if (!File.Exists("records/" + who.name + ".txt"))
                {
                    Player.SendMessage(p, "No records are found for " + who.color + who.name + ".");
                    return;
                }
                else
                {
                    
                    Player.SendMessage(p, "&b----------&eRecords of " + who.color + who.name + "&b----------");
                    string line;
                    System.IO.StreamReader file = new System.IO.StreamReader("records/" + who.name + ".txt");

                    while ((line = file.ReadLine()) != null)
                    {
                        Player.SendMessage(p, line); //Sending the records one line at a time
                    }
                    file.Close();
                }
            }
		}

		public override void Help(Player p)
		{
			Player.SendMessage(p, "/records - View the records of the specified player.");
		}
	}
}