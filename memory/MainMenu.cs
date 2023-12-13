using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FmodAudio;
using memoryGame;

namespace memory;

internal class MainMenu : IGlobabConsoleActions
{
  public override SoundSystem SoundSystem { get; set; }
  public int MaxScore { get; set; }
  public MainMenu(Parameters parameters)
  {
    SoundSystem = new SoundSystem(parameters.Volume);
    MaxScore = parameters.Score;
  }
  public Parameters Run()
  {
    ConsoleKeyInfo keyinfo;
    do
    {
      SoundSystem.LoadMenu();
      keyinfo = Console.ReadKey();
      switch (keyinfo.Key)
      {
        case ConsoleKey.Enter:
        case ConsoleKey.Spacebar:
          int score = PlayGame();
          if (this.MaxScore < score) MaxScore = score;
          break;
        case ConsoleKey.F1:
        case ConsoleKey.H:
          Console.WriteLine("Aide");
          break;
        default:
          GlobalActions(keyinfo.Key);
          break;
      }
    } while (keyinfo.Key != ConsoleKey.Escape);
    return new Parameters { Score = MaxScore, Volume = SoundSystem.Volume };
  }

  public int PlayGame()
  {
    SoundSystem.FreeRessources();
    var random = new Random();
    var groups = new List<string> { "misc64", "mario", "wario", "yoshi", "luigi" };
    int score = -1;
    Level level;
    do
    {
      score++;
      var randomGroup = groups[random.Next(groups.Count)];
      if (score % 4 == 0 || score == 1)
      {
        level = new(SoundSystem, 3, groups[0], 3);
      }
      else
      {
        level = new(SoundSystem, 4, randomGroup, 3);
      }
    } while (level.Play());
    return score;
  }
}
