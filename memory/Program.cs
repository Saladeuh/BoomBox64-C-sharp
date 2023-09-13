namespace memory
{
  internal class Program
  {
    static void Main(string[] args)
    {

      ConsoleKeyInfo keyinfo;
      do
      {
        keyinfo = Console.ReadKey();
        Console.WriteLine(keyinfo.Key + " was pressed");
      }
      while (keyinfo.Key != ConsoleKey.X);
    }
  }
}