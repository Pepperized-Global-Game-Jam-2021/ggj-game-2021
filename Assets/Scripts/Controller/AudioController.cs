using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public Sound[] CarpetFootsteps;
    public Sound[] HardFootsteps;
    public Sound SaferoomSong;
    public Sound Ambience;
    public Sound Chase;
    public Sound PickupNote;
    public Sound BurnNote;
    public Sound DoorOpen;
    public Sound ShortStinger;

    private Sound musicTrack;
    private MusicTrack currentTrack = MusicTrack.None;

    public enum MusicTrack
    {
        None,
        Saferoom,
        Ambience,
        Chase
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        foreach (var sound in CarpetFootsteps)
        {
            SetupSound(sound);
            sound.volume = 0.3f;
        }

        foreach (var sound in HardFootsteps)
        {
            SetupSound(sound);
            sound.volume = 0.3f;
        }

        SetupSound(SaferoomSong);
        SetupSound(Ambience);
        SetupSound(Chase);
        SetupSound(PickupNote);
        SetupSound(BurnNote);
        SetupSound(DoorOpen);
        SetupSound(ShortStinger);
    }

    private void SetupSound(Sound sound)
    {
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.audioClip;
        sound.source.pitch = sound.pitch;
        sound.source.volume = sound.volume;
        sound.source.loop = sound.loop;
    }

    public void PlayCarpetFootstep()
    {
        Sound sound = CarpetFootsteps[Random.Range(0, CarpetFootsteps.Length - 1)];
        sound.source.Play();
    }

    public void PlayHardFootstep()
    {
        Sound sound = HardFootsteps[Random.Range(0, HardFootsteps.Length - 1)];
        sound.source.Play();
    }

    public void PlayNotePickup()
    {
        PickupNote.source.Play();
    }

    public void PlayDoorOpen()
    {
        DoorOpen.source.Play();
    }

    public void PlayBurnNote()
    {
        BurnNote.source.Play();
    }

    public void PlayStinger()
    {
        ShortStinger.source.Play();
    }

    public void ChangeMusic(MusicTrack track)
    {
        if (track == currentTrack) return;
        if (musicTrack != null)
        {
            musicTrack.source.Stop();
        }

        currentTrack = track;

        switch (track)
        {
            case MusicTrack.Saferoom:
                musicTrack = SaferoomSong;
                SaferoomSong.source.Play();
                break;
            case MusicTrack.Ambience:
                musicTrack = Ambience;
                Ambience.source.Play();
                break;
            case MusicTrack.Chase:
                musicTrack = Chase;
                Chase.source.Play();
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
