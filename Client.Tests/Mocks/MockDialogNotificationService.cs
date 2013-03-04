using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Subsonic8.Framework.Services;

namespace Client.Tests.Mocks
{
    public class MockDialogNotificationService : IDialogNotificationService
    {
        public List<DialogNotificationOptions> Showed { get; set; }

        public MockDialogNotificationService()
        {
            Showed = new List<DialogNotificationOptions>();
        }

        public Task Show(DialogNotificationOptions options)
        {
            Showed.Add(options);

            return new Task(() => { });
        }

        public Task Show(DialogNotificationOptions options, Action onDialogClosed)
        {
            Showed.Add(options);

            return new Task(() => {});
        }
    }
}