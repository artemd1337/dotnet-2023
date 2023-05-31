using ReactiveUI;

namespace NonResidentialFund.Client.ViewModels;

public class TableViewModelBase : ViewModelBase
{
    public Interaction<OrganizationViewModel, OrganizationViewModel?> ShowDialog { get; } = new();
}
