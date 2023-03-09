using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THFHA_V1._0.Model
{
    public class MeetingUpdate
    {
        public MeetingState MeetingState { get; set; }
        public MeetingPermissions MeetingPermissions { get; set; }
    }

    public class MeetingState
    {
        public bool IsMuted { get; set; }
        public bool IsCameraOn { get; set; }
        public bool IsHandRaised { get; set; }
        public bool IsInMeeting { get; set; }
        public bool IsRecordingOn { get; set; }
        public bool IsBackgroundBlurred { get; set; }
    }

    public class MeetingPermissions
    {
        public bool CanToggleMute { get; set; }
        public bool CanToggleVideo { get; set; }
        public bool CanToggleHand { get; set; }
        public bool CanToggleBlur { get; set; }
        public bool CanToggleRecord { get; set; }
        public bool CanLeave { get; set; }
        public bool CanReact { get; set; }
        public bool CanToggleShareTray { get; set; }
        public bool CanToggleChat { get; set; }
        public bool CanStopSharing { get; set; }
    }
}
