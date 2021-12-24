using System;
using System.IO;
namespace MCDzienny
{
	public class CmdMyrecords : Command
	{
		public override string name { get { return "myrecords"; } }
		public override string shortcut { get { return "myr"; } }
		public override string type { get { return "information"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
		public override void Use(Player p, string message)
		{
            Player.SendMessage(p, "&f---&eRecords for:&b " + p.PublicName + "&f---");

            if (!File.Exists("records/" + p.name + ".txt"))
            {
                if (!Directory.Exists("records"))
                {
                    //Do nothing
                }

                Player.SendMessage(p, "You have no records so far!");

            }
            else
            {
                //Making all of the records look pretty when there are some
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader("records/" + p.name + ".txt");
                Player.SendMessage("&f---&cRecords for " + p.color + p.PublicName + "&f---");
                while ((line = file.ReadLine()) != null)
                {
                    Player.SendMessage(p, line); //Sending the records one line at a time
                }
                file.Close();
            }
		}

		public override void Help(Player p)
		{
			Player.SendMessage(p, "/myrecords - Shows you records against yourself.");
		}
	}
}