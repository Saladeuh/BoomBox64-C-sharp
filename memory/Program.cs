using System.Reflection.Emit;

namespace memoryGame
{
  internal class Program
  {
    static void Main(string[] args)
    {
      var random = new Random();
      var groups = new List<string> { "yoshi", "test" } ;
      var randomGroup = groups[random.Next(groups.Count)];
      Level level = new Level(3,randomGroup);
      while (level.PlayLevel())
      {
        level = new Level(4);
      }
    }
  }
}