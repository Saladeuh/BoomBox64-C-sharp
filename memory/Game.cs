using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using memory;

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
        TryCase(soundIndex + 1, caseIndex);
        foreach (var pair in Grid)
        { Console.WriteLine(pair); }
        SoundSystem.System.PlaySound(SoundSystem.Sounds[soundIndex], paused: false);
        if(Grid.All(pair => pair.Item2 == CaseState.Paired))
        {
          Console.WriteLine("gagné!");
          return;
        }
      }
    } while (keyinfo.Key != ConsoleKey.X);
  }
  private void TryCase(int soundIndex, int caseIndex)
  {
    var caseIndexTouched = Util.IndexesWhere(Grid, o => o.Item1 == soundIndex && o.Item2 == CaseState.Touched).ToList();
    var isACaseTouched = Util.IndexesWhere(Grid, o => o.Item2 == CaseState.Touched).ToList();
    if (Grid[caseIndex].Item2 == CaseState.Paired)
    {
      return;
    }
    else if (caseIndexTouched.Count() == 1 && !caseIndexTouched.Contains(caseIndex))
    {
      Grid[caseIndexTouched[0]] = (soundIndex, CaseState.Paired);
      Grid[caseIndex] = (soundIndex, CaseState.Paired);
    }
    else if (isACaseTouched.Count() == 1)
    {
      Grid[isACaseTouched[0]] = (Grid[isACaseTouched[0]].Item1, CaseState.None);
    }
    else if (caseIndexTouched.Count() == 0)
    {
      Grid[caseIndex] = (Grid[caseIndex].Item1, CaseState.Touched);
    }
  }
}