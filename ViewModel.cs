using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Balynn.ViewModelDemo
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        private readonly Lazy<ConcurrentDictionary<string, object>> _propertyRefValues =
            new Lazy<ConcurrentDictionary<string, object>>(LazyThreadSafetyMode.ExecutionAndPublication);

        private readonly Lazy<ConcurrentDictionary<string, PropertyChangedEventArgs>> _propertyChangedEventArgs =
            new Lazy<ConcurrentDictionary<string, PropertyChangedEventArgs>>();


        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual T Get<T>(T defaultValue = default, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException(nameof(propertyName));
            }

            if (_propertyRefValues.Value.ContainsKey(propertyName))
            {
                return (T)_propertyRefValues.Value[propertyName];
            }

            return defaultValue;
        }

        protected virtual bool Set<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException(nameof(propertyName));
            }

            var existing = Get(default(T), propertyName);

            if (object.Equals(existing, value) == false)
            {
                _propertyRefValues.Value[propertyName] = value;

                RaisePropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException(nameof(propertyName));
            }

            var propertyArgs = _propertyChangedEventArgs.Value
                .GetOrAdd(propertyName, new PropertyChangedEventArgs(propertyName));

            PropertyChanged?.Invoke(this, propertyArgs);
        }
    }
}
