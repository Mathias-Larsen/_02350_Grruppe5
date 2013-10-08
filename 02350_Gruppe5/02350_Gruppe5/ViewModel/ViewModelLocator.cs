using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace _02350_Gruppe5.ViewModel
{
    /// <summary>
    /// Denne klasse kommer med MVVM Toolkit som benyttes til denne demo. 
    /// Denne klasse gør det let at binde et vindue til en ViewModel.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            // Denne ServiceLocator holder styr på vores ViewModels.
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Tilføj en linje her for hver ViewModel.
            SimpleIoc.Default.Register<MainViewModel>();
        }

        // Tilføj en metode som denne for hver ViewModel.
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        // Hvis der er noget der skal ryddes op så gør det her.
        public static void Cleanup()
        {
        }
    }
}