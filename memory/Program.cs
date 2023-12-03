namespace memoryGame
{
  internal class Program
  {
    static void Main()
    {
      var random = new Random();
      var groups = new List<string> { "misc64", "mario", "wario", "yoshi" };
      var randomGroup = groups[random.Next(groups.Count)];
      Level level = new(3, randomGroup);
      while (level.PlayLevel())
      {
        level.Release();
        randomGroup = groups[random.Next(groups.Count)];
        level = new Level(4, randomGroup);
      }
    }
  }
}