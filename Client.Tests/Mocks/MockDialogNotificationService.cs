﻿namespace Client.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Subsonic8.Framework.Services;

    public class MockDialogNotificationService : IDialogNotificationService
    {
        #region Constructors and Destructors

        public MockDialogNotificationService()
        {
            Showed = new List<DialogNotificationOptions>();
        }

        #endregion

        #region Public Properties

        public List<DialogNotificationOptions> Showed { get; set; }

        #endregion

        #region Public Methods and Operators

        public async Task Show(DialogNotificationOptions options)
        {
            Showed.Add(options);

            await Task.Run(() => { });
        }

        public async Task Show(DialogNotificationOptions options, Action onDialogClosed)
        {
            Showed.Add(options);

            await Task.Run(() => { });
        }

        #endregion
    }
}