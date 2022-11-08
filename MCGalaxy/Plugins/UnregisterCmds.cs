/*
 * UnregisterCmds - Unregisters all the commands listed in the text/unregisteredCmds.txt file.
 * November 8th, 2022
 * @author Panda
*/

using System;
using System.IO;

namespace MCGalaxy
{
	public class UnregisterCmds: Plugin
	{
		public override string creator { get { return "Panda"; } }
		public override string MCGalaxy_Version { get { return "1.9.3.4"; } }
		public override string name { get { return "UnregisterCmds"; } }

        #region Private Members

        private string filePath = "text/unregisteredCmds.txt";
		private string[] commands;

        #endregion Private Members

        #region Load

        public override void Load(bool startup)
		{
			if (!File.Exists(filePath))
            {
				File.Create(filePath);
				Logger.Log(LogType.SystemActivity, $"CREATED FILE {filePath}.");
				Logger.Log(LogType.Debug, $"UnregisterCmds: To use this plugin, insert all commands to unregister in {filePath}.");
				return;
			}
			else
				commands = File.ReadAllLines(filePath);

			if (commands.Length > 0)
            {
				foreach (string command in commands)
                {
					try
                    {
						Command cmd = Command.Find(command);
						Command.Unregister(cmd);
					}
					catch (Exception e)
                    {
						Logger.LogError($"Command named \"{command}\" was not found and cannot be unregistered.", e);
					}
				}
            }
		}

        #endregion Load

        #region Unload

        public override void Unload(bool shutdown)
		{
			if (shutdown) return;
			foreach (string command in commands)
            {
				try
                {
					Command cmd = Command.Find(command);
					Command.Register(cmd);
                }
				catch (Exception e)
                {
					Logger.LogError($"Command named \"{command}\" was not found and cannot be unregistered.", e);
                }
            }
		}

        #endregion Unload
    }
}