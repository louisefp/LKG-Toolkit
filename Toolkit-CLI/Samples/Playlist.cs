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