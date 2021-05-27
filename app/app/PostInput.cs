using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app
{
    public class MyEventArgs : EventArgs
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    public class PostInput
    {
        public delegate void @EventHandler(object source, MyEventArgs args);

        // Deklaracja eventu który będzię używał powyższego delegata z argumentem
        public event @EventHandler @Event;
        public int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnEvent();
            }
        }
        public string _Title;
        public string Title
        {
            get { return _Title; }
            set
            { 
                _Title = value;
                OnEvent();
            }
        }
        public string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnEvent();
            }
        }
        public string _ImageUrl;
        public string ImageUrl
        {
            get { return _ImageUrl; }
            set
            {
                _ImageUrl = value;
                OnEvent();
            }
        }
        protected virtual void OnEvent()
        {
            if (@Event != null)
                @Event(this, new MyEventArgs { Id = _Id, Title = _Title, Description = _Description, ImageUrl = _ImageUrl });
        }
    }
}
