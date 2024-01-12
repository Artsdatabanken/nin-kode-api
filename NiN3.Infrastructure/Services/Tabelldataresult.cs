namespace NiN3KodeAPI.Services
{
    public class Tabelldataresult
    {
        public Tabelldataresult(string jsonresult, int numrows)
        {
            this.jsonresult = jsonresult;
            this.numrows = numrows;
        }

        public string jsonresult { get; set; }
        public int numrows { get; set; }

    }
}
