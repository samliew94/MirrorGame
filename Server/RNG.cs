using System;
using System.Collections.Generic;

public class RNG
{
    private Random randomizer = new Random();

    private static readonly Lazy<RNG> lazy = new Lazy<RNG>(() => new RNG());
    public static RNG GetInstance() => lazy.Value;
    private RNG() { }

    public Random Random() => randomizer;
  
    public void shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = randomizer.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    
}
