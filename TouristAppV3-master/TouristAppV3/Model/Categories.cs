using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace TouristAppV3.Model
{
    class Categories : INotifyCollectionChanged
    {
        //private String _imageUrl;
        private List<CategoryItemModel> _categoryItems;
        private ComboBox _categoryItems2;
        public string Name { get; set; }

        public List<CategoryItemModel> CategoryItems
        {
            get { return _categoryItems; }
            set { _categoryItems = value; }
        }

        public ComboBox CategBox
        {
            get { return _categoryItems2; }
            set { _categoryItems2 = value;  }
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        //public String ImageUrl
        //{
        //    get { return _imageUrl; }
        //    set { _imageUrl = value; }
        //}


        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
