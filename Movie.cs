namespace DotNetA5
{
    public class Movie
    {
        public ulong Id { get; set; }
        public string Title { get; set; }
        public List<string> Genres { get; set; }

        public Movie()
        {
            Genres = new List<string>();
        }

        public string CsvWrite()
        {
            return $"{Id},{Title},{String.Join('|', Genres.ToArray())}";
        }

        public override string ToString()
        {
            string info = $"{Id}. \"{Title}\" | ";
            for (int i = 0; i < Genres.Count; i++)
            {
                // Add proper punctuation and grammer
                switch (Genres.Count)
                {
                    case 1: info += $"{Genres[0]}"; break;
                    case 2: info += $"{Genres[0]} and {Genres[1]}"; i++; break;
                    default:
                        if (i == Genres.Count - 1)
                        {
                            info += $"and {Genres[i]}";
                        }
                        else
                        {
                            info += $"{Genres[i]}, ";
                        }
                        break;
                }
            }
            return info;
        }
    }
}
