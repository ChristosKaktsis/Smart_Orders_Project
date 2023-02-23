using SmartMobileWMS.Models;
using SmartMobileWMS.Repositories;
using SmartMobileWMS.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.ViewModels
{
    public class PaletteStartViewModel : BaseViewModel
    {
        private PaletteRepository repository = new PaletteRepository();
        private bool haserror;
        private string errorText;
        private Palette palette;
        private bool canEdit;

        public PaletteStartViewModel()
        {
        }
        public Palette Palette 
        { 
            get => palette;
            set 
            { 
                SetProperty(ref palette, value);
                CanEdit = value != null;
            }
        }
        public bool HasError 
        {
            get => haserror;
            set => SetProperty(ref haserror, value); 
        }
        public string ErrorText
        {
            get => errorText;
            set => SetProperty(ref errorText, value);
        }
        public async Task FindPalette(string sscc)
        {
            try
            {
                IsBusy = true;
                HasError = false;
                var item = await repository.GetItemAsync(sscc);
                if(HasError = item == null)
                {
                    ErrorText = "Η παλέτα δεν βρέθηκε";
                }
                Palette = item;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public bool CanEdit 
        { 
            get => canEdit;
            set => SetProperty(ref canEdit, value); 
        }
        public async Task DeletePaletteContent()
        {
            try 
            { 
                IsBusy = true;
                var result = await repository.DeleteItem(Palette);
                Debug.WriteLine($"Is Content Deleted?{result}");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
