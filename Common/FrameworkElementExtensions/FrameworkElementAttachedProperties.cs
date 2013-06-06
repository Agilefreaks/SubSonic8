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
    using System.Linq.Expressions;
    using Common.ExtensionsMethods;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    internal class FrameworkElementAttachedProperties : DependencyObject
    {
        #region  Constants

        private const string ExtensionDPsNamePrefix = "DependencyPropertyEx";

        #endregion

        #region Static Fields

        public static readonly DependencyProperty DependencyPropertyCallbacksProperty =
            DependencyProperty.RegisterAttached(
                "DependencyPropertyCallbacks",
                typeof(DependencyPropertyChangedCallbacks),
                typeof(FrameworkElementAttachedProperties),
                new PropertyMetadata(null));

        private static readonly Dictionary<string, DependencyProperty> StaticExtensionDPs =
            new Dictionary<string, DependencyProperty>();

        #endregion

        #region Public Methods and Operators

        public static DependencyPropertyChangedCallbacks GetDependencyPropertyCallbacks(DependencyObject d)
        {
            return (DependencyPropertyChangedCallbacks)d.GetValue(DependencyPropertyCallbacksProperty);
        }

        public static void RegisterDependencyPropertyBinding<T>(
            FrameworkElement element,
            Expression<Func<T>> dependencyPropertyFunc,
            Action<DependencyPropertyChangedEventArgs> changedCallback)
        {
            var propertyName = dependencyPropertyFunc.GetOperandName();
            var callbacks = GetCallbacksForElement(element);
            var attachedDependencyPropertyToBindTo = GetNextUnusedAttachedPropertyForFrameworkElement(propertyName);
            callbacks.Add(attachedDependencyPropertyToBindTo, changedCallback);

            RegisterDependencyPropertyBinding(element, propertyName, attachedDependencyPropertyToBindTo);
        }

        public static void SetDependencyPropertyCallbacks(
            DependencyObject dependencyObject, DependencyPropertyChangedCallbacks value)
        {
            dependencyObject.SetValue(DependencyPropertyCallbacksProperty, value);
        }

        #endregion

        #region Methods

        private static void DependencyPropertyExPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var callback = TryGetCallback((FrameworkElement)sender, e.Property);
            if (callback != null)
            {
                callback(e);
            }
        }

        private static DependencyPropertyChangedCallbacks GetCallbacksForElement(DependencyObject element)
        {
            var callbacks = GetDependencyPropertyCallbacks(element);
            if (callbacks == null)
            {
                callbacks = new DependencyPropertyChangedCallbacks();
                SetDependencyPropertyCallbacks(element, callbacks);
            }

            return callbacks;
        }

        private static DependencyProperty GetNextUnusedAttachedPropertyForFrameworkElement(string propertyName)
        {
            if (!StaticExtensionDPs.ContainsKey(propertyName))
            {
                var unusedDependencyProperty =
                    DependencyProperty.RegisterAttached(
                        ExtensionDPsNamePrefix + "_" + propertyName,
                        typeof(object),
                        typeof(FrameworkElementAttachedProperties),
                        new PropertyMetadata(null, DependencyPropertyExPropertyChanged));
                StaticExtensionDPs.Add(propertyName, unusedDependencyProperty);
            }

            return StaticExtensionDPs[propertyName];
        }

        private static void RegisterDependencyPropertyBinding(
            FrameworkElement element, string dependencyPropertyToBind, DependencyProperty attachedDp)
        {
            var bindingSource = new RelativeSource { Mode = RelativeSourceMode.Self };
            var dependencyPropertyExtensionBinding = new Binding
                                                         {
                                                             Path = new PropertyPath(dependencyPropertyToBind),
                                                             RelativeSource = bindingSource
                                                         };
            element.SetBinding(attachedDp, dependencyPropertyExtensionBinding);
        }

        private static Action<DependencyPropertyChangedEventArgs> TryGetCallback(
            FrameworkElement element, DependencyProperty boundAttachedDP)
        {
            Action<DependencyPropertyChangedEventArgs> callback = null;
            var callbacks = GetDependencyPropertyCallbacks(element);
            if (callbacks != null)
            {
                callbacks.TryGetValue(boundAttachedDP, out callback);
            }

            return callback;
        }

        #endregion
    }
}