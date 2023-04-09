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

    public void playEngine(Vector3 velocity){
        
        if(velocity.magnitude > 0 && !Engine.isPlaying ) {
            Debug.Log("Engine Sound!");
            Engine.PlayOneShot(Engine.clip);
            }
    }

    public void stopEngine(){
        Engine.Stop();
    }

    public void playLaser(){
        nowPlaying.PlayOneShot(Laser);


    }

    public void playMissile(){
        nowPlaying.PlayOneShot(Missile);
    }
}
