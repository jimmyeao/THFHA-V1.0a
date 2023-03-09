namespace THFHA_V1._0.Model
{
    public class MeetingPermissions
    {
        #region Public Properties

        public bool CanLeave { get; set; }
        public bool CanReact { get; set; }
        public bool CanStopSharing { get; set; }
        public bool CanToggleBlur { get; set; }
        public bool CanToggleChat { get; set; }
        public bool CanToggleHand { get; set; }
        public bool CanToggleMute { get; set; }
        public bool CanToggleRecord { get; set; }
        public bool CanToggleShareTray { get; set; }
        public bool CanToggleVideo { get; set; }

        #endregion Public Properties
    }

    public class MeetingState
    {
        #region Public Properties

        public bool IsBackgroundBlurred { get; set; }
        public bool IsCameraOn { get; set; }
        public bool IsHandRaised { get; set; }
        public bool IsInMeeting { get; set; }
        public bool IsMuted { get; set; }
        public bool IsRecordingOn { get; set; }

        #endregion Public Properties
    }

    public class MeetingUpdate
    {
        #region Public Properties

        public MeetingPermissions MeetingPermissions { get; set; }
        public MeetingState MeetingState { get; set; }

        #endregion Public Properties
    }
}