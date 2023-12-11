﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using memoryGame;

namespace memory;

internal class MainMenu
{
  public void Run()
  {
    ConsoleKeyInfo keyinfo;
    do
    {
      keyinfo = Console.ReadKey();
      switch (keyinfo.Key)
      {
        case ConsoleKey.Enter:
          PlayGame();
          break;
      }
    } while (keyinfo.Key != ConsoleKey.Escape);
  }
  public void PlayGame()
  {
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
        level = new(3, groups[0], 3);
      }
      else
      {
        level = new Level(4, randomGroup, 3);
      }
    } while (level.Play());
  }
}
