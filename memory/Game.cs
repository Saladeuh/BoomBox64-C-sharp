using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memory;

internal class Game
{
  private int Retry { get; set; }
  private int MaxRetry { get; set; }
  private int[] Grid { get; set; }
  public GameStatus Status { get; set; }
  public SoundSystem SoundSystem { get; set; }
  public Game()
  {
    MaxRetry = 10;
    Grid = new int[5];
    FillGridByRandomInt();
    Status = GameStatus.New;
    SoundSystem= new SoundSystem();
  }
  public void FillGridByRandomInt()
  {
   var rnd= new Random();
   Grid=Enumerable.Range(1, 3).Concat(Enumerable.Range(1, 3)).OrderBy(r => rnd.Next()).ToArray();
  }
  public void Play()
  {
    ConsoleKeyInfo keyinfo;
    foreach(int n in Grid) {  Console.WriteLine(n); }
    do
    {
      keyinfo = Console.ReadKey();
      if (char.IsDigit(keyinfo.KeyChar))
      {
        SoundSystem.System.PlaySound(SoundSystem.Sounds[int.Parse(keyinfo.KeyChar.ToString())-1], paused: false);
      }
      Console.WriteLine(keyinfo.KeyChar);
    } while (keyinfo.Key != ConsoleKey.X);
  }
}
