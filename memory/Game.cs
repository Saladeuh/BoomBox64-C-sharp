using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memoryGame;

internal class Game
{
  private int Retry { get; set; }
  private int MaxRetry { get; set; }
  private List<(int, CaseState)> Grid { get; set; }
  public GameStatus Status { get; set; }
  public SoundSystem SoundSystem { get; set; }
  public Game()
  {
    MaxRetry = 10;
    FillGridByRandomInt();
    Status = GameStatus.New;
    SoundSystem = new SoundSystem(3);
  }
  public void FillGridByRandomInt()
  {
    var rnd = new Random();
    var randomDisposition = Enumerable.Range(1, 3).Concat(Enumerable.Range(1, 3)).OrderBy(r => rnd.Next()).ToArray();
    Grid = new List<(int, CaseState)>();
    foreach (int n in randomDisposition)
    {
      Grid.Add((n, CaseState.None));
    }
  }
  public void Play()
  {
    ConsoleKeyInfo keyinfo;
    foreach (var pair in Grid)
    { Console.WriteLine(pair); }
    do
    {
      keyinfo = Console.ReadKey();
      if (char.IsDigit(keyinfo.KeyChar))
      {
        int caseIndex = int.Parse(keyinfo.KeyChar.ToString()) - 1;
        int soundIndex = Grid[caseIndex].Item1 - 1;
        TryCase(soundIndex + 1);
        Grid[caseIndex] = (Grid[caseIndex].Item1, CaseState.Touched);
        SoundSystem.System.PlaySound(SoundSystem.Sounds[soundIndex], paused: false);
      }
    } while (keyinfo.Key != ConsoleKey.X);
  }
  private void TryCase(int index)
  {
    if (Grid.Contains((index, CaseState.Touched))) { 
      Console.WriteLine("wéééé");
  }
  }
}
