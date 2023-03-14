﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class CommentPattern : Pattern
    {
        internal override bool IsSinglePattern => true;

        private readonly Pattern _pattern;

        public CommentPattern(Pattern pattern)
        {
            _pattern = pattern;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("(?#");
            _pattern.ToString(builder);
            builder.Append(')');
        }
    }
}
