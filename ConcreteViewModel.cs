namespace Balynn.ViewModelDemo
{
    public class ConcreteViewModel : ViewModel
    {
        public string Name
        {
            get => Get<string>();
            set => Set(value);
        }

        public int Age
        {
            get => Get<int>();
            set => Set(value);
        }
    }
}
