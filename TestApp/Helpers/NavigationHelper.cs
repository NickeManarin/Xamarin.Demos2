using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestApp.Helpers
{
    internal class NavigationHelper
    {
        private readonly object _sync = new object();

        public Stack<NavigationPage> PageStack { get; } = new Stack<NavigationPage>();

        public NavigationPage CurrentPage => PageStack.Peek();


        /// <summary>
        /// Sets the page as the main page of the navigation stack.
        /// Clears the page stack and pushes the page as the first item.
        /// </summary>
        /// <param name="start">The page to bet set as the main page.</param>
        public void Start(Page start)
        {
            lock (_sync)
            {
                NavigationPage.SetHasNavigationBar(start, false);

                var nav = new NavigationPage(start);
                PageStack.Clear();
                PageStack.Push(nav);

                Application.Current.MainPage = nav;
            }
        }

        /// <summary>
        /// Navigates non-modally to the page.
        /// </summary>
        /// <param name="page">The page to nativate to.</param>
        /// <param name="animated">True if the navigation should be animated.</param>
        public async Task NavigateAsync(Page page, bool animated = true)
        {
            NavigationPage.SetHasNavigationBar(page, false);

            var nav = new NavigationPage(page) { AutomationId = "1" };
            await CurrentPage.Navigation.PushAsync(nav, animated);

            lock (_sync)
                PageStack.Push(nav);
        }

        /// <summary>
        /// Navigates modally to the page.
        /// </summary>
        /// <param name="page">The page to nativate to.</param>
        /// <param name="animated">True if the navigation should be animated.</param>
        public async Task NavigateModalAsync(Page page, bool animated = true)
        {
            NavigationPage.SetHasNavigationBar(page, false);

            var nav = new NavigationPage(page) { AutomationId = "2" };
            await CurrentPage.Navigation.PushModalAsync(nav, animated);

            lock (_sync)
                PageStack.Push(nav);
        }

        /// <summary>
        /// Returns/navigates back to the previous page of the stack.
        /// </summary>
        public async Task GoBackAsync(bool animated = true)
        {
            //TODO: Test with non-modal pages.

            await CurrentPage.Navigation.PopModalAsync(animated);

            lock (_sync)
                PageStack.Pop();
        }

        /// <summary>
        /// Returns/navigates to the main page of the stack.
        /// </summary>
        public async Task GoToStartAsync(bool animated = true)
        {
            //TODO: Test with non-modal pages.

            while (PageStack.Count > 1)
            {
                await CurrentPage.Navigation.PopModalAsync(animated);

                lock (_sync)
                    PageStack.Pop();
            }
        }
    }
}