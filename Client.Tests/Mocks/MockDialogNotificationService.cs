using System.Collections.Generic;
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

        public void Show(DialogNotificationOptions options)
        {
            Showed.Add(options);
        }
    }
}