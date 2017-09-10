namespace AsciiTable
{
    public class DataRow
    {
        [AsciiTableRow(10, "1234567890")]
        public int LineNo { get; set; }

        [AsciiTableRow(10, "1234567890")]
        public string Name { get; set; }

        [AsciiTableRow(20, "12345678901234567890")]
        public string Name1 { get; set; }

        [AsciiTableRow(10, "12345678901234567890")]
        public string Name2 { get; set; }

        [AsciiTableRow(10, "1234567890")]
        public string Name3 { get; set; }

        [AsciiTableRow(21, "this is the lastitem")]
        public string Name4 { get; set; }
    }
}
