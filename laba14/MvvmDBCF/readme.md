## Lab 14. DB Context Factory 

## Управление жизненным циклом DbContext в MVVM-приложениях.

### Цель работы: 
Изучить проблемы, возникающие при некорректном времени жизни DbContext в десктопных MVVM-приложениях (Captive Dependency, утечки памяти). Освоить паттерн IDbContextFactory для управления жизненным циклом контекста базы
данных, научиться работать с отсоединенными сущностями (Detached Entities) и реализовывать безопасные CRUD-операции без утечек состояния Change Tracker.

### Задание:
Модернизировать приложение «Телефонная книга», исправив проблему времени жизни DbContext.

As of now, DbContext service has a scoped lifetime, but it's bound to Root Scope which lives the entire application lifetime

We can use IDbContextFactory in order to control lifetime of DbContext inside our app manually

In `App.xaml.cs`, in DI service registration
```csharp
// DbContext
string? connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDbContextFactory<PhoneBookDbSaiko2307b2Context>(options => options.UseSqlServer(connectionString));
```

Now we need adapt the code to use factory and create dbcontext in it

In `ContactListViewModel`
```csharp
private readonly IDbContextFactory<PhoneBookDbSaiko2307b2Context> _contextFactory;
```
And in the constructor 
```csharp
 public ContactListViewModel(IDialogService ds, 
    INavigationService navigation, 
    IDbContextFactory<PhoneBookDbSaiko2307b2Context> contextFactory) : base(navigation)
 {
     _contextFactory = contextFactory;
     _dialogService = ds;

     using (var context = _contextFactory.CreateDbContext()) // here
     {
         Contacts = new ObservableCollection<Contact>((IEnumerable<Contact>)context.Contacts.ToList());
     }

     FilteredContacts = new ObservableCollection<Contact>(Contacts);
```

Let's move to our Create, Update, and Remove operations

We now need to create DbContext from factory every time we need to access DbContext

```csharp
private void AddContact()
{
    Contact c = new Contact(0, Name, Phone);
    if (c.Validate())
    {
        if (Contacts.Any(c => c.Phone == _phone))
        {
            _dialogService.ShowError("A contact with that phone already exists");
        }
        else
        {
            using (var context = _contextFactory.CreateDbContext()) // here
            {
                context.Contacts.Add(c);
                context.SaveChanges();
            }
            Contacts.Add(c);

            ApplyFilter();
            Name = string.Empty;
            Phone = string.Empty;
            _dialogService.ShowInfo("Contact has been added");
        }
    }
}
```

```csharp
private void DeleteContact()
{
    if (SelectedContact is not null && Contacts.Contains(SelectedContact))
    {
        if (_dialogService.GetConfirm($"Delete contact {SelectedContact}?"))
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Contacts.Remove(SelectedContact);
                context.SaveChanges();
            }

            Contacts.Remove(SelectedContact);

            ApplyFilter();
        }
    }
}
```

And in Edit, we need to update the values for the tracked record

```csharp
public ContactEditViewModel(INavigationService navigation, IDbContextFactory<PhoneBookDbSaiko2307b2Context> contextFactory) : base(navigation)
{
    SaveCommand = new RelayCommand(
        () => {
            using (var context = contextFactory.CreateDbContext())
            {
                var contactToUpdate = context.Contacts.Find(_contact.Id); // Find - we now track the object

                if (contactToUpdate != null)
                {
                    // change the tracked object's fields
                    contactToUpdate.Name = _contact.Name;
                    contactToUpdate.Phone = _contact.Phone;
                    context.SaveChanges(); // sync with db
                }
                
            }
            _navigation.NavigateTo<ContactListViewModel>();
        });
    CancelCommand = new RelayCommand(
        () => _navigation.NavigateTo<ContactListViewModel>());
}
```

### Summary
I've successfully remade my app so DbContext lifetime is now shorter and it is controlled manually via IDbContextFactory