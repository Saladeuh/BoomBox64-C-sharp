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
  private int[,] Grid { get; set; }
  public GameStatus Status { get; set; }
  public SoundSystem SoundSystem { get; set; }
  public Game()
  {
    MaxRetry = 10;
    Grid = new int[3,2];
    FillGridByRandomInt();
    Status = GameStatus.New;
    SoundSystem= new SoundSystem();
  }
  public void FillGridByRandomInt()
  {
   var rnd= new Random();
    for (int i = 0; i < Grid.GetLength(0); i++)
    {
      for (int j = 0; j < Grid.GetLength(1); j++)
        Grid[i, j] = rnd.Next(1, 3);
    }
  }
  public void Play()
  {
    ConsoleKeyInfo keyinfo;
    do
    {
      keyinfo = Console.ReadKey();
      if (char.IsDigit(keyinfo.KeyChar))
      {
        SoundSystem.System.PlaySound(SoundSystem.Sounds[int.Parse(keyinfo.KeyChar.ToString())-1], paused: false);
        //Console.WriteLine(Grid[keyinfo.KeyChar]);
      }
      Console.WriteLine(keyinfo.KeyChar);
    } while (keyinfo.Key != ConsoleKey.X);
  }
}
