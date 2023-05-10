using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] EnergyBar energyBar;
    public const int MAX = 100;
    public const int PERCNET = 1;
    int energy;

    public void Start() 
    {
        energyBar.Initialize(energy, MAX);
    }

    public void Obtain(int value)
    {
        if (energy == MAX) return;

        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyBar.UpdateStats(energy, MAX);
    }

    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);
    }

    public bool IsEnough(int value) => energy >= value;
}
