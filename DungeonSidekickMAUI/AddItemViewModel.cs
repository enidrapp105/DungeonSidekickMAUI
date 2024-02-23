using System.Collections.ObjectModel;

namespace DungeonSidekickMAUI;

public class AddItemViewModel : BindableObject
{
    public class UserItem
    {
        public string Name { get; set; }

        public string Type { get; set; }

    }

    private ObservableCollection<UserItem> userItems;
    public ObservableCollection<UserItem> UserItems
    {
        get
        {
            return userItems;
        }
        set
        {
            if (userItems != value)
            {
                userItems = value;
                OnPropertyChanged("UserItems");
            }
        }
    }


    public AddItemViewModel()
    {
        UserItems = new ObservableCollection<UserItem>
            {
                
            };

    }
}