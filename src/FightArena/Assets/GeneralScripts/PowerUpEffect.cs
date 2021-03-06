﻿using UnityEngine;

//[ExecuteInEditMode]
public class PowerUpEffect : MonoBehaviour, ISpawnable
{
    /* for testing
    [SerializeField]
    int testPowerLevel;

    [SerializeField]
    bool update = false;

    private void Update()
    {
        if(update)
        {
            update = false;
            SetPowerLevel(testPowerLevel);
        }
    }
    */

    public GameObject GameObject { get { return (this == null) ? null : base.gameObject; } }
    public Transform Transform { get { return (this == null) ? null : transform; } }
    public string PrefabName { get { return "PowerUpEffect"; } }
    private const int maxLevel = Body.maxEmpowermentLevel;
    private ParticleSystem[] pSystems = new ParticleSystem[maxLevel + 1];//0 represents no powerup, but it plays a clearing splash

    private const float maxLifetime = 3;
    float currentLifetime = maxLifetime;

    private void Awake()
    {
        for (int i = 0; i <= maxLevel; i++)
        {
            pSystems[i] = transform.Find("Level_" + i).gameObject.GetComponent<ParticleSystem>();
            pSystems[i].Stop();
            var m = pSystems[i].main;
            m.stopAction = ParticleSystemStopAction.None;
            m.duration = Body.empowerTime;
        }
    }

    private void Update()
    {
        var isAlive = false;
        for (int i = 0; i <= maxLevel; i++)
        {
            isAlive |= pSystems[i].IsAlive(true);
        }

        if(currentLifetime > 0 || isAlive)
        {
            currentLifetime -= Time.deltaTime;
        }
        else
        {
            CleanUp();
        }
    }

    public void SetPowerLevel(int powerLevel)
    {
        currentLifetime = maxLifetime;
        powerLevel = Mathf.Clamp(powerLevel, 0, maxLevel);
        for (int i = 0; i <= maxLevel; i++)
        {
            var pSys = pSystems[i];
            pSys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            if (i == powerLevel)
                pSys.Play();
        }
        /*
        if(powerLevel == 0)
        {
            var zero = Instantiate<GameObject>(pSystems[0].gameObject, transform.parent);
            zero.transform.position = pSystems[0].transform.position; 
            zero.transform.rotation = pSystems[0].transform.rotation;
            var pSys = zero.GetComponent<ParticleSystem>();
            pSys.Stop();
            pSys.Play();
            var main = pSys.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            Destroy(this.gameObject);
        }*/
    }

    public void CleanUp()
    {
        currentLifetime = maxLifetime;
        Destroy(gameObject);
    }

}