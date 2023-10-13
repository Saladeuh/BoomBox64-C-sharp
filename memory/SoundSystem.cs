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
  public List<Sound> Musics { get; private set; }

  public SoundSystem(int maxSounds)
  {
    //Creates the FmodSystem object
    System = FmodAudio.Fmod.CreateSystem();
    //System object Initialization
    System.Init(4093, InitFlags._3D_RightHanded);
    System.MasterSoundGroup.GetValueOrDefault().Volume = 0.5f;
    //Set the distance Units (Meters/Feet etc)
    System.Set3DSettings(1.0f, 1.0f, 1.0f);
    System.Set3DListenerAttributes(0, in ListenerPos, default, in Forward, in Up);
    //Load some sounds
    float min = 2f, max = 40f; // 40 is apprximatively
    this.maxSounds = maxSounds;
    loadSounds();
    LoadMusics();
  }
  private void loadSounds()
  {
    Sounds = new Sound[maxSounds];
    Channels = new Channel[maxSounds];
    Sound sound;
    for (int i = 0; i < maxSounds; i++)
    {
      Sounds[i] = sound = System.CreateSound($"test{i+1}.wav", Mode._3D | Mode.Loop_Off | Mode._3D_LinearSquareRolloff);
      //Channels[i] = System.PlaySound(sound, paused: true);
      //Channels[i].Volume = 0.5f;
    }
  }
  private void LoadMusics()
  {
    Musics = new List<Sound>();
    Sound sound;
    sound = System.CreateStream("music/OTOATE.wav");
    System.PlaySound(sound, paused: false);
    Musics.Add(sound);
  }
      
  public void PlayQueue(Sound sound)
  {
    Thread thread=new Thread(() =>
    {
      int all, real;
      do
      {
        System.GetChannelsPlaying(out all, out real);
      } while (real > 1);
        Channel channel=System.PlaySound(sound, paused: false);
      while(channel.IsPlaying) { }
      channel.Stop();
    });
    thread.Start();
  }
}
