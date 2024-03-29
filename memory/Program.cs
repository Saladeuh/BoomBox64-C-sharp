﻿using System.Globalization;
using System.Reflection.Emit;
using LocalizationCultureCore.StringLocalizer;
using memoryGame;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace BoomBox;

internal class Program
{
  private const string DATAFILEPATH = "data.json";
  static void Main()
  {
    var parameters = LoadJson();
    SetConsoleParams(parameters.Language);
    var menu = new MainMenu(parameters);
    parameters= menu.Run();
    WriteJson(parameters);
  }
  public static void SetConsoleParams(string language)
  {
    Console.Title = "Boom box";
    if (language!=null) CultureInfo.CurrentUICulture = new CultureInfo(language);
  }
  public static Parameters LoadJson()
  {
    if (File.Exists(DATAFILEPATH))
    {
      using StreamReader r = new(DATAFILEPATH);
      string json = r.ReadToEnd();
      Parameters parameters = JsonConvert.DeserializeObject<Parameters>(json);
      return parameters;
    }
    else
    {
      return new Parameters { Score = 0, Volume = 0.5f };
    }
  }
  public static void WriteJson(Parameters parameters)
  {
    var json = JsonConvert.SerializeObject(parameters);
    File.WriteAllText(DATAFILEPATH, json);
  }
}