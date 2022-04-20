using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPlayerInfo
{
    /// <summary>
    /// <para>main key = 'global'; value = Dictionary</para>
    /// <para>sub keys = 'displayName', 'colorId', 'isHost'</para>
    /// </summary>
    public void actOnPlayerInfo(string json);
}