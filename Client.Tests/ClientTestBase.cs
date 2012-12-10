﻿using Caliburn.Micro;
using Client.Tests.Mocks;
using Subsonic8.Shell;

namespace Client.Tests
{
    public abstract class ClientTestBase
    {
        protected ClientTestBase()
        {
            Configure();
        }

        protected void Configure()
        {
            var mockEventAggregator = new MockEventAggregator();
            var mockSubsonicService = new MockSubsonicService();
            var mockNavigationService = new MockNavigationService();

            var shellViewModel = new ShellViewModel(mockEventAggregator, mockSubsonicService, mockNavigationService);

            IoC.GetInstance = (type, s) =>
                                  {
                                      object instance = null;
                                      if (type == typeof (IShellViewModel))
                                      {
                                          instance = shellViewModel;
                                      }
                                      return instance;
                                  };
        }
    }
}
