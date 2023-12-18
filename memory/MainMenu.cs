using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FmodAudio;
using memoryGame;
using Microsoft.Extensions.Localization;

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
      Console.WriteLine(Localizer.GetString("gameName"));
      SoundSystem.LoadMenu();
      keyinfo = Console.ReadKey();
      switch (keyinfo.Key)
      {
        case ConsoleKey.Enter:
        case ConsoleKey.Spacebar:
          int score = PlayGame();
          Console.Clear();
          Console.WriteLine($"{Localizer.GetString("score")}: {score}");
          if (this.MaxScore < score) MaxScore = score;
          break;
        case ConsoleKey.F1:
        case ConsoleKey.H:
          Console.WriteLine("Aide");
          break;
        case ConsoleKey.L:
        case ConsoleKey.F5:
          ChangeLanguageMenu();
          break;
        default:
          GlobalActions(keyinfo.Key);
          break;
      }
    } while (keyinfo.Key != ConsoleKey.Escape);
    return new Parameters { Score = MaxScore, Volume = SoundSystem.Volume, Language=CultureInfo.CurrentUICulture.TwoLetterISOLanguageName };
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
      int maxRetry = 4;
      if (score % 3 == 0 && score != 0)
      {
        do
        {
          group2 = groups[random.Next(groups.Count)];
        } while (group1 == group2);
      }
      else if (score % 4 == 0 || score == 0)
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
  private bool ChangeLanguageMenu()  // bool to indicate if a n^ew language is choosed
  {
    Console.WriteLine(this.Localizer.GetString("changeLang"));
    for (int i = 0; i < SUPPORTEDLANGUAGES.Length; i++)
    {
      Console.WriteLine($"{i}: {SUPPORTEDLANGUAGES[i]}");
    }
    ConsoleKeyInfo keyinfo;
    do
    {
      keyinfo = Console.ReadKey();
      if (char.IsDigit(keyinfo.KeyChar) && int.Parse(keyinfo.KeyChar.ToString()) < SUPPORTEDLANGUAGES.Length)
      {
        CultureInfo.CurrentUICulture = new CultureInfo(SUPPORTEDLANGUAGES[Int32.Parse(keyinfo.KeyChar.ToString())]);
        return true;
      }
    } while (keyinfo.Key != ConsoleKey.Escape);
    return false;
  }
}
