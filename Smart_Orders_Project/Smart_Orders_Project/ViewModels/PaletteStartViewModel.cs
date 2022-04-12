using Smart_Orders_Project.Models;
using Smart_Orders_Project.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.ViewModels
{
    public class PaletteStartViewModel : BaseViewModel
    {
        private RepositoryPalette repository;
        private bool haserror;
        private string errorText;
        private Palette palette;
        private bool canEdit;

        public PaletteStartViewModel()
        {
            repository = new RepositoryPalette();
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
                var item = await repository.GetPalette(sscc);
                if(HasError = item == null)
                {
                    ErrorText = "Η παλέτα δεν βρέθηκε";
                }
                Palette = item;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
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
                var result = await repository.DeletePaletteContent(Palette);
                Console.WriteLine($"Is Content Deleted?{result}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
