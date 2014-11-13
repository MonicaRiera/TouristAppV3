using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using TouristAppV3.Annotations;
using TouristAppV3.Common;
using TouristAppV3.Model;
using TouristAppV3.View;

namespace TouristAppV3.ViewModel
{
    class CommentViewModel : TouristAppV3.Model.CommentModel, INotifyPropertyChanged
    {
        private ObservableCollection<Categories> _categoriesModels;
        private Categories _selectedCategory;
        private ObservableCollection<CategoryItemModel> _categoryItems;
        private CategoryItemModel _selectedCategoryItemModel;
        private ObservableCollection<CommentModel> _commentModels;
        private CommentModel _selectedCommentModel;
        private List<CommentModel> _comments;
        private CommentModel _newCommentModel;
        private ICommand _addNewComment;
        private ICommand _saveNewComment;
        private CategoryItemModel _selectedItrem;
        public ObservableCollection<CommentModel> FilteredComments { get; set; }

        public CommentViewModel()
        {
            _addNewComment = new RelayCommand(AddNewCommentCommand);
            _newCommentModel = new CommentModel();
            //_saveNewComment = new RelayCommand(SaveNewCommentCommand);
            
            CommentModels = new ObservableCollection<CommentModel>();
            FilteredComments = CommentModels;
            LoadCategories();
            LoadCategoryItemModels();
            LoadComments();
            
        }
        private async void AddNewCommentCommand()
        {
            StorageFile file = null;
            try
            {
                file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("comments.xml");
            }
            catch (Exception)
            {
            }

            if (file == null)
            {
                StorageFolder installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                string xmlfile = @"Assets\xml\Comments.xml";
                file = await installationFolder.GetFileAsync(xmlfile);
            }

            Stream LoadStream = await file.OpenStreamForReadAsync();
            XDocument commentDocument = XDocument.Load(LoadStream);
            LoadStream.Dispose();

            XElement commentList = commentDocument.Element("comments");

            XElement comment = new XElement("comment");
            comment.Add(new XElement("name", SelectedItrem.Name));
            //comment.Add(new XElement("name", NewCommentModel.Name));
            comment.Add(new XElement("content", NewCommentModel.Content));

            commentList.LastNode.AddAfterSelf(comment);

            StorageFile saveFile = null;

            try
            {
                saveFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("Comments.xml");
            }
            catch { }

            if (saveFile == null)
            {
                saveFile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("Comments.xml");
            }

            Stream saveStream = await saveFile.OpenStreamForWriteAsync();
            commentDocument.Save(saveStream);
            saveStream.Dispose();
            OnPropertyChanged("CommentModels");

            LoadComments();

        }

        private async void LoadComments()
        {
            StorageFile file = null;
            try
            {
                file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("Comments.xml");
            }
            catch (Exception)
            {
            }

            if (file == null)
            {
                StorageFolder installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                string xmlfile = @"Assets\xml\Comments.xml";
                file = await installationFolder.GetFileAsync(xmlfile);
            }

            Stream commentStream = await file.OpenStreamForReadAsync();
            XDocument commentsModelDocument = XDocument.Load(commentStream);

            IEnumerable<XElement> commentsModelList = commentsModelDocument.Descendants("comment");
            CommentModels = new ObservableCollection<CommentModel>();

            foreach (XElement xElement in commentsModelList)
            {
                CommentModels.Add(new CommentModel()
                {
                    Name = xElement.Element("name").Value,
                    Content = xElement.Element("content").Value
                });
            }

            FilteredComments = new ObservableCollection<CommentModel>();
            if (SelectedItrem == null)
            {
                FilteredComments = CommentModels;
            }
            else
            {
                foreach (CommentModel commentModel in _commentModels)
                {
                    if (SelectedItrem.Name == commentModel.Name)
                    {
                        FilteredComments.Add(commentModel);
                    }
                }
            } OnPropertyChanged("FilteredComments");


            //foreach (CommentModel commentModel in CommentModels)
            //{
            //    if (SelectedItrem.Name == commentModel.Name)
            //    {
            //        CommentModels.Add(commentModel);
            //    }

            //}
            


            OnPropertyChanged("CommentModels");
        }

        public List<CommentModel> Comments
        {
            get { return _comments; }
            set { _comments = value; }
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

            CategoryItems = new ObservableCollection<CategoryItemModel>();

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

                CategoryItems.Add(new CategoryItemModel()
                {
                    Comments = new List<CommentModel>()
                });

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

        public CategoryItemModel SelectedItrem
        {
            get
            {
                return _selectedItrem;
            }
            set
            {
                _selectedItrem = value;
                LoadComments();
            }
        }

        public ObservableCollection<Categories> CategoriesModels
        {
            get { return _categoriesModels; }
            set { _categoriesModels = value; }
        }
        public ObservableCollection<CategoryItemModel> CategoryItems
        {
            get { return _categoryItems; }
            set { _categoryItems = value; }
        }
        public ObservableCollection<CommentModel> CommentModels
        {
            get { return _commentModels; }
            set { _commentModels = value; }
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
        public Categories SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }
        public CommentModel SelectedCommentModel
        {
            get { return _selectedCommentModel; }
            set
            {
                _selectedCommentModel = value;
                OnPropertyChanged("SelectedCommentModel");
            }
        }
        public CommentModel NewCommentModel
        {
            get { return _newCommentModel; }
            set { _newCommentModel = value; }
        }
        public ICommand AddNewComment
        {
            get { return _addNewComment; }
            set { _addNewComment = value; }
        }
        public ICommand SaveNewComment
        {
            get { return _saveNewComment; }
            set { _saveNewComment = value; }
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

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
