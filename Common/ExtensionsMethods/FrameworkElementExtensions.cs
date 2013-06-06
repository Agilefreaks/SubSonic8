/**
 * Code taken from the following article
 * Title:   Accessing Dependency Property (DP) Changed in WinRT
 * Author:  Travis Schilling
 * Url:     http://blogs.interknowlogy.com/2012/11/28/dpchangedwinrt/
 * */
namespace Common.ExtensionsMethods
{
    using System;
    using System.Linq.Expressions;
    using Common.FrameworkElementExtensions;
    using Windows.UI.Xaml;

    public static class FrameworkElementExtensions
    {
        public static void RegisterDependencyPropertyChanged<T>(
            this FrameworkElement element,
            Expression<Func<T>> dependencyPropertyFunc,
            Action<DependencyPropertyChangedEventArgs> changedCallback)
        {
            FrameworkElementAttachedProperties.RegisterDependencyPropertyBinding(element, dependencyPropertyFunc, changedCallback);
        }
    }
}