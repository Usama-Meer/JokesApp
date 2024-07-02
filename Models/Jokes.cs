namespace JokesApp.Models
{
    public class Jokes
    {

        public int Id { get; set; }

        public required string JokeQuestion { get; set; }

        public required string JokeAnswer { get; set; }

        public required string JokeCreator { get; set; }

        public Jokes()
        {
            
        }
    }
}
