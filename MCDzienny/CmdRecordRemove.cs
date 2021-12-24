/*
 * @author Bryce Thompson (Panda)
 * 6/14/2020
 * Desc: Command to remove a specific line out of a Player record in the event of a mistake. This will
 * allow staff to edit records without needing access to private text files.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MCDzienny
{
	public class CmdRecordRemove : Command
{
        public override string name { get { return "recordremove"; } }
    	public override string shortcut { get { return "rere"; } }
    	public override string type { get { return "mod"; } }
    	public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
		public override void Use(Player p, string message)
		{
            Player who = Player.Find(message.Split(' ')[0]);
            string sLineNumber = message.Split(' ')[1];
            int lineNumber = Int32.Parse(sLineNumber);


            if (lineNumber == null)
            {
                Player.SendMessage(p, "You must enter a line number to remove.");
                return;
            }

            
            // If the selected player is OFFLINE
            if (who == null)
            {
                string offlinePlayer = message.Split(' ')[0];
                Player.SendMessage(p, who + " is currently offline. Checking for an existing record file...");
                if (!File.Exists("records/" + offlinePlayer + ".txt"))
                    Player.SendMessage(p, "No record exists for this player.");
                else
                {
                    List<String> linesList = File.ReadAllLines("records/" + offlinePlayer + ".txt").ToList();

                    // Checking for an out of bounds line number
                    int lineLimit = linesList.Count;
                    if (lineNumber < 1 || lineNumber > lineLimit)
                    {
                        Player.SendMessage(p, "Invalid line number. Please recount the number of lines in the record.");
                    }
                    else
                    {
                        lineNumber--;
                        //sLineNumber = lineNumber.ToString();
                        linesList.RemoveAt(lineNumber);
                        File.WriteAllLines("records/" + offlinePlayer + ".txt", linesList.ToArray());
                        //DeleteLinesFromFile(sLineNumber, offlinePlayer);
                        Player.SendMessage(p, "Record for " + offlinePlayer + " has been removed.");
                    }
                        
                }
            }
            else
            {
                string playername = who.name;
                if (!File.Exists("records/" + playername + ".txt"))
                    Player.SendMessage(p, "No record exists for this player.");
                else
                {
                    List<String> linesList = File.ReadAllLines("records/" + playername + ".txt").ToList();

                    // Checking for an out of bounds line number
                    int lineLimit = linesList.Count;
                    if (lineNumber < 1 || lineNumber > lineLimit)
                        Player.SendMessage(p, "Invalid line number. Please recount the number of lines in the record.");
                    else
                    {
                        lineNumber--;
                        //sLineNumber = lineNumber.ToString();
                        linesList.RemoveAt(lineNumber);
                        File.WriteAllLines("records/" + playername + ".txt", linesList.ToArray());
                        //DeleteLinesFromFile(sLineNumber, playername);
                        Player.SendMessage(p, "Record for " + playername + " has been removed.");
                    }
                        
                }
            }

        }
        /*public void DeleteLinesFromFile(string strLineToDelete, string playername)
        {
            string strFilePath = "records/" + playername + ".txt";
            string strSearchText = strLineToDelete;
            string strOldText;
            string n = "";
            StreamReader sr = File.OpenText(strFilePath);
            while ((strOldText = sr.ReadLine()) != null)
            {
                if (!strOldText.Contains(strSearchText))
                {
                    n += strOldText + Environment.NewLine;
                }
            }
            sr.Close();
            File.WriteAllText(strFilePath, n);
        }       */

        // This one controls what happens when you use /help [commandname].
        public override void Help(Player p)
		{
			Player.SendMessage(p, "/recordremove {name} {line #} - removes a specific line from a player's record.");
            Player.SendMessage(p, "{line #} begins at 1");
		}
	}
}