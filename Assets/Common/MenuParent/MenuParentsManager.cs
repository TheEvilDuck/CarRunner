using System;
using System.Collections.Generic;

namespace Common.MenuParent
{
    public class MenuParentsManager
    {
        private List<IMenuParent> _menus;

        public MenuParentsManager()
        {
            _menus = new List<IMenuParent>();
        }

        public void Add(IMenuParent menuParent)
        {
            if (_menus.Contains(menuParent))
                throw new ArgumentException("This menuParentsManager aleady contains passed menu parent");

            _menus.Add(menuParent);
        }

        public void Show(IMenuParent menuParent)
        {
            if (!_menus.Contains(menuParent))
                throw new ArgumentException("This menuParentsManager does not contain passed menu parent");

            foreach (IMenuParent otherParent in _menus)
                if (otherParent != menuParent)
                    otherParent.Hide();

            menuParent.Show();
        }

        public void HideAll()
        {
            foreach (IMenuParent otherParent in _menus)
                otherParent.Hide();
        }
    }
}
