//reference MCGalaxy_.dll
//reference System.dll
//reference System.Core.dll

/*
BlacklistPlugin - Staff command to blacklist specific users from issuing specified commands or building on specified maps.
Date: September 10th, 2024
@author Panda
*/

using MCGalaxy.Events.PlayerEvents;
using System.Collections.Generic;
using System.Linq;

namespace MCGalaxy
{
    #region BlacklistPlugin
    public class BlacklistPlugin : Plugin
    {
        public override string creator { get { return "Panda"; } }
        public override string MCGalaxy_Version { get { return "1.9.1.2"; } }
        public override string name { get { return "BlacklistPlugin"; } }

        public const string PATH = "extra/cmdblacklist.txt";

        // Change this value to true if you want the command to additionally pervisit blacklist in LvlBlacklist.
        public static bool DoPervisitBlacklist = true;

        public static PlayerExtList blacklistData;

        public override void Load(bool startup)
        {
            Command.Register(new CmdCmdBlacklist());
            Command.Register(new CmdLvlBlacklist());
            blacklistData = PlayerExtList.Load(PATH);
            OnPlayerCommandEvent.Register(CheckBlacklist, Priority.High);
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("CmdBlacklist"));
            Command.Unregister(Command.Find("LvlBlacklist"));
        }

