using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IHost
{
    /// <summary>
    /// method fired if and only if the user is a host
    /// </summary>
    public void onIsHostChanged(bool isHost);

}
