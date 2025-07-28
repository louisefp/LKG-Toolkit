using CommandLine;
using LookingGlass.Toolkit.Bridge;
using LookingGlass.Toolkit.CLI.Samples;

namespace LookingGlass.Toolkit.CLI
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(RunOptions);
        }

        private static void RunOptions(CommandLineOptions args)
        {
            switch (args.task)
            {
                case CLI_Task.listen:
                    ListenForEvents.Run(args);
                    break;
                case CLI_Task.hide:
                    HideWindow.Run(args);
                    break;
                case CLI_Task.list:
                    ListDevices.Run(args);
                    break;
                case CLI_Task.play:
                    PlayRGBDItem.Run(args);
                    break;
                case CLI_Task.quiltify_RGBD:
                    QuiltifyRGBDItem.Run(args);
                    break;
                case CLI_Task.playlist:
                    Console.Write("sdjfkljdlk");
                    PlayPlaylist.Run(args);
                    break;
                case CLI_Task.build_playlist:
                    BuildPlaylist.Run(args);
                    break;
                case CLI_Task.playlist_seek:
                     PlaylistSeek.Run(args);
                     break;
                case CLI_Task.playlist_pause:
                     PlaylistPause.Run(args);
                     break;                
                case CLI_Task.playlist_resume:
                     PlaylistResume.Run(args);
                     break;
                case CLI_Task.lkg_displays:
                    GetPossibleDevices.Run(args);
                    break;
                case CLI_Task.studio_playlist:
                    StudioPlaylist.Run(args);
                    break;
            }

        }
    }
}