using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAppV3.Model
{
    class CommentModel
    {
        private string _name;
        private string _content;
        private List<CommentModel> _commentsModels;


        public List<CommentModel> CommentsModels
        {
            get { return _commentsModels; }
            set { _commentsModels = value; }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public String Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public override string ToString()
        {
            return Content.ToString();
        }
    }
}
