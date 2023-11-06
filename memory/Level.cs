﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FmodAudio;
using memory;

namespace memoryGame;

internal class Level
{
  private int Retry { get; set; }
  public int NbSounds { get; }
  private int MaxRetry { get; set; }
  private List<(int, CaseState)> Grid { get; set; }
  public SoundSystem SoundSystem { get; set; }
  public Level(int nbSounds=3, string group="test", int maxRetry=5)
  {
    NbSounds = nbSounds;
    MaxRetry = maxRetry;
    FillGridByRandomInt();
    SoundSystem = new SoundSystem(nbSounds, group);
  }
  public void FillGridByRandomInt()
  {
    var rnd = new Random();
    var randomDisposition = Enumerable.Range(1, NbSounds).Concat(Enumerable.Range(1, NbSounds)).OrderBy(r => rnd.Next()).ToArray();
    Grid = new List<(int, CaseState)>();
    foreach (int n in randomDisposition)
    {
      Grid.Add((n, CaseState.None));
    }
  }
  public bool PlayLevel()
  {
    ConsoleKeyInfo keyinfo;
    foreach (var pair in Grid)
    { Console.WriteLine(pair); }
    do
    {
      keyinfo = Console.ReadKey();
      if (char.IsDigit(keyinfo.KeyChar) && int.Parse(keyinfo.KeyChar.ToString())<=NbSounds*2)
      {
        int caseIndex = int.Parse(keyinfo.KeyChar.ToString()) - 1;
        int soundIndex = Grid[caseIndex].Item1 - 1;
        SoundSystem.PlayQueue(SoundSystem.Sounds[soundIndex]);
        TryCase(soundIndex + 1, caseIndex);
        foreach (var pair in Grid)
        { Console.WriteLine(pair); }
        if (Grid.All(pair => pair.Item2 == CaseState.Paired))
        {
          Console.WriteLine("gagné!");
          SoundSystem.PlayQueue(SoundSystem.JingleWin);
          return true;
        } else if (Retry >= MaxRetry)
        {
          Console.WriteLine("perdu");
          SoundSystem.PlayQueue(SoundSystem.JingleLose);
          return false;
        }
      }
    } while (keyinfo.Key != ConsoleKey.X);
    return false;
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
      SoundSystem.PlayQueue(SoundSystem.JingleCaseWin);
    }
    else if (isACaseTouched.Count() == 1)
    {
      Grid[isACaseTouched[0]] = (Grid[isACaseTouched[0]].Item1, CaseState.None);
      Retry++;
      SoundSystem.PlayQueue(SoundSystem.JingleCaseLose);
    }
    else if (caseIndexTouched.Count() == 0)
    {
      Grid[caseIndex] = (Grid[caseIndex].Item1, CaseState.Touched);
    }
  }

}