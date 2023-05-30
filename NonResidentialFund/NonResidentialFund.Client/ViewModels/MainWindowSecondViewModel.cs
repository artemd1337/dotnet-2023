using Newtonsoft.Json.Bson;
using NonResidentialFund.Client.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace NonResidentialFund.Client.ViewModels;

public class MainWindowSecondViewModel : ViewModelBase
{
    private TableViewModelBase _content;

    public TableViewModelBase Content
    {
        get => _content;
        private set => this.RaiseAndSetIfChanged(ref _content, value);
    }
    public MenuViewModel Menu { get; }

    public MainWindowSecondViewModel()
    {
        Content = Menu = new MenuViewModel();
    }

    public void Organizations()
    {
        var vm = new OrganizationsViewModel();
        Observable.Any(vm.Cancel).Subscribe(model => { Content = Menu; });
        Content = vm;
    }
}
