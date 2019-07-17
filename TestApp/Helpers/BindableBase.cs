﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace TestApp.Helpers
{
    /// <summary>
    /// Implementation of <see cref="INotifyPropertyChanged" /> to simplify models.
    /// From here: https://www.danrigby.com/2015/09/12/inotifypropertychanged-the-net-4-6-way/
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged
    {
        [NotMapped]
        public bool AreNotificationsDisabled { get; set; }

        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">
        /// Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.
        /// </param>
        /// <returns>
        /// True if the value was changed, false if the existing value matched the
        /// desired value.
        /// </returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute" />.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (AreNotificationsDisabled)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Creates a copy of the current object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <returns>A shallow copy of the object.</returns>
        public T ShallowCopy<T>()
        {
            return (T)MemberwiseClone();
        }
    }
}