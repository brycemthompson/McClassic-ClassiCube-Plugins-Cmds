/*
Apply - Provide the user with a link to apply for a new server with MCCH.
@author Panda
*/
using System;
using System.IO;

using MCGalaxy;
using MCGalaxy.Commands;

namespace MCGalaxy {

    public class ApplyPlugin : Plugin {
        public override string creator { get { return "Panda"; } }
        public override string MCGalaxy_Version { get { return "1.9.1.2"; } }
        public override string name { get { return "Apply"; } }

        public override void Load(bool startup) {
            Command.Register(new CmdApply());
        }

        public override void Unload(bool shutdown) {
            Command.Unregister(Command.Find("Apply"));
        }
    }

    public sealed class CmdApply : Command {
        public override string name { get { return "Apply"; } }
		public override string shortcut { get { return "app"; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }

        public override void Use(Player p, string message) {
            p.Message("%SApply for a %anew%S server by clicking this link:");
			p.Message("%bhttps://forms.gle/fcJFemh9fGQBbkSt5");
        }

        public override void Help(Player p)
		{
			p.Message("/Apply - Get the link to apply for a new server with MCCH.");
			p.Message("Shortcut - /app");
		}
    }
}
