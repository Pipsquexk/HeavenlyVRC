using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heavenly.VRChat.Utilities
{
    public static class WU
    {

        public static string BuildInstanceID()
        {
            return RoomManager.field_Internal_Static_ApiWorldInstance_0.id;
        }

    }
}
