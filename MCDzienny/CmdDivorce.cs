using System;
using System.IO;

namespace MCDzienny
{
	public class CmdDivorce : Command
	{
		public override string name { get { return "divorce"; } }
		public override string shortcut { get { return "divo"; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
		public override void Use(Player p, string message)
		{
            string[] thisMessage = message.Split(' ');
            string arg0 = thisMessage[0];
            // Propose the divorce to your spouse (it takes two to tango)
            if (arg0 == "propose" && thisMessage.Length == 2)
                Propose(p, thisMessage);
            // Check if you have any pending divorce papers
            else if (arg0 == "check" && thisMessage.Length == 1)
                Check(p, thisMessage);
            // Sign the divorce papers
            else if (arg0 == "accept" && thisMessage.Length == 1)
                Accept(p, thisMessage);
            // Burn the divorce papers
            else if (arg0 == "reject" && thisMessage.Length == 1)
                Reject(p, thisMessage);
            // Improper command
            else
            {
                Player.SendMessage(p, "Command failed. Make sure your arguments are correct.");
                Help(p);
            }
		}

        public void Propose(Player p, string[] thisMessage)
        {
            string arg1 = thisMessage[1];
            Player who = Player.Find(arg1);

            // Check for offline spouse
            if(who == null)
            {
                 if(!File.Exists("marriages/" + p.name + ".txt"))
                 {
                    Player.SendMessage(p, "You can't divorce someone if you're not married!");
                    return;
                 }
                 else if(File.Exists("marriages/divorcepapers/" + arg1 + ".txt"))
                 {
                    Player.SendMessage(p, "This player already has pending divorce papers.");
                    return;
                 }
                else
                {
                    // Check for p.name in spouse's marriage file
                    string name = "";
                    string line;
                    System.IO.StreamReader file = new System.IO.StreamReader("marriages/" + arg1 + ".txt");
                    while ((line = file.ReadLine()) != null)
                    {
                        name = line;
                    }
                    file.Close();

                    // Submit the divorce papers
                    if(name == p.name)
                    {
                        File.WriteAllText("marriages/divorcepapers/" + arg1 + ".txt", p.name);
                        Player.SendMessage(p, "Successfully submitted divorce papers to (offline) " + arg1);
                    }
                    else
                    {
                        Player.SendMessage(p, "You can't divorce someone that you're not married to!");
                        return;
                    }

                }
            }
            else
            {
                if (!File.Exists("marriages/" + p.name + ".txt"))
                {
                    Player.SendMessage(p, "You can't divorce someone if you're not married!");
                    return;
                }
                else if (File.Exists("marriages/divorcepapers/" + who.name + ".txt"))
                {
                    Player.SendMessage(p, "This player already has pending divorce papers.");
                    return;
                }
                else
                {
                    // Check for p.name in spouse's marriage file
                    string name = "";
                    string line;
                    System.IO.StreamReader file = new System.IO.StreamReader("marriages/" + who.name + ".txt");
                    while ((line = file.ReadLine()) != null)
                    {
                        name = line;
                    }
                    file.Close();

                    // Submit the divorce papers
                    if (name == p.name)
                    {
                        File.WriteAllText("marriages/divorcepapers/" + who.name + ".txt", p.name);
                        Player.SendMessage(p, "Successfully submitted divorce papers to " + who.color + who.name);
                        Player.SendMessage(who, p.color + p.name + Server.DefaultColor + " has proposed %cdivorce " + Server.DefaultColor + "papers to you!");
                        Player.SendMessage(who, "Use /divorce check");
                    }
                    else
                    {
                        Player.SendMessage(p, "You can't divorce someone that you're not married to!");
                        return;
                    }

                }
            }
        }

        // Tells a player if they have pending divorce papers
        public void Check(Player p, string[] thisMessage)
        {
            bool response = CheckForAction(p, thisMessage);
            
            if(response == true)
            {
                Player.SendMessage(p, "You have a pending divorce paper from your spouse.");
                Player.SendMessage(p, "To accept, type /divorce accept");
                Player.SendMessage(p, "To reject, type /divorce reject");
            }
            else
                Player.SendMessage(p, "You have no pending divorce papers.");
        }

        // Checks for pending divorce papers
        public bool CheckForAction(Player p, string[] thisMessage)
        {
            if (!File.Exists("marriages/divorcepapers/" + p.name + ".txt"))
                return false;
            return true;
        }

        // Sign your divorce papers
        public void Accept(Player p, string[] thisMessage)
        {
            bool canAccept = CheckForAction(p, thisMessage);

            if(canAccept)
            {
                string name = "";
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader("marriages/" + p.name + ".txt");
                while ((line = file.ReadLine()) != null)
                {
                    name = line;
                }
                file.Close();
                File.Delete("marriages/divorcepapers/" + p.name + ".txt");
                File.Delete("marriages/" + p.name + ".txt");
                File.Delete("marriages/" + name + ".txt");

                Player.SendMessage(p, "You have successfully %asigned" + Server.DefaultColor + " your divorce papers. Welcome back to the single life!");
                Player.GlobalMessage(p.color + p.name + Server.DefaultColor + " is now %cDIVORCED " + Server.DefaultColor + " from " + name + "!");
                return;
            }
            else
            {
                Check(p, thisMessage);
                return;
            }
        }
        
        // Burn your divorce papers
        public void Reject(Player p, string[] thisMessage)
        {
            bool canReject = CheckForAction(p, thisMessage);

            if (canReject)
            {
                File.Delete("marriages/divorcepapers/" + p.name + ".txt");
                Player.SendMessage(p, "You have successfully %cburned " + Server.DefaultColor + "your divorce papers. 'TIL DEATH DO US PART!");
                return;
            }
            else
            {
                Check(p, thisMessage);
                return;
            }
        }

        // Halp plz
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/divorce - A mortal sin, but your privilege to leave your spouse.");
            Player.SendMessage(p, "/divorce check - checks for any pending divorce papers.");
            Player.SendMessage(p, "/divorce propose {name} - proposes divorce papers to your spouse.");
            Player.SendMessage(p, "/divorce {accept/reject} - accept or reject your pending divorce.");
            Player.SendMessage(p, "Shortcut: /divo");
		}
	}
}