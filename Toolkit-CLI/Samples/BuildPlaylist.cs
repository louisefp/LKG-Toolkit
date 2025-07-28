using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LookingGlass.Toolkit.Bridge;
using System.Text.Json;

namespace LookingGlass.Toolkit.CLI.Samples
{
    internal class BuildPlaylist
    {
        public static void Run(CommandLineOptions args)
        {
            // Create BridgeConnectionHTTP instance.
            // Make sure to use the using pattern or properly dispose of the BridgeConnectionHTTP object
            using BridgeConnectionHTTP b = new BridgeConnectionHTTP(args.address);

            // Connect to bridge
            bool connectionStatus = b.Connect();
            if (connectionStatus)
            {
                Console.WriteLine("Connected to bridge");

                // Enter the named Orchestration
                // This is similar to a session but multiple
                // clients can connect to the same instance, receive
                // the same events and control the same state
                if (!b.TryEnterOrchestration(args.orchestrationName))
                {
                    Console.WriteLine("Failed to enter orchestration");
                    return;
                }

                // if (!b.TrySubscribeToEvents())
                // {
                //     Console.WriteLine("Failed to subscribe to events");
                //     return;
                // }

                // if (!b.TryUpdateConnectedDevices())
                // {
                //     Console.WriteLine("Failed to update devices");
                //     return;
                // }

                // Load json from disk
                //Console.WriteLine(args.inputFile);
                string jsonString = File.ReadAllText(args.inputFile);
                string directoryPath = Path.GetDirectoryName(args.inputFile);
                List<PlaylistItem> playlistItems = JsonSerializer.Deserialize<List<PlaylistItem>>(jsonString);
                Random rng = new Random();

                Playlist p = new Playlist("default_" + rng.Next(0, 10000), args.loopPlaylist);

                //Console.WriteLine(jsonString);
                
                foreach (PlaylistItem playlistItem in playlistItems)
                {
                    string itemPath = directoryPath +  Path.DirectorySeparatorChar  + playlistItem.filename + ".json";
                    //Console.WriteLine(itemPath);
                    string itemJsonString = File.ReadAllText(itemPath);
                    PlaylistItemDetail playlistItemDetail = JsonSerializer.Deserialize<PlaylistItemDetail>(itemJsonString);
                    if (playlistItemDetail.mediaType == "quilt") {

                        p.AddQuiltItem(directoryPath +  Path.DirectorySeparatorChar + playlistItem.filename, 
                        playlistItemDetail.quilt_settings.viewX, 
                        playlistItemDetail.quilt_settings.viewY, 
                        playlistItemDetail.quilt_settings.aspect, 
                        playlistItemDetail.quilt_settings.viewTotal);

                    }
                    else if (playlistItemDetail.mediaType == "rgbd") {
                        p.AddRGBDItem(directoryPath +  Path.DirectorySeparatorChar + playlistItem.filename, args.rows, args.cols, args.aspect,
                            playlistItemDetail.depthiness,
                            0.9f,   //depth_cutoff
                            playlistItemDetail.focus,
                            2,
                            5f,    //cam_dist
                            30, //fov
                            "",
                            playlistItemDetail.zoom );

                    }

                }
                
                if (!b.TryBuildPlaylist(p, args.head))
                {
                    Console.WriteLine("Failed to build playlist");
                    return;
                }
                
                // if (!b.TryTransportControlsSeekToIndex(args.seek))
                // {
                //     Console.WriteLine("Failed to seek to index");
                //     return;
                // }

                // if (!b.TryTransportControlsPause())
                // {
                //     Console.WriteLine("Failed to control pause");
                //     return;
                // }


            }
            else
            {
                Console.WriteLine("Failed to connect to bridge, ensure bridge is running");
                return;
            }

            Console.WriteLine("Create playlist done.");
        }

    }
}
