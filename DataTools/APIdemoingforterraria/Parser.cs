using System.Net.Http;
using System.Text.Json;
using System.Web;
using System.IO;
using reader;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

namespace parser
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
    class Parser
    {
        public static void ParseOutputJson()
        {
            var stages = new List<string> {"Gearing Up", "Pre-Bosses", "Pre-Skeletron", "Pre-Wall of Flesh", "Pre-mechanical bosses",  "Pre-Plantera", "Pre-Golem", "Pre-Lunatic Cultist", "Pre-Moon Lord", "Endgame"};
            var classes = new List<string> {"Melee", "Ranged", "Magic", "Summoning", "Mixed"};
            

            string fileName = "output.json";
            string jsonString = File.ReadAllText(fileName);
            using JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
            JsonElement root = jsonDocument.RootElement; // I believe the root is the array [...json...]

            foreach (JsonElement page in root.EnumerateArray()) // not necessary because only one page is used
            {
                // string? title = page.GetProperty("title").GetString();
                // Console.WriteLine($"Title: {title}");

                // ultimately want to build a clean json from this:
                // {
                //   "progressionStages": [
                //     {
                //       "stage": "Gearing Up",
                //       "description": "Wiki text for the stage",
                //       "classes": [
                //         {
                //   "class": "Melee",
                //   "weapons": ["Copper Shortsword", "Zombie Arm"],
                //   "armor": ["Copper Helmet", "Copper Chainmail", "Copper Greaves"],
                //   "accessories": ["Cloud in a Bottle"],
                // },

                string? wikiText = page.GetProperty("wikitext").GetString();
                // Convert [[PageName|Display Text]] -> Display Text, and [[PageName]] -> PageName
                string cleanedWikitext = wikiText.Replace("[[", "").Replace("]]", "");

                var stageInfoList = new List<StageInfo>();

                foreach (string stage in stages)
                {
                    string escapedStage = Regex.Escape(stage);
                    string pattern = $@"==\s*{escapedStage}\s*==(.*?)--\s*{escapedStage}\s*--";

                    var match = Regex.Match(cleanedWikitext, pattern, RegexOptions.Singleline);
                    string description = match.Success ? match.Groups[1].Value.Trim() : "";

                    stageInfoList.Add(new StageInfo
                    {
                        Stage = stage,
                        Description = description
                    });
                    foreach (string cls in classes)
                    {
                        string escapedClass = Regex.Escape(cls);
                        string cls_pattern = $@"infocard\/start\|type={escapedStage}\|name={escapedClass}(.*?)infocard\/end";
                        // /infocard\/start\|type={escapedStage}\|name={escapedClass}(.*?)infocard\/start\|type={escapedStage}/gm
                        // string cls_pattern = $@"\{{\{{infocard/start\|type={escapedStage}\|name={escapedClass}\|theme=[^}}]+\}}\}}(.*?)\{{\{{infocard/end\}}\}}";

                        var cls_match = Regex.Match(cleanedWikitext, cls_pattern, RegexOptions.Singleline);
                        string cls_description = cls_match.Success ? cls_match.Groups[1].Value.Trim() : "";
                        Console.WriteLine($"Description: {cls_description}");

                        // /infocard\/box\/start\|title={Weapons}(.*?)infocard\/box\/end/gm


                        var classEntry = new ClassEntry
                        {
                            Class = cls,
                            Boxes = new List<InfoBox>(),
                        };

                        stageInfoList.Last().Classes.Add(classEntry);

                        // infocard/box/start|title=
                        // infocard/box/end
                        string markerPattern =
                            @"\{\{infocard/box/start\|title=(.*?)\}\}|\{\{infocard/box/end\}\}";

                        var markers = Regex.Matches(
                            cls_description,
                            markerPattern,
                            RegexOptions.Singleline
                        );

                        var indices = new Stack<(int index, string title)>();
                        var childrenByParentIndex = new Dictionary<int, List<InfoBox>>();

                        foreach (Match marker in markers)
                        {
                            bool isStart = marker.Value.StartsWith("{{infocard/box/start");


                            if (isStart)
                            {
                                string title = marker.Groups[1].Value.Trim(); // not cleaned yet
                                indices.Push((marker.Index, title)); // or push (title, index) maybe
                                
                            }
                            else
                            {
                                if (indices.Count == 0)
                                {
                                    continue; // ignore unmatched closing tag, don't crash
                                }
                                var (startIndex, title) = indices.Pop();

                                var parent = indices.Count > 0 ? indices.Peek().index : -1; // -1 means no parent

                                var childrenBoxes =  childrenByParentIndex.ContainsKey(startIndex) ? childrenByParentIndex[startIndex] : new List<InfoBox>();
                                var sortedChildren = childrenBoxes.OrderBy(c => c.start).ToList();

                                var pieces = new List<string>();
                                int cursor = startIndex;


                                // fixes conte overlap issue between parent and children
                                foreach (InfoBox child in sortedChildren)
                                {
                                    
                                    string gap = cls_description.Substring(cursor, child.start - cursor);
                                    pieces.Add(gap);

                                    cursor = child.end;
                                }

                                string finalGap = cls_description.Substring(cursor, marker.Index - cursor);
                                pieces.Add(finalGap);

                                var parentItems = string.Join("", pieces).Trim();

                                // item cleanup
                                var itemPattern = new Regex(@"\{\{item\|([^|}]+)");
                                var items = new List<string>();

                                foreach (Match itemMatch in itemPattern.Matches(parentItems))
                                {
                                    string itemName = itemMatch.Groups[1].Value.Trim();
                                    items.Add(itemName);
                                }
                                
                                var infobox = new InfoBox
                                {
                                    Title = title,
                                    Items = items,
                                    uncleanedItems = parentItems,
                                    Children = childrenBoxes,
                                    start = startIndex,
                                    end = marker.Index,
                                };

                                // Current problem: text overlap between child and parents
                                // naive solution would be to just remove the substring in the parent

                                if (parent != -1)
                                {
                                    
                                    if (!childrenByParentIndex.ContainsKey(parent))
                                    {
                                        childrenByParentIndex[parent] = new List<InfoBox>();
                                    }
                                    childrenByParentIndex[parent].Add(infobox);
                                }
                                else
                                {
                                    classEntry.Boxes.Add(infobox);
                                }
                            }

                        }

                        // /infocard\/start\|type={escapedStage}\|name={escapedClass}(.*?)infocard\/start\|type={escapedStage}\|name={escapedClass}/gm
                    }
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(stageInfoList, options);
                File.WriteAllText("progression.json", json);
            
            }
        }
            public static void Run()
            {
                ParseOutputJson();
            }
    }


}