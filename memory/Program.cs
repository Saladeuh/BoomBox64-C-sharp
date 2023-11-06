using System.Reflection.Emit;

namespace memoryGame
{
  internal class Program
  {
    static void Main(string[] args)
    {
        Level level = new Level();
      while (level.PlayLevel())
      {
        //level.SoundSystem.System.Release();
        level = new Level();
      }
    }
  }
}