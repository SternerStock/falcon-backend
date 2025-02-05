﻿namespace Falcon.MtG.Models.Json
{
    using System;
    using System.Collections.Generic;

    public class JsonSet
    {
        public List<JsonCard> Cards { get; set; }

        public string Block { get; set; }

        public string Code { get; set; }
        public string KeyruneCode { get; set; }

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Type { get; set; }

        public bool IsPartialPreview { get; set; }
    }

    public class JsonSetComparer : IComparer<JsonSet>
    {
        public int Compare(JsonSet a, JsonSet b)
        {
            return a.ReleaseDate.CompareTo(b.ReleaseDate);
        }
    }
}