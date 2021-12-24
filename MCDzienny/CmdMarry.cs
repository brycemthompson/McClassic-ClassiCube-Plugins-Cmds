using System;
using System.IO;

namespace MCDzienny
{
	public class CmdMarry : Command
	{
		public override string name { get { return "marry"; } }
		public override string shortcut { get { return "mar"; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
		public override void Use(Player p, string message)
		{
            string[] thisMessage = message.Split(' ');

            // if a secondary argument is issued
            if (message.Length > 0)
            {
                if (thisMessage[0] == "check" && thisMessage.Length == 2)
                {
                    Check(p, thisMessage);
                }
                else if (thisMessage[0] == "proposed" && thisMessage.Length == 1)
                {
                    if (!File.Exists("marriages/proposals/" + p.name + ".txt"))
                    {
                        Player.SendMessage(p, "You have no pending proposals.");
                        return;
                    }
                    else
                    {
                        string line;
                        System.IO.StreamReader file = new System.IO.StreamReader("marriages/proposals/" + p.name + ".txt");
                        while ((line = file.ReadLine()) != null)
                        {
                            Player.SendMessage(p, "You have a pending proposal from " + line + Server.DefaultColor + ".");
                            Player.SendMessage(p, "To accept the proposal, use: /marry proposed accept");
                            Player.SendMessage(p, "To reject the proposal, use: /marry proposed reject");
                        }
                        file.Close();
                    }
                }
                else if (thisMessage[0] == "proposed" && thisMessage.Length == 2)
                {
                    if (thisMessage[1] == "accept")
                    {
                        Accept(p, thisMessage);
                    }
                    else if (thisMessage[1] == "reject")
                    {
                        Reject(p, thisMessage);
                    }
                    else
                    {
                        Player.SendMessage(p, "Command Failed. Make sure your arguments are correct.");
                        Help(p);
                    }
                }
                else if (thisMessage[0] == "proposed" && thisMessage.Length == 3)
                { 
                    if (thisMessage[1] == "revoke")
                    {
                        Revoke(p, thisMessage);
                    }
                    else
                    {
                        Player.SendMessage(p, "Command failed. Make sure you double check your arguments.");
                        return;
                    }
                }
                // Checking if the requested player is already married
                else
                {
                    Player who = Player.Find(thisMessage[0]);

                    // You can't marry yourself
                    if(who == p)
                    {
                        Player.SendMessage(p, "You can't marry yourself!");
                        return;
                    }

                    if (who == null) // offline player
                    {
                        string offlinePlayer = thisMessage[0];
                        if (!File.Exists("marriages/" + offlinePlayer + ".txt"))
                        {
                            if (!File.Exists("marriages/proposals/" + offlinePlayer + ".txt"))  //Propose!
                            {
                                File.WriteAllText("marriages/proposals/" + offlinePlayer + ".txt", p.name);
                                Player.GlobalMessage(p.color + p.name + Server.DefaultColor + " just proposed to (offline) " + offlinePlayer + "!");
                                return;
                            }
                            else
                            {
                                Player.SendMessage(p, Server.DefaultColor + "(offline) " + offlinePlayer + " is already pending proposal, convince them otherwise!");
                                return;
                            }
                        }
                        else
                        {
                            Player.SendMessage(p, Server.DefaultColor + "(offline) " + offlinePlayer + " is already married! Sorry friend :(");
                            return;
                        }
                    }
                    else
                    {
                        if (!File.Exists("marriages/" + who.name + ".txt"))
                        {
                            if (!File.Exists("marriages/proposals/" + who.name + ".txt"))  //Propose!
                            {
                                File.WriteAllText("marriages/proposals/" + who.name + ".txt", p.name);
                                Player.GlobalMessage(p.color + p.name + Server.DefaultColor + " just proposed to " + who.color + who.name + Server.DefaultColor + "!");
                                return;
                            }
                            else
                            {
                                Player.SendMessage(p, who.color + who.name + Server.DefaultColor + " is already pending proposal, convince them otherwise!");
                                return;
                            }
                        }
                        else
                        {
                            Player.SendMessage(p, who.color + who.name + Server.DefaultColor + " is already married! Sorry friend :(");
                            return;
                        }
                    }
                }
            }
		}
        public void Check(Player p, string[] thisMessage) // check arg
        {
            Player who = Player.Find(thisMessage[1]);
            if (who == null)
            {
                string offlinePlayer = thisMessage[1];
                Player.SendMessage(p, "Player (" + offlinePlayer + ") is offline. Checking for a marriage anyway.");
                if (!File.Exists("marriages/" + offlinePlayer + ".txt"))
                {
                    Player.SendMessage(p, Server.DefaultColor + "(offline) " + offlinePlayer + " has no recorded marriages.");
                    return;
                }
                else
                {
                    // If the player is married, read the file
                    string line;
                    System.IO.StreamReader file = new System.IO.StreamReader("marriages/" + offlinePlayer + ".txt");
                    while ((line = file.ReadLine()) != null)
                    {
                        Player.SendMessage(p, Server.DefaultColor + "(offline) " + offlinePlayer + " is married to " + line + Server.DefaultColor + ".");
                    }
                    file.Close();

                }
            }
            if (!File.Exists("marriages/" + who.name + ".txt"))
            {
                Player.SendMessage(p, who.color + who.name + Server.DefaultColor + " is still awaiting their Zoulmate!");
                return;
            }
            else
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader("marriages/" + who.name + ".txt");
                while ((line = file.ReadLine()) != null)
                {
                    Player.SendMessage(p, who.color + who.name + Server.DefaultColor + " is married to " + line + Server.DefaultColor + ".");
                }
                file.Close();
            }
        }
        public void Accept(Player p, string[] thisMessage)     // Accept arg
        {
            if (!File.Exists("marriages/" + p.name + ".txt"))
            {
                string line;
                string spouse = "";
                System.IO.StreamReader file = new System.IO.StreamReader("marriages/proposals/" + p.name + ".txt");
                while ((line = file.ReadLine()) != null)
                {
                    spouse = line;
                }
                file.Close();
                File.WriteAllText("marriages/" + p.name + ".txt", spouse); // File for accepter
                File.WriteAllText("marriages/" + spouse + ".txt", p.name); // File for acceptee
                File.Delete("marriages/proposals/" + p.name + ".txt"); // Delete the proposal
                Player.GlobalMessage("ATTENTION: " + p.color + p.name + Server.DefaultColor + " is now %bMARRIED %eto " + spouse + "!");
            }
            else
            {
                Player.SendMessage(p, "How dare you cheat on your spouse like this!");
                Command.all.Find("kick").Use(null, p.name + " Got caught cheating on their spouse!");
                return;
            }
        }
        public void Reject(Player p, string[] thisMessage)     // Reject arg
        {
            File.Delete("marriages/proposals/" + p.name + ".txt");
            Player.SendMessage(p, "Successfully rejected the pending proposal.");
            return;
        }
        public void Revoke(Player p, string[] thisMessage)       // Revoke arg
        {
            if (!File.Exists("marriages/proposals/" + thisMessage[2] + ".txt"))
            {
                Player.SendMessage(p, "You have no pending proposal to this person. Try adding a + to the name.");
                return;
            }
            else
            {
                File.Delete("marriages/proposals/" + thisMessage[2] + ".txt");
                Player.SendMessage(p, "You have successfully revoked your proposal to " + thisMessage[2]);
                return;
            }
        }
        
        // Player help
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/marry {name} - Propose holy McMatromony to another player!");
            Player.SendMessage(p, "/marry check {name} - checks if a player is already married.");
            Player.SendMessage(p, "/marry proposed - checks if you have a pending proposal.");
            Player.SendMessage(p, "/marry proposed {accept/reject} - accepts or rejects your pending proposal.");
            Player.SendMessage(p, "/marry proposed revoke {name} - revokes your proposal to the defined player.");
            Player.SendMessage(p, "Shortcut: /mar");
		}
	}
}