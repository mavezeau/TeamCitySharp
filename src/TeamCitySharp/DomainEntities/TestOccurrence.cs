﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TeamCitySharp.DomainEntities
{
    public class TestMetadata
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class TestMetadataContainer
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("typedValues")]
        public List<TestMetadata> Metadata { get; set; }
    }

    public class TestOccurrence
    {
        public override string ToString()
        {
            return "testOccurrence";
        }


        [JsonIgnore]
        public string BuildName => Name?.Split(':')?.ElementAtOrDefault(0)?.Replace(".dll", "")?.Replace(".", "");

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("ignored")]
        public bool Ignored { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("runOrder")]
        public string RunOrder { get; set; }

        [JsonProperty("muted")]
        public bool Muted { get; set; }

        [JsonProperty("currentlyMuted")]
        public bool CurrentlyMuted { get; set; }

        [JsonProperty("currentlyInvestigated")]
        public bool CurrentlyInvestigated { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("ignoreDetails")]
        public string IgnoreDetails { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("test")]
        public Test Test { get; set; }

        [JsonProperty("mute")]
        public Mute Mute { get; set; }

        [JsonProperty("build")]
        public Build Build { get; set; }

        [JsonProperty("firstFailed")]
        public Build FirstFailed { get; set; }

        [JsonProperty("nextFixed")]
        public Build NextFixed { get; set; }

        [JsonProperty("logAnchor")]
        public string LogAnchor { get; set; }

        [JsonProperty("newFailure")]
        public bool? NewFailure { get; set; }

        [JsonProperty("metadata")]
        public TestMetadataContainer Metadata { get; set; }
    }
}
