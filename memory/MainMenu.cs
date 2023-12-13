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
    var groups = new List<string> { "misc64", "mario", "wario", "yoshi", "luigi", "red_coin" };
    int score = -1;
    Level level;
    do
    {
      score++;
      var group1 = groups[random.Next(groups.Count)];
     string? group2 = null;
      int nbSounds = 4;
      int maxRetry = 3;
      if (score % 2 == 0)
      {
        do
        {
          group2 = groups[random.Next(groups.Count)];
        } while (group1 == group2);
      }
      else if (score % 3 == 0 || score == 0)
      {
        nbSounds = 3;
        group1 = groups[0];
      }
      if (score < 4)
      {
        maxRetry = 3;
      }
      level = new(SoundSystem, nbSounds, maxRetry, group1, group2);
    } while (level.Play());
    return score;
  }
}
