## Lab 13. EF Core CRUD 

## CRUD-операции с БД

### Цель работы: 
Изучить внутреннее устройство класса DbContext в Entity Framework Core, механизмы отслеживания изменений сущностей. Реализовать полный цикл CRUD-операций (Create, Read, Update, Delete) в приложении «Телефонная книга», работая с базой данных напрямую через контекст.

### Задание:
Модернизировать приложение «Телефонная книга» (из предыдущих работ) для полноценной работы с базой данных.

## Looking back at Lab 12

So we already have a working reading operations from lab 12.
Because I didn't do the report for lab12, let's back track a little bit on how I've achieved it

After I executed scaffoldiing command, two files were generated
```
|- PhoneBookDbSaiko2307b2Context.cs
|- Contacts.cs
```

Let's look into db context

```csharp
public partial class PhoneBookDbSaiko2307b2Context : DbContext
{
    // constructor for any dbcontext
    public PhoneBookDbSaiko2307b2Context()
    {
    }
    // constructor for any dbcontext with params
    public PhoneBookDbSaiko2307b2Context(DbContextOptions<PhoneBookDbSaiko2307b2Context> options)
        : base(options)
    {
    }
```

I already had a `Contact` class, so I've added `id` field to its constructor and used it in the db context

```csharp
    public virtual DbSet<Contact> Contacts { get; set; } // table

    // ensuring that class has the fields of entity
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contacts__3214EC07CAB4834C");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
```

And I get the connection string from `appsettings.json` in `App.xaml.cs`

```csharp
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var services = new ServiceCollection();

        // Retrieve the settings
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // DbContext
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<PhoneBookDbSaiko2307b2Context>(options => options.UseSqlServer(connectionString));
```

Now let's get into how we integrate db context into ViewModels.

In Contacts List we use the local observable collection

```csharp
public ObservableCollection<Contact> Contacts { get; }
```
And we link it to `Contacts.ToList()` from db context
```csharp
public ContactListViewModel(IDialogService ds, INavigationService navigation, PhoneBookDbSaiko2307b2Context context) : base(navigation)
{
    _context = context;
    _dialogService = ds;
    Contacts = new ObservableCollection<Contact>((IEnumerable<Contact>)_context.Contacts.ToList());
```

...and we did it! The List appears on the screen 

## Now Lab 13

Now we need to create, update, and delete

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
            _context.Contacts.Add(c); // add a record to a db context
            _context.SaveChanges(); // sync changes with an actual db
            Contacts.Add(c); // add to our local collection

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
            _context.Contacts.Remove(SelectedContact); // remove a record from db context
            _context.SaveChanges(); // sync changes with an actual db
            Contacts.Remove(SelectedContact); // remove from our local collection
        }
    }
}
```

We have and edit viewmodel so we need to get context there

```csharp
public ContactEditViewModel(INavigationService navigation, PhoneBookDbSaiko2307b2Context context) : base(navigation)
{
    SaveCommand = new RelayCommand(
        () => {
            _navigation.NavigateTo<ContactListViewModel>();
            context.SaveChanges(); // sync
        });
    CancelCommand = new RelayCommand(
        () => _navigation.NavigateTo<ContactListViewModel>());
}
```

And that's pretty much it. It creates, reads, updates, and deletes records in the database

## Lab 13: Search

In order to do a search, we need to create a filtered collection, so our original won't be changed

```csharp
private ObservableCollection<Contact> _filteredContacts = new();
public ObservableCollection<Contact> FilteredContacts
{
    get => _filteredContacts;
    set => Set(ref _filteredContacts, value);
}
```
```csharp
private void ApplyFilter()
{
    var filtered = string.IsNullOrWhiteSpace(_searchText)
        ? Contacts
        : Contacts.Where(c => c.Name.Contains(_searchText) || c.Phone.Contains(_searchText));

    FilteredContacts = new ObservableCollection<Contact>(filtered);
}
```

And let's call `ApplyFilter()` where it's needed to be

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
            _context.Contacts.Add(c);
            _context.SaveChanges();
            Contacts.Add(c);

            ApplyFilter(); // here
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
            _context.Contacts.Remove(SelectedContact);
            _context.SaveChanges();
            Contacts.Remove(SelectedContact);

            ApplyFilter(); // here
        }
    }
}
```
And let's apply filter when navigatig to Contacts List (so after Edit filter would be applied)
```csharp
public override void OnNavigatedTo(object? parameter) =>
    ApplyFilter();
```

And of course we need to do a search field

```csharp
private string _searchText = string.Empty;
public string SearchText
{
    get => _searchText;
    set { if (Set(ref _searchText, value)) ApplyFilter(); }
}
```

```xml
<Grid Grid.Row="3">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="70"/>
        <ColumnDefinition Width="*"/>
    </Grid.ColumnDefinitions>
    <Label Grid.Column="0" Content="Search:"/>
    <TextBox  Margin="20,5" Grid.Column="1" Text="{Binding SearchText, UpdateSourceTrigger=PropertyChanged}"/>
</Grid>

```

And we need to display the filtered list
```xml
<DataGrid Grid.Row="4" AutoGenerateColumns="False" IsReadOnly="True"
ItemsSource="{Binding FilteredContacts}"
```

### Summary
I've successfully implemented CRUD db operations in my app