using System.Numerics;
using FmodAudio;

namespace memoryGame;
public class SoundSystem
{
  public FmodSystem System { get; }
  public List<Vector3> GPSPath { get; set; }
  public Sound[] Sounds { get; set; }

  private Vector3 ListenerPos = new Vector3() { Z = -1.0f };
  public Vector3 Up = new Vector3(0, 1, 0), Forward = new Vector3(0, 0, -1);
  public int maxSouns { get; set; }
  public SoundSystem(int maxSounds)
  {
    //Creates the FmodSystem object
    System = FmodAudio.Fmod.CreateSystem();
    //System object Initialization
    System.Init(4093, InitFlags._3D_RightHanded);
    //Set the distance Units (Meters/Feet etc)
    System.Set3DSettings(1.0f, 1.0f, 1.0f);
    System.Set3DListenerAttributes(0, in ListenerPos, default, in Forward, in Up);
    //Load some sounds
    float min = 2f, max = 40f; // 40 is apprximatively
    this.maxSounds = maxSounds;
    loadSounds();
  }
  private void loadSounds()
  {
    Sounds = new Sound[maxSounds];
    Sound sound;
    for (int i = 0; i < maxSounds; i++)
    {
      Sounds[i] = sound = System.CreateSound($"test{i+1}.wav", Mode._3D | Mode.Loop_Off | Mode._3D_LinearSquareRolloff);
    }
  }
}
