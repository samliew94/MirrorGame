using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ClientRNG
{
    private ClientRNG() { }
    private static readonly Lazy<ClientRNG> lazy = new Lazy<ClientRNG>(() => new ClientRNG());
    public static ClientRNG getInstance() => lazy.Value;

    public readonly Random random = new Random();


}