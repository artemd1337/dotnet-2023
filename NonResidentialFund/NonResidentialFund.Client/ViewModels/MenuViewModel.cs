using ReactiveUI;

namespace NonResidentialFund.Client.ViewModels;
public class MenuViewModel : TableViewModelBase
{
    public MenuViewModel()
    {
        ShowDialog = new Interaction<OrganizationViewModel, OrganizationViewModel?>();
    }
}
