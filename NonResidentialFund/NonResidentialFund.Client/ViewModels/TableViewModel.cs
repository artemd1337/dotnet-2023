using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonResidentialFund.Client.ViewModels;
public class TableViewModelBase : ViewModelBase
{
    public Interaction<OrganizationViewModel, OrganizationViewModel?> ShowDialog { get; set; }
}
