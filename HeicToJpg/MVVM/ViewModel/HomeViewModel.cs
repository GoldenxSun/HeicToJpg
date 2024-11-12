using HeicToJpg.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeicToJpg.MVVM.ViewModel
{
    internal class HomeViewModel
    {
        private HomeModel? _homeModel;

        public HomeModel? HomeModel
        {
            get { return _homeModel; }
            set { _homeModel = value; }
        }

        public HomeViewModel()
        {
            HomeModel = new HomeModel();
        }
    }
}
