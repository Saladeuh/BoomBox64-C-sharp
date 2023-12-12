using System.Reflection.Emit;
using memoryGame;

namespace memory;

internal class Program
{
  static void Main()
  {
    SetConsoleParams();
    var menu =new MainMenu();
    menu.Run();
  }
  public static void SetConsoleParams()
  {
    Console.Title = "Memory";
  }
}