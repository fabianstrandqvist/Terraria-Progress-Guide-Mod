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

        public List<int> Items { get; set; } = new();

        // The JSON also carries "uncleanedItems" (the raw wiki markup the generator parsed
        // these IDs out of). It's deliberately not mapped here — nothing reads it at runtime,
        // and skipping it keeps ~170 KB of markup out of memory. It stays in the file for
        // debugging provenance.
        public List<InfoBox> Children { get; set; } = new();
        public int start;
        public int end;
        
    }
}