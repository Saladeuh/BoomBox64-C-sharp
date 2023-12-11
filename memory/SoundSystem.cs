using System.Numerics;
using FmodAudio;

namespace memoryGame;
public class SoundSystem
{
  private const string CONTENTFOLDER = "Content/";
  public FmodSystem System { get; }
  public Sound[] Sounds { get; set; }
  public Channel[] Channels { get; set; }
  private Vector3 ListenerPos = new() { Z = -1.0f };
  public Vector3 Up = new(0, 1, 0), Forward = new(0, 0, -1);
  public int MaxSounds { get; set; }
  public List<Channel> Musics { get; private set; }
  public Sound JingleCaseWin { get; private set; }
  public Sound JingleCaseLose { get; private set; }
  public Sound JingleWin { get; private set; }
  public Sound JingleLose { get; private set; }
  public Sound JingleError { get; private set; }
  public float Volume { get { return System.MasterSoundGroup.GetValueOrDefault().Volume; } set { System.MasterSoundGroup.GetValueOrDefault().Volume = value; } }
  public SoundSystem()
  {
    //Creates the FmodSystem object
    System = FmodAudio.Fmod.CreateSystem();
    //System object Initialization
    System.Init(4093, InitFlags._3D_RightHanded);
    Volume = 0.5f;
    //Set the distance Units (Meters/Feet etc)
    System.Set3DSettings(1.0f, 1.0f, 1.0f);
    System.Set3DListenerAttributes(0, in ListenerPos, default, in Forward, in Up);
    Sounds = Array.Empty<Sound>();
    Channels = Array.Empty<Channel>();
    Musics = new List<Channel>();
  }

  public void LoadLevel(int maxSounds, string group)
  {
    //Load sounds
    this.MaxSounds = maxSounds;
    Sounds = new Sound[maxSounds];
    LoadLevelSounds(group);
    Musics = new List<Channel>();
    Channels = new Channel[MaxSounds];
    LoadLevelMusics();
  }
  public void LoadMenu()
  {
    if (!Musics.Any())
    {
      LoadMenuMusics();
    }
  }
  private void LoadLevelSounds(string group)
  {
    Sound sound;
    var rnd = new Random();
    var files = Directory.GetFiles(CONTENTFOLDER + group, "*.wav");
    var rndArray = Enumerable.Range(0, files.Length).OrderBy(item => rnd.Next()).ToArray();
    for (int i = 0; i < MaxSounds; i++)
    {
      Sounds[i] = sound = System.CreateSound(files[rndArray[i]], Mode._3D | Mode.Loop_Off | Mode._3D_LinearSquareRolloff);
      //Channels[i] = System.PlaySound(sound, paused: true);
      //Channels[i].Volume = 0.5f;
    }
  }
  private void LoadLevelMusics()
  {
    Sound sound;
    sound = System.CreateStream(CONTENTFOLDER + "music/OTOATE.wav");
    //PlayQueue(sound);
    Musics.Add((Channel?)System.PlaySound(sound, paused: false));
    JingleCaseWin = System.CreateStream(CONTENTFOLDER + "music/Jingle_SLVSTAR1.mp3");
    JingleCaseLose = System.CreateStream(CONTENTFOLDER + "music/Jingle_DROPSTAR.mp3");
    JingleWin = System.CreateStream(CONTENTFOLDER + "music/Jingle_MINICLEAR.mp3");
    JingleLose = System.CreateStream(CONTENTFOLDER + "music/Jingle_MINIOVER.mp3");
    JingleError = System.CreateStream(CONTENTFOLDER + "music/SM64_Error.ogg");
  }
  private void LoadMenuMusics()
  {
    Sound sound;
    sound = System.CreateStream(CONTENTFOLDER + "music/PLAYROOM.mp3", Mode.Loop_Normal);
    Channel? channel = (Channel?)System.PlaySound(sound, paused: false);
    channel.LoopCount = -1;
    channel.SetLoopPoints(TimeUnit.MS, 0, TimeUnit.MS, 31623);
    Musics.Add(channel);
  }

  public List<Task> tasks = new();
  public void PlayQueue(Sound sound, bool queued = true)
  {
    if (queued)
    {
      tasks.Add(Task.Factory.StartNew(() =>
      {
        int real;
        do
        {
          System.GetChannelsPlaying(out int all, out real);
        } while (real > 1);
        Channel? channel = System.PlaySound(sound, paused: false);
        if (channel != null)
        {
          while (channel.IsPlaying) { Thread.Sleep(5); }
          try
          {
            channel.Stop();
          }
          catch { }
        }
      }));
    }
    else
    {
      tasks.Add(Task.Factory.StartNew(() =>
      {
        Channel? channel = System.PlaySound(sound, paused: false);
        if (channel != null)
        {
          while (channel.IsPlaying) { Thread.Sleep(5); }
          channel.Stop();
        }
      }));
    }
  }
  public void FreeRessources()
  {
    Musics.ForEach((music) =>
    {
      if (music != null) music.Stop();
    });
    Musics.Clear();
  }
}
