namespace memoryGame
{
  internal class Program
  {
    static void Main(string[] args)
    {
      var random = new Random();
      var groups = new List<string> { "misc64", "mario", "wario", "yoshi" };
      var randomGroup = groups[random.Next(groups.Count)];
      Level level = new Level(3, randomGroup);
      while (level.PlayLevel())
      {
        Task.WaitAll(level.SoundSystem.tasks.ToArray());
        level.SoundSystem.Musics[0].Stop();
        level.SoundSystem.System.Release();
        randomGroup = groups[random.Next(groups.Count)];
        level = new Level(4, randomGroup);
      }
    }
  }
}