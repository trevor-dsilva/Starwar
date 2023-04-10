using System.Collections.Generic;
using UnityEngine;
public class MachineGunManager : MonoBehaviour
{
    public List<MachineGun> machineGuns;
    public float FireInterval;
    public int Ammunition, MaxAmmunition;
    public float ReloadInterval;

    private float lastFire;
    private int lastMachineGun;
    private Rigidbody _rigidbody;
    private float lastReloadTime = 0;
    [SerializeField] private SoundController soundController;
    private void Start()
    {
        lastFire = Time.fixedTime;
        lastMachineGun = 0;
        Ammunition = MaxAmmunition;
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void Fire()
    {
        if (Ammunition <= 0) { return; }
        if (lastFire + FireInterval > Time.fixedTime) { return; }

        machineGuns[lastMachineGun].velocity = _rigidbody.velocity;
        machineGuns[lastMachineGun].Fire();
        soundController.playLaser();
        Ammunition--;
        lastMachineGun++;
        if (lastMachineGun >= machineGuns.Count)
        { lastMachineGun = 0; }
        //Debug.Log(Ammunition);

        lastFire = Time.fixedTime;
    }

    public void Reload(int amount = 1)
    {
        //Debug.Log("Machine Gun Manager Reload");
        if (Ammunition < MaxAmmunition)
        {
            if (lastReloadTime + ReloadInterval <= Time.fixedTime)
            {
                lastReloadTime = Time.fixedTime;
                Ammunition += amount;
            }
        }
    }
}
