using System.Reflection.Emit;
using memoryGame;

namespace memory;

internal class Program
{
  static void Main()
  {
    var random = new Random();
    var groups = new List<string> { "misc64", "mario", "wario", "yoshi" };
    int score = -1;
    Level level;
    do
    {
      score++;
      var randomGroup = groups[random.Next(groups.Count)];
      if (score % 4 == 0 || score==1)
      {
        level = new(3, groups[0], 3);
      }
      else
      {
        level = new Level(4, randomGroup, 3);
      }
    } while (level.PlayLevel());
  }
}