namespace LohnverrechnerGastro.Models
{
    public class Errormessage
    { 
        public string Header { get; set; }
        public string Text { get; set; }
        public string Solution { get; set; }


        public Errormessage() : this("", "", "") { }

        public Errormessage(string header, string text) : this(header, text, "") { }

        public Errormessage(string header, string text, string solution)
        {
            this.Header = header;
            this.Text = text;
            this.Solution = solution;
        }
    }
}
