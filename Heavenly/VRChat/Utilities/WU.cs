using System;
using System.Linq;

namespace Heavenly.VRChat.Utilities
{
    public static class WU
    {
        public static string BuildInstanceID() => RoomManager.field_Internal_Static_ApiWorldInstance_0.id;
    }
}
