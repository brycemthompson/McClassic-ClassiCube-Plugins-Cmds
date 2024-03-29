﻿/* 
 * @author Panda
 * Date: 12/22/2023
 * Description: VoteBan - Forked and rewritten from MCDzienny so that players
 * can start a vote to remove a player from the game for 1 hour.
 */

using System;
using MCGalaxy.Tasks;

namespace MCGalaxy
{

    /// <summary>
    /// CustomVoteBanObject - Used in the VoteCallback to send information
    /// </summary>
    public class CustomVoteBanObject
    {
        public Player targerPlayer;
        public string reason;
        public string duration;

        public CustomVoteBanObject(Player targerPlayer, string reason, string duration)
        {
            this.targerPlayer = targerPlayer;
            this.reason = reason;
            this.duration = duration;
        }
    }
    public class CmdVoteBan : Command
    {
        public override string name { get { return "VoteBan"; } }
        public override string shortcut { get { return "vb"; } }
        public override string type { get { return "moderation"; } }
        public override bool museumUsable {  get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }

        public override void Use(Player p, string message)
        {
            // Permission conditionals
            if (message == string.Empty) { Help(p); return; }
            if (p.muted) { p.Message("%cYou can't use this command while muted."); return; }

            string[] args = message.Split(' ');
            string target = args[0];
            string reason = string.Empty;
            Player targetPlayer = PlayerInfo.FindExact(target);

            if (args.Length > 1) {
                reason = message.Substring(args[0].Length);
            }

            if (Server.voting) {
                p.Message("Voting is already in progress. Please wait for the current poll to end before starting another one.");
                return;
            }
            
            if (targetPlayer == null) {
                p.Message("%cPlayer %S{0} %cwasn't found. Are you sure you spelled the name correctly?", target);
                return;
            }

            if (p.group.Permission <= targetPlayer.group.Permission) {
                p.Message("You can't voteban a player of equal with a higher rank than you.");
                return;
            }

            // Perform the votekick
            Logger.Log(LogType.UserActivity, "Voteban of " + targetPlayer.name + " was called by " + p.truename);
            Chat.MessageGlobal("%cVoteban was called by {0}", p.ColoredName);
            Chat.MessageGlobal("%SDo you want {0} to be tempbanned for 1 hour?", targetPlayer.ColoredName);
            
            // Provide the reason if present
            if (!string.IsNullOrEmpty(reason)) { Chat.MessageGlobal("%cGiven reason: %S{0}", reason); }

            Server.voting = true;
            Server.NoVotes = 0; Server.YesVotes = 0;
            Chat.MessageGlobal("&2 VOTE: &S{0} &S(type &2Yes &Sor &cNo &Sin chat)", message);
            CustomVoteBanObject cvbo = new CustomVoteBanObject(targetPlayer, reason, "1h");
            Server.MainScheduler.QueueOnce(VoteCallback, cvbo, TimeSpan.FromSeconds(15));
        }

        /// <summary>
        /// Help - Describes command usage to the inquiring player
        /// </summary>
        /// <param name="p"></param>
        public override void Help(Player p)
        {
            p.Message("&T/voteban <player> &H- calls a vote on tempbanning <player> for 1 hour.");
            p.Message("&T/voteban <player> [reason] &H- calls a vote on tempbanning <player> for [reason] for 1 hour.");
        }

        /// <summary>
        /// VoteCallback - Code block executed upon completion of the vote
        /// </summary>
        /// <param name="task"></param>
        void VoteCallback(SchedulerTask task)
        {
            Server.voting = false;
            Chat.MessageGlobal("The votes are in! &2Y: {0} &cN: {1}", Server.YesVotes, Server.NoVotes);
            Player[] players = PlayerInfo.Online.Items;
            foreach (Player pl in players) pl.voted = false;

            CustomVoteBanObject cvbo = (CustomVoteBanObject)task.State;

            // If the majority of users vote yes, kick the player
            if (Server.YesVotes > Server.NoVotes) {
                Command.Find("tempban").Use(Player.Console, string.Format(" {0} {1} {2}", cvbo.targerPlayer.truename, cvbo.reason, cvbo.duration));
            }
        }
    }
}
