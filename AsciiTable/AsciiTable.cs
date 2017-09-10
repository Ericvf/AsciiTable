using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AsciiTable
{
    public class AsciiTable<T> : IAsciiTable
    {
        private const int MAXWIDTH = 81;
        private const char FILLER = '-';
        private const char EDGE = '|';

        private readonly IEnumerable<PropertyInfo> _properties;
        private readonly IEnumerable<AsciiTableRowAttribute> _labels;

        private readonly List<IEnumerable<object>> _rows;
        private readonly int _endStopSize;
        private readonly int _columnCount;

        public AsciiTable()
        {
            var propertyAttributes = GetPropertyAttributes();
            var columnSum = propertyAttributes.Sum(pa => pa.Item2.Width);

            if (columnSum > MAXWIDTH)
                throw new ApplicationException($"The combined total of all columns in {typeof(T)} is {columnSum}, which is larger than the maximum width of {MAXWIDTH}");

            _properties = propertyAttributes.Select(pa => pa.Item1);
            _labels = propertyAttributes.Select(pa => pa.Item2);

            _columnCount = propertyAttributes.Count();
            _endStopSize = MAXWIDTH - columnSum - _columnCount - 1;
            _endStopSize = Math.Max(0, _endStopSize);

            _rows = new List<IEnumerable<object>>();
        }

        private IEnumerable<(PropertyInfo, AsciiTableRowAttribute)> GetPropertyAttributes()
        {
            return from propertyInfo in typeof(T).GetProperties()
                   let attributes = propertyInfo.GetCustomAttributes(true)
                   let attribute = attributes.Single(a => a is AsciiTableRowAttribute)
                   select (propertyInfo, attribute as AsciiTableRowAttribute);
        }

        public void Add(T row)
        {
            var rowValues = _properties.Select(propertyInfo => propertyInfo.GetValue(row));
            _rows.Add(rowValues);
        }

        public string GetOutput()
        {
            var stringBuilder = new StringBuilder();

            AppendHeader(stringBuilder);

            foreach (var row in _rows)
            {
                AppendLine(stringBuilder, row);
            }

            AppendDividor(stringBuilder);

            return stringBuilder.ToString();
        }

        private void AppendHeader(StringBuilder stringBuilder)
        {
            AppendDividor(stringBuilder);

            for (int i = 0; i < _columnCount; i++)
            {
                var label = _labels.ElementAt(i);

                // Cut off the pipe and two spaces. For the last column cut off trailing pipe too
                var cutOff = i == _columnCount - 1 ? 4 : 3;
                var itemValue = SetLength(label.Name, label.Width - cutOff);

                stringBuilder.Append($"{EDGE} {itemValue} ");
            }

            AppendEndStop(stringBuilder);

            AppendDividor(stringBuilder);
        }

        private void AppendLine(StringBuilder stringBuilder, IEnumerable<object> row)
        {
            for (int i = 0; i < _columnCount; i++)
            {
                var item = row.ElementAt(i);
                var label = _labels.ElementAt(i);

                // Cut off the pipe and two spaces. For the last column cut off trailing pipe too
                var cutOff = i == _columnCount - 1 ? 4 : 3;
                var itemValue = SetLength(Convert.ToString(item), label.Width - cutOff);

                stringBuilder.Append($"{EDGE} {itemValue} ");
            }

            AppendEndStop(stringBuilder);
        }

        private void AppendDividor(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine(new string(FILLER, MAXWIDTH));
        }

        private void AppendEndStop(StringBuilder stringBuilder)
        {
            stringBuilder.Append(new string(' ', _endStopSize));
            stringBuilder.Append(EDGE);
            stringBuilder.AppendLine();
        }

        private string SetLength(string value, int length)
        {
            return value.Length > length
                ? value.Substring(0, length)
                : value.PadRight(length);
        }
    }
}
