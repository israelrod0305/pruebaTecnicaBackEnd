﻿using Abp.Application.Navigation;
using PruebaTecnica.Web.Utils;

namespace PruebaTecnica.Web.Views.Shared.Components.TopMenu
{
    public class TopMenuViewModel
    {
        public UserMenu MainMenu { get; set; }

        public string ActiveMenuItemName { get; set; }

        public string CalculateMenuUrl(string applicationPath, UserMenuItem menuItem)
        {
            if (string.IsNullOrEmpty(menuItem.Url))
            {
                return applicationPath;
            }

            if (UrlHelper.IsRooted(menuItem.Url))
            {
                return menuItem.Url;
            }

            return applicationPath + menuItem.Url;
        }
    }
}