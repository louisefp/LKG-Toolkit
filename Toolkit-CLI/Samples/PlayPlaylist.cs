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
    internal class PlayPlaylist
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

                if (!b.TrySubscribeToEvents())
                {
                    Console.WriteLine("Failed to subscribe to events");
                    return;
                }

                if (!b.TryUpdateConnectedDevices())
                {
                    Console.WriteLine("Failed to update devices");
                    return;
                }

                // Load json from disk
                Console.WriteLine(args.inputFile);
                string jsonString = File.ReadAllText(args.inputFile);
                string directoryPath = Path.GetDirectoryName(args.inputFile);
                List<PlaylistItem> playlistItems = JsonSerializer.Deserialize<List<PlaylistItem>>(jsonString);

                Playlist p = new Playlist("default", args.loopPlaylist);

                Console.WriteLine(jsonString);
                
                foreach (PlaylistItem playlistItem in playlistItems)
                {
                    string itemPath = directoryPath +  Path.DirectorySeparatorChar  + playlistItem.filename + ".json";
                    Console.WriteLine(itemPath);
                    string itemJsonString = File.ReadAllText(itemPath);
                    PlaylistItemDetail playlistItemDetail = JsonSerializer.Deserialize<PlaylistItemDetail>(itemJsonString);
                    if (playlistItemDetail.mediaType == "quilt") {

                        p.AddQuiltItem(directoryPath +  Path.DirectorySeparatorChar + playlistItem.filename, 
                        playlistItemDetail.quilt_settings.viewX, 
                        playlistItemDetail.quilt_settings.viewY, 
                        playlistItemDetail.quilt_settings.aspect, 
                        playlistItemDetail.quilt_settings.viewTotal);

                    }

                }

                if (!b.TryPlayPlaylist(p, args.head))
                {
                    Console.WriteLine("Failed to play playlist");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Failed to connect to bridge, ensure bridge is running");
                return;
            }

            Console.WriteLine("Listening for events, press any key to stop.");
            Console.ReadKey();
        }

    }
    public class PlaylistItem {
        public string filename {get; set;}
        public string last_updated {get; set;}
        public bool needs_sync {get; set;}

    }
    public class QuilSettings {
        public int viewX {get; set;}
        public int viewY {get; set;}
        public int viewTotal {get; set;}
        public bool invertViews {get; set;}
        public float aspect {get; set;}

    }
    public class PlaylistItemDetail {
        public bool movie {get; set;}
        public string mediaType {get; set;}
        public QuilSettings quilt_settings {get; set;}
        public float depthiness {get; set;}
        public bool depthInversion {get; set;}
        public bool  chromaDepth {get; set;}
        public string depthPosition {get; set;}
        public float focus {get; set;}
        public bool viewOrderReversed {get; set;}
        public float zoom {get; set;}
        public float position_x  {get; set;}
        public float position_y  {get; set;}
        public float duration {get; set;}
    }
}
