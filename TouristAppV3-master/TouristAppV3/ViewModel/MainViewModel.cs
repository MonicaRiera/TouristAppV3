using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.Devices.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using TouristAppV3.Annotations;
using TouristAppV3.Common;
using TouristAppV3.Model;

namespace TouristAppV3.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Categories> _categoriesModels;
        private ICommand _editCategoryItemName;
        private Categories _selectedCategory;
        private ICommand _removeSelectedCategoryItem;
        private ICommand _addNewCategoryItem;
        private CategoryItemModel _newCategoryItemModel;
        private CategoryItemModel _selectedCategoryItemModel;
        private ObservableCollection<CategoryItemModel> _categoryItems;

        public MainViewModel()
        {
            LoadCategories();
            LoadCategoryItemModels();

            _newCategoryItemModel = new CategoryItemModel();
            _addNewCategoryItem = new RelayCommand(AddCategoryItem);
            _removeSelectedCategoryItem = new RelayCommand(RemoveCategoryItem);
        }

        private async void LoadCategoryItemModels()
        {
            StorageFile file = null;
            try
            {
                file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("categoryitems.xml");
            }
            catch (Exception)
            {
            }

            if (file == null)
            {
                StorageFolder installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                string xmlfile = @"Assets\xml\CategoryItems.xml";
                file = await installationFolder.GetFileAsync(xmlfile);
            }

            Stream categoryItemStream = await file.OpenStreamForReadAsync();
            XDocument categoryItemDocument = XDocument.Load(categoryItemStream);

            IEnumerable<XElement> categoryItemList = categoryItemDocument.Descendants("categoryitem");

            foreach (XElement xElement in categoryItemList)
            {
                CategoryItemModel ci = new CategoryItemModel();
                ci.Name = xElement.Element("name").Value;
                ci.Category = xElement.Element("category").Value;
                ci.Adress = xElement.Element("adress").Value;
                ci.Email = xElement.Element("email").Value;
                ci.ImageUrl = xElement.Element("imageurl").Value;
                ci.Phone = xElement.Element("phone").Value;
                ci.Web = xElement.Element("web").Value;

                foreach (Categories categoriesModel in CategoriesModels)
                {
                    if (categoriesModel.Name.Equals(ci.Category))
                    {
                        categoriesModel.CategoryItems.Add(ci);
                    }
                }
            }
            OnPropertyChanged("CategoryItems");
        }

        private async void LoadCategories()
        {
            StorageFolder installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            string xmlfile = @"Assets\xml\Categories.xml";
            StorageFile file = await installationFolder.GetFileAsync(xmlfile);

            Stream categoryStream = await file.OpenStreamForReadAsync();
            XDocument categoriesModelDocument = XDocument.Load(categoryStream);

            IEnumerable<XElement> categoriesModelsList = categoriesModelDocument.Descendants("category");

            CategoriesModels = new ObservableCollection<Categories>();

            foreach (XElement xElement in categoriesModelsList)
            {
                CategoriesModels.Add(new Categories()
                {
                    Name = xElement.Element("name").Value,
                    //ImageUrl = xElement.Element("imageurl").Value,
                    CategoryItems = new List<CategoryItemModel>()
                });
            }
            OnPropertyChanged("CategoriesModels");
        }

        public ICommand EditCategoryItemName
        {
            get { return _editCategoryItemName; }
            set { _editCategoryItemName = value; }
        }

        public Categories SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        public ObservableCollection<Categories> CategoriesModels
        {
            get { return _categoriesModels; }
            set { _categoriesModels = value; }
        }

        private void RemoveCategoryItem()
        {
            _categoryItems.Remove(_selectedCategoryItemModel);
        }

        private void AddCategoryItem()
        {
            _categoryItems.Add(_newCategoryItemModel);
            OnPropertyChanged("CategoryItems");
        }

        public ICommand RemoveSelectedCategoryItem
        {
            get { return _removeSelectedCategoryItem; }
            set { _removeSelectedCategoryItem = value; }
        }

        public ICommand AddNewCategoryItem
        {
            get { return _addNewCategoryItem; }
            set { _addNewCategoryItem = value; }
        }

        public CategoryItemModel NewCategoryItemModel
        {
            get { return _newCategoryItemModel; }
            set { _newCategoryItemModel = value; }
        }

        public CategoryItemModel SelectedCategoryItemModel
        {
            get { return _selectedCategoryItemModel; }
            set
            {
                _selectedCategoryItemModel = value;
                OnPropertyChanged("SelectedCategoryItemModel");
            }
        }

        public ObservableCollection<CategoryItemModel> CategoryItems
        {
            get { return _categoryItems; }
            set { _categoryItems = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal TouristAppV3.Model.Categories Categories
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        internal TouristAppV3.Model.CategoryItemModel CategoryItemModel
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
