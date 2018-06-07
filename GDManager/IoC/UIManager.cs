using GDManager.Core;
using System;
using System.Windows;

namespace GDManager
{
    /// <summary>
    /// The applications implementation of the <see cref="IUIManager"/>
    /// </summary>
    public class UIManager : IUIManager
    {
        /// <summary>
        /// Performs an action by taking it on dispatcher UI Thread
        /// </summary>
        /// <param name="action">An action to invoke</param>
        public void DispatcherThreadAction(Action action)
        {
            // Invoke the action on the UI Thread
            Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}