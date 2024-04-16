/*
 * @author Bryce Thompson (Panda)
 * 10/27/2021
 * ReportPlugin - Send an embedded report message to a specified Discord channel.
 * This plugin works in correlation with MCGalaxy's built-in Discord-Relay.
*/
using System;
using System.IO;

using MCGalaxy;
using MCGalaxy.Commands;
using MCGalaxy.Modules.Relay.Discord;

namespace MCGalaxy {

    public class ReportPlugin : Plugin {
        public override string creator { get { return "Panda"; } }
        public override string MCGalaxy_Version { get { return "1.9.1.2"; } }
        public override string name { get { return "ReportPlugin"; } }

        public override void Load(bool startup) {
            // Unregister MCGalaxy's default report command.
            Command.Unregister(Command.Find("Report"));
            Command.Register(new CmdReport());
        }

        public override void Unload(bool shutdown) {
            Command.Unregister(Command.Find("Report"));
        }
    }

    public sealed class CmdReport : Command {
        public override string name { get { return "Report"; } }
        public override string type { get { return "moderation"; } }
        public override string shortcut { get { return "rep"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public const string playerReportID = "YOUR CHANNEL ID"; // REPLACE THE CONTENTS OF THIS STRING WITH YOUR DISCORD REPORT CHANNEL ID.
        public const string bugReportID = "YOUR CHANNEL ID"; // REPLACE THE CONTENTS OF THIS STRING WITH YOUR DISCORD BUG REPORT CHANNEL ID.
        public const string bugEmbedColor = "2303786";
        public const string reportEmbedColor = "15158332";

        public override void Use(Player p, string message) {
            DiscordBot discBot = DiscordPlugin.Bot;
            try
            {
                // Check for report type and protect against empty string.
                string reportType = message.Split(' ')[0];
                if (reportType.Equals(string.Empty))
                {
                    p.Message("Please either use the word \"bug\" or mention a player name when using /report.");
                    return;
                }
                else if (reportType.ToLower().Equals("bug"))
                {
                    EmbedReport(discBot, p, bugReportID, message, true);
                }
                else
                {
                    EmbedReport(discBot, p, playerReportID, message, false);
                }
            }
            catch (Exception e)
            {
                p.Message("Failed to submit report. Error message:");
                p.Message(e.StackTrace);
                return;
            }
            
            // Strip the first word and trailing space from the report message
            string reportMessage = message.Substring(message.IndexOf(" ") + 1);
            p.Message($"%aSubmitted Report: %S\"{reportMessage}\" %ato the staff team.");
            p.Message("%SPlease be patient. Staff are generally AFK, and will privately contact you ASAP!");
        }

        public void EmbedReport(DiscordBot disc, Player p, string channelID, string message, bool isBug)
        {
            ChannelSendEmbed embed = new ChannelSendEmbed(channelID);
            DiscordConfig config = DiscordPlugin.Config;
            if (isBug)
            {
                string bugMessage = message.Substring(message.IndexOf(" "));
                embed.Color = Int32.Parse(bugEmbedColor);
                embed.Title = string.Format($"Bug Report by {p.name} at {DateTime.UtcNow}");
                embed.Fields.Add("Bug", bugMessage);
            }
            else
            {
                // Grab the reported player name
                string target = message.Split(' ')[0];
                string reportMessage = message.Substring(message.IndexOf(" "));
                embed.Color = Int32.Parse(reportEmbedColor);
                embed.Title = string.Format($"Report by {p.name} against {target} at {DateTime.UtcNow}");
                embed.Fields.Add("Report", reportMessage);
            }
            disc.Send(embed);
        }

        public override void Help(Player p)
		{
            p.Message("&T/Report <player/bug> <report message here>.");
            p.Message("&HEmbeds a bug or player report message in Discord to notify staff.");
            p.Message("&HReplaces default MCGalaxy Report command that logs to text files.");
            p.Message("&HShortcut: /rep <bug/player> <report message>");
		}
    }
}
