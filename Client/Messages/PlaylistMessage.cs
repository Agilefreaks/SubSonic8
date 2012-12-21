using System.Collections.Generic;
using Client.Common.Models;

namespace Subsonic8.Messages
{
    public class PlaylistMessage
    {
        public List<ISubsonicModel> Queue { get; set; }

        public bool ClearCurrent { get; set; }

        public PlaylistMessage()
        {
            Queue = new List<ISubsonicModel>();
        }
    }
}
