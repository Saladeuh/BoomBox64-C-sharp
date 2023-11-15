using System.Numerics;
using FmodAudio;

namespace memoryGame;
public class SoundSystem
{
  public FmodSystem System { get; }
  public Sound[] Sounds { get; set; }
  public Channel[] Channels { get; set; }
  private Vector3 ListenerPos = new Vector3() { Z = -1.0f };
  public Vector3 Up = new Vector3(0, 1, 0), Forward = new Vector3(0, 0, -1);
  public int maxSounds { get; set; }
  public List<Channel> Musics { get; private set; }
  public Sound JingleCaseWin { get; private set; }
  public Sound JingleCaseLose { get; private set; }
  public Sound JingleWin { get; private set; }
  public Sound JingleLose { get; private set; }
  public Sound JingleError { get; private set; }

  public SoundSystem(int maxSounds, string group)
  {
    //Creates the FmodSystem object
    System = FmodAudio.Fmod.CreateSystem();
    //System object Initialization
    System.Init(4093, InitFlags._3D_RightHanded);
    System.MasterSoundGroup.GetValueOrDefault().Volume = 0.5f;
    //Set the distance Units (Meters/Feet etc)
    System.Set3DSettings(1.0f, 1.0f, 1.0f);
    System.Set3DListenerAttributes(0, in ListenerPos, default, in Forward, in Up);
    //Load sounds
    this.maxSounds = maxSounds;
    Sounds = new Sound[maxSounds];
    LoadSounds(group);
    Musics = new List<Channel>();
    LoadMusics();
  }
  private const string CONTENTFOLDER = "Content/";

  private void LoadSounds(string group)
  {
    Channels = new Channel[maxSounds];
    Sound sound;
    var rnd = new Random();
    var files = Directory.GetFiles(CONTENTFOLDER + group, "*.wav");
    var rndArray = Enumerable.Range(0, files.Length).OrderBy(item => rnd.Next()).ToArray();
    for (int i = 0; i < maxSounds; i++)
    {
      Sounds[i] = sound = System.CreateSound(files[rndArray[i]], Mode._3D | Mode.Loop_Off | Mode._3D_LinearSquareRolloff);
      //Channels[i] = System.PlaySound(sound, paused: true);
      //Channels[i].Volume = 0.5f;
    }
  }
  private void LoadMusics()
  {
    Sound sound;
    sound = System.CreateStream(CONTENTFOLDER + "music/OTOATE.wav");
    //PlayQueue(sound);
    Musics.Add(((Channel?)System.PlaySound(sound, paused: false)));
    JingleCaseWin = sound = System.CreateStream(CONTENTFOLDER + "music/Jingle_SLVSTAR1.mp3");
    JingleCaseLose = sound = System.CreateStream(CONTENTFOLDER + "music/Jingle_DROPSTAR.mp3");
    JingleWin = sound = System.CreateStream(CONTENTFOLDER + "music/Jingle_MINICLEAR.mp3");
    JingleLose = sound = System.CreateStream(CONTENTFOLDER + "music/Jingle_MINIOVER.mp3");
    JingleError = sound = System.CreateStream(CONTENTFOLDER + "music/SM64_Error.ogg");

  }
  public List<Task> tasks = new List<Task>();
  public void PlayQueue(Sound sound, bool queued = true)
  {
    if (queued)
    {
      tasks.Add(Task.Factory.StartNew(() =>
      {
        int all, real;
        do
        {
          System.GetChannelsPlaying(out all, out real);
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
}
