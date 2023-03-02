using DotNetA5;
using DotNetA5.FileTypes;
using Trivial.CommandLine;

namespace ApplicationTemplate.Services
{
    public class MainService : IMainService
    {
        private static string _file = $@"{Environment.CurrentDirectory}\data\movies.json";
        // Main Program code goes here
        public void Invoke()
        {
            // Declarations
            string menuOption;

            do
            {
                // Gets user input for menu navigation
                Console.WriteLine("1) List all movies.");
                Console.WriteLine("2) Add movie to file.");
                Console.WriteLine("Enter any other key to exit.");
                menuOption = Console.ReadLine();

                if (menuOption == "1" && File.Exists(_file))
                {
                    DisplayMovies();
                }
                else if (menuOption == "2")
                {
                    AddNewMovie();
                }
                else if (!File.Exists(_file))
                {
                    Console.WriteLine($"{_file} does not exist.");
                }
            } while (menuOption == "1" || menuOption == "2");

            Console.WriteLine("\nShutting Down...");
        }

        private static List<string> AddGenres()
        {
            List<string> genres = new();
            // Loop to add genres
            while (true)
            {
                Console.Write("Enter a genre (or N to exit): ");
                string genre = Console.ReadLine();
                if (genre.ToUpper() != "N" && genre != null)
                {
                    genres.Add($"{genre}");
                }
                else
                {
                    if (genres.Count == 0)
                    {
                        genres.Add($"(no genres listed)");
                    }
                    return genres;
                }
            }
        }

        private static void AddNewMovie()
        {
            // Loop to add movies
            do
            {
                List<Movie> movies = ReadFile();
                List<string> lowerCaseMovieTitles = new();
                Movie newMovie = new();
                Console.Write("Enter the movie title: ");
                newMovie.Title = Console.ReadLine();

                // Check for duplicate movie title
                foreach (Movie movie in movies)
                {
                    lowerCaseMovieTitles.Add(movie.Title.ToLower());
                }

                if (lowerCaseMovieTitles.Contains(newMovie.Title.ToLower()))
                {
                    Console.WriteLine("That movie has already been entered.");
                }
                else
                {
                    if (movies.Count > 0)
                    {
                        newMovie.Id = movies[^1].Id + 1;
                    }
                    else
                    {
                        newMovie.Id = 1;
                    }

                    newMovie.Genres = AddGenres();

                    // Check for comma in title
                    newMovie.Title = newMovie.Title.IndexOf(',') != -1 ? $"\"{newMovie.Title}\"" : newMovie.Title;

                    // Create newMovie in file and write to console
                    WriteFile(newMovie);
                    Console.WriteLine(newMovie);
                }

                Console.Write("\nAdd another movie (Y/N)? ");
            } while (Console.ReadLine().ToUpper() == "Y");
        }

        private static void DisplayMovies()
        {
            // nuget Trival.Console (7.0.0) package for paging
            // It does have some bugs with pressing down the arrow keys too fast
            List<Movie> movies = ReadFile();
            if (movies.Count == 0)
            {
                Console.WriteLine("No movies in file");
            }
            else
            {
                var col = new Trivial.Collection.SelectionData<string>();
                for (int i = 0; i < movies.Count; i++)
                {
                    // Add movie to display
                    col.Add(movies[i].ToString());
                }

                // Create options for display
                var options = new SelectionConsoleOptions
                {
                    Tips = "Tips: You can use arrow key to select and press ENTER key to exit.",
                    TipsForegroundConsoleColor = ConsoleColor.Yellow,
                    SelectedPrefix = "> ",
                    Prefix = " ",
                    Column = 2,
                    MaxRow = 20
                };

                if (movies.Count <= 20)
                {
                    options = new SelectionConsoleOptions
                    {
                        Tips = "Tips: You can use arrow key to select and press ENTER key to exit.",
                        TipsForegroundConsoleColor = ConsoleColor.Yellow,
                        SelectedPrefix = "> ",
                        Prefix = " ",
                        Column = 1,
                        MaxRow = 20
                    };
                }


                // Pause to let user read and and see full description of selected item
                DefaultConsole.Select(col, options);
                Console.WriteLine(); // Spacer line for returning to main menu
            }
        }

        private static List<Movie> ReadFile()
        {
            if (_file.Contains(".json"))
            {
                return Json.Read(_file);
            }
            else
            {
                return Csv.Read(_file);
            }
        }

        private static void WriteFile(Movie movie)
        {

            if (_file.Contains(".json"))
            {
                Json.Write(movie, _file);
            }
            else
            {
                Csv.Write(movie, _file);
            }
        }
    }
}