        /// <summary>
        /// CheckBlacklist
        /// Description: Scheduled Task function to check a player's 
        /// blacklisted commands whenever a command is issued. If the player is
        /// command blacklisted, the command will cancel.
        /// </summary>
        /// <param name="p">Player issuing the command</param>
        /// <param name="command">Command being issued</param>
        /// <param name="args"></param>
        /// <param name="data"></param>
        public void CheckBlacklist(Player p, string command, string args, CommandData data) {

            // Grab the list of blacklisted commands for the player
            string targetUser = p.truename;
            if (Server.Config.ClassicubeAccountPlus) {
                if (!targetUser.CaselessContains("+")) {
                    targetUser += "+";
                }
            }

            string results = blacklistData.FindData(targetUser);
            List<string> commands = new List<string>();

            if (!string.IsNullOrEmpty(results)) {
                commands = results.Split(',').ToList();
            }

            // Cancel the command if found in the blacklist
            if (commands.CaselessContains(command)) {
                p.cancelcommand = true;
                p.Message(string.Format("%cYou're %0blacklisted %cfrom using %b{0}%c.", command));
            }
        }
    }

    #region CmdBlacklist
    public sealed class CmdCmdBlacklist : Command
    {
        public override string name { get { return "CmdBlacklist"; } }
        public override string type { get { return "mod"; } }
        public override string shortcut { get { return "bl"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message)
        {
            Dictionary<string, string> arguments = ParseMessage(p, message);

            if (arguments == null) { return; }

            // User is already command blacklisted, remove the blacklist
            if (IsCommandBlacklisted(arguments["TargetUser"], arguments["Command"])) {
                UpdateCommandBlacklist(p, arguments["TargetUser"], arguments["Command"], true);

                // Specify the reason if one is given
                if (arguments.ContainsKey("Reason")) {
                    Chat.MessageGlobal(string.Format("{0} %awhitelisted %f{1} to use %b{2} %ffor %a{3}%f.",
                        p.ColoredName, arguments["TargetUser"], arguments["Command"], arguments["Reason"]));

                    Find("send").Use(p, string.Format("{0} %awhitelisted %fto use %b{1} %ffor %a{2}%f.",
                        arguments["TargetUser"], arguments["Command"], arguments["Reason"]));
                    return;
                }
                Chat.MessageGlobal(string.Format("{0} %awhitelisted %f{1} to use %b{2}%f.",
                    p.ColoredName, arguments["TargetUser"], arguments["Command"]));

                Find("send").Use(p, string.Format("{0} %awhitelisted %fto use %b{1}%f.",
                    arguments["TargetUser"], arguments["Command"]));
            }
            // User is not yet command blacklisted, add the blacklist
            else {
                UpdateCommandBlacklist(p, arguments["TargetUser"], arguments["Command"]);

                // Specify the reason if one is given
                if (arguments.ContainsKey("Reason")) {
                    Find("warn").Use(p, string.Format("{0} %0blacklisted %ffrom using %b{1} %ffor %c{2}%f.",
                        arguments["TargetUser"], arguments["Command"], arguments["Reason"]));

                    Find("send").Use(p, string.Format("{0} %0blacklisted %ffrom using %b{1} %ffor %c{2}%f.",
                        arguments["TargetUser"], arguments["Command"], arguments["Reason"]));
                    return;
                }
                Find("warn").Use(p, string.Format("{0} %0blacklisted %ffrom using %b{1}%f.",
                        arguments["TargetUser"], arguments["Command"]));

                Find("send").Use(p, string.Format("{0} %0blacklisted %ffrom using %b{1}%f.",
                    arguments["TargetUser"], arguments["Command"]));
            }
        }

        /// <summary>
        /// IsCommandBlacklisted
        /// Description: Helper function to determine if the user has previously
        /// been blacklisted from the specified command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool IsCommandBlacklisted (string targetUser, string command) {
            // Grab commands that user is blacklisted from
            Command targetCmd = Find(command);

            if (Server.Config.ClassicubeAccountPlus) {
                if (!targetUser.CaselessContains("+")) {
                    targetUser += "+";
                }
            }
            string results = BlacklistPlugin.blacklistData.FindData(targetUser);
            List<string> commands = new List<string>();

            if (!string.IsNullOrEmpty(results)) {
                commands = results.Split(',').ToList();
            }

            return commands.CaselessContains(targetCmd.name);
        }

        /// <summary>
        /// ParseMessage
        /// Description: Helper function to break down the arguments provided to
        /// the command and returns a structured dictionary.
        /// </summary>
        /// <param name="p">Player issuing the command</param>
        /// <param name="message">The message to break down into arguments</param>
        /// <returns>Dictionary of key-value paired arguments</returns>
        public Dictionary<string, string> ParseMessage(Player p, string message) {

            Dictionary<string, string> arguments = new Dictionary<string, string>();
            string[] words = message.Split(' ');

            // Bad args, fail cmd
            if (words.Length < 2) { Help(p); return null; }

            // Check for valid username
            Player who = PlayerInfo.FindExact(words[0]);
            if (who == null) {
                string targetUser = PlayerInfo.FindMatchesPreferOnline(p, words[0]);
                if (string.IsNullOrEmpty(targetUser)) {
                    p.Message(string.Format("%cPlayer %f{0} %cnot found. Do you spell the username correctly?"), words[0]);
                    return null;
                }
                arguments.Add("TargetUser", targetUser);
            }
            else {
                arguments.Add("TargetUser", who.truename);
            }


            if (words.Length >= 2) {
                // Check for a valid command name in the second argument
                if (Find(words[1]) != null) {
                    arguments.Add("Command", words[1]);
                }
                else {
                    p.Message(string.Format("%cCommand %f{0} %cwas not found. Are you sure you spelled it correctly?", words[1]));
                    return null;
                }

                // Long reason, join the separated words into one entry
                if (words.Length >= 3) {
                    List<string> reasonBuilder = new List<string>();
                    for (int i = 2; i < words.Length; i++) {
                        reasonBuilder.Add(words[i]);
                    }
                    string reason = string.Join(" ", reasonBuilder.ToArray<string>());
                    arguments.Add("Reason", reason);
                    return arguments;
                }
            }
            return arguments;
        }

        /// <summary>
        /// UpdateCommandBlacklist
        /// Description: Helper function to update the PlayerExtList 
        /// containing users and their respective blacklisted cmds
        /// </summary>
        /// <param name="p">Player issuing the command</param>
        /// <param name="targetUser">The user to check</param>
        /// <param name="command">The command to check</param>
        /// <param name="toRemove">Whether or not to remove the command from the player's list</param>
        public void UpdateCommandBlacklist(Player p, string targetUser, string command, bool toRemove = false) {
            // Grab commands that user is blacklisted from
            Command targetCmd = Find(command);

            if (Server.Config.ClassicubeAccountPlus) {
                if (!targetUser.CaselessContains("+")) {
                    targetUser += "+";
                }
            }

            string results = BlacklistPlugin.blacklistData.FindData(targetUser);
            List<string> commands = new List<string>();

            if (!string.IsNullOrEmpty(results)) {
                commands = results.Split(',').ToList();
            }

            // Remove the command blacklist
            if (toRemove) {
                commands.Remove(targetCmd.name);
            }
            // Add the command blacklist
            else {
                commands.Add(targetCmd.name);
            }

            // Update the PlayerExtList
            BlacklistPlugin.blacklistData.Update(targetUser, commands.Join().ToString());
            BlacklistPlugin.blacklistData.Save();
        }

        public override void Help(Player p)
        {
            p.Message("%T/CmdBlacklist - Prevents a user from using a specific command.");
            p.Message("%HUsage:");
            p.Message("%H/CmdBlacklist <user> <command> - Removes the specified user's ability to issue the specified command.");
            p.Message("%H/CmdBlacklist <user> <command> <reason> - Removes the specified user's ability to issue the specified command with a reason.");
        }
    }
    #endregion CmdBlacklist

    #region LvlBlacklist
    public sealed class CmdLvlBlacklist : Command2 {
        public override string name { get { return "LvlBlacklist"; } }
        public override string type { get { return "mod"; } }
        public override string shortcut { get { return "lbl"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message, CommandData data) {

            Dictionary<string, string> arguments = ParseMessage(p, message);
            bool isBlacklisted;
            
            // Bad args, fail cmd
            if (arguments == null || arguments.Count < 1) { return; }
            
            Level targetLevel = LevelInfo.FindExact(arguments["Map"]);

            // Reason argument found, use it.
            if (arguments.ContainsKey("Reason")) {
                isBlacklisted = UpdateLvlPerbuild(p, targetLevel, arguments["TargetUser"]);

                if (BlacklistPlugin.DoPervisitBlacklist) {
                    UpdateLvlPervisit(p, targetLevel, arguments["TargetUser"]);
                }

                SendPlayerNotices(p, targetLevel, arguments["TargetUser"], isBlacklisted, arguments["Reason"]);
            }
            else {
                isBlacklisted = UpdateLvlPerbuild(p, targetLevel, arguments["TargetUser"]);

                if (BlacklistPlugin.DoPervisitBlacklist) {
                    UpdateLvlPervisit(p, targetLevel, arguments["TargetUser"]);
                }

                SendPlayerNotices(p, targetLevel, arguments["TargetUser"], isBlacklisted);
            }
        }

        /// <summary>
        /// ParseMessage
        /// Description: Helper function to break down the arguments provided to
        /// the command and returns a structured dictionary.
        /// </summary>
        /// <param name="p">Player issuing the command</param>
        /// <param name="message">The message to break down into arguments</param>
        /// <returns>Dictionary of key-value paired arguments</returns>
        public Dictionary<string, string> ParseMessage(Player p, string message) {

            Dictionary<string, string> arguments = new Dictionary<string, string>();
            string[] words = message.Split(' ');

            // Bad args, fail cmd
            if (words.Length < 2) { Help(p); return null; }

            // Check for valid username
            Player who = PlayerInfo.FindExact(words[0]);
            if (who == null) {
                string targetUser = PlayerInfo.FindMatchesPreferOnline(p, words[0]);
                if (string.IsNullOrEmpty(targetUser)) {
                    p.Message(string.Format("%cPlayer %f{0} %cnot found. Do you spell the username correctly?"), words[0]);
                    return null;
                }
                // Check if CC account plus is active. If it is, make sure the account has a + on the end of it
                if (Server.Config.ClassicubeAccountPlus) {
                    if (!targetUser.CaselessContains("+")) {
                        targetUser += "+";
                    }
                }
                arguments.Add("TargetUser", targetUser);
            }
            else {
                // Check if CC account plus is active. If it is, make sure the account has a + on the end of it
                string targetUser = who.truename;
                if (Server.Config.ClassicubeAccountPlus) {
                    if (!targetUser.CaselessContains("+")) {
                         targetUser += "+";
                    }
                }
                arguments.Add("TargetUser", targetUser);
            }

            // Check for valid map name
            if (LevelInfo.MapExists(words[1].ToLower())) {
                arguments.Add("Map", words[1].ToLower());
            }
            else {
                p.Message(string.Format("%cMap %f{0} %cnot found. Did you spell it correctly?", words[1]));
                Help(p);
                return null;
            }

            // Reason found, join the separated words into one entry
            if (words.Length > 2) {
                List<string> reasonBuilder = new List<string>();
                for (int i = 2; i < words.Length; i++) {
                    reasonBuilder.Add(words[i]);
                }
                string reason = string.Join(" ", reasonBuilder.ToArray<string>());
                arguments.Add("Reason", reason);
                return arguments;
            }
            return arguments;
        }

        /// <summary>
        /// SendPlayerNotices
        /// Description: Helper function to send the correct player notices 
        /// upon a moderation action.
        /// </summary>
        /// <param name="p">User issuing the command</param>
        /// <param name="targetLevel">The level to blacklist on</param>
        /// <param name="targetUser">The user to blacklist</param>
        /// <param name="reason">The reason for the blacklist</param>
        /// <param name="toBlacklist">Whether to blacklist or whitelist the user</param>
        public void SendPlayerNotices(Player p, Level targetLevel, string targetUser, bool toBlacklist, string reason = "") {
            if (toBlacklist) {
                if (!string.IsNullOrEmpty(reason)) {
                    Find("warn").Use(p, string.Format("{0} %0blacklisted %ffrom {1} %ffor %c{2}%f.",
                        targetUser, targetLevel.ColoredName, reason));

                    Find("send").Use(p, string.Format("{0} %0blacklisted %ffrom {1} %ffor %c{2}%f.",
                        targetUser, targetLevel.ColoredName, reason));
                }
                else {
                    Find("warn").Use(p, string.Format("{0} %0blacklisted %ffrom {1}%f.",
                        targetUser, targetLevel.ColoredName));

                    Find("send").Use(p, string.Format("{0} %0blacklisted %ffrom {1}%f.",
                        targetUser, targetLevel.ColoredName));
                }
            }
            else {
                if (!string.IsNullOrEmpty(reason)) {
                    Chat.MessageGlobal(string.Format("{0} %abuild whitelisted {1} %fon %b{2} %ffor %a{3}%f.",
                        p.ColoredName, targetUser, targetLevel.ColoredName, reason));

                    Find("send").Use(p, string.Format("{0} %abuild whitelisted %fon {1} %ffor %a{2}%f.",
                        targetUser, targetLevel.ColoredName, reason));
                }
                else {
                    Chat.MessageGlobal(string.Format("{0} %abuild whitelisted %f{1} on {2}%f.",
                        p.ColoredName, targetUser, targetLevel.ColoredName));

                    Find("send").Use(p, string.Format("{0} %abuild whitelisted %fon {1}%f.",
                        targetUser, targetLevel.ColoredName));
                }
            }
        }

        /// <summary>
        /// UpdateLvlPerbuild
        /// Description: Helper function that checks the target user's map
        /// perbuild status and updates the status accordingly.
        /// </summary>
        /// <param name="p">User issuing the command</param>
        /// <param name="targetLevel">The level to check and update</param>
        /// <param name="targetUser">The user to check and update</param>
        /// /// <returns>True if blacklisted, False if whitelisted</returns>
        public bool UpdateLvlPerbuild(Player p, Level targetLevel, string targetUser) {
            if (!targetLevel.Config.BuildBlacklist.CaselessContains(targetUser)) {
                Find("perbuild").Use(p, string.Format("-{0}", targetUser));
                return true;
            }
            else {
                Find("perbuild").Use(p, string.Format("+{0}", targetUser));
                return false;
            }
        }

        /// <summary>
        /// UpdateLvlPervisit
        /// Description: Helper function that checks the target user's map 
        /// pervisit status and updates the status accordingly.
        /// </summary>
        /// <param name="p">User issuing the command</param>
        /// <param name="targetLevel">The level to check and update</param>
        /// <param name="targetUser">The user to check and update</param>
        public void UpdateLvlPervisit(Player p, Level targetLevel, string targetUser) {
            if (!targetLevel.Config.VisitBlacklist.CaselessContains(targetUser)) {
                Find("pervisit").Use(p, string.Format("-{0}", targetUser));
            }
            else {
                Find("pervisit").Use(p, string.Format("+{0}", targetUser));
            }
        }

        public override void Help(Player p) {
            p.Message("%T/LvlBlacklist - Prevents a user from using a specific command.");
            p.Message("%HUsage:");
            p.Message("%H/LvlBlacklist <user> <map> - Removes the specified user's perbuild permissions for the specified map.");
            p.Message("%H/LvlBlacklist <user> <map> <reason> - Removes the specified user's perbuild permissions for the specified map for a specific reason.");
        }
    }
    #endregion LvlBlacklist

#endregion BlacklistPlugin
}
