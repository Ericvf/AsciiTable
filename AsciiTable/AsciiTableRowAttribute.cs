using System;

namespace AsciiTable
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AsciiTableRowAttribute : Attribute
    {
        public int Width { get; set; }

        public string Name { get; set; }

        public AsciiTableRowAttribute(int width, string name)
        {
            Width = width;
            Name = name;
        }
    }
}
