using AmazingHome.HomeAPI.Models;

namespace AmazingHome.HomeAPI.Data
{
    public static class HomeStore
    {
        public static List<Home> homeList = new List<Home>
                 {
                new Home { Id = 1,Name="Tarrace View",Sqft=100,Occupancy=4},
                new Home { Id = 2,Name="Beach View",Sqft=150,Occupancy=5}

            };

    }
}
