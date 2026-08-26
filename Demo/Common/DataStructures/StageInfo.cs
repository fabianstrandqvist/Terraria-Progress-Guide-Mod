using System;
using System.Collections.Generic;

namespace Demo.Common.DataStructures
{
    public class StageInfo
    {
        public string Stage { get; set; }
        public string Description { get; set; }
        public List<ClassEntry> Classes { get; set; } = new List<ClassEntry>();
    }

    public class ClassEntry
    {
        public string Class { get; set; }

        public List<InfoBox> Boxes { get; set; } = new();
    }

    public class InfoBox
    {
        public string Title { get; set; }

        public List<string> Items { get; set; } = new();
        public string uncleanedItems { get; set; } = "";

        public List<InfoBox> Children { get; set; } = new();
        public int start;
        public int end;
        
    }
}