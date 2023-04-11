using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip Explosion, Laser, Missile;
    public AudioSource Engine;

    public AudioSource nowPlaying;
    private void Awake() {
        
    }

    public void playExplosion(){
        nowPlaying.PlayOneShot(Explosion);        
    }

    public void playEngine(){
        if(!Engine.isPlaying) Engine.Play();
    }

    public void stopEngine(){
        if(Engine.isPlaying) Engine.Stop();
    }

    public void playLaser(){
        nowPlaying.PlayOneShot(Laser);


    }

    public void playMissile(){
        nowPlaying.PlayOneShot(Missile);
    }
}
