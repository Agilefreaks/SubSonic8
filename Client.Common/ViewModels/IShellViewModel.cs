﻿using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using Windows.UI.Xaml;

namespace Client.Common.ViewModels
{
    public interface IShellViewModel : IViewAware, IScreen, IBottomBarViewModelProvider
    {
        Uri Source { get; set; }

        ISubsonicService SubsonicService { get; set; }

        Action<SearchResultCollection> NavigateToSearhResult { get; set; }

        Task PerformSubsonicSearch(string query);

        void PlayNext(object sender, RoutedEventArgs routedEventArgs);

        void PlayPrevious(object sender, RoutedEventArgs routedEventArgs);
    }
}