﻿using System.Text;

namespace Wilgysef.FluentRegex
{
    public class RawPattern : Pattern
    {
        public string Regex { get; set; }

        internal override bool IsSinglePattern => true;

        public RawPattern(string regex)
        {
            Regex = regex;
        }

        public RawPattern WithRegex(string regex)
        {
            Regex = regex;
            return this;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append(Regex);
        }
    }
}
