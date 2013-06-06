/**
 * Code taken from the following article
 * Title:   Accessing Dependency Property (DP) Changed in WinRT
 * Author:  Travis Schilling
 * Url:     http://blogs.interknowlogy.com/2012/11/28/dpchangedwinrt/
 * */
namespace Common.FrameworkElementExtensions
{
    using System;
    using System.Collections.Generic;
    using Windows.UI.Xaml;

    internal class DependencyPropertyChangedCallbacks : Dictionary<DependencyProperty, Action<DependencyPropertyChangedEventArgs>>
    {
    }
}