using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Controls;
using TouristAppV3.View;

namespace TouristAppV3.Model
{
    class OrderModel
    {
        //private DateTimeOffset date;
        //private TimeSpan time
            
        private string _firstName;
        private TimeSpan _time;
        private DateTimeOffset _from;
        private string _place;
        private string _lastName;
        private DateTimeOffset _to;

        public String FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public String LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public String Place
        {
            get { return _place; }
            set { _place = value; }
        }

        public DateTimeOffset From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
                
            }
        }

        public DateTimeOffset To
        {
            get { return _to; }
            set { _to = value; }
        }

        public TimeSpan Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public override string ToString()
        {
            return From.ToString();
        }

        
    }
}
