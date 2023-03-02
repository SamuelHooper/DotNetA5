using Newtonsoft.Json;

namespace DotNetA5.FileTypes
{
    public static class Json
    {
        public static List<Movie> Read(string file)
        {
            List<Movie> movies = new();
            if (File.Exists(file))
            {
                StreamReader sr = new(file);
                try
                {
                    string json = sr.ReadToEnd();
                    movies = JsonConvert.DeserializeObject<List<Movie>>(json);
                    movies ??= new();
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
            List<Movie> movies = Read(file);
            StreamWriter sw = new(file);
            movies.Add(movie);
            try
            {
                string json = JsonConvert.SerializeObject(movies);
                sw.WriteLine(json);
                Console.WriteLine(JsonConvert.SerializeObject(movie));
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
