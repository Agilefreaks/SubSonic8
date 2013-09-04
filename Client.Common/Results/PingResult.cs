﻿namespace Client.Common.Results
{
    using System.Xml.Linq;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class PingResult : EmptyResponseResultBase, IPingResult
    {
        #region Constructors and Destructors

        public PingResult(ISubsonicServiceConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion

        #region Public Properties

        public Error ApiError { get; set; }

        public override string ViewName
        {
            get
            {
                return "ping.view";
            }
        }

        #endregion

        #region Methods

        protected override void HandleResponse(XDocument xDocument)
        {
            Result = Error != null;
        }

        #endregion
    }
}