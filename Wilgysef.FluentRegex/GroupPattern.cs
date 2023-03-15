using System.Text;

namespace Wilgysef.FluentRegex
{
    public class GroupPattern : AbstractGroupPattern
    {
        public string? Name
        {
            get => _name;
            set
            {
                // TODO: group name validation
                _name = value;
                if (_name != null)
                {
                    _capturing = true;
                }
            }
        }
        private string? _name;

        public string? SecondName
        {
            get => _secondName;
            set
            {
                // TODO: group name validation
                _secondName = value;
                if (_secondName != null)
                {
                    _capturing = true;
                }
            }
        }
        private string? _secondName;

        public bool IsCapturing
        {
            get => _capturing;
            set
            {
                _capturing = value;
                if (!_capturing)
                {
                    _name = null;
                    _secondName = null;
                }
            }
        }
        private bool _capturing;

        protected override bool HasContents => true;

        public GroupPattern(Pattern? pattern, string? name = null, bool capture = true) : base(pattern)
        {
            Name = name;
            IsCapturing = capture;
        }

        public GroupPattern(Pattern? pattern, string name1, string name2) : base(pattern)
        {
            Balancing(name1, name2);
        }

        public GroupPattern WithName(string? name)
        {
            Name = name;
            return this;
        }

        public GroupPattern WithSecondName(string? name)
        {
            SecondName = name;
            return this;
        }

        public GroupPattern Balancing(string name1, string name2)
        {
            Name = name1;
            SecondName = name2;
            return this;
        }

        public GroupPattern Capture(bool capture = true)
        {
            IsCapturing = capture;
            return this;
        }

        protected override void GroupContents(StringBuilder builder)
        {
            if (IsCapturing)
            {
                if (Name != null || SecondName != null)
                {
                    builder.Append("?<");

                    if (Name != null)
                    {
                        builder.Append(Name);
                    }

                    if (SecondName != null)
                    {
                        builder.Append('-');
                        builder.Append(SecondName);
                    }

                    builder.Append('>');
                }
            }
            else
            {
                builder.Append("?:");
            }

            _pattern?.ToString(builder);
        }

        internal static void NonCaptureGroup(StringBuilder builder, Pattern pattern)
        {
            builder.Append("(?:");
            pattern.ToString(builder);
            builder.Append(')');
        }
    }
}
