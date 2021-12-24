# Legacy MCDzienny Commands
Legacy commands from Minecraft Classic (MCDzienny server software).

Commands written with the "namespace MCDzienny" are legacy and will only work on the MCDzienny server software. 
To use these commands, generate a clean build of MCDzienny, copy these commands into the extra\commands\source folder. 

Once copied, start your server and run the command `/compile <command_name_here>`. 

If your command compiles successfully, run `/cmdload <command_name_here>`. Once executed, the command is now live on your server! 

If you'd like these commands to automatically load per server-boot, enter the command name into cmdautoload.txt in the text directory.

# MCGalaxy Commands / Plugins
Files prefixed with "Cmd" and that have the MCGalaxy namespace are up-to-date commands that function with the MCGalaxy server software.

To compile these commands, follow the same instructions as laid out for MCDzienny.

Files *without* the prefix "Cmd" and that have the MCGalaxy namespace are up-to-date plugins that function with the MCGalaxy server software.
To use these plugins, generate a clean build of MCGalaxy if you've not generated the files already and copy these plugins into the \plugins\ folder.

Once copied, start your server and run the command `/pcompile <plugin_name_here>`.

If your plugin compiles successfully, run `/pload <plugin_name_here>`. Once executed, the plugin is now live on your server!
