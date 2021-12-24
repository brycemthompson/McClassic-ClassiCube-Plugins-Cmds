/*
 * @author Bryce Thompson (Panda)
 * 10/27/2021
 * CmdKiss - A command that permits users to "kiss" a provided target user with random messages.
 */
using System;

namespace MCGalaxy.Commands.Fun
{
    public class CmdKiss : Command
    {
        public override string name { get { return "kiss"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "fun"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            if (p.muted)
            {
                p.Message("%SYou are muted.");
                return;
            }
            else if (message == "") { Help(p); return; }

            string target = message.Split(' ')[0];
            Player who = PlayerInfo.FindMatches(p, target);

            if (who == null)
            {
                p.Message("%SPlayer is not online.");
                return;
            }

            Random rnd = new Random();
            int msg = rnd.Next(1, 5);
            switch (msg)
            {
                case 1:
                    Chat.MessageFrom(p, $"{p.ColoredName}%S kissed {who.ColoredName}%S.");
                    break;
                case 2:
                    Chat.MessageFrom(p, $"{p.ColoredName}%S kissed {who.ColoredName}%S on the forehead.");
                    break;
                case 3:
                    Chat.MessageFrom(p, $"{p.ColoredName}%S tried to kiss {who.ColoredName}%S but poked their eye with their nose.");
                    break;
                case 4:
                    Chat.MessageFrom(p, $"{p.ColoredName}%S gave {who.ColoredName}%S a friendly kiss on the cheek.");
                    break;
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/Kiss");
            p.Message("&HGenerate a random kiss message directed toward a target user.");
            p.Message("&H/kiss <player> - Kiss the specified player.");
        }
    }
}

