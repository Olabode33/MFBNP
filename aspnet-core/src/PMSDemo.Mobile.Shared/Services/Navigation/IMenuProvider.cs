using System.Collections.Generic;
using MvvmHelpers;
using PMSDemo.Models.NavigationMenu;

namespace PMSDemo.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}