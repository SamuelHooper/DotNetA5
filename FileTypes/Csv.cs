using Newtonsoft.Json;

namespace DotNetA5.FileTypes
{
    public static class Csv
    {
        public static List<Movie> Read(string file)
        {
            List<Movie> movies = new();
            if (File.Exists(file))
            {
                StreamReader sr = new(file);
                try
                {
                    sr.ReadLine(); // Skips header line
                    while (!sr.EndOfStream)
                    {
                        Movie currentMovie = new();
                        string line = sr.ReadLine();

                        // Check for quotes
                        int index = line.IndexOf('"');
                        if (index == -1)
                        {
                            // If no quote act normally
                            string[] movieLine = line.Split(',');
                            currentMovie.Id = ulong.Parse(movieLine[0]);
                            currentMovie.Title = movieLine[1];
                            currentMovie.Genres = new List<string>(movieLine[2].Split('|'));
                        }
                        else
                        {
                            // quote = comma in movie title
                            // extract the movieId
                            currentMovie.Id = ulong.Parse(line.Substring(0, index - 1));
                            // remove movieId and first quote from string
                            line = line.Substring(index + 1);
                            // find the next quote
                            index = line.IndexOf('"');
                            // extract the movieTitle
                            currentMovie.Title = line.Substring(0, index);
                            // remove title and last comma from the string
                            line = line.Substring(index + 2);
                            // replace the "|" with ", "
                            currentMovie.Genres = new List<string>(line.Split('|'));
                        }
                        // Add movie to list
                        movies.Add(currentMovie);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Unable to read {file}");
                }
                finally
                {
                    sr.Close();
                }
            }
            return movies;
        }

        public static void Write(Movie movie, string file)
        {
            string header = "";
            if (!File.Exists(file))
            {
                header = "movieId,title,genres\n";
            }

            StreamWriter sw = new(file, true);
            try
            {
                sw.WriteLine(header + movie.CsvWrite());
            }
            catch (IOException)
            {
                Console.WriteLine($"Unable to write to {file}");
            }
            finally
            {
                sw.Close();
            }
        }
    }
}
