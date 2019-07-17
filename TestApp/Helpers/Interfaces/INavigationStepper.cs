using System.Threading.Tasks;

namespace TestApp.Helpers.Interfaces
{
    public interface INavigationStepper<T> where T : class
    {
        T Transient { get; set; }

        /// <summary>
        /// A value between 0 and 1, indicating the progress of the multi step task.
        /// </summary>
        double Progress { get; }

        /// <summary>
        /// True if the step can be skipped.
        /// </summary>
        bool IsSkippable { get; set; }

        /// <summary>
        /// True if the step can be interacted with it.
        /// </summary>
        bool IsInteractive { get; set; }

        /// <summary>
        /// The text of the header of the page.
        /// </summary>
        string Header { get; }

        /// <summary>
        /// The text of the action that will be displayed inside the [Next] button.
        /// </summary>
        string ActionTitle { get; set; }

        void Refresh();
        void Unload();
        void Loaded();
        INavigationStepper<T> Previous();
        Task<bool> IsValid();
        T FillIn(T param);
        INavigationStepper<T> Next();
    }
}