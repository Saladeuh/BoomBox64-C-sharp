using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using memoryGame;

namespace memory;

internal abstract class IGlobabConsoleActions
{
  public abstract SoundSystem SoundSystem { get; set; }
  protected void GlobalActions(ConsoleKey keyinfo)
  {
    switch (keyinfo)
    {
      case ConsoleKey.F2:
        SoundSystem.Volume -= 0.1f;
        break;
      case ConsoleKey.F3:
        SoundSystem.Volume += 0.1f;
        break;
    }
  }
}
