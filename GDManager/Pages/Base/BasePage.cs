﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using GDManager.Core;
using Testinator.AnimationFramework;

namespace GDManager
{
    /// <summary>
    /// The base page for all pages to gain base functionality
    /// </summary>
    public class BasePage : UserControl
    {
        #region Private Members

        /// <summary>
        /// The View Model associated with this page
        /// </summary>
        private object mViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// The animation to play when the page is first loaded
        /// </summary>
        public ElementAnimation PageLoadAnimation { get; set; } = ElementAnimation.SlideAndFadeInFromLeft;

        /// <summary>
        /// The animation to play when the page is unloaded
        /// </summary>
        public ElementAnimation PageUnloadAnimation { get; set; } = ElementAnimation.SlideAndFadeOutToRight;

        /// <summary>
        /// The time any slide animation takes to complete
        /// </summary>
        public float SlideSeconds { get; set; } = 0.7f;

        /// <summary>
        /// A flag to indicate if this page should animate out on load.
        /// Useful for when we are moving the page to another frame
        /// </summary>
        public bool ShouldAnimateOut { get; set; }

        /// <summary>
        /// The View Model associated with this page
        /// </summary>
        public object ViewModelObject
        {
            get => mViewModel;
            set
            {
                // If nothing has changed, return
                if (mViewModel == value)
                    return;

                // Update the value
                mViewModel = value;

                // Fire the view model changed method
                OnViewModelChanged();

                // Set the data context for this page
                DataContext = mViewModel;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage()
        {
            // Don't bother animating in design time
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            // If we are animating in, hide to begin with
            if (PageLoadAnimation != ElementAnimation.None)
                Visibility = Visibility.Collapsed;

            // Listen out for the page loading
            Loaded += BasePage_LoadedAsync;
        }

        #endregion

        #region Animations

        /// <summary>
        /// Once the page is loaded, perform any required animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BasePage_LoadedAsync(object sender, RoutedEventArgs e)
        {
            // If we are setup to animate out on load
            if (ShouldAnimateOut)
                // Animate out the page
                await this.AnimateOutAsync(PageUnloadAnimation, SlideSeconds);
            // Otherwise...
            else
                // Animate the page in
                await this.AnimateInAsync(PageLoadAnimation, SlideSeconds);
        }

        #endregion

        /// <summary>
        /// Fired when the view model changes
        /// </summary>
        protected virtual void OnViewModelChanged()
        {

        }
    }

    /// <summary>
    /// A base page with added ViewModel support
    /// </summary>
    public class BasePage<VM> : BasePage
        where VM : BaseViewModel, new()
    {
        #region Public Properties

        /// <summary>
        /// The view model associated with this page
        /// </summary>
        public VM ViewModel
        {
            get => (VM)ViewModelObject;
            set => ViewModelObject = value;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage() : base()
        {
            // Don't bother doing anything in design time
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            // Create a default view model
            ViewModel = IoC.Get<VM>();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use, if any</param>
        public BasePage(VM specificViewModel = null) : base()
        {
            // Don't bother doing anything in design time
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            // Set specific view model
            if (specificViewModel != null)
                ViewModel = specificViewModel;
            else
                // Create a default view model
                ViewModel = IoC.Get<VM>();
        }

        #endregion
    }
}