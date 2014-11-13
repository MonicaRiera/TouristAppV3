using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TouristAppV3.Annotations;

namespace TouristAppV3.Model
{
    class CategoryItemModel : INotifyPropertyChanged
    {
        private string _name;
        private string _adress;
        private string _imageUrl;
        private string _web;
        private string _email;
        private string _phone;
        private string _category;
        private List<CommentModel> _comments;

        public List<CommentModel> Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public String Adress
        {
            get { return _adress; }
            set { _adress = value; }
        }

        public String ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        public String Web
        {
            get { return _web; }
            set { _web = value; }
        }

        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public String Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public String Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
